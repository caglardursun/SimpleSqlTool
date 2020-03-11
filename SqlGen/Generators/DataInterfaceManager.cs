using SqlGen.Templeates;
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
            
            var interfaceCreator = new DataManagerInterfaceTempleates();
            interfaceCreator.Session = new Dictionary<string, object>();
            interfaceCreator.Session.Add("_namespace", "PenMail");
            interfaceCreator.Session.Add("tableName", table.TableName.ToPascalCase());            
            interfaceCreator.Session.Add("columns", table.Columns);

           
            interfaceCreator.Initialize();
            

            return interfaceCreator.TransformText();
        }

        public override string ToString()=> "Data Manager Interface";
        
    }
}
