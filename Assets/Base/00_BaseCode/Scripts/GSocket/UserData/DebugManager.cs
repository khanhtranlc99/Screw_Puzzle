using System;
using UnityEngine;

public class DebugManager
{
    public static void LogError(string msg)
    {
#if G_TEST
        Debug.LogError(msg);
#endif
    }

    public static void Log(string tag, string msg)
    {
#if G_TEST
        Debug.Log(tag + ": " + msg);
#endif
    }

    public static void Log(string msg)
    {
#if G_TEST
        Debug.Log(msg);
#endif
    }

    public static void LogErrorFormat(string msg, params object[] args)
    {
#if G_TEST
        Debug.LogErrorFormat(msg, args);
#endif
    }
}
