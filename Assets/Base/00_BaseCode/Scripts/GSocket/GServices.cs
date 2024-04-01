using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GServices
{
    public static bool IsVerified()
    {
        return IsFBLinked() || IsGoogleLinked() || IsGameCenterLinked() || IsAppleLinked();
    }

    public static bool IsFBLinked()
    {
        return Context.CurrentUserPlayfabProfile != null && !string.IsNullOrEmpty(Context.CurrentUserPlayfabProfile.FacebookId);
    }

    public static bool IsGoogleLinked()
    {
        return Context.CurrentUserPlayfabProfile != null && !string.IsNullOrEmpty(Context.CurrentUserPlayfabProfile.GoogleId);
    }

    public static bool IsGameCenterLinked()
    {
        return Context.CurrentUserPlayfabProfile != null && !string.IsNullOrEmpty(Context.CurrentUserPlayfabProfile.GameCenterId);
    }

    public static bool IsAppleLinked()
    {
        return Context.CurrentUserPlayfabProfile != null && !string.IsNullOrEmpty(Context.CurrentUserPlayfabProfile.AppleId);
    }

    public static IObservable<Unit> LoginSequence(bool showLoading = false)
    {
        return GSocket.Instance.SendLoginRequest(showLoading)
            .AsUnitObservable();
    }

   
}
