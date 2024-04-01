using MoreMountains.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using System.Security.Cryptography;
using System.Text;
#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif


public class GameController : MonoBehaviour
{
    public static GameController Instance;

     public MoneyEffectController moneyEffectController;
     public UseProfile useProfile;
    public DataContain dataContain;
    public MusicManagerGameBase musicManager;
    public AdmobAds admobAds;

    public AnalyticsController AnalyticsController;
    public IapController iapController;
    [HideInInspector] public SceneType currentScene;

    public StartLoading startLoading;

    protected void Awake()
    {
        Instance = this;
        Init();

        DontDestroyOnLoad(this);

        //GameController.Instance.useProfile.IsRemoveAds = true;


#if UNITY_IOS

    if(ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == 
    ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
    {

        ATTrackingStatusBinding.RequestAuthorizationTracking();

    }

#endif

    }

    private void Start()
    {
     //   musicManager.PlayBGMusic();
     
    }

    public void Init()
    {
        Application.targetFrameRate = 60;
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            SetUp();
        }
        else
        {
            NotiBox.Setup(SetUp).Show();
        }       
    }

    public void SetUp()
    {      
        
        admobAds.Init();
        musicManager.Init();
        iapController.Init();
        MMVibrationManager.SetHapticsActive(useProfile.OnVibration);
        startLoading.Init();
    }

    public void LoadScene(string sceneName)
    {
        Initiate.Fade(sceneName.ToString(), Color.black, 2f);
    }

  
}
public enum SceneType
{
    StartLoading = 0,
    MainHome = 1,
    GamePlay = 2
}