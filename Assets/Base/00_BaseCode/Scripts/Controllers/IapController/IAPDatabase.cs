using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Events;
using Newtonsoft.Json;
public enum TypePackIAP
{
    NoAdsCoinPack = 0,
    NoAdsHeartPack = 1,
    NoAdsPack = 2,
    GoodsPass = 3,
    SuperGamePack = 4,
    Special_Pack = 5,
    ValuePack = 6,
    ShinyPack = 7,
    HugePack = 8,
    MegaPack = 9,
    MasterPack = 10,
    CoinPack_500 = 11,
    CoinPack_1000 = 12,



}


[CreateAssetMenu(menuName = "ScriptableObject/IAPDatabase", fileName = "IAPDatabase.asset")]
public class IAPDatabase : SerializedScriptableObject
{
    public List<IAPPack> lstPacksInapp;
    public List<IAPPack> lstPacksNotInapp;

    public IAPPack GetPack(TypePackIAP type)
    {
        for (int i = 0; i < lstPacksInapp.Count; i++)
        {
            if (lstPacksInapp[i].type != type) continue;

            return lstPacksInapp[i];
        }

        return null;
    }


    public IAPPack GetPackNotInapp(TypePackIAP type)
    {
        for (int i = 0; i < lstPacksNotInapp.Count; i++)
        {
            if (lstPacksNotInapp[i].type != type) continue;

            return lstPacksNotInapp[i];
        }

        return null;
    }

    public IAPPack GetPackAll(TypePackIAP type)
    {
        var pack = GetPack(type);
        if (pack == null)
            pack = GetPackNotInapp(type);

        return pack;
    }
}

public class IAPPack
{
    private const string SALE = "sale";

    public string namePack;
    public TypePackIAP type;
    public ProductType productType;
    public TypeBuy typeBuy;
    [HideInInspector] public bool isNotInappPack { get { return typeBuy != TypeBuy.Inapp ? true : false; } }
    public string shortID;
    private UnityAction actClaimDone;
    public string ProductID
    {
        get
        {
            return shortID;
        }
    }
    public string ProductID_Origin
    {
        get
        {
            return string.Format("{0}.{1}.{2}", ConfigGameBase.package_name_2, shortID, SALE);
        }
    }

    //Đã mua hay chưa
    public bool IsBought
    {
        get
        {
            return PlayerPrefs.GetInt("Is_Buy_" + ProductID, 0) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("Is_Buy_" + ProductID, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public Dictionary<GiftType, int> itemsResult;//Các Item nhận được sau khi mua Pack
    //                Kiểu     Số lượng

    [HideIf("typeBuy", TypeBuy.Free)] public string defaultPrice;
    [ShowIf("isNotInappPack")][HideIf("typeBuy", TypeBuy.Free)] public int price;
    public string tittle;
    public Sprite icon;

    public bool isSale;
    [ShowIf("isSale", true)] public string idSale;
    [ShowIf("isSale", true)] public float percentSale;
    public void Claim(bool isIapInited = true)
    {
        int value = 0;
        GiftType typeItem = GiftType.Coin;

        foreach (var item in itemsResult)
        {
            switch (type)
            {
            
                case TypePackIAP.NoAdsCoinPack:
                    break;
                case TypePackIAP.NoAdsHeartPack:

                    break;
                case TypePackIAP.NoAdsPack:
                    GameController.Instance.useProfile.IsRemoveAds = true;
                    //GameController.Instance.admobAds.DestroyBanner();
                    EventDispatcher.EventDispatcher.Instance.PostEvent(EventID.REMOVE_ADS);
              
                    break;

      
            }

        }
        if (typeBuy == TypeBuy.Coin)
        {
            List<GiftRewardShow> lstReward = new List<GiftRewardShow>();
            foreach (var item in itemsResult)
            {
                GameController.Instance.dataContain.giftDatabase.Claim(item.Key, item.Value);

                GiftRewardShow rw = new GiftRewardShow();
                rw.type = item.Key;
                rw.amount = item.Value;

                lstReward.Add(rw);
            }
            if (lstReward.Count <= 1)
            {
                RewardIAPBox.Setup2().Show(lstReward, actionClaim: () => { actClaimDone?.Invoke(); });
            }
            else
            {
                RewardIAPBox.Setup2(true).Show(lstReward, actionClaim: () => { actClaimDone?.Invoke(); });
            }

        }

        if (typeBuy == TypeBuy.Video)
        {
            List<GiftRewardShow> lstReward = new List<GiftRewardShow>();
            foreach (var item in itemsResult)
            {
                GameController.Instance.dataContain.giftDatabase.Claim(item.Key, item.Value);

                GiftRewardShow rw = new GiftRewardShow();
                rw.type = item.Key;
                rw.amount = item.Value;

                lstReward.Add(rw);
            }
            if (lstReward.Count <= 1)
            {
                RewardIAPBox.Setup2().Show(lstReward, actionClaim: () => { actClaimDone?.Invoke(); });
            }
            else
            {
                RewardIAPBox.Setup2(true).Show(lstReward, actionClaim: () => { actClaimDone?.Invoke(); });
            }

        }
        else
        {
            List<GiftRewardShow> lstReward = new List<GiftRewardShow>();
            foreach (var item in itemsResult)
            {
                GameController.Instance.dataContain.giftDatabase.Claim(item.Key, item.Value);
            }
                
            if (typeBuy == TypeBuy.Inapp && productType == ProductType.NonConsumable)
            {
                if (isIapInited)
                {
                    if (!IsBought)
                    {
                        if (lstReward.Count <= 1)
                        {
                            //  RewardIAPBox.Setup2().Show(lstReward, actionClaim: () => { actClaimDone?.Invoke(); });
                           // NotificationPopup.instance.AddNotification("Buy Success!");
                        }
                        else
                        {
                            //  RewardIAPBox.Setup2(true).Show(lstReward, actionClaim: () => { actClaimDone?.Invoke(); });
                        //    NotificationPopup.instance.AddNotification("Buy Success!");
                        }
                    }
                }
            }
            else
            {
                if (lstReward.Count <= 1)
                {
                    // RewardIAPBox.Setup2().Show(lstReward, actionClaim: () => { actClaimDone?.Invoke(); });
                 //   NotificationPopup.instance.AddNotification("Buy Success!");
                }
                else
                {
                    //   RewardIAPBox.Setup2(true).Show(lstReward, actionClaim: () => { actClaimDone?.Invoke(); });
                   // NotificationPopup.instance.AddNotification("Buy Success!");
                }
            }    
        
            IsBought = true;
        }
    }

    public UnityAction ActClaimDone
    {
        set
        {
            actClaimDone = value;
        }
    }

    public int GetAmount(GiftType itmName)
    {
        int amount = 0;
        if (itemsResult.TryGetValue(itmName, out amount))
        {
            return amount;
        }

        return amount;
    }
}