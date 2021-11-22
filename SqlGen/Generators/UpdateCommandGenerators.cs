using SqlGen.Templates;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlGen.Generators
{
    public class UpdateCommandGenerators : Generator
    {
        public override string ToString() => "Update Command Handler Generator";

        public override string Generate(Table table, GeneratorOptions options)
        {
            try
            {

                var template = new UpdateCommandHandlerTemplates();
                template.Session = new Dictionary<string, object>();

                var fk = table.ForeignKeys.ToForegnTableColumns();
                template.Session.Add("foregnkeys", fk);

                template.Session.Add("table", table);
                template.Session.Add("tableName", table.TableName);
                template.Session.Add("tableNameToPascal", table.TableName.ToPascalCase());
                template.Session.Add("_namespace", AppSettings.Instance.Namespace);
                template.Session.Add("tableNameToLower", $"{table.TableName.ElementAt(0).ToString().ToLower()}{table.TableName.Substring(1, table.TableName.Length - 1)}");

                template.Session.Add("columns", table.InsertableColumns);
                template.Initialize();
                return template.TransformText();



            }
            catch (Exception exc)
            {

                throw new Exception("Json generator error occured", exc.InnerException);
            }
        }
    }
}
