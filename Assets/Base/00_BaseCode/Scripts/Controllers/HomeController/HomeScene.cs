using MoreMountains.NiceVibrations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class HomeScene : BaseScene
{

    public Button btnSetting;

    public Text tvCoin;

    public Button btnCoin;

    public HorizontalScrollSnap horizontalScrollSnap;
    public List<MenuTabButton> lsMenuTabButtons;
    public List<SceneBase> lsSceneBases;
    public void ShowGift()
    {
        

    }
    public int NumberPage(ButtonType buttonType)
    {
        switch (buttonType)
        {
            case ButtonType.ShopButton:
                return 0;
                break;

            case ButtonType.HomeButton:
                return 1;
                break;

            case ButtonType.RankButton:
                return 2;
                break;

        }
        return 0;
    }


    public void Init()
    {


        foreach (var item in lsMenuTabButtons)
        {
            item.Init(this);
        }
        HandleClickButton(ButtonType.HomeButton);

        foreach (var item in lsSceneBases)
        {
            item.Init();
        }
        tvCoin.text = "" + UseProfile.Coin;
        btnSetting.onClick.AddListener(delegate { GameController.Instance.musicManager.PlayClickSound(); OnSettingClick(); });
        EventDispatcher.EventDispatcher.Instance.RegisterListener(EventID.CHANGE_COIN, OnCoinChange);
        btnCoin.onClick.AddListener(delegate { HandleClickButton(ButtonType.ShopButton); });

    }
    //private void Update()
    //{

    //       // OnScreenChange();


    //}
    public void HandleClickButton(ButtonType buttonType)
    {

        GameController.Instance.musicManager.PlayClickSound();
        foreach (var item in lsMenuTabButtons)
        {
            item.GetBackToNormal();
        }

        foreach (var item in lsMenuTabButtons)
        {
            if (item.buttonType == buttonType)
            {
                item.GetSelected();
                ChangePage(NumberPage(item.buttonType));
                break;
            }
        }

        MMVibrationManager.Haptic(HapticTypes.MediumImpact);

    }
    private void ChangeTab(ButtonType buttonType)
    {
        foreach (var item in lsMenuTabButtons)
        {
            item.GetBackToNormal();
        }
        foreach (var item in lsMenuTabButtons)
        {
            if (item.buttonType == buttonType)
            {
                item.GetSelected();
                break;
            }
        }
    }

    public void ChangePage(int param)
    {

        horizontalScrollSnap.ChangePage(param);

    }


    public void OnScreenChange(int currentPage)
    {
        switch (currentPage)
        {
            case 0:
                ChangeTab(ButtonType.ShopButton);

                break;
            case 1:
                ChangeTab(ButtonType.HomeButton);

                break;
            case 2:
                ChangeTab(ButtonType.RankButton);

                break;
        }
    }

    public override void OnEscapeWhenStackBoxEmpty()
    {
        //Hiển thị popup bạn có muốn thoát game ko?
    }
    private void OnSettingClick()
    {
        SettingBox.Setup().Show();
        MMVibrationManager.Haptic(HapticTypes.MediumImpact);
    }

    private void OnCoinChange(object param)
    {
        tvCoin.text = "" + UseProfile.Coin;
    }


}
