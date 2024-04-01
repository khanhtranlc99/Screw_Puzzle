using System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Assembly, AllowMultiple = true)]
public class MessageAttribute : Attribute
{
    public string Name        { set; get; }
    public bool   Log         { get; set; }
    public string Route { get; set; }

    public MessageAttribute(string name, string Route = "msg", bool log = true)
    {
        this.Name = name;
        Log = log;
        this.Route = Route;
    }
}