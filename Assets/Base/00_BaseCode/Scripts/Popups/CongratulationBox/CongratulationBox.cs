using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CongratulationBox : BaseBox
{
    private static CongratulationBox instance;
    public static CongratulationBox Setup(bool isSaveBox = false, Action actionOpenBoxSave = null)
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load<CongratulationBox>(PathPrefabs.REWARD_CONGRATULATION_BOX));
            instance.Init();
        }
        instance.InitState();
        return instance;
    }

    public void Init()
    {
       
    }
    private void InitState()
    {

    }
}
