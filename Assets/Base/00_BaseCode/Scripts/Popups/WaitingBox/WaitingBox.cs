using UnityEngine;
using UniRx;
using UnityEngine.UI;
using DG.Tweening;
using System;

//  ----------------------------------------------
//  Author:     CuongCT <caothecuong91@gmail.com> 
//  Copyright (c) 2016 OneSoft JSC
// ----------------------------------------------
public class WaitingBox : MonoBehaviour
{
    public Action action;
    [SerializeField] Text msgText;
    private static WaitingBox instance;
    private IDisposable _waitDispose;

    public static WaitingBox Setup()
    {
        if (instance == null)
        {
            // Create popup and attach it to UI
            instance = Instantiate(Resources.Load<WaitingBox>(PathPrefabs.WAITING_BOX));
            // Configure popup
        }

        instance.gameObject.SetActive(true);
        return instance;
    }

    private void Start()
    {
        msgText.text = Localization.Get("LOADING");
    }

    private void OnDestroy()
    {
        if (_waitDispose != null)
            _waitDispose.Dispose();
    }

    public void ShowWaiting(bool useTimeout = true)
    {
        BoxController.Instance.isLockEscape = true;
        gameObject.SetActive(true);
        GetComponent<RectTransform>().rect.Set(0, 0, 0, 0);
        msgText.DOKill();
        
        if (useTimeout)
            TimeOut(40);
    }


    public void HideWaiting(bool isLockEscape = false)
    {
        BoxController.Instance.isLockEscape = isLockEscape;
        //Debug.LogError("===========================HideWaiting==========================");
        gameObject.SetActive(false);
        if (_waitDispose != null)
            _waitDispose.Dispose();
    }

    public void ShowWaiting(float time)
    {
        // Show va Hide, ko lam gi ca
        action = null;
        ShowWaiting();
        TimeOut(time);
    }

    public void ShowWaiting(float time, Action action)
    {
        // Show va Hide, ko lam gi ca
        ShowWaiting();
        this.action = action;
        Debug.Log("stop all waitingbox");
        TimeOut(time);
    }

    private void TimeOut(float time)
    {
        if (_waitDispose != null)
            _waitDispose.Dispose();
        _waitDispose = Observable.Timer(TimeSpan.FromSeconds(time), Scheduler.MainThreadIgnoreTimeScale)
                                 .Subscribe(_ =>
                                 {
                                     Debug.Log("TimeOut");
                                     HideWaiting();
                                     if (action != null)
                                         action();
                                 });
    }
}