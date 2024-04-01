#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// You must obfuscate your secrets using Window > Unity IAP > Receipt Validation Obfuscator
// before receipt validation will compile in this sample.
#define RECEIPT_VALIDATION
#endif

using EventDispatcher;
using System;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;
#if RECEIPT_VALIDATION
using UnityEngine.Purchasing.Security;
#endif

public enum InAppStatus
{
    NotAvailable,
    Owned,
    NotOwned
}

public enum TypeBuy
{

    Inapp = 0,
    Diamod = 1,
    Video = 2,
    Coin = 3,
    Free = 4,
    Event = 5
}
public class IapController : MonoBehaviour, IStoreListener
{
    private string m_LastTransationID;
    private string m_LastReceipt;
    private bool m_IsLoggedIn = false;
    private bool m_PurchaseInProgress;

    private ConfigurationBuilder builder;
    private bool m_IsGooglePlayStoreSelected;
    public bool isInited;
    private UnityAction actBuyFault;
    private UnityAction actBuySuccess;
    public IAPDatabase inappDatabase;

#if RECEIPT_VALIDATION
    private CrossPlatformValidator validator;
#endif

    private IStoreController m_Controller;
    private IAppleExtensions m_AppleExtensions;

    WaitForSeconds wait = new WaitForSeconds(5);

    public void Init()
    {
        var module = StandardPurchasingModule.Instance();
        // The Fake
        //Store supports: no-ui (always succeeding), basic ui (purchase pass/fail), and 
        // developer ui (initialization, purchase, failure code setting). These correspond to 
        // the FakeStoreUIMode Enum values passed into StandardPurchasingModule.useFakeStoreUIMode.
        module.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;

        builder = ConfigurationBuilder.Instance(module);
        // builder.Configure<IMicrosoftConfiguration>().useMockBillingSystem = true;
        // builder.Configure<IGooglePlayConfiguration>().(Config.inappAndroidKeyHash);

        m_IsGooglePlayStoreSelected = Application.platform == RuntimePlatform.Android && module.appStore == AppStore.GooglePlay;

        // Define our products.
        // In this case our products have the same identifier across all the App stores,
        // except on the Mac App store where product IDs cannot be reused across both Mac and
        // iOS stores.
        // So on the Mac App store our products have different identifiers,
        // and we tell Unity IAP this by using the IDs class.

        for (int i = 0; i < inappDatabase.lstPacksInapp.Count; i++)
        {
            var pack = inappDatabase.lstPacksInapp[i];
            builder.AddProduct(pack.ProductID, pack.productType, new IDs
                {
                    {pack.ProductID, AppleAppStore.Name},
                    {pack.ProductID, GooglePlay.Name},
                });
           
            Debug.LogError(pack.ProductID);
            
        }


        //builder.AddProduct("com.globalplay.birdsort.noads1", ProductType.NonConsumable, new IDs
        //        {
        //            {"com.globalplay.birdsort.noads1", AppleAppStore.Name},
        //            {"com.globalplay.birdsort.noads1", GooglePlay.Name},
        //        });

#if RECEIPT_VALIDATION
        //validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);
#endif

        // Now we're ready to initialize Unity IAP.
        UnityPurchasing.Initialize(this, builder);
        StartCoroutine(AutoInit());
        //GameServices.InternetSubject.DistinctUntilChanged()
        //   .Take(1).Subscribe(haveInternet =>
        //   {
        //       UnityPurchasing.Initialize(this, builder);

        //   }).AddTo(this);
    }

