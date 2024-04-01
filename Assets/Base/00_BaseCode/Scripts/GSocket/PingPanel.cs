using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
public class PingPanel : MonoBehaviour
{
    private static PingPanel _instance;

    public Text infoText;
    public Text infoText_2;

    private int _version;
    private string _osName;

    public static int PingMs;

    private Ping newPing;

    private Coroutine _coroutine;
    public static int segment_id = -3;
    private void Awake()
    {
        if (_instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _version = ConfigGameBase.versionCode;
#if UNITY_ANDROID
        _osName = "A.";
#else
        _osName = "I.";
#endif
        GSocket.OnLoginSuccess.Subscribe(LoginSuccessEvent).AddTo(this);
        if (_coroutine == null) _coroutine = StartCoroutine(InitPing());
    }



    private void LoginSuccessEvent(Unit unit)
    {
        StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(InitPing());
    }

    private IEnumerator InitPing()
    {
        while (true)
        {
            yield return Yielders.Get(0.5f);
            newPing = new Ping("8.8.8.8");

            while (!newPing.isDone)
            {
                PingMs = -1;
                if (!GSocket.IntenetAvaiable)
                {
                    infoText.text = $"{PingMs} ms-{_osName}{_version}.{segment_id}";
                    StopCoroutine(_coroutine);
                    yield break;
                }

                yield return null;
            }

            infoText.text = $"{newPing.time} ms-{_osName}{_version}.{segment_id}";
            PingMs = newPing.time;
        }
    }
}