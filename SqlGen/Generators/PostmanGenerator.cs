using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using SqlGen.Models;
using SqlGen.Templates;

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

            Item GetListItem = new Item();
            GetListItem.Name = $"Get{table.TableName}List";
            GetListItem.ProtocolProfileBehavior = new ProtocolProfileBehavior() { DisableBodyPruning = true };
            var request = new Request();
            request.Method = "GET";
            request.Header = new Header[] { };


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

            GetListItem.Request = request;

            #endregion

            #region GetById

            Item GetByIdItem = new Item();

            GetByIdItem.Name = $"Get{table.TableName}ById";
            GetByIdItem.ProtocolProfileBehavior = new ProtocolProfileBehavior() { DisableBodyPruning = true };
            var getByIdrequest = new Request();
            getByIdrequest.Method = "GET";
            getByIdrequest.Header = new Header[] { };


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

            GetByIdItem.Request = getByIdrequest;
            #endregion

            #region Create

            Item CreateItem = new Item();

            CreateItem.Name = $"Create{table.TableName}";
            CreateItem.ProtocolProfileBehavior = new ProtocolProfileBehavior() { DisableBodyPruning = true };

            var createRequest = new Request();
            createRequest.Method = "POST";
            createRequest.Header = new Header[] { };


            var createBody = new Body();
            createBody.Mode = "raw";

            #region json compile for body

            var jsontemplate = new JSonTemplates();
            var json = new JSonTemplates();
            json.Session = new Dictionary<string, object>();
            var fk = table.ForeignKeys.ToForegnTableColumns();
            json.Session.Add("foregnkeys", fk);
            json.Session.Add("columns", table.InsertableColumns);
            json.Initialize();
            createBody.Raw = json.TransformText();

            #endregion

            createBody.Options = new Options() { Raw = new Raw() { Language = "text" } };
            createRequest.Body = createBody;

            var createUrl = new Url();
            createUrl.Raw = string.Format(@"{{url}}/api/{0}/Create{0}", table.TableName);
            createUrl.Host = new string[] { "{{url}}" };
            createUrl.Path = new string[] { "api", $"{table.TableName}", $"Cretae{table.TableName}" };
            createRequest.Url = createUrl;

            CreateItem.Request = createRequest;

            #endregion

            #region Update

            Item UpdateItem = new Item();

            UpdateItem.Name = $"Update{table.TableName}";
            UpdateItem.ProtocolProfileBehavior = new ProtocolProfileBehavior() { DisableBodyPruning = true };

            var updateRequest = new Request();
            updateRequest.Method = "PUT";
            updateRequest.Header = new Header[] { };


            var updateBody = new Body();
            updateBody.Mode = "raw";

            #region json compile for body

            updateBody.Raw = json.TransformText();

            #endregion

            updateBody.Options = new Options() { Raw = new Raw() { Language = "text" } };
            updateRequest.Body = updateBody;

            var updateUrl = new Url();
            updateUrl.Raw = string.Format(@"{{url}}/api/{0}/Update{0}", table.TableName);
            updateUrl.Host = new string[] { "{{url}}" };
            updateUrl.Path = new string[] { "api", $"{table.TableName}", $"Update{table.TableName}" };
            updateRequest.Url = updateUrl;

            UpdateItem.Request = updateRequest;

            #endregion

            #region Delete

            Item DeleteItem = new Item();

            DeleteItem.Name = $"Delete{table.TableName}";
            DeleteItem.ProtocolProfileBehavior = new ProtocolProfileBehavior() { DisableBodyPruning = true };
            var deleteRequest = new Request();
            deleteRequest.Method = "DELETE";
            deleteRequest.Header = new Header[] { };


            var deleteBody = new Body();
            deleteBody.Mode = "raw";
            deleteBody.Raw = "";
            deleteBody.Options = new Options() { Raw = new Raw() { Language = "text" } };
            

            var deleteUrl = new Url();
            deleteUrl.Raw =  string.Format(@"{{url}}/api/{0}/Delete{0}", table.TableName);
            deleteUrl.Host = new string[] { "{{url}}" };
            deleteUrl.Path = new string[] { "api", $"{table.TableName}", $"Delete{table.TableName}" };
            deleteRequest.Url = getByIdUrl;

            GetByIdItem.Request = getByIdrequest;
            #endregion



            collection.Item = new Item[] { GetListItem, GetByIdItem, CreateItem, UpdateItem,DeleteItem };



            var postman_event = new PostmanEvent();
            postman_event.Listen = "prerequest";
            postman_event.Script = new Script() { Type = "text/javascript", Exec = new string[] { } };
            collection.Event = new PostmanEvent[] { postman_event };

            Variable variable = new Variable();
            variable.Key = "url";
            variable.Value = "https://localhost:44390";
            collection.Variable = new Variable[] { variable };


            return collection.ToJson();
        }

        public override string ToString() => "Postman Generator";
    }
}
