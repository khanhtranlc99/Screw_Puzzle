using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RewardIAPBox : PopupRewardBase
{
    public Button bgButton;
    public static RewardIAPBox _instance;
    public static RewardIAPBox Setup2(bool smallReward = false)
    {
        if (_instance == null)
        {
            _instance = Instantiate(Resources.Load<RewardIAPBox>(PathPrefabs.REWARD_IAP_BOX));
        }
        _instance.InitState(smallReward);
        return _instance;
    }
}