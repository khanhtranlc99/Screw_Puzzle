using Firebase;
using Firebase.Messaging;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Specialized;
using Firebase.Extensions;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Firebase.Crashlytics;
using Firebase.Analytics;
using Facebook.Unity;
using System.Text;
using com.adjust.sdk;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class GameServices : SingletonClass<GameServices>, IService
{
    #region Events and Delegates

    #endregion

    #region Variables

   

    private static Texture2D _defaultAvatar;
    public static Texture2D defaultAvatar
    {
        get
        {
            if (_defaultAvatar == null)
                _defaultAvatar = Resources.Load("avatar") as Texture2D;
            return _defaultAvatar;
        }
    }

    private static Texture2D defaultFlag
    {
        get
        {
            return Resources.Load("flag") as Texture2D;
        }
    }

   

    public static readonly ReactiveProperty<Texture2D> MyAvatar = new ReactiveProperty<Texture2D>();

    private static Texture2D _myflag;
    public static Texture2D MyFlag
    {
        get { return _myflag == null ? defaultFlag : _myflag; }
    }


    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;


    public static Subject<bool> InternetSubject = new Subject<bool>();
    public static AsyncSubject<bool> FetchFirebaseSubject = new AsyncSubject<bool>();

    public Subject<int> OnDataChange = new Subject<int>();
    #endregion

    #region Properties

    #endregion

    #region Unity Method

    public static bool didInitFireBase;
    public void Init()
    {

        var internetObservable = Observable.Interval(TimeSpan.FromSeconds(1), Scheduler.MainThreadIgnoreTimeScale)
             .Select(_ => IntenetAvaiable);

        internetObservable.DistinctUntilChanged().Subscribe(_ =>
        {
            InternetSubject.OnNext(_);
        });

        internetObservable.Where(haveInternet => haveInternet)
                    .Take(1)
                    .SelectMany(_ =>
                    {
                        return CheckAndFixDependenciesAsync().ToObservable();

                    })
                    .ObserveOnMainThread()
                    .SelectMany(isAvailable =>
                    {
                        if (isAvailable)
                        {
                            InitializeFirebase();
                            return RemoteConfigController.FetchData().ToObservable();
                        }
                        else
                        {
                            Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
                            return Observable.Throw<Unit>(new Exception("Could not resolve all Firebase dependencies: " + dependencyStatus));
                        }
                    })
                    .Timeout(TimeSpan.FromSeconds(60))
                    .Subscribe(_ =>
                    {
                        FetchFirebaseSubject.OnNext(true);
                        FetchFirebaseSubject.OnCompleted();

                    }, ex =>
                    {
                      //  Debug.LogError("Check Firebase dependencies: " + ex.Message);
                    });

        InitFacebook();
        InitializeAdjust();
#if UNITY_IOS
        Observable.EveryUpdate()
            .Where(_ => UnityEngine.iOS.NotificationServices.deviceToken != null)
            .Select(_ =>
            {
                byte[] token = UnityEngine.iOS.NotificationServices.deviceToken;

                //For iOS uninstall
                if (token != null)
                {
                   // Adjust.setDeviceToken(Encoding.UTF8.GetString(token));
                }
                return Unit.Default;
            })
            .Take(1).Subscribe();

        RequestIDFAiOS();
#endif

    }

    private void RequestIDFAiOS()
    {
        Adjust.requestTrackingAuthorizationWithCompletionHandler(status => {
        });
    }

    private bool isInitReady = false;
   
    private async Task<bool> CheckAndFixDependenciesAsync()
    {
        var check = await FirebaseApp.CheckAndFixDependenciesAsync();
        return check == DependencyStatus.Available;
    }

    private void InitializeFirebase()
    {
#if UNITY_ANDROID
        FirebaseMessaging.TokenReceived += OnTokenReceived;
        FirebaseMessaging.MessageReceived += OnMessageReceived;
#endif
#if UNITY_IOS
       // didInitFireBase = true;
      //  InitMessage();
#endif

        InitProperty();

        RemoteConfigController.RemoteConfigFirebaseInit();
        AnalyticsController.firebaseInitialized = true;
        //todo
        //RocketRemoteConfig.FetchData().ToObservable();
 
    }

    private void InitProperty()
    {

    }

    public static bool _isLoadAvatarDone;

    #endregion



    #region Private Methods

    private bool SendToken = false;
    private void OnTokenReceived(object sender, TokenReceivedEventArgs token)
    {
        Debug.LogError("Received Registration Token: " + token.Token);
#if UNITY_ANDROID
        //AppsFlyerAndroid.updateServerUninstallToken(token.Token);
#endif
        if (!SendToken)
        {
            SendToken = true;
        }

        Adjust.setDeviceToken(token.Token);
    }


    public void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        Debug.LogError("Received a new message");
    }

    #endregion


    public bool IntenetAvaiable
    {
        get { return Application.internetReachability != NetworkReachability.NotReachable; }
    }

    public void InitFacebook()
    {
        // Include Facebook namespace
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
        // Awake function from Unity's MonoBehavior

    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
       
    }


    private void InitializeAdjust()
    {
        AdjustEnvironment environment = AdjustEnvironment.Production;


        AdjustConfig config = new AdjustConfig(ConfigGameBase.ADJUST_APP_TOKEN, environment, true);
#if ENV_PROD
        config.setLogLevel(AdjustLogLevel.Suppress);
#else
        config.setLogLevel(AdjustLogLevel.Info);
#endif
        Adjust.start(config);
    }

}
