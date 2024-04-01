using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public enum GErrorCode
{
    Unknown = 1,
    Success = 0,
    UnknownSource = 2,
    InvalidBuild = 1001,
    AccountBanned = 1002,
    MultiDeviceOnline = 1003,
    ServerDown = -1,
    FACEBOOK_ALREADY_LINK = 1004,
    CheckInternet = 1005,
    Logining = 1006,
    NotLogin = 1007,
    UpdateRequire = 1008
}

[Serializable]
public class GError
{

    //public int HttpCode;
    //public string HttpStatus;
    public GErrorCode Error;
    public string ErrorMessage;
    public bool ClearData = false;
    public Dictionary<string, List<string>> ErrorDetails;
    public object CustomData;

    public override string ToString()
    {
        var sb = new System.Text.StringBuilder();
        if (ErrorDetails != null)
        {
            foreach (var kv in ErrorDetails)
            {
                sb.Append(kv.Key);
                sb.Append(": ");
                sb.Append(string.Join(", ", kv.Value.ToArray()));
                sb.Append(" | ");
            }
        }
        return string.Format("RocketError({0}, {1}", Error, ErrorMessage) + (sb.Length > 0 ? " - Details: " + sb.ToString() + ")" : ")");
    }

    [ThreadStatic]
    private static StringBuilder _tempSb;
    public string GenerateErrorReport()
    {
        if (_tempSb == null)
            _tempSb = new StringBuilder();
        _tempSb.Length = 0;
        _tempSb.Append(ErrorMessage);
        if (ErrorDetails != null)
            foreach (var pair in ErrorDetails)
                foreach (var msg in pair.Value)
                    _tempSb.Append("\n").Append(pair.Key).Append(": ").Append(msg);
        return _tempSb.ToString();
    }
}
