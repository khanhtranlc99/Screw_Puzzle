using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[CreateAssetMenu(menuName = "Datas/GiftDatabase", fileName = "GiftDatabase.asset")]
public class GiftDatabase : SerializedScriptableObject
{
    public Dictionary<GiftType, Gift> giftList;

    public bool GetGift(GiftType giftType, out Gift gift)
    {
        return giftList.TryGetValue(giftType, out gift);
    }

    public Sprite GetIconItem(GiftType giftType)
    {
        Gift gift = null;
        //if (IsCharacter(giftType))
        //{
        //    var Char = GameController.Instance.dataContain.dataSkins.GetSkinInfo(giftType);
        //    if (Char != null)
        //        return Char.iconSkin;
        //}
        bool isGetGift = GetGift(giftType, out gift);
        return isGetGift ? gift.getGiftSprite : null;
    }
    public GameObject GetAnimItem(GiftType giftType)
    {
        Gift gift = null;
        bool isGetGift = GetGift(giftType, out gift);
        return isGetGift ? gift.getGiftAnim : null;
    }

    public void Claim(GiftType giftType, int amount, Reason reason = Reason.none)
    {

        switch (giftType)
        {
            case GiftType.Coin:
                UseProfile.Coin += amount;
                EventDispatcher.EventDispatcher.Instance.PostEvent(EventID.CHANGE_COIN);
                break;
            case GiftType.Heart:
                UseProfile.Heart += amount;
                break;
            case GiftType.RemoveAds:
                GameController.Instance.admobAds.DestroyBanner();
                GameController.Instance.admobAds.lockShowOpenAppAds = false;
                GameController.Instance.useProfile.IsRemoveAds = true;
                EventDispatcher.EventDispatcher.Instance.PostEvent(EventID.REMOVE_ADS);
                List<GiftRewardShow> giftRewardShows = new List<GiftRewardShow>();
                giftRewardShows.Add(new GiftRewardShow() { amount = 1, type = GiftType.RemoveAds });
                PopupRewardBase.Setup().Show(giftRewardShows, delegate { GameController.Instance.admobAds.lockShowOpenAppAds = true; });
                break;
            case GiftType.AddPileBooster:
                UseProfile.AddPileBooster += amount;
             
            
                break;
            case GiftType.RedoBooster:
                UseProfile.RedoBooster += amount;
             
                break;
            case GiftType.RemoveBoomBooster:
                UseProfile.CurrentNumRemoveBomb += amount;

                break;

            case GiftType.RemoveCageBooster:
                UseProfile.CurrentNumRemoveCage += amount;

                break;

        }
    }

    public static bool IsCharacter(GiftType giftType)
    {
        //switch (giftType)
        //{
        //    case GiftType.RandomSkin:
        //        return true;
        //}
        return false;
    }
}

public class Gift
{
    [SerializeField] private Sprite giftSprite;
    [SerializeField] private GameObject giftAnim;
    public virtual Sprite getGiftSprite => giftSprite;
    public virtual GameObject getGiftAnim => giftAnim;

}

public enum GiftType
{
    None = 0,
    RemoveAds = 1,
    Coin = 2,
    Heart = 3,
    AddPileBooster = 4,
    RedoBooster = 5,
    RemoveBoomBooster = 6,
    RemoveCageBooster = 7



}

public enum Reason
{
    none = 0,
    play_with_item = 1,
    watch_video_claim_item_main_home = 2,
    daily_login = 3,
    lucky_spin = 4,
    unlock_skin_in_special_gift = 5,
    reward_accumulate = 6,
}

[Serializable]
public class RewardRandom
{
    public int id;
    public GiftType typeItem;
    public int amount;
    public int weight;

    public RewardRandom()
    {
    }
    public RewardRandom(GiftType item, int amount, int weight = 0)
    {
        this.typeItem = item;
        this.amount = amount;
        this.weight = weight;
    }

    public GiftRewardShow GetReward()
    {
        GiftRewardShow rew = new GiftRewardShow();
        rew.type = typeItem;
        rew.amount = amount;

        return rew;
    }
}
