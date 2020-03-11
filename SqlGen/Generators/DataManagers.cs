using SqlGen.Templeates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SqlGen.Generators
{
    class DataManagers : Generator
    {   
     
        public override string Generate(Table table, GeneratorOptions options)
        {

            DataManagerTempleates sft = new DataManagerTempleates();
            sft.Session = new Dictionary<string, object>();
            sft.Session.Add("_namespace","PenMail");
            sft.Session.Add("table", table);
            sft.Session.Add("tableName", table.TableName);
            sft.Session.Add("tableNameToLower", table.TableName);
            sft.Session.Add("tableNameToPascal", table.TableName.ToPascalCase());
            sft.Session.Add("columns", table.InsertableColumns);
            
            sft.Initialize();            

            return sft.TransformText();                        
        }



        public override string ToString() => "Data Managers";
    }
}
