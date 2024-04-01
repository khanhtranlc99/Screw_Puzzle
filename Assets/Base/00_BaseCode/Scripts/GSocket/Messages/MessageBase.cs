using System;

[Serializable]
public class MessageBase 
{
    public string Name { get; set; }
    public bool Log { get; set; }
    public string Route { get; set; }

    public MessageBase()
    {
        Route = "msg";
        object[] attrs = GetType().GetCustomAttributes(typeof(MessageAttribute), false);
        foreach (object attr in attrs)
        {
            var message = (MessageAttribute)attr;
            Name = message.Name;
            Log = message.Log;
            Route = message.Route;
            return;
        }
    }
}

