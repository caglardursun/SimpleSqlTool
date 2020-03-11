using SqlGen.Templeates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SqlGen.Generators
{
    class DataTransferObjects : Generator
    {
        public override string Generate(Table table, GeneratorOptions options)
        {


            var tdo = new DataTransferObjectTempleates();
            tdo.Session = new Dictionary<string, object>();
            tdo.Session.Add("_namespace", "PenMail");

          
            

            tdo.Session.Add("tableName", table.TableName.ToPascalCase());
            tdo.Session.Add("tableNameToLower", table.TableName.ToPascalCase().ToLower());
            tdo.Session.Add("columns", table.InsertableColumns);
            tdo.Session.Add("options", options);
            tdo.Session.Add("table", table);

            tdo.Initialize();

            return tdo.TransformText();

        }

        public override string ToString() => "Data Transfer Object";        
    }
}
