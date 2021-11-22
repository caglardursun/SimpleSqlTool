using SqlGen.Templates;
using System.Collections.Generic;


namespace SqlGen.Generators
{
    public class ViewGenerator : Generator
    {
        public override string Generate(Table table, GeneratorOptions options)
        {
            //throw new NotImplementedException();

            var template = new ViewTemplates();
            template.Session = new Dictionary<string, object>();

            var fk = table.ForeignKeys.ToForegnTableColumns();
            template.Session.Add("foregnkeys", fk);

            template.Session.Add("table", table);
            template.Session.Add("tableName", table.TableName);
            template.Session.Add("tableNameToLower", table.TableName.ToLower());
            template.Session.Add("tableNameToPascal", table.TableName.ToPascalCase());
            template.Session.Add("options", options);
            template.Session.Add("columns", table.InsertableColumns);
            template.Initialize();

            return template.TransformText();
        }

        public override string ToString() => "View Generator";
    }
}
