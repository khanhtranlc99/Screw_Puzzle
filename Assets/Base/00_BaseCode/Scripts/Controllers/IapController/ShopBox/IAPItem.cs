using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventDispatcher;
using DG.Tweening;

public class IAPItem : MonoBehaviour
{
    public TypePackIAP typePack;
    private IAPPack IAPpack;

    [SerializeField] private Text txtPriceInapp;
    [SerializeField] private Button btBuy;


    private void Start()
    {
#if UNITY_IOS
        //if(typePack == TypePackIAP.GEM_1 || typePack == TypePackIAP.GEM_2 || typePack == TypePackIAP.GEM_3 || typePack == TypePackIAP.GEM_4 || typePack == TypePackIAP.GEM_5 || typePack == TypePackIAP.GEM_6)
        //    gameObject.SetActive(false);
#endif
        btBuy.onClick.AddListener(() => OnClickBuy());
        this.RegisterListener(EventID.CHANGE_LANGUAGE, (param) => UpdateUI());
    }

    public void Init()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (IAPpack == null)
        {
            IAPpack = GameController.Instance.iapController.inappDatabase.GetPack(typePack);
        }

        if (IAPpack.typeBuy == TypeBuy.Inapp)
        {
            txtPriceInapp.gameObject.SetActive(true);
            txtPriceInapp.text = GameController.Instance.iapController.GetPrice(typePack);

        }
    }

    private void OnClickBuy()
    {
        if (IAPpack == null)
            return;

        if (IAPpack.typeBuy == TypeBuy.Inapp)
            GameController.Instance.iapController.BuyProduct(typePack);
        else
            GameController.Instance.iapController.BuyProductNotInapp(typePack);

        //GameController.Instance.musicManager.PlayClickBtnSound();
    }
}
