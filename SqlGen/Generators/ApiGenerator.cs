//using SqlGen.Templeates;
using SqlGen.Templates;
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

            APITemplates template = new APITemplates();

            template.Session = new Dictionary<string, object>();
            template.Session.Add("_namespace", AppSettings.Instance.Namespace);
            template.Session.Add("tableName", table.TableName);
            template.Session.Add("options", options);

            template.Session.Add("tableNameToLower", $"{table.TableName.ElementAt(0).ToString().ToLower()}{table.TableName.Substring(1, table.TableName.Length - 1)}");
            template.Session.Add("tableNameToPascal", table.TableName.ToPascalCase());
            template.Session.Add("table", table);            
            template.Session.Add("columns", table.InsertableColumns);
            template.Initialize();

            return template.TransformText();

            

        }

        public override string ToString() => "WebAPI Generator";        
    }
}
