using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

namespace SqlGen
{

    public class FkModel
    {
        public string TableName { get; set; }
        public Column FKey { get; set; }
        public List<Column> columns { get; set; }
    }

    public static class ForegnKeyHelper
    {
      

        public static List<FkModel> ToForegnTableColumns(this List<ForeignKey> foreignKeys)
        {

           

            List<FkModel> fkModel = new List<FkModel>();
            //Not an optimal solution but for now I don't have a time for this
            if (foreignKeys == null)
                return fkModel;

            foreach (ForeignKey fk in foreignKeys)
            {
               
                foreach (Column item in fk)
                {
                    string tableName = item.ReferancedTableName;
                    List<Column> columns = new List<Column>();
                    string targetColumnName = item.ReferancedColumnName;
                    Column fkey = null;
                    
                    foreach (var rCoumns in item.ForegnReferanceTable.Columns)
                    {
                        rCoumns.IsIdentity = !item.ForegnReferanceTable.InsertableColumns.Contains(rCoumns);
                        if (targetColumnName == rCoumns.ColumnName)
                            fkey = rCoumns;
                        columns.Add(rCoumns);
                        
                    }

                    fkModel.Add(new FkModel() { TableName = tableName, columns = columns, FKey = fkey});
                    
                }
            }

            return fkModel;
        }
    }
}
