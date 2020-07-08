//using SqlGen.Templeates;
using SqlGen.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlGen.Generators
{
    public class RestSharpGenerator : Generator
    {
        public override string Generate(Table table, GeneratorOptions options)
        {


            var restSharpClient = new RestSharpTemplates();

            restSharpClient.Session = new Dictionary<string, object>();
            restSharpClient.Session.Add("columns", table.InsertableColumns);
            restSharpClient.Initialize();

            return restSharpClient.TransformText();
            

        }

        public override string ToString() => "RestSharp Client Generator";
        
    }
}
