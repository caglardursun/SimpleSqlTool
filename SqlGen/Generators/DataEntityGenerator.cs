using SqlGen.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlGen.Generators
{
    class DataEntityGenerator : Generator
    {
        public override string Generate(Table table, GeneratorOptions options)
        {

            var dataEntity = new EntityTemplates();
            dataEntity.Session = new Dictionary<string, object>();
            dataEntity.Session.Add("_namespace", "NestPayment");
            dataEntity.Session.Add("table", table);
            dataEntity.Session.Add("schemaName", table.Schema);
            dataEntity.Session.Add("tableName", table.TableName);
           
            
            var fk = table.ForeignKeys.ToForegnTableColumns();

            dataEntity.Session.Add("foregnkeys", fk);

            var columns = table.Columns.Where(c => !c.IsRowVersion() && (options.Audit || !c.IsAuditColumn()));
            dataEntity.Session.Add("columns", columns);
            dataEntity.Initialize();


            return dataEntity.TransformText();

            //return "";
        }



        public override string ToString() => "Data Entity";
    }
}
