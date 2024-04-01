using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Tutorial_Extra_Boom_Step_1 : TutorialBase
{
    public Button btnEndTut;
    public override void Init(TutorialFunController controller)
    {
        base.Init(controller);
        btnEndTut.onClick.AddListener(controller.NextTut) ;
    }
    public override bool IsCanShowTut()
    {
        if (UseProfile.IsShowTutExtralBoom )
        {
            return false;
        }

     
        return base.IsCanShowTut();
    }
    public override bool IsCanEndTut()
    {

        StartCoroutine(ShowTut()); 
        return true;
    }
    private IEnumerator ShowTut()
    {
        yield return new WaitForSeconds(1);
        handTut.SetActive(false);
    }

    public override void StartTut()
    {
        handTut.SetActive(true);
    }

    protected override void SetNameTut()
    {
        nameTut = "Tutorial_Extra_Boom_Step_1";
    }
}
