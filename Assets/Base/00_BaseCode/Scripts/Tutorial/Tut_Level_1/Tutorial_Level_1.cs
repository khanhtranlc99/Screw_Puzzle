using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Level_1 : TutorialFunController
{
    public override void NextTut()
    {
        if (currentIDTut > tutorials.Count - 1)
        {
            return;
        }
        if (!tutorials[currentIDTut].IsCanShowTut())
            return;


        if (!tutorials[currentIDTut].IsCanEndTut())
            return;

   
        currentIDTut++;

        if (currentIDTut >= tutorials.Count)
            return;
        if (tutorials[currentIDTut].IsCanShowTut())
        {
            tutorials[currentIDTut].StartTut();
        }

    }

    public override void EndTut()
    {
        if (UseProfile.CurrentLevel < 2)
        {
            foreach (var item in GamePlayController.Instance.tutLevel_1.tutorials)
            {
                item.OnEndTut();
            }
        }
    }
   
}
