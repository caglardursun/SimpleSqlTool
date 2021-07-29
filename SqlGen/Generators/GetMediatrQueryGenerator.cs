﻿using SqlGen.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlGen.Generators
{
    public class GetMediatrQueryGenerator : Generator
    {

        public override string ToString() => "Mediatr Get";

        public override string Generate(Table table, GeneratorOptions options)
        {
            try
            {

                var template = new GetMediatRQueryTemplates();
                template.Session = new Dictionary<string, object>();

                var fk = table.ForeignKeys.ToForegnTableColumns();
                template.Session.Add("foregnkeys", fk);

                template.Session.Add("table", table);
                template.Session.Add("tableName", table.TableName);
                template.Session.Add("tableNameToLower", table.TableName.ToLower());
                template.Session.Add("tableNameToPascal", table.TableName.ToPascalCase());

                template.Session.Add("columns", table.InsertableColumns);
                template.Initialize();
                return template.TransformText();


                throw new Exception($"{this} generator error occured");
            }
            catch (Exception exc)
            {

                throw new Exception("Json generator error occured", exc.InnerException);
            }
        }

        
    }
}
