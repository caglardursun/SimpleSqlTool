using SqlGen.Templeates;
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
            
            var sft = new EntityTempleates();   
            sft.Session = new Dictionary<string, object>();
            sft.Session.Add("_namespace", "PenMail");
            sft.Session.Add("table", table);
            sft.Session.Add("tableName", table.TableName.ToPascalCase());
            sft.Session.Add("tableNameToLower", table.TableName);
            
            var columns = table.Columns.Where(c => !c.IsRowVersion() && (options.Audit || !c.IsAuditColumn()));
            sft.Session.Add("columns", columns);
            sft.Initialize();

            string test = "DatTransferObject".ToUnderScoredCase();

            return sft.TransformText();
        }



        public override string ToString() => "Data Entity";
    }
}
