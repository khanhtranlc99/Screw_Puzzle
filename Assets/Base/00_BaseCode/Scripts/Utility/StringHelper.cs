using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringHelper
{
    public const string FIRST_SHOW_OPEN_ADS = "first_show_open_ads";
    public const string LOADING_COMPLETE = "LOADING_COMPLETE";
    public const string ONOFF_SOUND = "ONOFF_SOUND";
    public const string ONOFF_MUSIC = "ONOFF_MUSIC";
    public const string ONOFF_VIBRATION = "ONOFF_VIBRATION";
    public const string FIRST_TIME_INSTALL = "FIRST_TIME_INSTALL";
    public const string VERSION_FIRST_INSTALL = "VERSION_FIRST_INSTALL";
    public const string REMOVE_ADS = "REMOVE_ADS";
    public const string CURRENT_LEVEL = "CURRENT_LEVEL";
    public const string CURRENT_LEVEL_PLAY = "CURRENT_LEVEL_PLAY";
    public const string PATH_CONFIG_LEVEL = "Levels/Level_";
    public const string PATH_CONFIG_LEVEL_TEST = "LevelTest/Level_{0}";
    public const string PATH_CONFIG_LEVEL_SPECIAL = "SpecialLevel/Level_{0}";
    public const string LEVEL_DEMO_IMAGE_WIN = "Demo/lv";
    public const string LEVEL_DEMO_IMAGE_NOT_WIN = "Demo/bw_lv";
    public const string ANIM_LEVEL = "AnimLevel/AnimLv";
    public const string SALE_IAP = "_sale";
    public const string RETENTION_D = "retent_type";
    public const string DAYS_PLAYED = "days_played";
    public const string PAYING_TYPE = "retent_type";
    public const string LEVEL = "level";
    public const string LAST_TIME_OPEN_GAME = "LAST_TIME_OPEN_GAME";
    public const string FIRST_TIME_OPEN_GAME = "FIRST_TIME_OPEN_GAME";
    public const string CAN_SHOW_RATE = "CAN_SHOW_RATE";
    public const string NUMBER_OF_ADS_IN_DAY = "NUMBER_OF_ADS_IN_DAY";
    public const string NUMBER_OF_ADS_IN_PLAY = "NUMBER_OF_ADS_IN_PLAY";
    public const string IS_PACK_PURCHASED_ = "IS_PACK_PURCHASED_";
    public const string IS_PACK_ACTIVATED_ = "IS_PACK_ACTIVATED_";
    public const string LAST_TIME_PACK_ACTIVE_ = "LAST_TIME_PACK_ACTIVE_";
    public const string LAST_TIME_PACK_SHOW_HOME_ = "LAST_TIME_PACK_SHOW_HOME_";
    public const string STARTER_PACK_IS_COMPLETED = "STARTER_PACK_IS_COMPLETED";
    public const string LAST_TIME_RESET_SALE_PACK_SHOP = "LAST_TIME_RESET_SALE_PACK_SHOP";
    public const string LAST_TIME_ONLINE = "LAST_TIME_ONLINE";
    public const string CURRENT_ID_MINI_GAME = "current_id_mini_game";
    public const string ITEM_HINT = "item_hint";
    public const string DATE_RECIVE_GIFT_DAILY = "date_recive_gift_daily";

    public const string EGG_CHEST = "egg_chest";
    public const string CURRENT_LEVEL_OF_LEVEL_CHEST = "current_level_of_level_chest";
    public const string CURRENT_LEVEL_OF_BIRD_CHEST = "current_level_of_bird_chest";
    public const string LEVEL_EGG_CHEST = "level_egg_chest";
    public const string LEVEL_OF_LEVEL_CHEST = "level_of_level_chest";
    public const string LEVEL_OF_BIRD_CHEST = "level_of_bird_chest";
    public const string SCORE_RANKING = "score_ranking";
    public const string COIN = "coin";
    public const string HEART = "heart";
    public const string SPECIAL_FEATHER = "special_feather";
    public const string REDO_BOOSTER = "redo_booster";
    public const string ADD_PILE_BOOSTER = "add_pile_booster";
    public const string COUNT_NUMBER_WATCH_VIDEO_IN_SHOP = "count_number_watch_video_in_shop";
    public const string IS_DONE_TUT = "is_done_tut";

    public const string NUMBER_OF_DISPLAYED_INTERST_ITIAL_D0_D1_KEY = "number_of_displayed_interst_itial_d0_d1_key";
    public const string NUMBER_OF_DISPLAYED_INTERST_ITIAL_D1_KEY = "number_of_displayed_interst_itial_d1_key";

    public const string NUMBER_REWARD_SHOWED = "number_reward_showed";
    public const string NUMBER_INTER_SHOWED = "number_inter_showed";

    public const string CURRENT_NUM_RETURN = "CURRENT_NUM_RETURN";
    public const string CURRENT_NUM_ADD_STAND = "CURRENT_NUM_ADD_STAND";
    public const string CURRENT_NUM_REMOVE_BOMB = "CURRENT_NUM_REMOVE_BOMB";
    public const string CURRENT_NUM_REMOVE_CAGE = "CURRENT_NUM_REMOVE_CAGE";
    public const string CURRENT_NUM_REMOVE_EGG = "CURRENT_NUM_REMOVE_EGG";
    public const string CURRENT_NUM_REMOVE_SLEEP = "CURRENT_NUM_REMOVE_SLEEP";
    public const string CURRENT_NUM_REMOVE_JAIL = "CURRENT_NUM_REMOVE_JAIL";

    public const string IS_SHOW_TUT_REDO = "is_show_tut_redo";
    public const string IS_SHOW_TUT_ADD_PILE = "is_show_tut_add_pile";
    public const string IS_SHOW_TUT_EXTRAL_BOOM = "is_show_tut_extral_boom";
    public const string IS_SHOW_TUT_EXTRAL_LOCK = "is_show_tut_extral_lock";
    public const string IS_SHOW_TUT_REMOVE_BOOM_BOOSTER = "is_show_tut_remove_boom_booster";
    public const string IS_SHOW_TUT_REMOVE_LOCK_BOOSTER = "is_show_tut_remove_lock_booster";
}

