using SqlGen.Templates;
using System.Collections.Generic;
using System.Linq;

namespace SqlGen.Generators
{
    class DataEntityGenerator : Generator
    {
        public override string Generate(Table table, GeneratorOptions options)
        {

            var template = new EntityTemplates();
            template.Session = new Dictionary<string, object>();

            template.Session.Add("_namespace", AppSettings.Instance.Namespace);
            template.Session.Add("tableName", table.TableName);

            template.Session.Add("table", table);
            template.Session.Add("options", options);
            template.Session.Add("tableNameToLower", $"{table.TableName.ElementAt(0).ToString().ToLower()}{table.TableName.Substring(1, table.TableName.Length - 1)}");
            template.Session.Add("tableNameToPascal", table.TableName.ToPascalCase());

            
            var fk = table.ForeignKeys.ToForegnTableColumns();

            template.Session.Add("foregnkeys", fk);

            var columns = table.Columns.Where(c => !c.IsRowVersion() && (options.Audit || !c.IsAuditColumn()));
            template.Session.Add("columns", columns);
            template.Initialize();


            return template.TransformText();

            //return "";
        }



        public override string ToString() => "Data Entity Generator";
    }
}
