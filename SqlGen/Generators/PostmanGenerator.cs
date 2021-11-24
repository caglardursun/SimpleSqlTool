using SqlGen.Models;
using SqlGen.Templates;
using System;
using System.Collections.Generic;

namespace SqlGen.Generators
{
    public enum CRUDType
    {
        GetList,
        GetById,
        Create,
        Update,
        Delete
    }
    public class PostmanItemCreator
    {
        private static PostmanItemCreator _instance;
        public static PostmanItemCreator Instance => (_instance == null ? _instance = new PostmanItemCreator() : _instance);

        public PostmanItemCreator()
        {
        }
        public Item CreateItem(CRUDType methodType, Table table)
        {
            Item item = new Item();
            item.ProtocolProfileBehavior = new ProtocolProfileBehavior() { DisableBodyPruning = true };
            var request = new Request();
            //key token stuff 4 auth & authenticaton
            request.Header = new Header[] { };

            var url = new Url();
            var body = new Body();
            body.Mode = "raw";
            body.Raw = "";
            body.Options = new Options() { Raw = new Raw() { Language = "json" } };

            url.Host = new string[] { @"{{url}}" };
            url.Path = new string[3] { "api", $"{table.TableName}", "" };

            //create json body 4 update & 
            var json = new JSonTemplates();
            json.Session = new Dictionary<string, object>();
            var fk = table.ForeignKeys.ToForegnTableColumns();
            json.Session.Add("foregnkeys", fk);
            json.Session.Add("columns", table.InsertableColumns);
            json.Initialize();

            switch (methodType)
            {
                case CRUDType.GetList:
                    item.Name = $"Get{table.TableName}List";
                    request.Method = "GET";
                    url.Raw = $"{url.Host[0]}/api/{table.TableName}/Get{table.TableName}List";                    
                    url.Path[2] = $"Get{table.TableName}List";                                       
                    break;
                case CRUDType.GetById:
                    item.Name = $"Get{table.TableName}ById";
                    url.Raw = $"{url.Host[0]}/api/{table.TableName}/Get{table.TableName}ById";                    
                    url.Path[2] = $"Get{table.TableName}ById";
                    request.Method = "GET";                    
                    break;
                case CRUDType.Create:
                    item.Name = $"Create{table.TableName}";                    
                    url.Raw = $"{url.Host[0]}/api/{table.TableName}/Create{table.TableName}";                    
                    url.Path[2] = $"Create{table.TableName}";
                    body.Raw = json.TransformText();
                    request.Method = "POST";                                        
                    break;
                case CRUDType.Update:
                    item.Name = $"Update{table.TableName}";
                    url.Raw = $"{url.Host[0]}/api/{table.TableName}/Update{table.TableName}";
                    url.Path[2] = $"Update{table.TableName}";
                    body.Raw = json.TransformText();
                    request.Method = "PUT";                    
                    break;
                case CRUDType.Delete:
                    item.Name = $"Delete{table.TableName}";
                    url.Raw = $"{url}/api/{table.TableName}/Delete{table.TableName}";
                    url.Path[2] = $"Delete{table.TableName}";
                    request.Method = "DELETE";
                    break;
   
            }
            request.Body = body;
            request.Url = url;
            item.Request = request;
            return item;
        }
    }

    public class PostmanGenerator : Generator
    {


        public override string Generate(Table table, GeneratorOptions options)
        {
            PostmanCollection collection = new PostmanCollection();
            collection.Info = new Info();
            collection.Info.PostmanId = Guid.NewGuid();
            collection.Info.Name = $"{table.TableName} Collection";
            collection.Info.Schema = new Uri("https://schema.getpostman.com/json/collection/v2.1.0/collection.json");

            var creator = PostmanItemCreator.Instance;
            List<Item> items = new List<Item>();
            foreach (CRUDType c in Enum.GetValues(typeof(CRUDType)))
            {
                items.Add(creator.CreateItem(c, table));
            }

            collection.Item = items.ToArray();

            var postman_event = new PostmanEvent();
            postman_event.Listen = "prerequest";
            postman_event.Script = new Script() { Type = "text/javascript", Exec = new string[] { } };
            collection.Event = new PostmanEvent[] { postman_event };

            Variable variable = new Variable();
            variable.Key = "url";
            variable.Value = "https://localhost:44343";
            collection.Variable = new Variable[] { variable };


            return collection.ToJson();
        }

        public override string ToString() => "Postman Generator";
    }
}