public class PathPrefabs
{
    public const string POPUP_REWARD_BASE = "UI/Popups/PopupRewardBase";
    public const string CONFIRM_POPUP = "UI/Popups/ConfirmBox";
    public const string WAITING_BOX = "UI/Popups/WaitingBox";
    public const string WIN_BOX = "UI/Popups/WinBox";
    public const string REWARD_IAP_BOX = "UI/Popups/RewardIAPBox";
    public const string SHOP_BOX = "UI/ShopBox";
    public const string RATE_GAME_BOX = "UI/Popups/RateGameBox";
    public const string SETTING_BOX = "UI/Popups/SettingBox";
    public const string LOSE_BOX = "UI/Popups/LoseBox";
    public const string LEVEL_FAILED_BOX = "UI/Popups/LevelFailedBox";
    public const string EXIT_LEVEL_BOX = "UI/Popups/ExitLevelBox";

    public const string SETTINGS_BOX = "UI/Popups/SettingsBox";
    public const string FAIL_CONNECTION_BOX = "UI/Popups/FailConnectionBox";
    public const string SELECT_LEVEL_BOX = "UI/Popups/SelectLevelPopups";

    public const string REWARD_CONGRATULATION_BOX = "UI/Popups/RewardCongratulationBox";
    public const string SHOP_GAME_BOX = "UI/Popups/ShopBox";
    public const string CONGRATULATION_BOX = "UI/Popups/CongratulationBox";

    public const string STARTER_PACK_BOX = "UI/Popups/PackBoxes/StarterPackBox";
    public const string THREE_SKIN_BIRD_PACK_BOX = "UI/Popups/PackBoxes/ThreeSkinBirdPackBox";
    public const string BRANCH_AND_THEME_PACK_BOX = "UI/Popups/PackBoxes/BranchAndThemePackBox";


