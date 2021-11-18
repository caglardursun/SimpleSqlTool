//using SqlGen.Templeates;
using SqlGen.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlGen.Generators
{
    class CreateDataManagerInterfaces : Generator
    {
        public override string Generate(Table table, GeneratorOptions options)
        {

            var interfaceCreator = new DataManagerInterfaceTemplates();
            interfaceCreator.Session = new Dictionary<string, object>();
            interfaceCreator.Session.Add("_namespace", AppSettings.Instance.Namespace);
            interfaceCreator.Session.Add("table", table);
            interfaceCreator.Session.Add("schemaName", table.Schema);
            interfaceCreator.Session.Add("tableName", table.TableName);


            interfaceCreator.Initialize();


            return interfaceCreator.TransformText();
            //return "";
        }

        public override string ToString()=> "Data Manager Interface";
        
    }
}
