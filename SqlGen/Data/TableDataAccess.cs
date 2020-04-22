using BusterWood.Mapper;
using SqlGen.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
//using Dapper;

namespace SqlGen
{
    public class TableDataAccess
    {
        private readonly DbConnection connection;

        

        public TableDataAccess(DbConnection connection)
        {
            this.connection = connection;
        }

      

        public Task<List<Table>> LoadNonAuditTable()
        {            
            return connection.QueryAsync(QueryHelper.TableSql).ToListAsync<Table>();
        }


        public List<Column> LoadColumns(string table, string schema = "dbo")
        {
            var cols = connection.Query(QueryHelper.ColumnSql, new { table, schema }).ToList<Column>();

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
            table.PrimaryKey = new PrimaryKey {
                ConstraintName = pkCols.FirstOrDefault()?.ConstraintName,
                KeyColumns = pkCols.Select(pkc => colMap[pkc.ColumnName]).ToList()
            };
        }

        List<KeyColumn> LoadPrimaryKeyColumns(string table, string schema)
        {
            return connection.Query(QueryHelper.PrimaryKeySql, new { table, schema }).ToList<KeyColumn>();
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
            return connection.Query(QueryHelper.ForeignKeySql, new { table, schema }).ToList<ForeignKey>();
        }

               

        HashLookup<string, KeyColumn> LoadForeignKeyColumns(string table, string schema)
        {
            return connection.Query(QueryHelper.ForeignKeyColumnSql, new { table, schema }).ToLookup<string, KeyColumn>(c => c.ConstraintName);
        }


        public Table GetTable(string table, string schema)
        {
            Table t = connection.Query(QueryHelper.TableSql).ToList<Table>().FirstOrDefault(h=>h.TableName.Equals(table));
            t.Columns = LoadColumns(table, schema);
            return t;
        }

        public async Task<List<string>> ListDatabases()
        {
            var dbs = await connection.QueryAsync(QueryHelper.ListDBSql).ToListAsync<Database>();
            return dbs.Select(db => db.Name).ToList();
        }

        public async Task<string> CurrentDatabase()
        {
            var db = await connection.QueryAsync(QueryHelper.CurrentDbNameSql).SingleAsync<Database>();
            return db.Name;
        }
    }

  

}
