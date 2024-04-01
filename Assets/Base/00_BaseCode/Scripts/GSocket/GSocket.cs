using BestHTTP.SocketIO;
using BestHTTP.SocketIO.Transports;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using CodeStage.AntiCheat.Storage;


public class GSocket : SingletonClass<GSocket>, IService
{
    private static string TAG = typeof(GSocket).Name;
    private static SocketManager socketManager;
    private static Socket socket;
    public static AsyncSubject<Socket> SocketSubject = new AsyncSubject<Socket>();
    public ConnectState connectState = ConnectState.INITIAL;

   
    private const string REMOTECONFIG_RESPONSE = "REMOTECONFIG_RESPONSE";
    private const string DATA_CHANGED_RESPONSE = "DATA_CHANGED_RESPONSE";
    private const string ON_USER_LOGIN = "ON_USER_LOGIN";

    private const string server = "";
    private const string server_test = "";

    Dictionary<Server, string> ServerList = new Dictionary<Server, string>()
    {
        { Server.Publish, server },
        { Server.Test, server_test },
    };

    public static Server CurrentServer
    {
        get
        {
            return (Server)ObscuredPrefs.GetInt(KeyPref.SERVER_INDEX, (int)Server.Publish);
        }
        set
        {
            ObscuredPrefs.SetInt(KeyPref.SERVER_INDEX, (int)value);
            ObscuredPrefs.Save();
        }
    }


    public static void ChangeServer(Server sv)
    {
        if (sv == Server.Publish)
        {
            CurrentServer = Server.Test;
        }
        else
        {
            CurrentServer = Server.Publish;
        }
    }

    public enum Server
    {
        Publish = 0,
        Test = 1
    }

    public enum ConnectState
    {
        INITIAL,
        ACTION_CONNECTED,
        ACTION_DISCONNECTED,
        ACTION_RECONNECTED,
        ACTION_RESCONNECTING,
    }

    public enum ELoginState
    {
        LOGINED,
        LOGOUT,
        BANNED
    }

    public static ELoginState LoginState { get; private set; } = ELoginState.LOGOUT;

    public static bool IntenetAvaiable
    {
        get { return Application.internetReachability != NetworkReachability.NotReachable; }
    }

 


    public void Init()
    {
        LoginState = ELoginState.LOGOUT;

        Connect(ServerList[CurrentServer]);

        SceneManager.sceneLoaded += OnSceneLoaded;

        MessageBroker.Default.Receive<ReloadPlayfabConfig>().Subscribe(x =>
        {
            bool ReloadData = GRemoteConfig.PlayfabBoolConfig("reload_data_savedKey", false);
            var savedKey = PlayerPrefs.GetString("d8426e4241c2ae920792ecc58c0c660c");
            if (ReloadData && !string.IsNullOrEmpty(savedKey))
            {
                ReconnectServer();
            }
        });

    }

    private static bool lb_reopen_game;




    public void Connect(string serverUri)
    {
        if (socketManager == null || !socketManager.GetSocket().IsOpen)
        {
            //init socket
            SocketOptions options = new SocketOptions();
            options.AutoConnect = true;
            options.Reconnection = true;
            options.ConnectWith = TransportTypes.WebSocket;
            options.Timeout = TimeSpan.FromMilliseconds(60000);

            //connect to server
            socketManager = new SocketManager(new Uri(serverUri), options);
            socketManager.Encoder = new JsonDotNetEncoder();
            socket = socketManager.GetSocket();
            SocketSubject.OnNext(socket);
            SocketSubject.OnCompleted();
            socket.On("connect", OnConnect);
            socket.On("disconnect", OnDisconnect);
            socket.On("reconnecting", OnReconnecting);
            socket.On("reconnect", OnReconnect);
            socket.On("msgAsync", OnMessageAsyn);

            
            socket.On("connect_error", (socket, packet, args) =>
            {
                Debug.LogError("connect_error");
            });
            socket.On("connect_timeout", (socket, packet, args) =>
            {
                Debug.LogError("connect_timeout");
            });
        }
        socketManager.Open();
    }
    private static Subject<Unit> _ConnectSubject = new Subject<Unit>();

