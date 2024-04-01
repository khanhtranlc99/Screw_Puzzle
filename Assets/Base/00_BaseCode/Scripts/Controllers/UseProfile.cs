using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MoreMountains.NiceVibrations;

public class UseProfile : MonoBehaviour
{
    public static bool FirstShowOpenAds
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.FIRST_SHOW_OPEN_ADS, 0) == 1;
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.FIRST_SHOW_OPEN_ADS, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    public static int CurrentNumRemoveBomb
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.CURRENT_NUM_REMOVE_BOMB, RemoteConfigController.GetIntConfig(FirebaseConfig.DEFAULT_NUM_REMOVE_BOMB, 2));
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.CURRENT_NUM_REMOVE_BOMB, value);
            PlayerPrefs.Save();
            EventDispatcher.EventDispatcher.Instance.PostEvent(EventID.CHANGE_REMOVE_BOOM);
        }
    }
    public static int CurrentNumRemoveEgg
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.CURRENT_NUM_REMOVE_EGG, RemoteConfigController.GetIntConfig(FirebaseConfig.DEFAULT_NUM_REMOVE_EGG, 0));
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.CURRENT_NUM_REMOVE_EGG, value);
            PlayerPrefs.Save();

        }
    }
    public static int CurrentNumRemoveCage
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.CURRENT_NUM_REMOVE_CAGE, RemoteConfigController.GetIntConfig(FirebaseConfig.DEFAULT_NUM_REMOVE_CAGE, 2));
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.CURRENT_NUM_REMOVE_CAGE, value);
            PlayerPrefs.Save();
            EventDispatcher.EventDispatcher.Instance.PostEvent(EventID.CHANGE_REMOVE_CAGE);
        }
    }
    public static int CurrentNumRemoveJail
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.CURRENT_NUM_REMOVE_JAIL, RemoteConfigController.GetIntConfig(FirebaseConfig.DEFAULT_NUM_REMOVE_JAIL, 0));
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.CURRENT_NUM_REMOVE_JAIL, value);
            PlayerPrefs.Save();

        }
    }
    public static int CurrentNumRemoveSleep
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.CURRENT_NUM_REMOVE_SLEEP, RemoteConfigController.GetIntConfig(FirebaseConfig.DEFAULT_NUM_REMOVE_SLEEP, 0));
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.CURRENT_NUM_REMOVE_SLEEP, value);
            PlayerPrefs.Save();

        }
    }
    public static bool FirstLoading
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.LOADING_COMPLETE, 0) == 1;
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.LOADING_COMPLETE, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public static int CurrentLevel
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.CURRENT_LEVEL, 1);
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.CURRENT_LEVEL, value);
            PlayerPrefs.Save();
        }
    }
    public static int EggRanking
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.SCORE_RANKING, 0);
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.SCORE_RANKING, value);
            PlayerPrefs.Save();
        }
    }

    #region ChestDataKey
    public static int EggChest
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.EGG_CHEST, 0);
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.EGG_CHEST, value);
            PlayerPrefs.Save();
        }
    }
    public static int LevelEggChest
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.LEVEL_EGG_CHEST, 1);
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.LEVEL_EGG_CHEST, value);
            PlayerPrefs.Save();
        }
    }
    public static int LevelOfLevelChest
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.LEVEL_OF_LEVEL_CHEST, 0);
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.LEVEL_OF_LEVEL_CHEST, value);
            PlayerPrefs.Save();
        }
    }
    public static int LevelOfBirdChest
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.LEVEL_OF_BIRD_CHEST, 0);
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.LEVEL_OF_BIRD_CHEST, value);
            PlayerPrefs.Save();
        }
    }

    public static int CurrentLevelOfLevelChest
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.CURRENT_LEVEL_OF_LEVEL_CHEST, 0);
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.CURRENT_LEVEL_OF_LEVEL_CHEST, value);
            PlayerPrefs.Save();
        }
    }
    public static int CurrentLevelOfBirdChest
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.CURRENT_LEVEL_OF_BIRD_CHEST, 0);
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.CURRENT_LEVEL_OF_BIRD_CHEST, value);
            PlayerPrefs.Save();
        }
    }
    #endregion

    public static int Coin
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.COIN, 0);
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.COIN, value);
            PlayerPrefs.Save();
            EventDispatcher.EventDispatcher.Instance.PostEvent(EventID.CHANGE_COIN);
        }
    }
    public static int Heart
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.HEART, 0);
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.HEART, value);
            PlayerPrefs.Save();
        }
    }

    public static int AddPileBooster
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.ADD_PILE_BOOSTER, 2);
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.ADD_PILE_BOOSTER, value);
            PlayerPrefs.Save();
            EventDispatcher.EventDispatcher.Instance.PostEvent(EventID.CHANGE_ADD_PILE);

        }
    }
    public static int RedoBooster
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.REDO_BOOSTER, 2);
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.REDO_BOOSTER, value);
            PlayerPrefs.Save();
            EventDispatcher.EventDispatcher.Instance.PostEvent(EventID.CHANGE_REDO);
        }
    }


    public static int NumberOfDisplayedInterstitialD0_D1
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.NUMBER_OF_DISPLAYED_INTERST_ITIAL_D0_D1_KEY, 0);
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.NUMBER_OF_DISPLAYED_INTERST_ITIAL_D0_D1_KEY, value);
            PlayerPrefs.Save();

        }
    }

    public static int NumberOfDisplayedInterstitialD1
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.NUMBER_OF_DISPLAYED_INTERST_ITIAL_D1_KEY, 0);
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.NUMBER_OF_DISPLAYED_INTERST_ITIAL_D1_KEY, value);
            PlayerPrefs.Save();

        }
    }

    public static int NumberRewardShowed
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.NUMBER_REWARD_SHOWED, 0);
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.NUMBER_REWARD_SHOWED, value);
            PlayerPrefs.Save();

        }
    }
    public static int NumberInterShowed
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.NUMBER_INTER_SHOWED, 0);
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.NUMBER_INTER_SHOWED, value);
            PlayerPrefs.Save();

        }
    }

    public int LevelUnlock
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.CURRENT_LEVEL_PLAY, 1);
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.CURRENT_LEVEL_PLAY, value);
            PlayerPrefs.Save();
        }
    }
    public bool IsRemoveAds
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.REMOVE_ADS, 0) == 1;
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.REMOVE_ADS, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    public bool OnVibration
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.ONOFF_VIBRATION, 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.ONOFF_VIBRATION, value ? 1 : 0);
            MMVibrationManager.SetHapticsActive(value);
            PlayerPrefs.Save();
        }
    }
    public bool OnSound
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.ONOFF_SOUND, 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.ONOFF_SOUND, value ? 1 : 0);
            GameController.Instance.musicManager.SetSoundVolume(value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    public bool OnMusic
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.ONOFF_MUSIC, 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.ONOFF_MUSIC, value ? 1 : 0);
            GameController.Instance.musicManager.SetMusicVolume(value ? 0.7f : 0);
            PlayerPrefs.Save();
        }
    }
    public static bool IsFirstTimeInstall
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.FIRST_TIME_INSTALL, 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.FIRST_TIME_INSTALL, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    public static int RetentionD
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.RETENTION_D, 0);
        }
        set
        {
            if (value < 0)
                value = 0;

            PlayerPrefs.SetInt(StringHelper.RETENTION_D, value);
            PlayerPrefs.Save();
        }
    }
    public static int DaysPlayed
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.DAYS_PLAYED, 1);
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.DAYS_PLAYED, value);
            PlayerPrefs.Save();
        }
    }
    public static int PayingType
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.PAYING_TYPE, 0);
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.PAYING_TYPE, value);
            PlayerPrefs.Save();
        }
    }
    public static DateTime FirstTimeOpenGame
    {
        get
        {
            if (PlayerPrefs.HasKey(StringHelper.FIRST_TIME_OPEN_GAME))
            {
                var temp = Convert.ToInt64(PlayerPrefs.GetString(StringHelper.FIRST_TIME_OPEN_GAME));
                return DateTime.FromBinary(temp);
            }
            else
            {
                var newDateTime = UnbiasedTime.Instance.Now().AddDays(-1);
                PlayerPrefs.SetString(StringHelper.FIRST_TIME_OPEN_GAME, newDateTime.ToBinary().ToString());
                PlayerPrefs.Save();
                return newDateTime;
            }
        }
        set
        {
            PlayerPrefs.SetString(StringHelper.FIRST_TIME_OPEN_GAME, value.ToBinary().ToString());
            PlayerPrefs.Save();
        }
    }
    public static DateTime LastTimeOpenGame
    {
        get
        {
            if (PlayerPrefs.HasKey(StringHelper.LAST_TIME_OPEN_GAME))
            {
                var temp = Convert.ToInt64(PlayerPrefs.GetString(StringHelper.LAST_TIME_OPEN_GAME));
                return DateTime.FromBinary(temp);
            }
            else
            {
                var newDateTime = UnbiasedTime.Instance.Now().AddDays(-1);
                PlayerPrefs.SetString(StringHelper.LAST_TIME_OPEN_GAME, newDateTime.ToBinary().ToString());
                PlayerPrefs.Save();
                return newDateTime;
            }
        }
        set
        {
            PlayerPrefs.SetString(StringHelper.LAST_TIME_OPEN_GAME, value.ToBinary().ToString());
            PlayerPrefs.Save();
        }
    }
    public static DateTime LastTimeResetSalePackShop
    {
        get
        {
            if (PlayerPrefs.HasKey(StringHelper.LAST_TIME_RESET_SALE_PACK_SHOP))
            {
                var temp = Convert.ToInt64(PlayerPrefs.GetString(StringHelper.LAST_TIME_RESET_SALE_PACK_SHOP));
                return DateTime.FromBinary(temp);
            }
            else
            {
                var newDateTime = UnbiasedTime.Instance.Now().AddDays(-1);
                PlayerPrefs.SetString(StringHelper.LAST_TIME_RESET_SALE_PACK_SHOP, newDateTime.ToBinary().ToString());
                PlayerPrefs.Save();
                return newDateTime;
            }
        }
        set
        {
            PlayerPrefs.SetString(StringHelper.LAST_TIME_RESET_SALE_PACK_SHOP, value.ToBinary().ToString());
            PlayerPrefs.Save();
        }
    }
    public static bool CanShowRate
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.CAN_SHOW_RATE, 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.CAN_SHOW_RATE, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    public static int NumberOfAdsInPlay;
    public static int NumberOfAdsInDay
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.NUMBER_OF_ADS_IN_DAY, 0);
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.NUMBER_OF_ADS_IN_DAY, value);
            PlayerPrefs.Save();
        }
    }
    public static DateTime LastTimeOnline
    {
        get
        {
            if (PlayerPrefs.HasKey(StringHelper.LAST_TIME_ONLINE))
            {
                var temp = Convert.ToInt64(PlayerPrefs.GetString(StringHelper.LAST_TIME_ONLINE));
                return DateTime.FromBinary(temp);
            }
            else
            {
                var newDateTime = UnbiasedTime.Instance.Now().AddDays(-1);
                PlayerPrefs.SetString(StringHelper.LAST_TIME_ONLINE, newDateTime.ToBinary().ToString());
                PlayerPrefs.Save();
                return newDateTime;
            }
        }
        set
        {
            PlayerPrefs.SetString(StringHelper.LAST_TIME_ONLINE, value.ToBinary().ToString());
            PlayerPrefs.Save();
        }
    }

    public string ListSave
    {
        get
        {
            return PlayerPrefs.GetString(GameData.LEVEL_SAVE);
        }
        set
        {
            PlayerPrefs.SetString(GameData.LEVEL_SAVE, value);
            PlayerPrefs.Save();
        }
    }
    public void SetDateTimeReciveDailyGift(DateTime value)
    {
        PlayerPrefs.SetString(StringHelper.DATE_RECIVE_GIFT_DAILY, value.ToBinary().ToString());
    }
    public DateTime GetDateTimeReciveDailyGift()
    {
        return GetDateTime(StringHelper.DATE_RECIVE_GIFT_DAILY, DateTime.MinValue);
    }
    public DateTime GetDateTime(string key, DateTime defaultValue)
    {
        string @string = PlayerPrefs.GetString(key);
        DateTime result = defaultValue;
        if (!string.IsNullOrEmpty(@string))
        {
            long dateData = Convert.ToInt64(@string);
            result = DateTime.FromBinary(dateData);
        }
        return result;
    }

    public static bool IsShowTutRedo
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.IS_SHOW_TUT_REDO, 0) == 1;
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.IS_SHOW_TUT_REDO, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    public static bool IsShowTutAddPile
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.IS_SHOW_TUT_ADD_PILE, 0) == 1;
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.IS_SHOW_TUT_ADD_PILE, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public static bool IsShowTutExtralBoom
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.IS_SHOW_TUT_EXTRAL_BOOM, 0) == 1;
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.IS_SHOW_TUT_EXTRAL_BOOM, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    public static bool IsShowTutExtralLock
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.IS_SHOW_TUT_EXTRAL_LOCK, 0) == 1;
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.IS_SHOW_TUT_EXTRAL_LOCK, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public static bool IsShowTutRemoveBoomBooster
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.IS_SHOW_TUT_REMOVE_BOOM_BOOSTER, 0) == 1;
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.IS_SHOW_TUT_REMOVE_BOOM_BOOSTER, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    public static bool IsShowTutRemoveLockBooster
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.IS_SHOW_TUT_REMOVE_LOCK_BOOSTER, 0) == 1;
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.IS_SHOW_TUT_REMOVE_LOCK_BOOSTER, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
}


