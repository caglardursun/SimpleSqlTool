
//using SqlGen.Templeates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SqlGen.Generators
{
    public class JSonGenerator : Generator
    {
        public override string Generate(Table table, GeneratorOptions options)
        {

            try
            {

                // var json = new JSonTempates();
                // json.Session = new Dictionary<string, object>();

                // var fk = table.ForeignKeys.ToForegnTableColumns();
                // json.Session.Add("foregnkeys", fk);
                

                // json.Session.Add("columns", table.InsertableColumns);
                // json.Initialize();
                // return json.TransformText();

                return "";
                
            }
            catch (Exception exc)
            {

                throw new Exception("Json generator error occured", exc.InnerException);
            }
        }

        public override string ToString() => "JSon Generator";        
    }
}