    public const string BIG_REMOVE_ADS_PACK_BOX = "UI/Popups/PackBoxes/BigRemoveAdsPackBox";
    public const string SALE_WEEKEND_1_PACK_BOX = "UI/Popups/PackBoxes/SaleWeekend1PackBox";
    public const string MINI_GAME_CONNECT_BIRD_BOX = "UI/Popups/ConnectBirdMGBox";
    public const string CONNECT_BIRD_MINI_GAME_SHOP_BOX = "UI/Popups/ConnectBirdMiniGameShop";
    public const string REWARD_CONNECT_BIRD_MN_BOX = "UI/Popups/RewardConnectBirdMNBox";
    public const string POPUP_DAILY_REWARD = "UI/Popups/PopupDailyReward";
    public const string POPUP_PAUSE_BOX = "UI/Popups/PauseBox";

    public const string SUGGET_BOX = "UI/Popups/SuggetBox";
    public const string OPEN_GIFT_BOX = "UI/Popups/OpenGiftBox";
    public const string SHOP_PACK_BOX = "UI/Popups/ShopPackBox";
    public const string LEVEL_GIFT_BOX = "UI/Popups/LevelGiftBox";
    public const string EGG_GIFT_BOX = "UI/Popups/EggGiftBox";
    public const string AD_BREAK_BOX = "UI/Popups/AdBreakBox";
    public const string NOTI_BOX = "UI/Popups/Notification";
}

public class SceneName
{
    public const string LOADING_SCENE = "LoadingScene";
    public const string HOME_SCENE = "HomeScene";
    public const string GAME_PLAY = "GamePlay";
}

public class AudioName
{
    public const string bgMainHome = "Music_BG_MainHome";
    public const string bgGamePlay = "Music_BG_GamePlay";

    //Ingame music
    public const string winMusic = "winMusic";
    public const string spawnerPlayerMusic = "spawnerPlayer";

    //Action Player music
    public const string jumpMusic = "jump";
    public const string jumpEndMusic = "jumpEnd";
    public const string swapMusic = "swap";
    public const string pushRockMusic = "pushRock";
    public const string dieMusic = "die";
    public const string reviveMusic = "revive";
    public const string flyMusic = "fly";

    //Collect music
    public const string collectCoinMusic = "collectCoin";
    public const string collectKeyMusic = "collectKey";
    public const string collectItemSound = "collectItem";

    //Level music
    public const string jumpOnWaterMusic = "jumpOnWater";
    public const string collisionDoorMusic = "collisionDoor";
    public const string doorOpenMusic = "doorOpen";
    public const string doorCloseMusic = "doorClose";
    public const string springMusic = "spring";
    public const string touchSwitchMusic = "touchSwitch";
    public const string bridgeMoveMusic = "bridgeMove";
    public const string bridgeMoveEndMusic = "bridgeMoveEnd";
    public const string iceDropFall = "rock1";
    public const string iceDropExplosion = "bigrock";
    public const string activeDiamond = "crystalactive";
    public const string releaseDiamond = "crystalrelease";
    //UI Music
    public const string buttonClick = "buttonClick";
}

public class KeyPref
{
    public const int MAX_LEVEL = 100;
    public const string SERVER_INDEX = "SERVER_INDEX";
}

public class FirebaseConfig
{
    public const string LINK_RATE = "link_rate_game";
    public const string SHOW_OPEN_ADS = "show_open_ads";
    public const string SHOW_OPEN_ADS_FIRST_OPEN = "show_open_ads_first_open";
    public const string DELAY_SHOW_OPEN_APP_ADS = "delay_show_open_app_ads";
    public const string DELAY_SHOW_INITSTIALL = "delay_show_initi_ads";//Thời gian giữa 2 lần show inital 30
    public const string LEVEL_START_SHOW_INITSTIALL = "level_start_show_initstiall";//Level bắt đầu show initial//3


