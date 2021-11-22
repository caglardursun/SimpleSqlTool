using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlGen
{
    public static class ForegnKeyExtensions
    {
        public static List<FkModel> ToForegnTableColumns(this List<ForeignKey> foreignKeys)
        {

            List<FkModel> fkModel = new List<FkModel>();
            //Not an optimal solution but for now I don't have a time for this
            if (foreignKeys == null)
            {
                return fkModel;
            }

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
                        {
                            fkey = rCoumns;
                        }

                        columns.Add(rCoumns);

                    }

                    fkModel.Add(new FkModel() { TableName = tableName, columns = columns, FKey = fkey });

                }
            }

            return fkModel;
        }

        public static string ToLeftJoins(this List<ForeignKey> foreignKeys)
        {

            StringBuilder stringBuilder = new StringBuilder();
            foreach (ForeignKey fk in foreignKeys)
            {

                foreach (Column item in fk)
                {
                    string sourceColumnName = item.ColumnName;
                    string targetColumnName = item.ReferancedColumnName;
                    string targetTableName = item.ForegnReferanceTable.TableName;
                    stringBuilder.Append($"left join {targetTableName} on {sourceColumnName} = {targetColumnName}\n\t\t\t\t");
                }
            }



            return stringBuilder.ToString();
        }
    }
}
