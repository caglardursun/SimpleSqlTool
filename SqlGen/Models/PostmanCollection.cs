using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;

namespace PostmanCollections
{
    public partial class PostmanCollection
    {
        [JsonProperty("info")]
        public Info Info { get; set; }

        [JsonProperty("item")]
        public Item[] Item { get; set; }

        [JsonProperty("event")]
        public Event[] Event { get; set; }

        [JsonProperty("variable")]
        public Variable[] Variable { get; set; }
    }

    public partial class Event
    {
        [JsonProperty("listen")]
        public string Listen { get; set; }

        [JsonProperty("script")]
        public Script Script { get; set; }
    }

    public partial class Script
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("exec")]
        public string[] Exec { get; set; }
    }

    public partial class Info
    {
        [JsonProperty("_postman_id")]
        public Guid PostmanId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("schema")]
        public Uri Schema { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("protocolProfileBehavior", NullValueHandling = NullValueHandling.Ignore)]
        public ProtocolProfileBehavior ProtocolProfileBehavior { get; set; }

        [JsonProperty("request")]
        public Request Request { get; set; }

        [JsonProperty("response")]
        public object[] Response { get; set; }
    }

    public partial class ProtocolProfileBehavior
    {
        [JsonProperty("disableBodyPruning")]
        public bool DisableBodyPruning { get; set; }
    }

    public partial class Request
    {
        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("header")]
        public object[] Header { get; set; }

        [JsonProperty("body", NullValueHandling = NullValueHandling.Ignore)]
        public Body Body { get; set; }

        [JsonProperty("url")]
        public Url Url { get; set; }
    }

    public partial class Body
    {
        [JsonProperty("mode")]
        public string Mode { get; set; }

        [JsonProperty("raw")]
        public string Raw { get; set; }

        [JsonProperty("options")]
        public Options Options { get; set; }
    }

    public partial class Options
    {
        [JsonProperty("raw")]
        public Raw Raw { get; set; }
    }

    public partial class Raw
    {
        [JsonProperty("language")]
        public string Language { get; set; }
    }

    public partial class Url
    {
        [JsonProperty("raw")]
        public string Raw { get; set; }

        [JsonProperty("host")]
        public string[] Host { get; set; }

        [JsonProperty("path")]
        public string[] Path { get; set; }

        [JsonProperty("protocol", NullValueHandling = NullValueHandling.Ignore)]
        public string Protocol { get; set; }

        [JsonProperty("port", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ParseStringConverter))]
        public long? Port { get; set; }
    }

    public partial class Variable
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public Uri Value { get; set; }
    }

    public partial class PostmanCollection
    {
        public static PostmanCollection FromJson(string json) => JsonConvert.DeserializeObject<PostmanCollection>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this PostmanCollection self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}

