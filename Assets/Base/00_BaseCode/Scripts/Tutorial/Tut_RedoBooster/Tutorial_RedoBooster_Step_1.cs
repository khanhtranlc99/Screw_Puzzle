using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_RedoBooster_Step_1 : TutorialBase
{
    public GameObject currentHand;
    public override bool IsCanShowTut()
    {
        if (UseProfile.CurrentLevel < 6)
        {
            return false;
        }
           
        if (UseProfile.IsShowTutRedo)         
        {
            return false;
        }

       
        return base.IsCanShowTut();
    }
    public override bool IsCanEndTut()
    {
        UseProfile.IsShowTutRedo = true;
        if(currentHand != null)
        {
            Destroy(currentHand.gameObject);
        }
        GamePlayController.Instance.tut_BoosterAddPile.StartTut();
        return true;
    }

    public override void StartTut()
    {
        if(currentHand == null)
        {
            currentHand = Instantiate(handTut, GamePlayController.Instance.gameScene.btnReturn.gameObject.transform);
        }
   
    }

    protected override void SetNameTut()
    {
        nameTut = "Tutorial_RedoBooster_Step_1";
    }
}
