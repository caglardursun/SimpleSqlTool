using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlGen.Helper
{
    public static class QueryHelper
    {
        #region queries ...

        private static string foreignKeyColumnSql = @"
                    SELECT KeyColumnUsage.*
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
            where RefConst.CONSTRAINT_SCHEMA = @schema and KeyColumnUsage.TABLE_NAME = @table";

        private static string foreignKeySql = @"select CONSTRAINT_NAME 
                                        from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS as RC
                                        where exists (
	                                        select * 
	                                        from INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU1
	                                        where KCU1.CONSTRAINT_CATALOG = RC.CONSTRAINT_CATALOG
			                                        AND KCU1.CONSTRAINT_SCHEMA = RC.CONSTRAINT_SCHEMA
			                                        AND KCU1.CONSTRAINT_NAME = RC.CONSTRAINT_NAME
			                                        and TABLE_SCHEMA = @schema
			                                        and TABLE_NAME = @table)";

        private static string columnSql = @"
                select *, cast(COLUMNPROPERTY(object_id(TABLE_SCHEMA+'.'+TABLE_NAME), COLUMN_NAME, 'IsIdentity') as bit) as [IsIdentity]
                from INFORMATION_SCHEMA.COLUMNS 
                where table_name = @table 
                and TABLE_SCHEMA = @schema
                order by ORDINAL_POSITION";

        private static string primaryKeySql = @"
                SELECT tc.CONSTRAINT_NAME, ku.COLUMN_NAME
                FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS tc
                JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS ku ON tc.CONSTRAINT_TYPE = 'PRIMARY KEY' AND tc.CONSTRAINT_NAME = ku.CONSTRAINT_NAME
                where tc.table_name = @table 
                and tc.TABLE_SCHEMA = @schema
                order by ku.ORDINAL_POSITION";

        private static string tableSql = @"select TABLE_SCHEMA, TABLE_NAME
                                from INFORMATION_SCHEMA.TABLES
                                where TABLE_NAME NOT LIKE '%_AUDIT'
                                order by TABLE_SCHEMA, TABLE_NAME";

        private static string listDBSql = @"
            select name from sys.databases where state = 0
        ";

        private static string currentDbNameSql = @"
            select DB_NAME() as Name
        ";

        #endregion

        public static string ForeignKeyColumnSql { get { return foreignKeyColumnSql; }  }

        public static string ForeignKeySql { get { return foreignKeySql; } }

        public static string ColumnSql { get { return columnSql; } }
        public static string PrimaryKeySql { get { return primaryKeySql; }  }

        public static string TableSql { get { return tableSql; } }

        public static string ListDBSql { get { return listDBSql; } }

        public static string CurrentDbNameSql { get { return currentDbNameSql; } }

    }
}