    private IEnumerator AutoInit()
    {
        while (true)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                yield return wait;
            }
            else
            {
                if (IsInitialized())
                {
                    yield break;
                }
                //if (!isInited)
                {
                    UnityPurchasing.Initialize(this, builder);
                    //isInited = true;
                }
                yield return wait;
            }
        }
    }

    private bool IsInitialized()
    {
        return m_Controller != null && m_AppleExtensions != null;
    }

    public InAppStatus CheckProductAvailable(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_Controller.products.WithID(productId);
            if (product == null)
                return InAppStatus.NotAvailable;
            else
            {
                if (product.hasReceipt)
                    return InAppStatus.Owned;
                else
                {
                    if (HaveProductIDCache(productId))
                        return InAppStatus.Owned;
                    else
                        return InAppStatus.NotOwned;
                }
            }
        }
        else
        {
            if (HaveProductIDCache(productId))
                return InAppStatus.Owned;
            else
                return InAppStatus.NotAvailable;
        }
    }

    public static bool HaveProductIDCache(string productId)
    {
        //  FunctionHelper.ShowDebug("checking cache " + productId + " ololo " + (PlayerPrefs.GetInt(string.Format("ProductID:{0}", productId), 0) == 1));
        return PlayerPrefs.GetInt(string.Format("ProductID:{0}", productId), 0) == 1;
    }

    /// <summary>
	/// This will be called when a purchase completes.
	/// </summary>
	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        //FunctionHelper.ShowDebug("Purchase OK: " + e.purchasedProduct.definition.id);
        //FunctionHelper.ShowDebug("Receipt: " + e.purchasedProduct.receipt);
        //if(e == null)
        Debug.Log("HOANG===============  " + e.purchasedProduct.definition.id);
        //m_LastTransationID = e.purchasedProduct.transactionID;
        //m_LastReceipt = e.purchasedProduct.receipt;
        //m_PurchaseInProgress = false;



        ProcessPurchaseSuccess(e);


        return PurchaseProcessingResult.Complete;
    }

    private void ProcessPurchaseSuccess(PurchaseEventArgs e)
    {
        ProcessEventSuccess(e.purchasedProduct);
    }

    #region Event
    public delegate void InitSuccess();
    public InitSuccess onInitSuccess;

    /// <summary>
	/// This will be called when Unity IAP has finished initialising.
	/// </summary>
	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("==================== OnInitialized");
        m_Controller = controller;
        m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();
        // On Apple platforms we need to handle deferred purchases caused by Apple's Ask to Buy feature.
        // On non-Apple platforms this will have no effect; OnDeferred will never be called.
        m_AppleExtensions.RegisterPurchaseDeferredListener(OnDeferred);
        if (onInitSuccess != null)
        {
            onInitSuccess();
        }
        isInited = true;
        try
        {
            for (int i = 0; i < inappDatabase.lstPacksInapp.Count; i++)
            {
                var pack = inappDatabase.lstPacksInapp[i];
                if (pack.productType == ProductType.NonConsumable)
                {
                    Product product = m_Controller.products.WithID(pack.ProductID);
                    Debug.Log("pack " + pack.shortID + " ============ " + product.hasReceipt);
                    if (product != null && product.hasReceipt)
                    {
                        // Owned Non Consumables and Subscriptions should always have receipts.
                        // So here the Non Consumable product has already been bought.
                        pack.IsBought = true;
                    }
                }

            }
        }
        catch
        {

        }
            
    }

    private void OnDeferred(Product item)
    {
        //FunctionHelper.ShowDebug("Purchase deferred: " + item.definition.id);
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        //FunctionHelper.ShowDebug("Billing failed to initialize!");
        switch (error)
        {
            case InitializationFailureReason.AppNotKnown:
                Debug.Log("IAP AppNotKnown");
                //FunctionHelper.ShowDebugError("Is your App correctly uploaded on the relevant publisher console?");
                break;
            case InitializationFailureReason.PurchasingUnavailable:
                // Ask the user if billing is disabled in device settings.
                //FunctionHelper.ShowDebug("Billing disabled!");
                Debug.Log("IAP PurchasingUnavailable");
                break;
            case InitializationFailureReason.NoProductsAvailable:
                // Developer configuration error; check product metadata.
                //FunctionHelper.ShowDebug("No products available for purchase!");
                Debug.Log("IAP NoProductsAvailable");
                break;
        }
    }

    /// <summary>
	/// This will be called is an attempted purchase fails.
	/// </summary>
	public void OnPurchaseFailed(Product item, PurchaseFailureReason r)
    {
        // FunctionHelper.ShowDebug("Purchase failed: " + item.definition.id);
        //FunctionHelper.ShowDebug(r);

        //GameController.instance.loadingTimeOut.OpenLoading(false);

        m_PurchaseInProgress = false;
    }
    #endregion

    public void RestorePurchases()
    {
        // If Purchasing has not yet been set up ...
        if (!IsInitialized())
        {
            // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        // If we are running on an Apple device ... 
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            // ... begin restoring purchases
            Debug.Log("RestorePurchases started ...");

            // Fetch the Apple store-specific subsystem.
            var apple = m_AppleExtensions;
            // Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
            // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
            apple.RestoreTransactions((result) =>
            {
                // The first phase of restoration. If no more responses are received on ProcessPurchase then 
                // no purchases are available to be restored.
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
                if (!result)
                {
                    ConfirmBox.Setup().AddMessageYes("Noti", "Restore Fail", () => { });
                }
                else
                {
                    ConfirmBox.Setup().AddMessageYes("Noti", "Restore Successful", () => { });
                }
            });
        }
        // Otherwise ...
        else
        {
            // We are not running on an Apple device. No work is necessary to restore purchases.
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    #region Use
    public ProductMetadata GetPackInfo(string idIAPPack)
    {
        if (IsInitialized() && m_Controller.products.WithID(idIAPPack) != null)
        {
            ProductMetadata _product = m_Controller.products.WithID(idIAPPack).metadata;

            return _product;
        }

        return null;
    }

    public string GetPrice(string idIAPPack)
    {
        if (IsInitialized() && m_Controller.products.WithID(idIAPPack) != null)
        {
            ProductMetadata _product = m_Controller.products.WithID(idIAPPack).metadata;

            return _product.localizedPriceString;
        }

        return "?";
    }

    public string GetPrice(TypePackIAP typePack)
    {
        var item = inappDatabase.GetPack(typePack);
        if (item == null)
            return "?";

        if (IsInitialized() && m_Controller.products.WithID(item.ProductID) != null)
        {
            ProductMetadata _product = m_Controller.products.WithID(item.ProductID).metadata;

            return _product.localizedPriceString;
        }

        return item.defaultPrice;
    }

    public string GetPriceSale(TypePackIAP typePack)
    {
        var item = inappDatabase.GetPack(typePack);
        if (item == null)
            return "?";

        string productId_Sale = string.Format("{0}.{1}.{2}", item.ProductID, StringHelper.SALE_IAP, item.idSale);
        if (IsInitialized() && m_Controller.products.WithID(productId_Sale) != null)
        {
            ProductMetadata _product = m_Controller.products.WithID(productId_Sale).metadata;
            return _product.localizedPriceString;
        }

        return item.defaultPrice;
    }

    public int GetPriceNotInapp(TypePackIAP typePack)
    {
        var item = inappDatabase.GetPackNotInapp(typePack);

        return item.price;
    }



    public void BuyProduct(TypePackIAP typePack)
    {
        var item = inappDatabase.GetPack(typePack);
        if (item == null)
        {
            //ConfirmBox.Setup().AddMessageYes("Notification", "Error! Product not avalible", () =>
            //{

            //    if (actBuyFault != null)
            //    {
            //        actBuyFault();
            //    }
            //});

            

            if (actBuyFault != null)
            {
                actBuyFault();
            }
            return;
        }
        string productId = item.ProductID;
        //if (item.isSale)
        //{
        //    productId = string.Format("{0}.{1}.{2}", item.ProductID, StringHelper.SALE_IAP, item.idSale);
        //}
        //Context.Waiting.
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            //ConfirmBox.Setup().AddMessageYes("Notification", "No Internet access!", () => {

            //    if (actBuyFault != null)
            //    {
            //        actBuyFault();
            //    }
            //});
            GameController.Instance.moneyEffectController.SpawnEffectText_FlyUp
                     (
                     Camera.main.ScreenToWorldPoint(Input.mousePosition),
                     "No Internet Connection!",
                     Color.white,
                     isSpawnItemPlayer: true
                     );

            if (actBuyFault != null)
            {
                actBuyFault();
            }

            return;
        }

        // If the stores throw an unexpected exception, use try..catch to protect my logic here.
        try
        {
            // If Purchasing has been initialized ...
            if (IsInitialized())
            {
                // ... look up the Product reference with the general product identifier and the Purchasing system's products collection.
                Product product = m_Controller.products.WithID(productId);

                // If the look up found a product for this device's store and that product is ready to be sold ... 
                if (product != null && product.availableToPurchase)
                {
#if HACK || TEST
					ProcessEventSuccess(product);
#else
                    m_Controller.InitiatePurchase(product);
#endif

                }
                // Otherwise ...
                else
                {
                    // ... report the product look-up failure situation  
                    //FunctionHelper.ShowDebug("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                    //WaitingBox.Setup().HideWaiting();
                    // GameController.instance.loadingTimeOut.OpenLoading(false);
                }
            }
            // Otherwise ...
            // else
            {
                // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or retrying initiailization.
                //Debug.("BuyProductID FAIL. Not initialized.");
                //ConfirmBox.Setup().AddMessageYes("Notification", "Please check your internet connection!"
                //  , () => {

                //      if (actBuyFault != null)
                //      {
                //          actBuyFault();
                //      }
                //  });

                //GameController.instance.loadingTimeOut.OpenLoading(false);
                //WaitingBox.Setup().HideWaiting();
            }
        }
        // Complete the unexpected exception handling ...
        catch (Exception e)
        {
            // ... by reporting any unexpected exception for later diagnosis.
            // FunctionHelper.ShowDebug("BuyProductID: FAIL. Exception during purchase. " + e);
            //GameController.instance.loadingTimeOut.OpenLoading(false);
            //WaitingBox.Setup().HideWaiting();
        }
        finally
        {
            //			Context.Waiting.HideWaiting();
        }
    }

    public void BuyProductNotInapp(TypePackIAP typePack)
    {
   
        this.PostEvent(EventID.BUY_PRODUCT_SUCCESS_FIRST, typePack.ToString());

        var item = inappDatabase.GetPackNotInapp(typePack);

        if (item.typeBuy == TypeBuy.Diamod)
        {
            //int myDiamod = DataManager.Gem;

            //if (myDiamod >= item.price)
            //{
            //    DataManager.AddGem(-item.price);
            //    item.Claim();

            //    //RewardIAPBox rwBox = RewardIAPBox.Setup();
            //    //rwBox.Show(item);
            //}
            //else
            //{
            //    //Confirm box
            //    ConfirmBox.Setup().AddMessageYesHasCloseBtn(
            //        Localization.Get("s_noti"), Localization.Get("s_not_enough_gem"), () => {

            //            InappBox.Setup(TypeItem.Gem).Show();


            //        });
            //}
        }
        else if (item.typeBuy == TypeBuy.Coin)
        {
            int myCoin = UseProfile.Coin;

            if (myCoin >= item.price)
            {
                GameController.Instance.dataContain.giftDatabase.Claim(GiftType.Coin, -item.price);
                item.Claim();

                //RewardIAPBox rwBox = RewardIAPBox.Setup();
                //rwBox.Show(item);
            }
            else
            {


                //GameController.Instance.moneyEffectController.SpawnEffectText_FlyUp
                //    (
                //    buttnRewardVideo.transform.position,
                //    "not_enough_coin",
                //    Color.white,
                //    isSpawnItemPlayer: true
                //    );
                //Confirm box
                // ConfirmBox.Setup().AddMessageYesNo(Localization.Get("s_noti"), Localization.Get("s_not_enough_coin"),
                //() => { InappBox.Setup(TypeItem.Coin).Show(); }, null, stringYes: Localization.Get("s_BuyNow"), stringNo: Localization.Get("s_No"));
                //InappBox.Setup(TypeItem.Coin).Show();

            }
        }
        else if (item.typeBuy == TypeBuy.Video)
        {
            //GameController.Instance.admobAds.ShowVideoReward(() =>
            //{
            //    item.Claim();
            //    this.PostEvent(EventID.BUY_PRODUCT_SUCCESS, typePack.ToString());
            //    //RewardIAPBox rwBox = RewardIAPBox.Setup();
            //    //rwBox.Show(item);
            //}
            //);
        }
        else
        {
            var pack = inappDatabase.GetPackNotInapp(typePack);
            pack.Claim();
            this.PostEvent(EventID.BUY_PRODUCT_SUCCESS, typePack.ToString());
        }

    }

    private void ProcessEventSuccess(Product product)
    {
        this.PostEvent(EventID.BUY_PRODUCT_SUCCESS_FIRST, product.definition.id);
        if (inappDatabase.lstPacksInapp == null)
            Debug.Log("====== inappDatabase ========== null");
        Debug.Log("Non comsume Back -======================");

        for (int i = 0; i < inappDatabase.lstPacksInapp.Count; i++)
        {
            var pack = inappDatabase.lstPacksInapp[i];
            if (pack != null)
            {
                if (pack.ProductID.CompareTo(product.definition.id) == 0 || (pack.isSale && (string.Format("{0}.{1}.{2}", pack.ProductID, StringHelper.SALE_IAP, pack.idSale).CompareTo(product.definition.id) == 0)))
                {
                    pack.ActClaimDone = actBuySuccess;
                    pack.Claim(isInited);
                    break;
                }
            }
        }

        this.PostEvent(EventID.BUY_PRODUCT_SUCCESS, product.definition.id);
        GameUtils.RaiseMessage(new GameEvent.OnIAPSucsses());

        AnalyticsController.LogBuyInapp(product.definition.id, product.transactionID);
        try
        {
            AnalyticsController.LogIAP(UseProfile.CurrentLevel, product.definition.id, product.metadata.localizedPrice.ToString(), product.metadata.isoCurrencyCode);
        }
        catch
        {

        }
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
    
    }

    // foreach (var item in inappDatabase.dictPackItem)
    // {
    //     if (item.Value.ProductID.CompareTo(product.definition.id) == 0 || (item.Value.isSale && (string.Format("{0}.{1}.{2}", item.Value.ProductID,StringHelper.SALE_IAP, item.Value.idSale).CompareTo(product.definition.id) == 0)))
    //     {
    //         item.Value.Claim();
    //     }
    // }

    public UnityAction ActBuyFault
    {
        set
        {
            actBuyFault = value;
        }
    }

    public UnityAction ActBuySuccess
    {
        set
        {
            actBuySuccess = value;
        }
    }
}
#endregion