    public IObservable<Unit> Connected
    {
        get
        {
            if (connectState == ConnectState.ACTION_CONNECTED)
            {
                return Observable.Return(Unit.Default);
            }
            else
            {
                return _ConnectSubject;
            }
        }
    }


    private void OnConnect(Socket socket, Packet packet, object[] args)
    {
        connectState = ConnectState.ACTION_CONNECTED;
        SendLoginRequest().CatchIgnore().Subscribe();
        _ConnectSubject.OnNext(Unit.Default);
    }
    private void OnReconnecting(Socket socket, Packet packet, params object[] args)
    {
        connectState = ConnectState.ACTION_RESCONNECTING;
    }

    private void OnReconnect(Socket socket, Packet packet, object[] args)
    {
      
    }

    private void OnDisconnect(Socket socket, Packet packet, object[] args)
    {
        connectState = ConnectState.ACTION_DISCONNECTED;
        LoginState = ELoginState.LOGOUT;
        var err = new GError() { Error = GErrorCode.CheckInternet, ErrorMessage = Localization.Get("lb_check_internet_v2") + ChickenErrorCodes.GetErrorCode(FunctionCode.ButtonTeamFunctionInMainHome, CantConnectCode.RocketIO_NoInternet) };
        _loginAsyncSubject.OnError(new GException(err));
        Logining = false;
        UserDataServices.Instance.AllowUpload(false, "OnDisconnect");
        GObservable.Disconnect.OnNext(Unit.Default);
    }

    private static void OnMessageAsyn(Socket socket, Packet packet, object[] args)
    {
        try
        {
            MessageResponse mesageData = JsonConvert.DeserializeObject<MessageResponse>(args[0].ToString());
            switch (mesageData.Name)
            {
                case REMOTECONFIG_RESPONSE:
                    RemoteConfigResponse configs = JsonConvert.DeserializeObject<RemoteConfigResponse>(mesageData.Body.ToString());
                    GRemoteConfig.OnLoadConfig(configs.Data);
#if UNITY_IOS
                    if(string.IsNullOrEmpty(Context.CurrentUserPlayfabProfile.FacebookId) && string.IsNullOrEmpty(Context.CurrentUserPlayfabProfile.GameCenterId) && string.IsNullOrEmpty(Context.CurrentUserPlayfabProfile.AppleId))
                    {
                        //if (RocketRemoteConfig.PlayfabBoolConfig("auto_login_gamecenter",false))
                        {
                          //  Instance.LoginGameCenter();
                        }
                    }
#endif
                    break;
                case DATA_CHANGED_RESPONSE:
                    DataChangedResponse dataChanged = JsonConvert.DeserializeObject<DataChangedResponse>(mesageData.Body.ToString());
                    UserDataServices.Instance.ClearPrivateQueue("HandlePrivateData");
                    UserDataServices.Instance.HandleDataChanged(dataChanged.Message, dataChanged.Data, dataChanged.ListKey, dataChanged.DataVersion, "DataChange From Server");
                    break;
                case ON_USER_LOGIN:
                    OnUserLogin onUserLogin = JsonConvert.DeserializeObject<OnUserLogin>(mesageData.Body.ToString());
                    GObservable.OnUserLogin.OnNext(onUserLogin);
                    break;
                default:
                    DebugManager.Log(TAG, "Could not find handler for message: " + args[0].ToString());
                    break;
            }
        }
        catch (Exception e)
        {
            DebugManager.Log(TAG, "Error when handler for message: " + args[0].ToString());
            Debug.LogException(e);
        }
    }

   

    private static void OnServerData(Socket socket, Packet packet, object[] args)
    {
        GObservable.ServerData.OnNext(args);
    }

  


