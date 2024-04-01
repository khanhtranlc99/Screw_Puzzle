using System;
using Frictionless;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public static class ApplicationStart
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void OnStartBeforeSceneLoad()
    {
        RegisterSceneEvent();
        RegisterServices();
        Debug.Log("AAAAAAAAAAAAAAAAAA");
    }

    private static void RegisterServices()
    {
        if (!UseProfile.IsFirstTimeInstall)
            RemoteConfigController.ReloadFirebaseKeys();

        RegisterService<GameServices>();
    }

    private static T RegisterService<T>() where T : class, IService, new()
    {
        SingletonClass<T>.Instance.Init();
        return SingletonClass<T>.Instance;
    }
    private static void RegisterSceneEvent()
    {
        ServiceFactory.Instance.RegisterSingleton<MessageRouter>();
        SceneManager.sceneUnloaded += _ =>
        {
            ServiceFactory.Instance.Resolve<MessageRouter>().Reset();
            ServiceFactory.Instance.Reset();
        };
        SceneManager.sceneLoaded += (scene, mode) =>
        {
          
        };
    }

 
    private static T RegisterMonoService<T>() where T : Component
    {
        var fullName = typeof(T).FullName;
        var obj = new GameObject();
        if (!string.IsNullOrEmpty(fullName))
            obj.name = fullName;
        Object.DontDestroyOnLoad(obj);
        var instance = obj.AddComponent<T>();
        return instance;
    }

    private static T RegisterPrefabService<T>(string path) where T : Component
    {
        var res = Resources.Load<T>(path);
        var obj = Object.Instantiate(res);
        return obj;
    }
}