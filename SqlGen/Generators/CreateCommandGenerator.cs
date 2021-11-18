using SqlGen.Templates;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlGen.Generators
{
    public class CreateCommandGenerator : Generator
    {
        public override string Generate(Table table, GeneratorOptions options)
        {

            try
            {

                var template = new CreateCommandHandlerTemplates();
                template.Session = new Dictionary<string, object>();

                var fk = table.ForeignKeys.ToForegnTableColumns();
                template.Session.Add("foregnkeys", fk);

                template.Session.Add("table", table);

                template.Session.Add("_namespace", AppSettings.Instance.Namespace);
                template.Session.Add("tableName", table.TableName);


                template.Session.Add("tableNameToLower", $"{table.TableName.ElementAt(0).ToString().ToLower()}{table.TableName.Substring(1, table.TableName.Length - 1)}");
                template.Session.Add("tableNameToPascal", table.TableName.ToPascalCase());

                template.Session.Add("columns", table.InsertableColumns);
                template.Initialize();
                return template.TransformText();



            }
            catch (Exception)
            {

                throw new Exception($"{this} generator error occured");
            }

        }

        public override string ToString() => "Create Command Handler Generator";
    }
}
