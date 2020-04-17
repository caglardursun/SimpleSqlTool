using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlGen
{

    public class FkModel
    {
        public string TableName { get; set; }
        public List<Column> columns { get; set; }
    }

    public static class ForegnKeyHelper
    {
        public static List<FkModel> ToForegnTableColumns(this List<ForeignKey> foreignKeys)
        {
            List<FkModel> fkModel = new List<FkModel>();
            

            foreach (var fk in foreignKeys)
            {                
                foreach (var item in fk)
                {
                    string tableName = item.ReferancedTableName;
                    List<Column> columns = new List<Column>();
                    foreach (var rCoumns in item.ForegnReferanceTable.Columns)
                    {
                        columns.Add(rCoumns);
                    }
                    fkModel.Add(new FkModel() { TableName = tableName, columns = columns });

                }
            }

            return fkModel;
        }
    }
}
