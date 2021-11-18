﻿//using SqlGen.Templeates;
using SqlGen.Templates;
using System.Collections.Generic;

namespace SqlGen.Generators
{
    class DataManagers : Generator
    {

        public override string Generate(Table table, GeneratorOptions options)
        {

            var template = new DataManagerTemplates();
            template.Session = new Dictionary<string, object>();
            template.Session.Add("_namespace", AppSettings.Instance.Namespace);
            template.Session.Add("table", table);
            template.Session.Add("tableName", table.TableName);
            template.Session.Add("columns", table.InsertableColumns);

            var fk = table.ForeignKeys.ToForegnTableColumns();
            template.Session.Add("foregnkeys", fk);



            template.Initialize();

            return template.TransformText();
            //return "";

        }



        public override string ToString() => "Data Managers";
    }
}
