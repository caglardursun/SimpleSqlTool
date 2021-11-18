//using SqlGen.Templeates;
using SqlGen.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SqlGen.Generators
{
    class DataManagers : Generator
    {   
     
        public override string Generate(Table table, GeneratorOptions options)
        {

            var dMT = new DataManagerTemplates();
            dMT.Session = new Dictionary<string, object>();
            dMT.Session.Add("_namespace", AppSettings.Instance.Namespace);
            dMT.Session.Add("table", table);
            dMT.Session.Add("tableName", table.TableName);
            dMT.Session.Add("columns", table.InsertableColumns);

            var fk = table.ForeignKeys.ToForegnTableColumns();
            dMT.Session.Add("foregnkeys", fk);



            dMT.Initialize();

            return dMT.TransformText();
            //return "";

        }



        public override string ToString() => "Data Managers";
    }
}
