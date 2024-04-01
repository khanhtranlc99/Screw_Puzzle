using System;
[Serializable]
public class MessageData : MessageBase
{
    public Object Body { get; set; }

    public MessageData()
    { }

    public MessageData(MessageBase messageBody)
    {
        Body = messageBody;
        Name = messageBody.Name;
        Route = messageBody.Route;
    }
}

