﻿using BusterWood.Mapper;
using System;
using System.Collections;
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

        const string tableSql = @"select TABLE_SCHEMA, TABLE_NAME
                                from INFORMATION_SCHEMA.TABLES
                                where TABLE_NAME NOT LIKE '%_AUDIT'
                                order by TABLE_SCHEMA, TABLE_NAME";

        public Task<List<Table>> LoadNonAuditTable()
        {
            return connection.QueryAsync(tableSql).ToListAsync<Table>();
        }

        const string columnSql = @"select *, cast(COLUMNPROPERTY(object_id(TABLE_SCHEMA+'.'+TABLE_NAME), COLUMN_NAME, 'IsIdentity') as bit) as [IsIdentity]
                from INFORMATION_SCHEMA.COLUMNS 
                where table_name = @table 
                and TABLE_SCHEMA = @schema
                order by ORDINAL_POSITION";

        public List<Column> LoadColumns(string table, string schema = "dbo")
        {
            var cols = connection.Query(columnSql, new { table, schema }).ToList<Column>();

            var last = cols.Where(c => c.IsAuditColumn() || c.IsRowVersion() || c.IsSequenceNumber()).ToList();
            foreach (var c in last)
            {
                cols.Remove(c);
            }
            cols.AddRange(last);
            return cols;
        }

        const string primaryKeySql = @"SELECT tc.CONSTRAINT_NAME, ku.COLUMN_NAME
                FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS tc
                JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS ku ON tc.CONSTRAINT_TYPE = 'PRIMARY KEY' AND tc.CONSTRAINT_NAME = ku.CONSTRAINT_NAME
                where tc.table_name = @table 
                and tc.TABLE_SCHEMA = @schema
                order by ku.ORDINAL_POSITION";

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
            return connection.Query(primaryKeySql, new { table, schema }).ToList<KeyColumn>();
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

        const string foreignKeySql = @"select CONSTRAINT_NAME 
                                        from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS as RC
                                        where exists (
	                                        select * 
	                                        from INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU1
	                                        where KCU1.CONSTRAINT_CATALOG = RC.CONSTRAINT_CATALOG
			                                        AND KCU1.CONSTRAINT_SCHEMA = RC.CONSTRAINT_SCHEMA
			                                        AND KCU1.CONSTRAINT_NAME = RC.CONSTRAINT_NAME
			                                        and TABLE_SCHEMA = @schema
			                                        and TABLE_NAME = @table)";

        List<ForeignKey> LoadForeignKeyContraints(string table, string schema)
        {
            return connection.Query(foreignKeySql, new { table, schema }).ToList<ForeignKey>();
        }

        const string foreignKeyColumnSql = @"
                    SELECT 
	        KeyColumnUsage.*
            ,KeyColumnUsage.TABLE_NAME AS  SourceTableName 
            ,KeyColumnUsage.COLUMN_NAME AS SourceColumnName
	        ,KeyColumnUsage2.TABLE_NAME AS  ReferancedTableName
            ,KeyColumnUsage2.COLUMN_NAME AS ReferancedColumnName
            FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS RefConst 

            INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KeyColumnUsage 
                ON KeyColumnUsage.CONSTRAINT_CATALOG = RefConst.CONSTRAINT_CATALOG  
                AND KeyColumnUsage.CONSTRAINT_SCHEMA = RefConst.CONSTRAINT_SCHEMA 
                AND KeyColumnUsage.CONSTRAINT_NAME = RefConst.CONSTRAINT_NAME 

            INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KeyColumnUsage2 
                ON KeyColumnUsage2.CONSTRAINT_CATALOG = RefConst.UNIQUE_CONSTRAINT_CATALOG  
                AND KeyColumnUsage2.CONSTRAINT_SCHEMA = RefConst.UNIQUE_CONSTRAINT_SCHEMA 
                AND KeyColumnUsage2.CONSTRAINT_NAME = RefConst.UNIQUE_CONSTRAINT_NAME 
                AND KeyColumnUsage2.ORDINAL_POSITION = KeyColumnUsage.ORDINAL_POSITION 

            where RefConst.CONSTRAINT_SCHEMA = @schema and KeyColumnUsage.TABLE_NAME = @table


        ";        

        HashLookup<string, KeyColumn> LoadForeignKeyColumns(string table, string schema)
        {
            return connection.Query(foreignKeyColumnSql, new { table, schema }).ToLookup<string, KeyColumn>(c => c.ConstraintName);
        }


        public Table GetTable(string table, string schema)
        {
            Table t = connection.Query(tableSql).ToList<Table>().FirstOrDefault(h=>h.TableName.Equals(table));
            t.Columns = LoadColumns(table, schema);
            return t;
        }

        public async Task<List<string>> ListDatabases()
        {
            var dbs = await connection.QueryAsync("select name from sys.databases where state = 0").ToListAsync<Database>();
            return dbs.Select(db => db.Name).ToList();
        }

        public async Task<string> CurrentDatabase()
        {
            var db = await connection.QueryAsync("select DB_NAME() as Name").SingleAsync<Database>();
            return db.Name;
        }
    }

    class Database
    {
        public string Name { get; set; }
    }

    public abstract class TableKey : IEnumerable<Column>
    {
        public string ConstraintName { get; set; }
        public abstract IEnumerator<Column> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class PrimaryKey : TableKey
    {
        public List<Column> KeyColumns { get; set; }
        public override IEnumerator<Column> GetEnumerator() => KeyColumns.GetEnumerator();
    }

    public class ForeignKey : TableKey
    {    

        public List<Column> TableColumns { get; set; }
        public override IEnumerator<Column> GetEnumerator() => TableColumns.GetEnumerator();
    }

    class KeyColumn : Column
    {
        public string ConstraintName { get; set; }

    }

}
