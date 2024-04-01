using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PackInShop : MonoBehaviour
{
    public TypePackIAP typePackIAP;
    public Button btnBuy;
    public Text tvBuy;
    public void Init()
    {
        //tvBuy.text = "" + ;
    
        tvBuy.text =  "" + GameController.Instance.iapController.GetPriceNotInapp(this.typePackIAP);      
        btnBuy.onClick.AddListener(delegate { ButtonOnClick(); });
    
    }

    public void ButtonOnClick()
    {
        var temp = GameController.Instance.iapController.GetPriceNotInapp(this.typePackIAP);
        if(UseProfile.Coin >= temp)
        {
            GameController.Instance.iapController.BuyProductNotInapp(typePackIAP);
        }
        else
        {
            GameController.Instance.moneyEffectController.SpawnEffectText_FlyUp
                               (
                               btnBuy.transform.position,
                               "No Enought Coin",
                               Color.white,
                               isSpawnItemPlayer: true
                               );
        }
    }
      
}
