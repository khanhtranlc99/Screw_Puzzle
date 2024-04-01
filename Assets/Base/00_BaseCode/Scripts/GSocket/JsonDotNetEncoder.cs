using System.Collections.Generic;
using BestHTTP.JSON;
using BestHTTP.SocketIO.JsonEncoders;
using Newtonsoft.Json;

public sealed class JsonDotNetEncoder : IJsonEncoder
{
    public List<object> Decode(string json)
    {
        return JsonConvert.DeserializeObject<List<object>>(json);
    }

    public string Encode(List<object> obj)
    {
        return JsonConvert.SerializeObject(obj);
    }
}

