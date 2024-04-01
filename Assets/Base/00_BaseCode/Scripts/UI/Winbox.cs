using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using MoreMountains.NiceVibrations;

public class Winbox : BaseBox
{
    public Button btnNext;
    public Image aura;
    public Image iconEmoji;
    public List<Sprite> lsSprites;
    public List<string> lsContent;
    public Text tvContent;
    public List<GameObject> lsIconOnOffMerce;
    #region instance
    private static Winbox instance;
    public static Winbox Setup(bool isSaveBox = false, Action actionOpenBoxSave = null)
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load<Winbox>(PathPrefabs.WIN_BOX));
            instance.Init();
        }

        instance.InitState();
        return instance;
    }
    #endregion

    private void Init()
    {
        btnNext.onClick.AddListener(HandleNext);




    }
    public void InitState()
    {
        iconEmoji.sprite = lsSprites[UnityEngine.Random.Range(0, lsSprites.Count)];
        //  tvContent.text = lsContent[UnityEngine.Random.Range(0, lsContent.Count)];
        btnNext.gameObject.SetActive(false);


        GameController.Instance.musicManager.PlayWinSound();
        Invoke("ShowBtnNext", 0.5f);
        if (  UseProfile.CurrentLevel < RemoteConfigController.GetIntConfig(FirebaseConfig.LEVEL_START_SHOW_INITSTIALL, 3))
        {
            return;
        }
        if (GameController.Instance.useProfile.IsRemoveAds  )
        {
            return;
        }
        if (GameController.Instance.admobAds.IsMRecReady)
          {
            foreach(var item in lsIconOnOffMerce)
            {
                item.gameObject.SetActive(false);
            }
            GameController.Instance.admobAds.HandleShowMerec();
 
          }

      
    }
    public void ShowBtnNext()
    {
        btnNext.gameObject.SetActive(true);
    }


    public void HandleShowBtnNext()
    {
        btnNext.gameObject.SetActive(true);
    }
 
    public void HandleNext()
    {
        GameController.Instance.admobAds.ShowInterstitial(false, actionIniterClose: () => { Next(); }, actionWatchLog: "replay");

        void Next()
        {
            GameController.Instance.AnalyticsController.WinLevel(UseProfile.CurrentLevel);
            GameController.Instance.admobAds.HandleHideMerec();
            MMVibrationManager.Haptic(HapticTypes.Selection, false, true, this);
            GameController.Instance.musicManager.PlayClickSound();
            GamePlayController.Instance.tutLevel_1.EndTut();
            if (UseProfile.CurrentLevel >= 100)
            {
                SceneManager.LoadScene("GamePlay");
                return;
            }
            UseProfile.CurrentLevel += 1;
            SceneManager.LoadScene("GamePlay");
         
        }
    

    }

    private void Update()
    {

        aura.transform.localEulerAngles += new Vector3(0, 0, 1);
    }

}
