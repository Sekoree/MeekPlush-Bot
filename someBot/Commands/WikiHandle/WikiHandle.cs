// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using QuickType;
//
//    var wikiHandle = WikiHandle.FromJson(jsonString);

namespace QuickType
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class WikiHandle
    {
        [JsonProperty("sections")]
        public Section[] Sections { get; set; }
    }

    public partial class Section
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("level")]
        public long Level { get; set; }

        [JsonProperty("content")]
        public Content[] Content { get; set; }

        [JsonProperty("images")]
        public Dictionary<string, string>[] Images { get; set; }
    }

    public partial class Content
    {
        [JsonProperty("type")]
        public TypeEnum Type { get; set; }

        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        [JsonProperty("elements", NullValueHandling = NullValueHandling.Ignore)]
        public Element[] Elements { get; set; }
    }

    public partial class Element
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("elements")]
        public Element[] Elements { get; set; }
    }

    public enum TypeEnum { List, Paragraph };

    public partial class WikiHandle
    {
        public static WikiHandle FromJson(string json) => JsonConvert.DeserializeObject<WikiHandle>(json, QuickType.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this WikiHandle self) => JsonConvert.SerializeObject(self, QuickType.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                TypeEnumConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class TypeEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(TypeEnum) || t == typeof(TypeEnum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "list":
                    return TypeEnum.List;
                case "paragraph":
                    return TypeEnum.Paragraph;
            }
            throw new Exception("Cannot unmarshal type TypeEnum");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (TypeEnum)untypedValue;
            switch (value)
            {
                case TypeEnum.List:
                    serializer.Serialize(writer, "list");
                    return;
                case TypeEnum.Paragraph:
                    serializer.Serialize(writer, "paragraph");
                    return;
            }
            throw new Exception("Cannot marshal type TypeEnum");
        }

        public static readonly TypeEnumConverter Singleton = new TypeEnumConverter();
    }
}

