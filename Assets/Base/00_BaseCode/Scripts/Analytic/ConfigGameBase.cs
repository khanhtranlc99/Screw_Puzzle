using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigGameBase
{
    public static string settingProductName = "Art Puzzle";

    public const string settingKeyStore = "artpuzzle";
    public static string keyaliasPass = "12345678two";
    public static string keystorePass = "12345678two";
    public static string settingAliasName = "artpuzzle";

    public const string settingLogo = "GAME_ICON";

    public static int versionCode = 2023022501;//sua
    public static string versionName = "1.0.9";//sua
    public static int settingVersionCode = 2023022501;//sua
    public static string settingVersionName = "1.0.9";//sua

    public static string productNameBuild = "Art Puzzle";

    public static int VersionCodeAll
    {
        get
        {
            return versionCode / 100;
        }
    }

    public static int VersionFirstInstall
    {
        get
        {
            int data = PlayerPrefs.GetInt(StringHelper.VERSION_FIRST_INSTALL, 0);
            if (data == 0)
            {
                PlayerPrefs.SetInt(StringHelper.VERSION_FIRST_INSTALL, versionCode);
                data = versionCode;
            }

            return data;
        }
    }

    public static string inappAndroidKeyHash
        = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEApBBUr7Jl2gbb4ZIcu+IKRNpnamrlghaGv8L55PDLQn3rnjYFJv1mlOrpblHB7A16D/FXSBaEFhkZpd9Zi7SmhHPNHBwA/b37+xdeN+1UuI4703aTVN4G89tgVPquc+LpiqUwHvpq4OgEwl9Idql5QchlnK7bmcdOscG2UhLMoGiYke9yJZLaj+yMnpwnlgG4jqJbWO278gHO/VuGrxtEDZ+Pe0oijXTewZimNqiQwzQhTC5iHWgxPc0gwTt1OHg9OvqxBXWpaLo+BLJmUCsdoihx5b7TWAdjYCV7NyMuB3uUOR1hLyNVdZi/kU/ZZYz7L2H631yJfu4gX/ixMJGk9QIDAQAB";
#if UNITY_ANDROID
    public const string package_name = "com.gks.crewsort";
    public const string package_name_2 = "com.gks.screwsort";
#else
    public const string package_name = "com.gks.crewsort";
#endif



#if UNITY_ANDROID
    public static string OPEN_LINK_RATE = "market://details?id=" + package_name;
#else
    public static string OPEN_LINK_RATE = "itms-apps://itunes.apple.com/app/id6443535076";
#endif

    public static string FanpageLinkWeb = "https://www.facebook.com/groups/402513540729752/";
    public static string FanpageLinkApp = "https://www.facebook.com/groups/402513540729752/";

    public static string LinkFeedback = "https://www.facebook.com/groups/402513540729752/";
    public static string LinkPolicy = "https://sites.google.com/view/onesoft/privacy-policy";
    public static string LinkTerm = "https://sites.google.com/view/mini-game-puzzle-fun-policy/";

#if UNITY_ANDROID
    public const string IRONSOURCE_DEV_KEY = "1583b0af5";
#else
 public const string IRONSOURCE_DEV_KEY = "16a9dd175";
#endif


#if UNITY_ANDROID
    public const string ADJUST_APP_TOKEN = "luf8x3td5vy8";
#else
    public const string ADJUST_APP_TOKEN = "luf8x3td5vy8";
#endif
}