    private static Subject<Unit> _loginAsyncSubject = new Subject<Unit>();
    public bool Logining = false;
    public IObservable<Unit> SendLoginRequest(bool showLoading = false)
    {
        if (IsLogined)
        {
            return Observable.Return(Unit.Default);
        }
        else
        {
            if (showLoading)
                WaitingBox.Setup().ShowWaiting();
            if (Logining)
            {
                return _loginAsyncSubject;
            }
            else
            {
                _loginAsyncSubject = new Subject<Unit>();
            }
            Logining = true;

            if (GameServices.Instance.IntenetAvaiable)
            {
                Connected
                .Take(1)
                .Timeout(TimeSpan.FromSeconds(10))
                .Subscribe(_ =>
                {
                    if (LoginState == ELoginState.BANNED && last_rocketException != null)
                    {
                        _loginAsyncSubject.OnError(new GException(last_rocketException));
                        OnLoginError(last_rocketException, showLoading);
                        Logining = false;
                    }
                    else
                    {
                        SendMessageP(LoginRequest, (e) =>
                        {
                            OnLoginSucess(e);
                            if (showLoading)
                                WaitingBox.Setup().HideWaiting();
                            _loginAsyncSubject.OnNext(Unit.Default);
                            _loginAsyncSubject.OnCompleted();
                            Logining = false;
                        }, (exception) =>
                        {
                            _loginAsyncSubject.OnError(new GException(exception));
                            OnLoginError(exception, showLoading);
                            Logining = false;
                        });
                    }
                }, ex =>
                {
                    GError error = new GError
                    {
                        Error = GErrorCode.ServerDown,
                        ErrorMessage = "ServerDown"
                    };
                    OnLoginError(error, showLoading);
                    Logining = false;
                    _loginAsyncSubject.OnError(new GException(error));

                });
            }
            else
            {
                var err = new GError() { Error = GErrorCode.CheckInternet, ErrorMessage = Localization.Get("lb_check_internet_v2") };
                OnLoginError(err, showLoading);
                Logining = false;
                _loginAsyncSubject.OnError(new GException(err));
            }
        }
        return _loginAsyncSubject;
    }


    public void SendMessage(MessageRequest request, Action<MessageResponse> SuccessCallback, Action<GError> ErrorCallback = null, float timeout = 10, Action TimeOutCallback = null)
    {
        if (!IsLogined)
        {
            if (ErrorCallback != null)
                ErrorCallback(new GError() { Error = GErrorCode.NotLogin });
            return;
        }
        SendMessageP(request, SuccessCallback, ErrorCallback);
    }
    public IObservable<T> SendMessage<T>(MessageRequest request, float timeout = 10, bool showLoading = false) where T : MessageResponse
    {
        return SendLoginRequest(showLoading)
            .Timeout(TimeSpan.FromSeconds(30))
            .SelectMany(_ =>
            {
                return SendMessageP<T>(request, timeout, showLoading);
            });
    }

    public void SendMessageT(MessageRequest messageOut, Action<MessageResponse> SuccessCallback, Action<GError> ErrorCallback = null, float timeout = 5, Action TimeOutCallback = null)
    {
        Observable.Create<Unit>(_ =>
        {
            if (!IsLogined)
            {
                GError error = new GError() { Error = GErrorCode.NotLogin };
                if (ErrorCallback != null)
                    ErrorCallback(error);
                _.OnError(new GException(error));
                _.OnCompleted();
            }
            SendMessageP(messageOut, (response) =>
            {
                SuccessCallback(response);
                _.OnNext(Unit.Default);
                _.OnCompleted();
            },
            (ex) =>
            {
                if (ErrorCallback != null)
                    ErrorCallback(ex);
                _.OnError(new GException(ex));
                _.OnCompleted();
            });
            return Disposable.Empty;
        })
        .Timeout(TimeSpan.FromSeconds(timeout))
        .DoOnError(exception =>
        {
            TimeoutException timeoutException = exception as TimeoutException;
            if (timeoutException != null)
            {
                if (TimeOutCallback != null)
                    TimeOutCallback();
            }

        })
         .CatchIgnore()
         .Subscribe();
    }


