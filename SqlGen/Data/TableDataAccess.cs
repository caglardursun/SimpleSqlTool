//using BusterWood.Mapper;
using Dapper;
using SqlGen.Helper;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace SqlGen
{
    public class TableDataAccess
    {
        private readonly DbConnection connection;



        public TableDataAccess(DbConnection connection)
        {
            this.connection = connection;
        }



        public async Task<IEnumerable<Table>> LoadNonAuditTable()
        {
            var result = await connection.QueryAsync<Table>(QueryHelper.TableSql);
            return result;
        }


        public List<Column> LoadColumns(string table, string schema = "dbo")
        {
            var cols = connection.Query<Column>(QueryHelper.ColumnSql, new { table, schema }).ToList();

            var last = cols.Where(c => c.IsAuditColumn() || c.IsRowVersion() || c.IsSequenceNumber()).ToList();
            foreach (var c in last)
            {
                cols.Remove(c);
            }
            cols.AddRange(last);

            return cols;
        }



        public void PopulatePrimaryKey(Table table)
        {
            var colMap = table.Columns.ToDictionary(col => col.ColumnName);
            var pkCols = LoadPrimaryKeyColumns(table.TableName, table.Schema);
            table.PrimaryKey = new PrimaryKey
            {
                ConstraintName = pkCols.FirstOrDefault()?.ConstraintName,
                KeyColumns = pkCols.Select(pkc => colMap[pkc.ColumnName]).ToList()
            };
        }

        List<KeyColumn> LoadPrimaryKeyColumns(string table, string schema)
        {
            return connection.Query<KeyColumn>(QueryHelper.PrimaryKeySql, new { table, schema }).ToList();
        }

        public void PopulateForeignKeys(Table table)
        {
            var colMap = table.Columns.ToDictionary(col => col.ColumnName);
            var fks = LoadForeignKeyContraints(table.TableName, table.Schema);
            var colsByFkName = LoadForeignKeyColumns(table.TableName, table.Schema);
            foreach (var fk in fks)
            {
                //var columns = colsByFkName[fk.ConstraintName].Select(c => colMap[c.ColumnName]).ToList();
                var columns = colsByFkName[fk.ConstraintName].ToList<Column>();

                foreach (var item in columns)
                {
                    item.ForegnReferanceTable = GetTable(item.ReferancedTableName, item.TableSchema);
                }

                fk.TableColumns = columns;
            }

            table.ForeignKeys = fks;
        }



        List<ForeignKey> LoadForeignKeyContraints(string table, string schema)
        {

            return connection.Query<ForeignKey>(QueryHelper.ForeignKeySql, new { table, schema }).ToList();
        }



        Dictionary<string, List<KeyColumn>> LoadForeignKeyColumns(string table, string schema)
        {

            var queryResult = connection
                .Query<KeyColumn>(QueryHelper.ForeignKeyColumnSql, new { table, schema })
                .ToList();

            // var eliminate = (from data in queryResult
            //                 group data by data.ConstraintName into key
            //                 );

            //Group by ConstraintName
            var eliminate = from data in queryResult
                            group data by data.ConstraintName into g
                            select new { key = g.Key, value = g.ToList() };
            //Sıçmış sıvamışsın ... 

            //result = queryResult.ToDictionary<string,List<Column>>(key=> key.ConstraintName, value => value);

            return eliminate.ToDictionary(h => h.key, v => v.value);
        }


        public Table GetTable(string table, string schema)
        {
            Table t = connection.Query<Table>(QueryHelper.TableSql).ToList<Table>().FirstOrDefault(h => h.TableName.Equals(table));
            t.Columns = LoadColumns(table, schema);
            return t;
        }

        public async Task<IEnumerable<string>> ListDatabases()
        {
            var dbs = await connection.QueryAsync<Database>(QueryHelper.ListDBSql);
            return dbs.Select(db => db.Name).ToList();
        }

        public async Task<string> CurrentDatabase()
        {
            var db = await connection.QueryFirstAsync<Database>(QueryHelper.CurrentDbNameSql);
            return db.Name;
        }
    }



}
