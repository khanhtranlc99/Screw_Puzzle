using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class RandomWatchVideo : MonoBehaviour
{
    public static int CountNumberWatchVideoInShop
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.COUNT_NUMBER_WATCH_VIDEO_IN_SHOP, 5);
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.COUNT_NUMBER_WATCH_VIDEO_IN_SHOP, value);
            PlayerPrefs.Save();
       
        }
    }
    public static DateTime LastTimeCountAds
    {
        get
        {
            if (PlayerPrefs.HasKey("LastTimeCountAds"))
            {
                var temp = Convert.ToInt64(PlayerPrefs.GetString("LastTimeCountAds"));
                return DateTime.FromBinary(temp);
            }
            else
            {
                var newDateTime = UnbiasedTime.Instance.Now();
                PlayerPrefs.SetString("LastTimeCountAds", newDateTime.ToBinary().ToString());
                PlayerPrefs.Save();
                return newDateTime;
            }
        }
        set
        {
            PlayerPrefs.SetString("LastTimeCountAds", value.ToBinary().ToString());
            PlayerPrefs.Save();
        }
    }
    //public Text tvCount;
    public Text tvCountTime;
  //  public GameObject iconDecor;
    public bool wasCountTime = false;
    private float timeCoolDown;
    public Button btnBuy;
    public List<RandomGift> lsRandomGifts;
    public  void Init()
    {
     //   tvCount.text = "" + CountNumberWatchVideoInShop;
        CheckOnOffButtonBuy();
        btnBuy.onClick.AddListener(delegate { OnButtonBuyClick(); });
    }

    public  void OnButtonBuyClick()
    {
        HandleOnClick();


    }

    private void HandleTackGift()
    {

        var temp = UnityEngine.Random.RandomRange(0,lsRandomGifts.Count);
        var gift = lsRandomGifts[temp];
        List<GiftRewardShow> lstReward = new List<GiftRewardShow>();
        lstReward.Add(new GiftRewardShow() { amount = gift.amount , type = gift.giftType });
        RewardIAPBox.Setup2().Show(lstReward);

    }






    private void HandleOnClick()
    {
        GameController.Instance.musicManager.PlayClickSound();
        CountNumberWatchVideoInShop -= 1;
      //  tvCount.text = "" + CountNumberWatchVideoInShop;

   

        CheckOnOffButtonBuy();
        HandleTackGift();
    }

    private void CheckOnOffButtonBuy()
    {
        timeCoolDown = TimeManager.CaculateTime(UnbiasedTime.Instance.Now(), LastTimeCountAds);
        Debug.Log("timeCoolDown " + timeCoolDown);
        if(timeCoolDown > 0)
        {
            btnBuy.interactable = false;
          //  iconDecor.gameObject.SetActive(false);
            tvCountTime.text = TimeManager.ShowTime2((long)timeCoolDown);
            wasCountTime = true;
        }
        else
        {
            btnBuy.interactable = true;
            tvCountTime.text = "";
           // iconDecor.gameObject.SetActive(true);
            if(CountNumberWatchVideoInShop == 0)
            {
                CountNumberWatchVideoInShop = 10;
            }
        }
        
    }
    private void Update()
    {
        if(wasCountTime)
        {
            timeCoolDown -= Time.unscaledDeltaTime;
            tvCountTime.text = TimeManager.ShowTime2((long)timeCoolDown);
            if (timeCoolDown <= 0)
            {
                wasCountTime = false;
                btnBuy.interactable = true;
                tvCountTime.text = "";
              //  iconDecor.gameObject.SetActive(true);
                if (CountNumberWatchVideoInShop == 0)
                {
                    CountNumberWatchVideoInShop = 5;
                 //   tvCount.text = "" + CountNumberWatchVideoInShop;
                }
            }
        }
       
    }

   


}
[Serializable]
public class RandomGift
{
    
    public GiftType giftType;
    public int amount;
}