    private void SendMessageP(MessageRequest messageOut, Action<MessageResponse> SuccessCallback, Action<GError> ErrorCallback = null)
    {
        if (!IntenetAvaiable)
        {
            if (ErrorCallback != null)
            {
                var err = new GError() { Error = GErrorCode.CheckInternet, ErrorMessage = Localization.Get("lb_check_internet_v2") };
                ErrorCallback(err);
            }
            return;
        }
        MessageData messageData = new MessageData(messageOut);
        SendMsg(messageData, SuccessCallback, ErrorCallback);
    }

    private IObservable<T> SendMessageP<T>(MessageRequest request, float timeout = 10, bool showLoading = false) where T : MessageResponse
    {
        return Observable.Create<T>(observer =>
        {
            if (showLoading)
                WaitingBox.Setup().ShowWaiting(timeout);
            var _dispose = new BooleanDisposable();

            SendMessageP<T>(request, response =>
            {
                if (showLoading)
                    WaitingBox.Setup().HideWaiting();
                if (_dispose.IsDisposed)
                    return;
                observer.OnNext(response);
                observer.OnCompleted();

            }, rocketError =>
            {
                if (showLoading)
                    WaitingBox.Setup().HideWaiting();
                if (_dispose.IsDisposed)
                    return;
                observer.OnError(new GException(rocketError));
            });
            return _dispose;
        }).Timeout(TimeSpan.FromSeconds(timeout));
    }

