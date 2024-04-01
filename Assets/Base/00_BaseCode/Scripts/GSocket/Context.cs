using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Context
{
    public static int MAX_LEVEL
    {
        get
        {
            if (RemoteConfigController.GetBoolConfig(FirebaseConfig.TEST_LEVEL_CAGE_BOOM, true))
                return 2500;
            else
                return 1700;
        }    
    }

    private static ProfileModel profile;
    public static ProfileModel CurrentUserPlayfabProfile
    {
        get
        {
            if (profile == null) return new ProfileModel();
            return profile;
        }
        set
        {
            profile = value;
        }
    }

    public static void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public static int GetSDKLevel()
    {
        var clazz = AndroidJNI.FindClass("android.os.Build$VERSION");
        var fieldID = AndroidJNI.GetStaticFieldID(clazz, "SDK_INT", "I");
        var sdkLevel = AndroidJNI.GetStaticIntField(clazz, fieldID);
        return sdkLevel;
    }
}
