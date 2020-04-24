//using SqlGen.Templeates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlGen.Generators
{
    class ApiGenerator : Generator
    {
        public override string Generate(Table table, GeneratorOptions options)
        {

            // APITempleate apitempleate = new APITempleate();

            // apitempleate.Session = new Dictionary<string, object>();
            // apitempleate.Session.Add("_namespace", "PenMail");
            // apitempleate.Session.Add("table", table);
            // apitempleate.Session.Add("tableName", table.TableName);
            // apitempleate.Session.Add("tableNameToLower", table.TableName);
            // apitempleate.Session.Add("tableNameToPascal", table.TableName.ToPascalCase());
            // apitempleate.Session.Add("columns", table.InsertableColumns);            
            // apitempleate.Initialize();

            // return apitempleate.TransformText();
            
            return "";

        }

        public override string ToString() => "REST API Generator";        
    }
}