    private void SendMsg(MessageData messageData, Action<MessageResponse> SuccessCallback, Action<GError> ErrorCallback = null)
    {
        try
        {
            socket.Emit(messageData.Route, (socket, packet, args) =>
            {
                MessageResponse mesageData = JsonConvert.DeserializeObject<MessageResponse>(args[0].ToString());
                if (mesageData != null)
                {
                    if (mesageData.IsSuccess)
                    {
                        if (SuccessCallback != null)
                        {
                            SuccessCallback(mesageData);
                        }
                    }
                    else
                    {
                        if (ErrorCallback != null)
                        {
                            ErrorCallback(mesageData.Error);
                        }
                    }
                }
                else
                {
                    if (ErrorCallback != null)
                        ErrorCallback(new GError());
                }
            }, messageData);
            //Utils.Log(TAG, "Send message: " + messageOut.Name + " | " + JsonConvert.SerializeObject(messageOut));
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private void SendMessageP<T>(MessageRequest messageOut, Action<T> SuccessCallback, Action<GError> ErrorCallback = null) where T : MessageResponse
    {
        try
        {
            MessageData messageData = new MessageData(messageOut);
            socket.Emit(messageOut.Route, (socket, packet, args) =>
            {
                T responseBody = JsonConvert.DeserializeObject<T>(args[0].ToString());
                if (responseBody != null)
                {
                    if (responseBody.IsSuccess)
                    {
                        T mesageData = JsonConvert.DeserializeObject<T>(responseBody.Body.ToString());
                        if (SuccessCallback != null)
                        {
                            SuccessCallback(mesageData);
                        }
                    }
                    else
                    {
                        if (ErrorCallback != null)
                        {
                            ErrorCallback(responseBody.Error);
                        }
                    }
                }
                else
                {
                    if (ErrorCallback != null)
                        ErrorCallback(new GError());
                }
            }, messageData);
            //Utils.Log(TAG, "Send message: " + messageOut.Name + " | " + JsonConvert.SerializeObject(messageOut));
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public IObservable<T> SendMessageChat<T>(MessageRequest request, float timeout = 10, bool showLoading = false) where T : MessageResponse
    {
        return SendLoginRequest(showLoading)
            .Timeout(TimeSpan.FromSeconds(30))
            .SelectMany(_ =>
            {
                return SendMessageC<T>(request, timeout, showLoading);
            });
    }

    private IObservable<T> SendMessageC<T>(MessageRequest request, float timeout = 10, bool showLoading = false) where T : MessageResponse
    {
        return Observable.Create<T>(observer =>
        {
            if (showLoading)
                WaitingBox.Setup().ShowWaiting(timeout);
            var _dispose = new BooleanDisposable();

            SendMessageC<T>(request, response =>
            {
                if (showLoading)
                    WaitingBox.Setup().HideWaiting();
                if (_dispose.IsDisposed)
                    return;
                observer.OnNext(response);
                observer.OnCompleted();

            }, rocketError =>
            {
                if (showLoading)
                    WaitingBox.Setup().HideWaiting();
                if (_dispose.IsDisposed)
                    return;
                observer.OnError(new GException(rocketError));
            });
            return _dispose;
        }).Timeout(TimeSpan.FromSeconds(timeout));
    }

    private void SendMessageC<T>(MessageRequest messageOut, Action<T> SuccessCallback, Action<GError> ErrorCallback = null) where T : MessageResponse
    {
        try
        {
            //MessageData messageData = new MessageData(messageOut);
            socket.Emit(messageOut.Route, (socket, packet, args) =>
            {
                //Debug.LogError("MessageData : " + args[0].ToString());
                T responseBody = JsonConvert.DeserializeObject<T>(args[0].ToString());
                if (responseBody != null)
                {
                    if (responseBody.IsSuccess)
                    {
                        if (SuccessCallback != null)
                        {
                            SuccessCallback(responseBody);
                        }
                    }
                    else
                    {
                        if (ErrorCallback != null)
                        {
                            ErrorCallback(responseBody.Error);
                        }
                    }
                }
                else
                {
                    if (ErrorCallback != null)
                        ErrorCallback(new GError());
                }
            }, messageOut);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }


    public void SendMessageAsync(MessageRequest messageOut)
    {
        try
        {
            if (!IsLogined) return;
            MessageData messageData = new MessageData(messageOut);
            socket.Emit("msgAsync", messageData);
            DebugManager.Log(TAG, "Send message: " + messageOut.Name + " | " + JsonConvert.SerializeObject(messageData));
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public static void OnDestroy()
    {
        DebugManager.Log(TAG, "OnDestroy");
        if (socketManager != null)
        {
            socketManager.Close();
        }
    }
    public void ReconnectServer()
    {
        Context.CurrentUserPlayfabProfile = null;
        LoginState = ELoginState.LOGOUT;
        if (socketManager != null)
        {
            socketManager.Close();
            socketManager = null;
        }
        Connect(ServerList[CurrentServer]);
    }

    public static MobileLoginRequest LoginRequest
    {
        get
        {
            MobileLoginRequest request = new MobileLoginRequest();
            request.OS = SystemInfo.operatingSystem;
#if UNITY_EDITOR
            request.Platform = 2;
#elif UNITY_IOS
            request.Platform = 1;
#else
            request.Platform = 0;
#endif
           
            request.AppVersion = ConfigGameBase.versionCode;
            request.PackageName = ConfigGameBase.package_name;
            request.DataVersion = UserDataServices.DataChangeCount;
            return request;
        }

    }

    public static Subject<Unit> OnLoginSuccess = new Subject<Unit>();


    public void OnLoginSucess(MessageResponse e)
    {
        ProfileModel profile = JsonConvert.DeserializeObject<ProfileModel>(e.Body.ToString());
        //ObscuredPrefs.SaveCryptoKey(profile.RocketId);
        OnLogin(profile);
        DoLoginAction(profile);
        OnLoginSuccess.OnNext(Unit.Default);
#if UNITY_EDITOR || REMOVE_HASH
        Context.CurrentUserPlayfabProfile.Verified.Value = true;
#endif
    }

  
    private void OnLogin(ProfileModel profile)
    {
        DebugManager.LogError("OnLogin");
        LoginState = ELoginState.LOGINED;
        Context.CurrentUserPlayfabProfile = profile;
        MessageBroker.Default.Publish(Context.CurrentUserPlayfabProfile);
        GObservable.GLoginObservable.OnNext(profile);
    }

    private void DoLoginAction(ProfileModel profile)
    {
        DebugManager.LogError("DoLoginAction");
    }


    private GError last_rocketException;
    public void OnLoginError(GError rocketException, bool showLoading)
    {
        LoginState = ELoginState.LOGOUT;
        if (rocketException != null)
        {
            switch (rocketException.Error)
            {
                case GErrorCode.AccountBanned:
                    LoginState = ELoginState.BANNED;
                    if (rocketException.ClearData)
                    {
                        PlayerPrefs.DeleteAll();
                    }
                    last_rocketException = rocketException;
                    if (showLoading)
                        ConfirmBox.Setup().AddMessageYes(Localization.Get("tit_warning"), rocketException.ErrorMessage);
                    break;
                case GErrorCode.UnknownSource:
                    if (showLoading)
                        ConfirmBox.Setup().AddMessageYes(Localization.Get("tit_warning"), "Unknown Source");
                    break;
                case GErrorCode.CheckInternet:
                    if (showLoading)
                        ConfirmBox.Setup().AddMessageYes(Localization.Get("tit_warning"), Localization.Get("lb_check_internet_v2") + ChickenErrorCodes.GetErrorCode(FunctionCode.RocketIO_OnLoginError, CantConnectCode.RocketIO_NoInternet));
                    break;
                case GErrorCode.ServerDown:
                    if (showLoading)
                        ConfirmBox.Setup().AddMessageYes(Localization.Get("tit_warning"), Localization.Get("lb_feature_maintenance"));
                    break;
                case GErrorCode.UpdateRequire:
                    if (showLoading)
                        ConfirmBox.Setup().AddMessageYes(Localization.Get("tit_warning"), Localization.Get("errorNeedUpdateNewerVersion"));
                    break;
                case GErrorCode.MultiDeviceOnline:
                    multi_device = true;
                    if (!IsPlaying)
                    {
                        OnMultipleDevice();
                    }
                    break;
                default:
                    if (showLoading && !string.IsNullOrEmpty(rocketException.ErrorMessage))
                        ConfirmBox.Setup().AddMessageYes(Localization.Get("tit_warning"), rocketException.ErrorMessage);
                    break;
            }
        }
        if (showLoading)
            WaitingBox.Setup().HideWaiting();
    }
    private static int Code;
    private bool hackdetect;
    public static void OnHackDetect()
    {
        var msg = "Hack Detected!";
        if (Code == 1)
        {
            msg = "Game from unknown source!";
        }
        else if (Code == 2)
        {
            msg = "Your device Rooted!";
        }
        else if (Code == 3)
        {
            msg = "Using hack tool!";
        }
        else if (Code == 4)
        {
            msg = "Emulator detected!";
        }

        BoxController.Instance.isLockEscape = true;
        ConfirmBox.Setup().AddMessageYes(Localization.Get("tit_warning"), msg,
            () =>
            {
                Context.ExitGame();
            });
    }
    private bool multi_device;
    private void OnMultipleDevice()
    {
        BoxController.Instance.isLockEscape = true;
        ConfirmBox.Setup().AddMessageYes(Localization.Get("tit_warning"), Localization.Get("lb_cant_play_multiple_device"),
        () =>
        {
            Context.ExitGame();
        });
    }
   
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!IsPlaying)
        {
            if (multi_device)
            {
                OnMultipleDevice();
            }
            if (hackdetect)
            {
                OnHackDetect();
            }
        }
    }

    public static bool IsLogined
    {
        get
        {
            return LoginState == ELoginState.LOGINED;
        }
    }

    private bool IsPlaying
    {
        get
        {
            return SceneManager.GetActiveScene().name.Equals(SceneName.GAME_PLAY);
        }
    }
}
