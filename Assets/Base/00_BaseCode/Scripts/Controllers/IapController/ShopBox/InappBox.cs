using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class InappBox : BaseBox
{
    private static InappBox instance;

    public static InappBox Setup()
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load<InappBox>(PathPrefabs.SHOP_BOX));
        }

        return instance;
    }

    [SerializeField] private List<IAPItem> lsPackItem;

    protected override void OnStart()
    {
        base.OnStart();

        InitData();
    }

    public override void Show()
    {
        base.Show();
     //   GameController.Instance.admobAds.DestroyBanner();
        OnCloseBox = () => { /*GameController.Instance.admobAds.ShowBanner();*/ OnCloseBox = null; };
    }

    private void InitData()
    {
        for (int i = 0; i < lsPackItem.Count; i++)
        {
            lsPackItem[i].Init();
        }
    }

}