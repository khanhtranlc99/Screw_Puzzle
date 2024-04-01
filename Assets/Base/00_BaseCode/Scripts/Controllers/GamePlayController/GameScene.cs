using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System;
using MoreMountains.NiceVibrations;
using UnityEngine.Events;

public class GameScene : BaseScene
{
    public Button btnNextLevel;
    public Button btnRemoveAds;
    public Button btnSetting;
    public Button btnReturn;
    public Button btnReload;
    public Button btnAddPile;
    public Button btnBack;
    public Text tvLevel;
    public Text tvBtnReturn;
    public Text tvBtnAddPile;
    public Text tvBtnRemoveBoom;
    public Text tvBtnRemoveCage;

    public Image iconPlusReturn;
    public Image iconPlusAddPile;
    public Image iconPlusRemoveBoom;
    public Image iconPlusRemoveCage;

    public Light light;
    public Button btnRemoveBoom;
    public Button btnRemoveCage;

  
    public void Init()
    {
        if(UseProfile.RedoBooster <= 0)
        {
            UseProfile.RedoBooster += 3;
        }
        btnNextLevel.onClick.AddListener(HandleNextLevelButton);
        btnRemoveAds.onClick.AddListener(delegate { HandleBtnRemoveAds(); });
        btnSetting.onClick.AddListener(delegate { GameController.Instance.musicManager.PlayClickSound(); SettingBox.Setup().Show(); });
        btnReturn.onClick.AddListener(delegate { HandleReturn(); });
        btnReload.onClick.AddListener(delegate { OnClickRetry(); });
        btnAddPile.onClick.AddListener(delegate { HandleAddPile();  });
        light.gameObject.SetActive(true);
        btnRemoveBoom.onClick.AddListener(delegate { HandleRemoveBoom(); });
        btnRemoveCage.onClick.AddListener(delegate { HandleRemoveCage(); });
        btnRemoveBoom.gameObject.SetActive(false);
        btnRemoveCage.gameObject.SetActive(false);


        tvLevel.text = "LEVEL " +"\n" + UseProfile.CurrentLevel;

        HandleShowTvReturn(null);
        HandleShowTvAddPile(null);
        HandleShowTvRemoveCage(null);
        HandleShowTvRemoveBoom(null);
       
        EventDispatcher.EventDispatcher.Instance.RegisterListener(EventID.CHANGE_ADD_PILE, HandleShowTvAddPile);
        EventDispatcher.EventDispatcher.Instance.RegisterListener(EventID.CHANGE_REDO, HandleShowTvReturn);
        EventDispatcher.EventDispatcher.Instance.RegisterListener(EventID.CHANGE_REMOVE_BOOM, HandleShowTvRemoveBoom);
        EventDispatcher.EventDispatcher.Instance.RegisterListener(EventID.CHANGE_REMOVE_CAGE, HandleShowTvRemoveCage);
        EventDispatcher.EventDispatcher.Instance.RegisterListener(EventID.REMOVE_ADS, HandleShowBtnRemoveAds);
        HandleShowBtnRemoveAds(null);
        HandOnOffTut();
  
    }
    public void HandleBtnRemoveAds()
    {
      
        GameController.Instance.musicManager.PlayClickSound();
        GameController.Instance.iapController.BuyProduct(TypePackIAP.NoAdsPack);
    }

    public void HandleNextLevelButton()
    {
        GameController.Instance.admobAds.ShowVideoReward(
              actionReward: () =>
              {
             
                  Winbox.Setup().Show();
              },
              actionNotLoadedVideo: () =>
              {
                  GameController.Instance.moneyEffectController.SpawnEffectText_FlyUp_UI
                   (
                      btnNextLevel.transform,
                   btnNextLevel.transform.position,
                   "No video at the moment!",
                   Color.white,
                   isSpawnItemPlayer: true
                   );
              },
              actionClose: null,
              ActionWatchVideo.Skip_level,
              UseProfile.CurrentLevel.ToString());
     
        GameController.Instance.musicManager.PlayClickSound();
    }

    public void HandOnOffTut()
    {
        if(UseProfile.CurrentLevel < 2)
        {
            btnReload.gameObject.SetActive(false);
            btnAddPile.gameObject.SetActive(false);
            btnReturn.gameObject.SetActive(false);
            btnRemoveAds.gameObject.SetActive(false);
            btnSetting.gameObject.SetActive(false);
            btnNextLevel.gameObject.SetActive(false);
        }
        if (UseProfile.CurrentLevel < 3)
        {
  
            btnAddPile.gameObject.SetActive(false);
            btnReturn.gameObject.SetActive(false);
            btnNextLevel.gameObject.SetActive(false);
        }


    }
    public void HandleShowBtnRemoveAds(object param)
    {
        if (GameController.Instance.useProfile.IsRemoveAds)
        {
            btnRemoveAds.gameObject.SetActive(false);
        }
        else
        {
            btnRemoveAds.gameObject.SetActive(true);
        }
    }
    public void HandleShowTvReturn(object param)
    {
        if(UseProfile.RedoBooster > 0)
        {
            tvBtnReturn.text = "" + UseProfile.RedoBooster;
            iconPlusReturn.gameObject.SetActive(false);
        }
        else
        {
            iconPlusReturn.gameObject.SetActive(true);
        }    
    }
    public void HandleShowTvAddPile(object param)
    {
        if (UseProfile.AddPileBooster > 0)
        {
            tvBtnAddPile.text = "" + UseProfile.AddPileBooster;
            iconPlusAddPile.gameObject.SetActive(false);
        }
        else
        {
            iconPlusAddPile.gameObject.SetActive(true);
        }
    }
    public void HandleShowTvRemoveCage(object param)
    {
        if (UseProfile.CurrentNumRemoveCage > 0)
        {
            tvBtnRemoveCage.text = "" + UseProfile.CurrentNumRemoveCage;
            iconPlusRemoveCage.gameObject.SetActive(false);
        }
        else
        {
            iconPlusRemoveCage.gameObject.SetActive(true);
        }
    }
    public void HandleShowTvRemoveBoom(object param)
    {
        if (UseProfile.CurrentNumRemoveBomb > 0)
        {
            tvBtnRemoveBoom.text = "" + UseProfile.CurrentNumRemoveBomb;
            iconPlusRemoveBoom.gameObject.SetActive(false);
        }
        else
        {
            iconPlusRemoveBoom.gameObject.SetActive(true);
        }
    }

