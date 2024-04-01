using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Tutorial_Extra_Lock_Step_1 : TutorialBase
{
    public Button btnEndTut;
    public override void Init(TutorialFunController controller)
    {
        base.Init(controller);
        btnEndTut.onClick.AddListener(controller.NextTut);
    }
    public override bool IsCanShowTut()
    {
        if (UseProfile.IsShowTutExtralLock)
        {
            return false;
        }


        return base.IsCanShowTut();
    }
    public override bool IsCanEndTut()
    {
        handTut.SetActive(false);

        return true;
    }
    private IEnumerator ShowTut()
    {
        yield return new WaitForSeconds(0.7f);
        handTut.SetActive(true);
    }

    public override void StartTut()
    {
       
        StartCoroutine(ShowTut());
    }

    protected override void SetNameTut()
    {
        nameTut = "Tutorial_Extra_Lock_Step_1";
    }
}