    public const string LEVEL_START_SHOW_RATE = "level_start_show_rate";//Level bắt đầu show popuprate

    public const string DEFAULT_NUM_ADD_BRANCH = "default_num_add_branch";//2
    public const string DEFAULT_NUM_REMOVE_BOMB = "default_num_remove_bomb";//0
    public const string DEFAULT_NUM_REMOVE_EGG = "default_num_remove_egg";//0
    public const string DEFAULT_NUM_REMOVE_JAIL = "default_num_remove_jail";//0
    public const string DEFAULT_NUM_REMOVE_SLEEP = "default_num_remove_sleep";//0
    public const string DEFAULT_NUM_REMOVE_CAGE = "default_num_remove_cage";//0

    public const string DEFAULT_NUM_RETURN = "default_num_return";//2
    public const string NUM_RETURN_CLAIM_VIDEO_REWARD = "num_return_claim_video_reward";//3

    public const string LEVEL_START_TUT_RETURN = "level_start_tut_return";//4
    public const string LEVEL_START_TUT_BUY_STAND = "level_start_tut_buy_stand";//5

    public const string ON_OFF_REMOVE_ADS = "on_off_remove_ads_2";//5
    public const string MAX_LEVEL_SHOW_RATE = "max_level_show_rate";//30

    public const string TEST_LEVEL_CAGE_BOOM = "test_level_cage_boom";//30
    public const string SHOW_INTER_PER_TIME = "show_inter_per_time";
    public const string ON_OFF_ACCUMULATION_REWARD_LEVEL_START = "on_off_accumulation_reward_level_start";//true
    public const string ACCUMULATION_REWARD_LEVEL_START = "accumulation_reward_level_start";//6
    public const string ACCUMULATION_REWARD_END_LEVEL = "accumulation_reward_end_level_{0}";//
    public const string ACCUMULATION_REWARD_TIME_SHOW_NEXT_BUTTON = "accumulation_reward_time_show_next_button";//1.5
    public const string ACCUMULATION_REWARD_END_LEVEL_RANDOM = "accumulation_reward_end_level_random";//10
    public const string MAX_TURN_ACCUMULATION_REWARD_END_LEVEL_RANDOM = "max_turn_accumulation_reward_end_level_random";//150

    public const string ON_OFF_SALE_INAPP = "on_off_sale_inapp";//true

    public const string LEVEL_UNLOCK_SALE_PACK = "level_unlock_sale_pack"; //11
    public const string LEVEL_UNLOCK_PREMIUM_PACK = "level_unlock_premium_pack"; //25
    public const string TIME_LIFE_STARTER_PACK = "time_life_starter_pack"; // 3DAY
    public const string TIME_LIFE_PREMIUM_PACK = "time_life_premium_pack"; // 2DAY
    public const string TIME_LIFE_SALE_PACK = "time_life_premium_pack"; // 1DAY
    public const string TIME_LIFE_BIG_REMOVE_ADS_PACK = "time_life_big_remove_ads_pack"; // 3h

    public const string NUMBER_OF_ADS_IN_DAY_TO_SHOW_PACK = "number_of_ads_in_day_to_show_pack"; //5ADS
    public const string NUMBER_OF_ADS_IN_PLAY_TO_SHOW_PACK = "number_of_ads_in_play_to_show_pack"; //3ADS
    public const string TIME_DELAY_SHOW_POPUP_SALE_PACK_ = "time_delay_show_popup_sale_pack_"; // 6H
    public const string TIME_DELAY_ACTIVE_SALE_PACK = "time_delay_active_sale_pack_"; // 6H


    public const string REVIEW_IAP_VERSION = "review_iap_version"; // 6H
}
public class GameData
{
    public const string LEVEL_SAVE = "Datas/DataLevelSave";
}
public static class NameOfAnimBird
{
    public const string IDLE = "idle";

    public const string FLY = "fly";
    public const string JUMP = "jump";
    public const string GROUND = "grounding";
    public const string HAPPY = "happy";
    public const string UN_HAPPY = "angry";
}
