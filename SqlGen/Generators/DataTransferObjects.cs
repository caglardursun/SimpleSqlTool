//using SqlGen.Templeates;
using SqlGen.Templates;
using System.Collections.Generic;

namespace SqlGen.Generators
{
    class DataTransferObjects : Generator
    {
        public override string Generate(Table table, GeneratorOptions options)
        {


            var template = new DataTransferObjectTemplates();
            template.Session = new Dictionary<string, object>();
            template.Session.Add("_namespace", AppSettings.Instance.Namespace);

            template.Session.Add("tableName", table.TableName);
            template.Session.Add("columns", table.InsertableColumns);
            template.Session.Add("options", options);
            template.Session.Add("table", table);

            template.Initialize();

            return template.TransformText();


        }

        public override string ToString() => "Data Transfer Object";
    }
}