    public void OnClickShowReward()
    {
        GameController.Instance.admobAds.ShowVideoReward(delegate { }, delegate { }, delegate { }, ActionWatchVideo.RewardEndGame, UseProfile.CurrentLevel.ToString());
    }
    public void OnClickRetry()
    {
 
        GameController.Instance.admobAds.ShowInterstitial(false, actionIniterClose: ()=> { SceneManager.LoadScene("GamePlay"); } ,actionWatchLog: "replay");
        GameController.Instance.musicManager.PlayClickSound();
        
       

    }
    public void HandleReturn()
    {
        if(!GamePlayController.Instance.returnController.CanReturn)
        {
            return;
        }

        if(UseProfile.RedoBooster > 0)
        {
            UseProfile.RedoBooster -= 1;
            GamePlayController.Instance.returnController.HandleReturn();
            GamePlayController.Instance.tut_BoosterRedo.NextTut();
        }
        else
        {
           
            GameController.Instance.admobAds.ShowVideoReward(
                actionReward: () =>
                {
                    // GamePlayController.Instance.returnController.HandleReturn();
                    UseProfile.RedoBooster += 1;
                    List<GiftRewardShow> giftRewardShows = new List<GiftRewardShow>();
                    giftRewardShows.Add(new GiftRewardShow() { amount = 1, type = GiftType.RedoBooster });
                    PopupRewardBase.Setup().Show(giftRewardShows, delegate { });

                },
                actionNotLoadedVideo: () =>
                {
                    GameController.Instance.moneyEffectController.SpawnEffectText_FlyUp_UI
                     (
                        btnReturn.transform,
                     btnReturn.transform.position,
                     "No video at the moment!",
                     Color.white,
                     isSpawnItemPlayer: true
                     );
                },
                actionClose: null,
                ActionWatchVideo.BuyStand,
                UseProfile.CurrentLevel.ToString());
        }
        GameController.Instance.musicManager.PlayClickSound();
    }
    public void HandleAddPile()
    {
       
        if (UseProfile.AddPileBooster > 0)
        {
            UseProfile.AddPileBooster -= 1;
            GamePlayController.Instance.playerContain.SpawnPileBooster();
            GamePlayController.Instance.tut_BoosterAddPile.NextTut();
            btnAddPile.interactable = false;
        }
        else
        {
           
            GameController.Instance.admobAds.ShowVideoReward(
                actionReward: () =>
                {
                    UseProfile.AddPileBooster += 1;
                    List<GiftRewardShow> giftRewardShows = new List<GiftRewardShow>();
                    giftRewardShows.Add(new GiftRewardShow() { amount = 1, type = GiftType.AddPileBooster });
                    PopupRewardBase.Setup().Show(giftRewardShows, delegate { });
                },
                actionNotLoadedVideo: () =>
                {
                    GameController.Instance.moneyEffectController.SpawnEffectText_FlyUp_UI
                     (
                     btnAddPile.transform,
                     btnAddPile.transform.position,
                     "No video at the moment!",
                     Color.white,
                     isSpawnItemPlayer: true
                     );
                },
                actionClose: null,
                ActionWatchVideo.BuyStand,
                UseProfile.CurrentLevel.ToString());
        }


        GameController.Instance.musicManager.PlayClickSound();

    }    

    public void CheckExtralInLevel()
    {
      

        if (GamePlayController.Instance.extralPlayController.extralInGame.Count > 0)
        {
            foreach(var item in GamePlayController.Instance.extralPlayController.extralInGame)
            {
                if(item.data.type == ExtralPlayType.Boom)
                {
                    btnRemoveBoom.gameObject.SetActive(true);
                    GamePlayController.Instance.tut_Extral_Boom.StartTut();
                }
                if (item.data.type == ExtralPlayType.LockBird)
                {
                    btnRemoveCage.gameObject.SetActive(true);
                    GamePlayController.Instance.tut_Extral_Lock.StartTut();
                }
            }
        }

    }

