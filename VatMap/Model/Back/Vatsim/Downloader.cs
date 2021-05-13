using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using VatMap.Model.Back.Vatsim.JsonModel;

namespace VatMap.Model.Back.Vatsim
{
  public class Downloader
  {

    public Root Download(string url)
    {
      string s = this.Fetch(url);
      Root ret = this.LoadJson(s);

      return ret;
    }

    private Root LoadJson(string txt)
    {
      Root ret = JsonConvert.DeserializeObject<Root>(txt); 
      return ret;
    }

    private string Fetch(string url)
    {
      WebRequest request = WebRequest.Create(url);
      request.Method = "GET";
      WebResponse response = request.GetResponse();
      Stream stream = response.GetResponseStream();
      StreamReader reader = new StreamReader(stream);
      string ret = reader.ReadToEnd();
      reader.Close();
      response.Close();

      return ret;
    }

    class MyConverter : CustomCreationConverter<IDictionary<string, object>>
    {
      //from: https://stackoverflow.com/questions/6416017/json-net-deserializing-nested-dictionaries
      public override IDictionary<string, object> Create(Type objectType)
      {
        return new Dictionary<string, object>();
      }

      public override bool CanConvert(Type objectType)
      {
        // in addition to handling IDictionary<string, object>
        // we want to handle the deserialization of dict value
        // which is of type object
        return objectType == typeof(object) || base.CanConvert(objectType);
      }

      public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
      {
        if (reader.TokenType == JsonToken.StartObject
            || reader.TokenType == JsonToken.Null)
          return base.ReadJson(reader, objectType, existingValue, serializer);

        // if the next token is not an object
        // then fall back on standard deserializer (strings, numbers etc.)
        return serializer.Deserialize(reader);
      }
    }
  }
}
