using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_RemoveLockBooster_Step_1 : TutorialBase
{
    public GameObject currentHand;
    public override void Init(TutorialFunController controller)
    {
        base.Init(controller);

    }
    public override bool IsCanShowTut()
    {
        if (UseProfile.IsShowTutRemoveLockBooster)
        {
            return false;
        }


        return base.IsCanShowTut();
    }
    public override bool IsCanEndTut()
    {

        Destroy(currentHand.gameObject);
        return true;
    }


    public override void StartTut()
    {
        if (currentHand == null)
        {
            currentHand = Instantiate(handTut, GamePlayController.Instance.gameScene.btnRemoveCage.transform);
        }
    }

    protected override void SetNameTut()
    {
        nameTut = "Tutorial_RemoveLockBooster_Step_1";
    }
}