    public void HandleRemoveBoom()
    {

        if (UseProfile.CurrentNumRemoveBomb > 0)
        {
            UseProfile.CurrentNumRemoveBomb -= 1;
            RemoveBoom();
            GamePlayController.Instance.tut_RemoveBoom_Booster.NextTut();
        }
        else
        {

            GameController.Instance.admobAds.ShowVideoReward(
                actionReward: () =>
                {
                    UseProfile.CurrentNumRemoveBomb += 1;
                    List<GiftRewardShow> giftRewardShows = new List<GiftRewardShow>();
                    giftRewardShows.Add(new GiftRewardShow() { amount = 1, type = GiftType.RemoveBoomBooster });
                    PopupRewardBase.Setup().Show(giftRewardShows, delegate { });
                },
                actionNotLoadedVideo: () =>
                {
                    GameController.Instance.moneyEffectController.SpawnEffectText_FlyUp_UI
                     (
                        btnRemoveBoom.transform,
                     btnRemoveBoom.transform.position,
                     "No video at the moment!",
                     Color.white,
                     isSpawnItemPlayer: true
                     );
                },
                actionClose: null,
                ActionWatchVideo.BuyStand,
                UseProfile.CurrentLevel.ToString());
        }


        GameController.Instance.musicManager.PlayClickSound();

        void RemoveBoom()
        {
            ExtralPlayBase boom = GamePlayController.Instance.extralPlayController.GetExtraPlayBase(ExtralPlayType.Boom);
            if (boom != null)
            {
                boom.PlayAnimEnd();
                btnRemoveBoom.interactable = false;
            }
        }
    }

    public void HandleRemoveCage()
    {
        if (UseProfile.CurrentNumRemoveCage > 0)
        {
            UseProfile.CurrentNumRemoveCage -= 1;
            RemoveCage();
            GamePlayController.Instance.tut_RemoveLock_Booster.NextTut();
        }
        else
        {

            GameController.Instance.admobAds.ShowVideoReward(
                actionReward: () =>
                {
                    UseProfile.CurrentNumRemoveCage += 1;
                    List<GiftRewardShow> giftRewardShows = new List<GiftRewardShow>();
                    giftRewardShows.Add(new GiftRewardShow() { amount = 1, type = GiftType.RemoveCageBooster });
                    PopupRewardBase.Setup().Show(giftRewardShows, delegate { });
                },
                actionNotLoadedVideo: () =>
                {
                    GameController.Instance.moneyEffectController.SpawnEffectText_FlyUp_UI
                     (
                        btnAddPile.transform,
                     btnAddPile.transform.position,
                     "No video at the moment!",
                     Color.white,
                     isSpawnItemPlayer: true
                     );
                },
                actionClose: null,
                ActionWatchVideo.BuyStand,
                UseProfile.CurrentLevel.ToString());
        }


        GameController.Instance.musicManager.PlayClickSound();


        void RemoveCage()
        {
            ExtralPlayBase cage = GamePlayController.Instance.extralPlayController.GetExtraPlayBase(ExtralPlayType.LockBird);
            if (cage != null)
            {
                cage.PlayAnimEnd();
                btnRemoveCage.interactable = false;
            }
        }
      
    }




    public override void OnEscapeWhenStackBoxEmpty()
    {
    
    }


    #region Plus


    private int numTypeExtralInGame;
    private ExtralPlayType currentExtralType;
    [Header("Extrals")]
    public Button extralBtn;
    public Image iconExtralImg;
    public GameObject iconVideoRemoveExtral;
    public GameObject numRemoveExtralObj;
    [SerializeField] private Text numExtralTxt;
    [HideInInspector] public bool isExtralSuccess;
    [Header("Extrals Combination")]
    public Button extralCombinationBtn;
    public Button chooseExtralBtn;
    public Image chooseIcon;
    public List<Image> extralCombinationIcons;
    [SerializeField] private List<Text> extralCombinationTxts;
    public Image adIcon;
    public Image extralAdIcon;

    public GameObject subExtrals;
    public List<Button> subExtralBtns;
    public List<Image> subExtralIcons;
    public List<Text> subExtralTexts;
    public List<GameObject> subExtralAds;

    private int numExtral = 0;
    private List<HomeExtral> listActiveExtrals;
    private List<HomeExtral> listIngameExtrals;
    //private HomeExtral currentAdExtral;
    public GameObject iconVideoBuyStand;
    private int numOfExtral;

