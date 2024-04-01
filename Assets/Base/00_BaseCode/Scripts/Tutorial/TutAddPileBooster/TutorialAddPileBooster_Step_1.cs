using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAddPileBooster_Step_1 : TutorialBase
{
    public GameObject currentHand;
    public override bool IsCanShowTut()
    {
        if (UseProfile.CurrentLevel < 6)
        {
            return false;
        }

        if (UseProfile.IsShowTutAddPile)
        {
            return false;
        }


        return base.IsCanShowTut();
    }
    public override bool IsCanEndTut()
    {
        UseProfile.IsShowTutAddPile = true;
        if (currentHand != null)
        {
            Destroy(currentHand.gameObject);
        }
        Debug.LogError("IsCanEndTut");
        return true;
    }

    public override void StartTut()
    {
        if (currentHand == null)
        {
            currentHand = Instantiate(handTut, GamePlayController.Instance.gameScene.btnAddPile.gameObject.transform);
        }

    }

    protected override void SetNameTut()
    {
        nameTut = "TutorialAddPileBooster_Step_1";
    }
}
