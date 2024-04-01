using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using MoreMountains.NiceVibrations;

public class LoseBox : BaseBox
{
    #region instance
    private static LoseBox instance;
    public static LoseBox Setup(bool isSaveBox = false, Action actionOpenBoxSave = null)
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load<LoseBox>(PathPrefabs.LOSE_BOX));
            instance.Init();
        }

        instance.InitState();
        return instance;
    }
    #endregion
    public Button btnReplay;
    public AudioSource audioSource;
    public AudioClip audioClip;
    public GameObject icon;

    private void Init()
    {
        btnReplay.onClick.AddListener(HandleNext);




    }
    public void InitState()
    {
        MMVibrationManager.Haptic(HapticTypes.Selection, false, true, this);
        audioSource.PlayOneShot(audioClip);
        btnReplay.gameObject.SetActive(false);
        Invoke("HandleShowBtnNext", 2);
        if (GameController.Instance.useProfile.IsRemoveAds)
        {
            return;
        }
        if (GameController.Instance.admobAds.IsMRecReady)
        {
            icon.SetActive(false);
            GameController.Instance.admobAds.HandleShowMerec();
        }
   
    }


    public void HandleShowBtnNext()
    {
        btnReplay.gameObject.SetActive(true);
    }
    public void HandleNext()
    {
        GameController.Instance.admobAds.HandleHideMerec();
        GameController.Instance.musicManager.PlayClickSound();
        SceneManager.LoadScene("GamePlay");
    }
}
