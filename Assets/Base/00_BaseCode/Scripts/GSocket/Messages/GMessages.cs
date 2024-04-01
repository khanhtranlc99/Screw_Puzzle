using UnityEngine;
using System;
using System.Collections.Generic;
using System.Globalization;
using UniRx;

//using PlayFab.ClientModels;

[Serializable]
[Message("MOBILE_LOGIN_REQUEST")]
public class MobileLoginRequest : MessageRequest
{
    public string DeviceId;
    public int AppVersion;
    public string OS;
    public string DeviceModel;
    public int Platform;
    public string AppsflyerId;
    public string AdvertisingId;
    public bool FirstOpen;
    public string AccessToken;
    public string PackageName;
    public int DataVersion;
}
[Serializable]
[Message("FB_LINK_REQUEST_V2")]
public class FBLinkRequest : MessageRequest
{
    public string AccessToken;
    public string DeviceId;
    public string OS;
    public string DeviceModel;
    public int Platform;
    public int DataVersion;
    public string Signature;
}

public enum LinkFBType
{
    FirstLink = 0,
    LinkNewDevice = 1,
    ReLogin = 2,
}

[Serializable]
public class LinkResponse : MessageResponse
{
    public string RocketId;
    public LinkFBType LinkType;
    public string DisplayName;
    public string AvatarUrl;
    public string FacebookId;
    public string GoogleId;
    public string GameCenterId;
    public string AppleId;
    public Dictionary<string, object> Data;
    public bool UploadAll;
    public int DataVersion;

    //public Dictionary<string, object> DataProteced;
    public List<string> ListKey;

}
//[Serializable]
//[Message("FB_DEVICE_DATA_REQUEST")]
//public class FBDeviceDataRequest : MessageRequest
//{
//    public Dictionary<string, DataField> Data;
//}

[Serializable]
[Message("DEVICE_UPLOAD_REQUEST")]
public class DeviceDataRequest : MessageRequest
{
    public int DataVersion;
    public Dictionary<string, DataField> Data;
}

[Serializable]
[Message("SET_NAME")]
public class SetNameRequest : MessageRequest
{
    public string DisplayName;
}

[Serializable]
[Message("CONVERSION_REQUEST")]
public class ConversionRequest : MessageRequest
{
    public string Af_status;
    public string Media_source;
    public string Campaign;
    public int Cost_cents_USD;
}

public enum AggregateType
{
    ADDED = 1,
    OVERRIDE = 2
}


[Serializable]
public class ProfileModel
{
    public string CountryCode;
    public string GId;
    public string FacebookId;
    public string GoogleId;
    public string GameCenterId;
    public string AppleId;
    public int DataVersion;
    public string DisplayName;
    public string AvatarUrl;
    public float LTVInApp;

    public bool NewlyCreated;
    public bool UploadAll;
    public bool IsHacker;
    public UserRole UserRole;

    public int Title;

    //public int Referral;

    public BoolReactiveProperty Verified = new BoolReactiveProperty(false);
    public bool Verifed => Verified.Value;

    public Dictionary<string, object> Data;
    public List<string> ListKey;

    public Dictionary<string, object> Settings;
}

public enum UserRole
{
    Player = 0,
    Tester = 1,
    Manager = 2,
    Admin = 99
}

[Serializable]
public class DeviceInfo
{
    public string DeviceId;
    public int AppVersion;
    public string OS;
    public string DeviceModel;
    public int Platform;
    public string AppsflyerId;
    public string AdvertisingId;
    public string PushToken;
}




[Serializable]
public class RemoteConfigResponse : MessageResponse
{
    public Dictionary<string, string> Data;
}

[Serializable]
[Message("LOG_DONG_BO")]
public class LogDongbo : MessageRequest
{
    public string Reason;
    public string Action;
    public string Note;
}


[Serializable]
public class AllKeyResponse : MessageResponse
{
    public List<string> ListKey;
}


[Serializable]
public class DataChangedResponse : MessageResponse
{

    public int DataVersion;
    public string Message;
    public Dictionary<string, object> Data;

    public Dictionary<string, object> DataProtected;
    public List<string> ListKey;
}


[Serializable]
public class OnUserLogin : MessageResponse
{
    public bool FirstLoginToDay;

}

[Serializable]
[Message("UPDATE_CLOUD_DATA_REQUEST")]
public class UpdateCloudDataRequest : MessageRequest
{
    public string Key;
    public object Value;
    public AggregateType UpdateType;

    public UpdateCloudDataRequest()
    {
    }

    public UpdateCloudDataRequest(string Key, object Value, AggregateType UpdateType)
    {
        this.Key = Key;
        this.Value = Value;
        this.UpdateType = UpdateType;
    }
    public UpdateCloudDataRequest(string Key, string Value)
    {
        this.Key = Key;
        this.Value = Value;
        this.UpdateType = AggregateType.OVERRIDE;
    }
}




[Serializable]
[Message("GOOGLE_LOGIN_REQUEST")]
public class GoogleLoginRequest : MessageRequest
{
    public string GoogleId;
    public string DisplayName;
    public string AvatarUrl;

    public int DataVersion;
}

[Serializable]
[Message("GAMECENTER_LOGIN_REQUEST")]
public class GameCenterLoginRequest : MessageRequest
{
    public string GameCenterId;
    public string DisplayName;

    public int DataVersion;
}

[Serializable]
[Message("APPLE_LOGIN_REQUEST")]
public class AppleLoginRequest : MessageRequest
{
    public string AppleId;
    public string DisplayName;
}


[Serializable]
[Message("BUY", "energy")]
public class BuyEnergyRequest : MessageRequest
{
    public int Type;

    public static BuyEnergyRequest CreateBuyByCoinRequest()
    {
        return new BuyEnergyRequest {Type = 0};
    }

    public static BuyEnergyRequest CreateBuyByVideoRequest()
    {
        return new BuyEnergyRequest {Type = 1};
    }

    public static BuyEnergyRequest CreateBuyByGemRequest()
    {
        return new BuyEnergyRequest {Type = 2};
    }

    public static BuyEnergyRequest CreateBuyByInappRequest()
    {
        return new BuyEnergyRequest {Type = 3};
    }
}


[Message("CHECK_HASH_REQUEST")]
public class CheckHashRequest : MessageRequest
{
    public string ProcessorType;
    public string SummaryHash;
    public Dictionary<string,string> FileHash;
    public bool IsCheckRooted;
    public bool IsCheckHacked;
    public bool IsEmulator;
    public string InforHack;

    public bool IsCheckPackages;
    public bool IsCheckFiles;
    public bool IsCheckDangerousProps;
    public bool IsCheckReadWritePaths;
    public bool IsCheckTestKeys;
    public bool IsCheckCommandsExists;
}

[Serializable]
public class CheckHashResponse : MessageResponse
{
    public bool Verified;
    public bool Close;
    public bool ClearData;

    public int Code; // 1: Hash invalid ,  2: Rooted, 3: Hacked, 4: Emulator

    public BoolReactiveProperty CloseApp = new BoolReactiveProperty(false);
    public bool ForceClose => CloseApp.Value;
}

