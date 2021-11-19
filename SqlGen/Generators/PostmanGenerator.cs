using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using SqlGen.Models;

namespace SqlGen.Generators
{
    public class PostmanGenerator : Generator
    {
        

        public override string Generate(Table table, GeneratorOptions options)
        {
            PostmanCollection collection = new PostmanCollection();
            collection.Info = new Info();
            collection.Info.PostmanId = Guid.NewGuid();
            collection.Info.Name = $"{table.TableName} Collection";
            collection.Info.Schema = new Uri("https://schema.getpostman.com/json/collection/v2.1.0/collection.json");


            #region GetList

            Item getListItem = new Item();
            getListItem.Name = "List";
            getListItem.ProtocolProfileBehavior = new ProtocolProfileBehavior() { DisableBodyPruning = true };
            var request = new Request();
            request.Method = "GET";
            request.Header = new object[] { };


            var body = new Body();
            body.Mode = "raw";
            body.Raw = "";
            body.Options = new Options() { Raw = new Raw() { Language = "text" } };
            request.Body = body;

            var url = new Url();
            url.Raw = string.Format(@"{{url}}/api/{0}/Get{0}List", table.TableName);
            url.Host = new string[] { "{{url}}" };
            url.Path = new string[] { "api", $"{table.TableName}", $"Get{table.TableName}List" };
            request.Url = url;

            getListItem.Request = request;

            #endregion

            #region GetById

            Item getByIdItem = new Item();

            getByIdItem.Name = $"Get{table.TableName}ById";
            getByIdItem.ProtocolProfileBehavior = new ProtocolProfileBehavior() { DisableBodyPruning = true };
            var getByIdrequest = new Request();
            getByIdrequest.Method = "GET";
            getByIdrequest.Header = new object[] { };


            var getByIdBody = new Body();
            getByIdBody.Mode = "raw";
            getByIdBody.Raw = "";
            getByIdBody.Options = new Options() { Raw = new Raw() { Language = "text" } };
            getByIdrequest.Body = body;

            var getByIdUrl = new Url();
            getByIdUrl.Raw = string.Format(@"{{url}}/api/{0}/Get{0}ById", table.TableName);
            getByIdUrl.Host = new string[] { "{{url}}" };
            getByIdUrl.Path = new string[] { "api", $"{table.TableName}", $"Get{table.TableName}ById" };
            getByIdrequest.Url = getByIdUrl;

            #endregion

            #region Create

            Item CreateItem = new Item();

            CreateItem.Name = $"Create{table.TableName}";
            CreateItem.ProtocolProfileBehavior = new ProtocolProfileBehavior() { DisableBodyPruning = true };
            var createRequest = new Request();
            createRequest.Method = "POST";
            createRequest.Header = new object[] { };


            var createBody = new Body();
            createBody.Mode = "raw";
            createBody.Raw = ""; //json generate edilebilir ...
            createBody.Options = new Options() { Raw = new Raw() { Language = "text" } };
            createRequest.Body = body;

            var createUrl = new Url();
            createUrl.Raw = string.Format(@"{{url}}/api/{0}/Cretae{0}", table.TableName);
            createUrl.Host = new string[] { "{{url}}" };
            createUrl.Path = new string[] { "api", $"{table.TableName}", $"Cretae{table.TableName}" };
            createRequest.Url = createUrl;

            #endregion

            #region Update

            #endregion

            #region Delete


            #endregion



            collection.Item = new Item[] { getListItem, getByIdItem, CreateItem };

            

            var postman_event = new PostmanEvent();
            postman_event.Listen = "prerequest";
            postman_event.Script = new Script() { Type = "text/javascript", Exec = new string[] { } };
            collection.Event = new PostmanEvent[] { postman_event };

            Variable variable = new Variable();
            variable.Key = "url";
            variable.Value = new Uri("https://localhost:44390");
            collection.Variable = new Variable[] { variable };


            return collection.ToJson();
        }

        public override string ToString() => "Postman Generator";
    }
}