    public void InitExtral(ExtralPlayType type, Sprite iconExtral, UnityAction actionExtralDone)
    {
        //có 1 extral in game
        if (numTypeExtralInGame == 1)
        {
            currentExtralType = type;
            iconExtralImg.sprite = iconExtral;
            iconExtralImg.SetNativeSize();
            extralBtn.gameObject.SetActive(true);

            extralBtn.transform.DOKill();
            extralBtn.transform.localScale = Vector3.zero;
            extralBtn.transform.DOScale(1, 0.3f);
            ActiveExtral(true);

            extralBtn.onClick.RemoveAllListeners();
            extralBtn.onClick.AddListener(() => { OnClickBuyExtral(actionExtralDone); });

            UpdateNumRemoveExtral();
        }
        // có trên 2 extral in game
        else
        {
            switch (type)
            {
                case ExtralPlayType.Boom:
                    HomeExtral boomExtral = new HomeExtral(type, UseProfile.CurrentNumRemoveBomb, iconExtral, actionExtralDone);
                    listActiveExtrals.Add(boomExtral);
                    listIngameExtrals.Add(boomExtral);
                    break;
                case ExtralPlayType.EggLock:
                    HomeExtral eggExtral = new HomeExtral(type, UseProfile.CurrentNumRemoveEgg, iconExtral, actionExtralDone);
                    listActiveExtrals.Add(eggExtral);
                    listIngameExtrals.Add(eggExtral);
                    break;
                case ExtralPlayType.LockBird:
                    HomeExtral lockExtral = new HomeExtral(type, UseProfile.CurrentNumRemoveCage, iconExtral, actionExtralDone);
                    listActiveExtrals.Add(lockExtral);
                    listIngameExtrals.Add(lockExtral);
                    break;
                case ExtralPlayType.LockStand:
                    HomeExtral jailExtral = new HomeExtral(type, UseProfile.CurrentNumRemoveJail, iconExtral, actionExtralDone);
                    listActiveExtrals.Add(jailExtral);
                    listIngameExtrals.Add(jailExtral);
                    break;
                case ExtralPlayType.SleepBird:
                    HomeExtral sleepExtral = new HomeExtral(type, UseProfile.CurrentNumRemoveSleep, iconExtral, actionExtralDone);
                    listActiveExtrals.Add(sleepExtral);
                    listIngameExtrals.Add(sleepExtral);
                    break;
            }
            if (listActiveExtrals.Count == numTypeExtralInGame)
            {
                extralCombinationBtn.gameObject.SetActive(true);
                extralCombinationBtn.transform.DOKill();
                extralCombinationBtn.transform.localScale = Vector3.zero;
                extralCombinationBtn.transform.DOScale(1, 0.3f);
                ActiveExtral(true);

                extralCombinationBtn.onClick.RemoveAllListeners();
                extralCombinationBtn.onClick.AddListener(() => { OnClickBuyExtralCombination(); });

                SetupExtralIcons();
                SetAdIcon();
            }
        }

      
    }
    private void SetAdIcon()
    {
        bool hasItemOutOfStock = false;
        if (listActiveExtrals.Count > 0)
        {
            for (int i = 0; i < listActiveExtrals.Count; i++)
            {
                switch (listActiveExtrals[i].type)
                {
                    case ExtralPlayType.Boom:
                        if (UseProfile.CurrentNumRemoveBomb <= 0)
                        {
                            hasItemOutOfStock = true;
                        }
                        break;
                    case ExtralPlayType.EggLock:
                        if (UseProfile.CurrentNumRemoveEgg <= 0)
                        {
                            hasItemOutOfStock = true;
                        }
                        break;
                    case ExtralPlayType.LockBird:
                        if (UseProfile.CurrentNumRemoveCage <= 0)
                        {
                            hasItemOutOfStock = true;
                        }
                        break;
                    case ExtralPlayType.LockStand:
                        if (UseProfile.CurrentNumRemoveJail <= 0)
                        {
                            hasItemOutOfStock = true;
                        }
                        break;
                    case ExtralPlayType.SleepBird:
                        if (UseProfile.CurrentNumRemoveSleep <= 0)
                        {
                            hasItemOutOfStock = true;
                        }
                        break;
                }
                if (hasItemOutOfStock)
                {
                    adIcon.gameObject.SetActive(true);
                    extralAdIcon.sprite = listActiveExtrals[i].icon;
                    break;
                }
            }
            if (!hasItemOutOfStock)
            {
                adIcon.gameObject.SetActive(false);
            }
        }
    }
    public void SetupExtralIcons()
    {
        if (numTypeExtralInGame <= 0)
        {
            return;
        }
        for (int i = 0; i < listIngameExtrals.Count; i++)
        {
            if (i < extralCombinationIcons.Count)
            {
                extralCombinationIcons[i].sprite = listIngameExtrals[i].icon;
                extralCombinationIcons[i].SetNativeSize();
            }
            if (i < subExtralIcons.Count)
            {
                subExtralIcons[i].sprite = listIngameExtrals[i].icon;
            }
            if (i < subExtralBtns.Count)
            {
                subExtralBtns[i].onClick.RemoveAllListeners();
                HomeExtral extral = listIngameExtrals[i];
                subExtralBtns[i].onClick.AddListener(() => { OnClickBuySubExtral(extral); });
            }
            switch (listIngameExtrals[i].type)
            {
                case ExtralPlayType.Boom:
                    if (UseProfile.CurrentNumRemoveBomb > 0)
                    {
                        adIcon.gameObject.SetActive(false);
                        if (i < subExtralAds.Count)
                        {
                            subExtralAds[i].gameObject.SetActive(false);
                        }
                        if (i < extralCombinationTxts.Count)
                        {
                            extralCombinationTxts[i].gameObject.SetActive(true);
                            extralCombinationTxts[i].text = UseProfile.CurrentNumRemoveBomb.ToString();
                        }
                        if (i < subExtralTexts.Count)
                        {
                            subExtralTexts[i].text = UseProfile.CurrentNumRemoveBomb.ToString();
                            subExtralTexts[i].gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        if (i < subExtralTexts.Count)
                        {
                            subExtralTexts[i].text = UseProfile.CurrentNumRemoveBomb.ToString();
                            subExtralTexts[i].gameObject.SetActive(false);
                        }
                        if (i < subExtralAds.Count)
                        {
                            subExtralAds[i].gameObject.SetActive(true);
                        }
                        if (i < extralCombinationTxts.Count)
                        {
                            extralCombinationTxts[i].gameObject.SetActive(false);
                        }
                    }

                    break;
                case ExtralPlayType.EggLock:
                    if (UseProfile.CurrentNumRemoveEgg > 0)
                    {
                        adIcon.gameObject.SetActive(false);
                        if (i < subExtralAds.Count)
                        {
                            subExtralAds[i].gameObject.SetActive(false);
                        }
                        if (i < extralCombinationTxts.Count)
                        {
                            extralCombinationTxts[i].gameObject.SetActive(true);
                            extralCombinationTxts[i].text = UseProfile.CurrentNumRemoveEgg.ToString();
                        }
                        if (i < subExtralTexts.Count)
                        {
                            subExtralTexts[i].text = UseProfile.CurrentNumRemoveEgg.ToString();
                            subExtralTexts[i].gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        if (i < subExtralTexts.Count)
                        {
                            subExtralTexts[i].text = UseProfile.CurrentNumRemoveEgg.ToString();
                            subExtralTexts[i].gameObject.SetActive(false);
                        }
                        if (i < subExtralAds.Count)
                        {
                            subExtralAds[i].gameObject.SetActive(true);
                        }
                        if (i < extralCombinationTxts.Count)
                        {
                            extralCombinationTxts[i].gameObject.SetActive(false);
                        }
                    }
                    break;
                case ExtralPlayType.LockBird:
                    if (UseProfile.CurrentNumRemoveCage > 0)
                    {
                        adIcon.gameObject.SetActive(false);
                        if (i < subExtralAds.Count)
                        {
                            subExtralAds[i].gameObject.SetActive(false);
                        }
                        if (i < extralCombinationTxts.Count)
                        {
                            extralCombinationTxts[i].gameObject.SetActive(true);
                            extralCombinationTxts[i].text = UseProfile.CurrentNumRemoveCage.ToString();
                        }
                        if (i < subExtralTexts.Count)
                        {
                            subExtralTexts[i].text = UseProfile.CurrentNumRemoveCage.ToString();
                            subExtralTexts[i].gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        if (i < subExtralTexts.Count)
                        {
                            subExtralTexts[i].text = UseProfile.CurrentNumRemoveCage.ToString();
                            subExtralTexts[i].gameObject.SetActive(false);
                        }
                        if (i < subExtralAds.Count)
                        {
                            subExtralAds[i].gameObject.SetActive(true);
                        }
                        if (i < extralCombinationTxts.Count)
                        {
                            extralCombinationTxts[i].gameObject.SetActive(false);
                        }
                    }
                    break;
                case ExtralPlayType.LockStand:
                    if (UseProfile.CurrentNumRemoveJail > 0)
                    {
                        adIcon.gameObject.SetActive(false);
                        if (i < subExtralAds.Count)
                        {
                            subExtralAds[i].gameObject.SetActive(false);
                        }
                        if (i < extralCombinationTxts.Count)
                        {
                            extralCombinationTxts[i].gameObject.SetActive(true);
                            extralCombinationTxts[i].text = UseProfile.CurrentNumRemoveJail.ToString();
                        }
                        if (i < subExtralTexts.Count)
                        {
                            subExtralTexts[i].text = UseProfile.CurrentNumRemoveJail.ToString();
                            subExtralTexts[i].gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        if (i < subExtralTexts.Count)
                        {
                            subExtralTexts[i].text = UseProfile.CurrentNumRemoveJail.ToString();
                            subExtralTexts[i].gameObject.SetActive(false);
                        }
                        if (i < subExtralAds.Count)
                        {
                            subExtralAds[i].gameObject.SetActive(true);
                        }
                        if (i < extralCombinationTxts.Count)
                        {
                            extralCombinationTxts[i].gameObject.SetActive(false);
                        }
                    }
                    break;
                case ExtralPlayType.SleepBird:
                    if (UseProfile.CurrentNumRemoveSleep > 0)
                    {
                        adIcon.gameObject.SetActive(false);
                        if (i < subExtralAds.Count)
                        {
                            subExtralAds[i].gameObject.SetActive(false);
                        }
                        if (i < extralCombinationTxts.Count)
                        {
                            extralCombinationTxts[i].gameObject.SetActive(true);
                            extralCombinationTxts[i].text = UseProfile.CurrentNumRemoveSleep.ToString();
                        }
                        if (i < subExtralTexts.Count)
                        {
                            subExtralTexts[i].text = UseProfile.CurrentNumRemoveSleep.ToString();
                            subExtralTexts[i].gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        if (i < subExtralTexts.Count)
                        {
                            subExtralTexts[i].text = UseProfile.CurrentNumRemoveSleep.ToString();
                            subExtralTexts[i].gameObject.SetActive(false);
                        }
                        if (i < subExtralAds.Count)
                        {
                            subExtralAds[i].gameObject.SetActive(true);
                        }
                        if (i < extralCombinationTxts.Count)
                        {
                            extralCombinationTxts[i].gameObject.SetActive(false);
                        }
                    }
                    break;
            }
        }
    }
    public void OnClickBuySubExtral(HomeExtral extral)
    {
        GameController.Instance.musicManager.PlayClickSound();
        bool hasItem = false;
        switch (extral.type)
        {
            case ExtralPlayType.Boom:
                if (UseProfile.CurrentNumRemoveBomb > 0)
                {
                    UseProfile.CurrentNumRemoveBomb--;
                    hasItem = true;
                }
                break;
            case ExtralPlayType.EggLock:
                if (UseProfile.CurrentNumRemoveEgg > 0)
                {
                    UseProfile.CurrentNumRemoveEgg--;
                    hasItem = true;
                }
                break;
            case ExtralPlayType.LockBird:
                if (UseProfile.CurrentNumRemoveCage > 0)
                {
                    UseProfile.CurrentNumRemoveCage--;
                    hasItem = true;
                }
                break;
            case ExtralPlayType.LockStand:
                if (UseProfile.CurrentNumRemoveJail > 0)
                {
                    UseProfile.CurrentNumRemoveJail--;
                    hasItem = true;
                }
                break;
            case ExtralPlayType.SleepBird:
                if (UseProfile.CurrentNumRemoveSleep > 0)
                {
                    UseProfile.CurrentNumRemoveSleep--;
                    hasItem = true;
                }
                break;
        }
        if (hasItem)
        {
            extral.actionExtralDone?.Invoke();
            listActiveExtrals.Remove(extral);
        }
        else
        {
            GameController.Instance.admobAds.ShowVideoReward(
            actionReward: () =>
            {
                extral.actionExtralDone?.Invoke();
                listActiveExtrals.Remove(extral);
                SetupExtralIcons();
                SetAdIcon();
                CheckExtrals();
            },
            actionNotLoadedVideo: () =>
            {
                GameController.Instance.moneyEffectController.SpawnEffectText_FlyUp
                 (
                     extralCombinationBtn.transform.position,
                     "No video at the moment!",
                     Color.white,
                     isSpawnItemPlayer: true
                 );
            },
            actionClose: null,
            ActionWatchVideo.BuyExtral,
           UseProfile.CurrentLevel.ToString());
        }
        SetupExtralIcons();
        SetAdIcon();
        CheckExtrals();
    }
    private void CheckExtrals()
    {
        if (listActiveExtrals.Count == 1)
        {
            subExtrals.SetActive(false);
            chooseExtralBtn.interactable = false;

            currentExtralType = listActiveExtrals[0].type;
            iconExtralImg.sprite = listActiveExtrals[0].icon;
            iconExtralImg.SetNativeSize();
            extralBtn.gameObject.SetActive(true);
            extralCombinationBtn.gameObject.SetActive(false);

            extralBtn.transform.DOKill();
            extralBtn.transform.localScale = Vector3.zero;
            extralBtn.transform.DOScale(1, 0.3f);
            ActiveExtral(true);

            extralBtn.onClick.RemoveAllListeners();
            extralBtn.onClick.AddListener(() => { OnClickBuyExtral(listActiveExtrals[0].actionExtralDone); });

            UpdateNumRemoveExtral();
        }
        if (listActiveExtrals.Count == 0)
        {
            isExtralSuccess = true;
            extralCombinationBtn.interactable = false;
            extralBtn.interactable = false;
        }
        UpdateNumRemoveExtral();
    }
    public void OnClickBuyExtralCombination()
    {
        GameController.Instance.musicManager.PlayClickSound();
        List<HomeExtral> temp = new List<HomeExtral>();
        int index = 0;
        foreach (var item in listActiveExtrals.ToArray())
        {
            switch (item.type)
            {
                case ExtralPlayType.Boom:
                    if (UseProfile.CurrentNumRemoveBomb >= 1)
                    {
                        item.actionExtralDone?.Invoke();
                        temp.Add(item);
                        UseProfile.CurrentNumRemoveBomb--;
                        extralCombinationTxts[index].text = UseProfile.CurrentNumRemoveBomb.ToString();
                        subExtralTexts[index].text = UseProfile.CurrentNumRemoveBomb.ToString();
                    }
                    break;
                case ExtralPlayType.EggLock:
                    if (UseProfile.CurrentNumRemoveEgg >= 1)
                    {
                        item.actionExtralDone?.Invoke();
                        temp.Add(item);
                        UseProfile.CurrentNumRemoveEgg--;
                        extralCombinationTxts[index].text = UseProfile.CurrentNumRemoveEgg.ToString();
                        subExtralTexts[index].text = UseProfile.CurrentNumRemoveEgg.ToString();
                    }
                    break;
                case ExtralPlayType.LockBird:
                    if (UseProfile.CurrentNumRemoveCage >= 1)
                    {
                        item.actionExtralDone?.Invoke();
                        temp.Add(item);
                        UseProfile.CurrentNumRemoveCage--;
                        extralCombinationTxts[index].text = UseProfile.CurrentNumRemoveCage.ToString();
                        subExtralTexts[index].text = UseProfile.CurrentNumRemoveCage.ToString();
                    }
                    break;
                case ExtralPlayType.LockStand:
                    if (UseProfile.CurrentNumRemoveJail >= 1)
                    {
                        item.actionExtralDone?.Invoke();
                        temp.Add(item);
                       UseProfile.CurrentNumRemoveJail--;
                        extralCombinationTxts[index].text = UseProfile.CurrentNumRemoveJail.ToString();
                        subExtralTexts[index].text = UseProfile.CurrentNumRemoveJail.ToString();
                    }
                    break;
                case ExtralPlayType.SleepBird:
                    if (UseProfile.CurrentNumRemoveSleep >= 1)
                    {
                        item.actionExtralDone?.Invoke();
                        temp.Add(item);
                        UseProfile.CurrentNumRemoveSleep--;
                        extralCombinationTxts[index].text = UseProfile.CurrentNumRemoveSleep.ToString();
                        subExtralTexts[index].text = UseProfile.CurrentNumRemoveSleep.ToString();
                    }
                    break;
            }
            index++;
        }
        foreach (var item in temp)
        {
            listActiveExtrals.Remove(item);
        }
        if (listActiveExtrals.Count > 0)
        {
            HomeExtral adExtral = listActiveExtrals[0];
            extralAdIcon.sprite = adExtral.icon;
            GameController.Instance.admobAds.ShowVideoReward(
            actionReward: () =>
            {
                adExtral.actionExtralDone?.Invoke();
                listActiveExtrals.Remove(adExtral);
                SetupExtralIcons();
                SetAdIcon();
                CheckExtrals();
            },
            actionNotLoadedVideo: () =>
            {
                GameController.Instance.moneyEffectController.SpawnEffectText_FlyUp
                 (
                 extralBtn.transform.position,
                 "No video at the moment!",
                 Color.white,
                 isSpawnItemPlayer: true
                 );
            },
            actionClose: null,
            ActionWatchVideo.BuyExtral,
            UseProfile.CurrentLevel.ToString());
        }
        else
        {
            extralCombinationBtn.interactable = false;
            chooseExtralBtn.interactable = false;
            adIcon.gameObject.SetActive(false);
        }
        SetupExtralIcons();
        SetAdIcon();
        CheckExtrals();
    }
    public void UpdateNumRemoveExtral()
    {
        switch (currentExtralType)
        {
            case ExtralPlayType.Boom:
                numOfExtral = UseProfile.CurrentNumRemoveBomb;
                //numExtralTxt.text = GameController.Instance.useProfile.CurrentNumRemoveBomb.ToString();
                break;
            case ExtralPlayType.EggLock:
                numOfExtral = UseProfile.CurrentNumRemoveEgg;
                //numExtralTxt.text = GameController.Instance.useProfile.CurrentNumRemoveEgg.ToString();
                break;
            case ExtralPlayType.LockBird:
                numOfExtral = UseProfile.CurrentNumRemoveCage;
                //numExtralTxt.text = GameController.Instance.useProfile.CurrentNumRemoveCage.ToString();
                break;
            case ExtralPlayType.LockStand:
                numOfExtral = UseProfile.CurrentNumRemoveJail;
                //numExtralTxt.text = GameController.Instance.useProfile.CurrentNumRemoveJail.ToString();
                break;
            case ExtralPlayType.SleepBird:
                numOfExtral = UseProfile.CurrentNumRemoveSleep;
                //numExtralTxt.text = GameController.Instance.useProfile.CurrentNumRemoveSleep.ToString();
                break;
        }
        if (numOfExtral > 0)
        {
            iconVideoBuyStand.gameObject.SetActive(false);
            numRemoveExtralObj.gameObject.SetActive(true);
            numExtralTxt.text = numOfExtral.ToString();
            iconVideoRemoveExtral.gameObject.SetActive(false);
        }
        else
        {
            iconVideoRemoveExtral.gameObject.SetActive(true);
            numRemoveExtralObj.gameObject.SetActive(false);
        }
    }
    public void HandleCompleteExtral(ExtralPlayType extralType)
    {
        if (listActiveExtrals != null)
        {
            for (int i = 0; i < listActiveExtrals.Count; i++)
            {
                if (listActiveExtrals[i].type == extralType)
                {
                    listActiveExtrals.RemoveAt(i);
                    break;
                }
            }
            SetupExtralIcons();
            SetAdIcon();
            CheckExtrals();
        }


    }
    public void ActiveExtral(bool isActive)
    {
        // StopAllCoroutines();
      

        if (extralBtn.gameObject.activeSelf)
        {
            if (isActive)
            {
                if (isExtralSuccess)
                {
                    extralBtn.interactable = false;
                    extralCombinationBtn.interactable = false;
                }
                else
                {
                    extralBtn.interactable = true;
                    extralCombinationBtn.interactable = true;
                }
            }
            else
            {
                extralBtn.interactable = false;
                extralCombinationBtn.interactable = false;
            }
        }

        if (extralCombinationBtn.gameObject.activeSelf)
        {
            if (isActive)
            {
                if (isExtralSuccess)
                {
                    extralCombinationBtn.interactable = false;
                }
                else
                {
                    extralCombinationBtn.interactable = true;
                }
            }
            else
            {
                extralCombinationBtn.interactable = false;
            }
        }
    }
    public void OnClickBuyExtral(UnityAction actionExtralDone)
    {
        GameController.Instance.musicManager.PlayClickSound();

        if (numOfExtral > 0)
        {
            switch (currentExtralType)
            {
                case ExtralPlayType.Boom:
                    UseProfile.CurrentNumRemoveBomb--;
                    if (UseProfile.CurrentNumRemoveBomb < 0)
                        UseProfile.CurrentNumRemoveBomb = 0;
                    break;
                case ExtralPlayType.EggLock:
                    numOfExtral = UseProfile.CurrentNumRemoveEgg--;
                    if (UseProfile.CurrentNumRemoveEgg < 0)
                        UseProfile.CurrentNumRemoveEgg = 0;
                    break;
                case ExtralPlayType.LockBird:
                    numOfExtral = UseProfile.CurrentNumRemoveCage--;
                    if (UseProfile.CurrentNumRemoveCage < 0)
                        UseProfile.CurrentNumRemoveCage = 0;
                    break;
                case ExtralPlayType.LockStand:
                    numOfExtral = UseProfile.CurrentNumRemoveJail--;
                    if (UseProfile.CurrentNumRemoveJail < 0)
                        UseProfile.CurrentNumRemoveJail = 0;
                    break;
                case ExtralPlayType.SleepBird:
                    numOfExtral = UseProfile.CurrentNumRemoveSleep--;
                    if (UseProfile.CurrentNumRemoveSleep < 0)
                        UseProfile.CurrentNumRemoveSleep = 0;
                    break;
            }

            extralBtn.interactable = false;
            isExtralSuccess = true;
            actionExtralDone?.Invoke();
            UpdateNumRemoveExtral();
        }
        else
        {
            GameController.Instance.admobAds.ShowVideoReward(
            actionReward: () =>
            {
                extralBtn.interactable = false;
                isExtralSuccess = true;
                actionExtralDone?.Invoke();
            },
            actionNotLoadedVideo: () =>
            {
                GameController.Instance.moneyEffectController.SpawnEffectText_FlyUp
                 (
                 extralBtn.transform.position,
                 "No video at the moment!",
                 Color.white,
                 isSpawnItemPlayer: true
                 );
            },
            actionClose: null,
            ActionWatchVideo.BuyExtral,
           UseProfile.CurrentLevel.ToString());
        }
    }

    #endregion

    private void OnDestroy()
    {
        EventDispatcher.EventDispatcher.Instance.RemoveListener(EventID.CHANGE_ADD_PILE, HandleShowTvAddPile);
        EventDispatcher.EventDispatcher.Instance.RemoveListener(EventID.CHANGE_REDO, HandleShowTvReturn);
        EventDispatcher.EventDispatcher.Instance.RemoveListener(EventID.CHANGE_REMOVE_BOOM, HandleShowTvRemoveBoom);
        EventDispatcher.EventDispatcher.Instance.RemoveListener(EventID.CHANGE_REMOVE_CAGE, HandleShowTvRemoveCage);
    }

    private Camera camera;
    public float weight;
    public float height;
    public void HandleCanculateCamera()
    {
       
        weight = Screen.width;
        height = Screen.height;
        var temp = Math.Round(weight / (float)height, 2);

        camera = Camera.main;
        if (temp < 0.48f)
        {

            camera.orthographicSize = 4f;
        }
        if (temp <= 0.46f)
        {

            camera.orthographicSize = 4f;
            if (temp <= 0.43f)
            {
                camera.orthographicSize = 4.7f;
            }    
        }
        Debug.LogError("HandleCanculateCamera " + temp);
    }
    public GameObject testPanel;
    public bool isOff = false;
    public List<Image> lsImage;
    public List<Text> lsTv;
     
    public void OpenTest()
    {
        if (testPanel.activeSelf)
        {
            testPanel.SetActive(false);
        }
        else
        {
            testPanel.SetActive(true);
        }
    }
    public InputField input;
    public void OnClickPasslevel()
    {
        var level = Int32.Parse(input.text);
        UseProfile.CurrentLevel = level;
        SceneManager.LoadScene("GamePlay");
    }
    public void OnOffUI()
    {
        if (!isOff)
        {
            isOff = true;
            foreach (var item in lsImage)
            {
                item.color = new Color32(0, 0, 0, 0);
            }
            foreach (var item in lsTv)
            {
                item.color = new Color32(0, 0, 0, 0);
            }
         
        }
        else
        {
            isOff = false;
            foreach (var item in lsImage)
            {
                item.color = new Color32(255, 255, 255, 255);
            }
            foreach (var item in lsTv)
            {
                item.color = Color.black;
            }
          
        }
    }
    public void CheatBooster()
    {
        UseProfile.RedoBooster += 20;
        UseProfile.AddPileBooster += 20;
        UseProfile.CurrentNumRemoveCage += 20;
        UseProfile.CurrentNumRemoveBomb += 20;
    }
}
public class HomeExtral
{
    public ExtralPlayType type;
    public int quantity;
    public Sprite icon;
    public UnityAction actionExtralDone;
    public HomeExtral(ExtralPlayType type, int quantity, Sprite icon, UnityAction actionExtralDone)
    {
        this.type = type;
        this.quantity = quantity;
        this.icon = icon;
        this.actionExtralDone = actionExtralDone;
    }
}