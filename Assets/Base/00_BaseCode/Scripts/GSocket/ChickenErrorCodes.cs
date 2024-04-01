using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenErrorCodes
{
    public static string GetErrorCode(FunctionCode function, CantConnectCode cantConnectCode)
    {
        int internetStatus = GSocket.IntenetAvaiable ? 1 : 0;
        string errorCode = " " + (int)function + "&" + (int)cantConnectCode + "&" + internetStatus + "&" +  + PingPanel.PingMs;
      
        return errorCode;
    }
}

public enum FunctionCode
{
    AdmobAds_ShowAdsReward = 0,
    HomeScene_HandleCoopMessage = 1,
    InappController_BuyProductID = 2,
    PlayfabService_LoginPlayfabSequence = 3,
    Coop_Ingame_RealDisconect = 4,
    Coop_LobbyGameController_Update = 5,
    Coop_LobbyGameController_OnConnectionFail = 6,
    Coop_LobbyGameController_OnFailedToConnectToPhoton = 7,
    Coop_LobbyGameController_ButtonEvent_ConnecRoom = 8,
    Coop_LobbyGameController_OnClickCreateRoomButton = 9,
    P2v2_PanelEndGame_OnDisconnectedFromPhoton = 10,
    P2v2_GameController_RealDisconnect = 11,
    P2v2_HomeScene_StartPlaying = 12,
    P2v2_LobbyController_OnDisconnectedFromPhoton = 13,
    RocketServices_LoginSequence = 14,
    RocketServices_LinkFBLogin = 15,
    OnlineGameController_RealDisconect = 16,
    OnlineGameLobbyController_Update = 17,
    OnlineGameLobbyController_OnConnectionFail = 18,
    OnlineGameLobbyController_OnFailedToConnectToPhoton = 19,
    OnlineGameLobbyController_ButtonEvent_ConnecRoom = 20,
    OnlineGameLobbyController_OnClickCreateRoomButton = 21,
    RocketIO_OnLoginError = 22,
    MainHomeEventButton_OnClickCoop = 23,
    GiftCodeBox_OnClickSend = 24,
    GiftCodeBox_OnError = 25,
    HomeButtonPlayMode_OnClickCoopMode = 26,
    BaseEndlessPackUI_OnClickBuy = 27,
    BasePopupMultiPack_OnIAPStaticShow = 28,
    BasePopupMultiPack_ProgressGift_OnIAPStaticShow = 29,
    ButtonBuyShip_OnClickBuyShip = 30,
    EndlessMultiPackUI_New_OnClickBuy = 31,
    EndlessSinglePackUI_New_OnClickBuy = 32,
    PanelPopupChooseLevelSale_OnClickBuy = 33,
    PanelPopupEpicPack_OnClickBuy = 34,
    PanelPopupFantasticPack_OnClickBuy = 35,
    PanelPopupGemPack_OnClickBuy = 36,
    PanelPopupIAPTrialShip_OnClickBuy = 37,
    PanelPopupItemPack_OnClickBuy = 38,
    PanelPopupLimitedPack_OnClickBuy = 39,
    PanelPopupSaleOff_OnClickBuy = 40,
    PanelPopupSaleShip_OnClickBuy = 41,
    PanelPopupSpecialPack_OnClickBuy = 42,
    PanelPopupStarterPack_OnClickBuy = 43,
    PopupIAPMultiPack_OnClickBuy = 44,
    ReportBox_OnClickReportButton = 45,
    BaseLeaderBoardPanel = 46,
    FacebookGroupPanel_LoginObservable = 47,
    FacebookPanel_LoginObservable = 48,
    PlayfabService_LinkingFacebookManual = 49,
    PlayfabService_OnErrorAskDataAgain = 50,
    PlayfabService_BindDeviceToPlayfabFB = 51,
    PlayfabService_ChooseFacebookAction = 52,
    PlayfabService_ChooseDeviceAction = 53,
    FacebookLogin = 54,
    PvPEntryButton_OnClickEntryButton = 55,
    GroupButtonPlayMode_OnClickTrial = 56,
    GroupButtonPlayMode_OnClickArena = 57,
    ButtonGoPvpMode_PlayPVPMode = 58,
    ButtonGoAnyWhere_PlayPVPMode = 59,
    HomeButtonPlayMode_OnClickPvpMode = 60,
    ButtonTeamFunctionInMainHome = 61,
    PanelPopupSaleByNotify_OnClickBuy = 62,
    PanelPopupDailySale_OnClickBuy = 63,
    UpdateNameBox_OnClickOK = 64
}

public enum CantConnectCode
{
    NoInternet = 0,
    IAPCantInit = 1,
    RocketIO_NoInternet = 2,
    Photon_Disconect = 3,
    PlayfabError = 4,
    FacebookError = 5
}