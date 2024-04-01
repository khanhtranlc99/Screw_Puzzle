using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class NotiBox : BaseBox
{
    #region instance
    private static NotiBox instance;
    public static NotiBox Setup(Action callback, bool isSaveBox = false, Action actionOpenBoxSave = null)
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load<NotiBox>(PathPrefabs.NOTI_BOX));
            instance.Init(callback);
        }

        instance.InitState();
        return instance;
    }
    #endregion

    public Button btnRetry;
    public Action callback;

    private void Init(Action param)
    {
        callback = null;
        callback = param;
        btnRetry.onClick.AddListener(delegate { HandleBtnRetry(); });
    }
    private void InitState()
    {

    }

    private void HandleBtnRetry()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            callback?.Invoke();
            Close();
        }
        else
        {
            GameController.Instance.moneyEffectController.SpawnEffectText_FlyUp_UI
                (
                    btnRetry.transform,
                    btnRetry.transform.position,
                    "Please connect to the internet to continue!",
                    Color.white,
                    isSpawnItemPlayer: true
                );
        }
    }    

}
