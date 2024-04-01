using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainSceneController : SceneBase
{
    public Button btnHome;
    public RandomWatchVideo btnReward;
    public override void Init()
    {
        btnHome.onClick.AddListener(delegate { OnClickPlay(); });
        btnReward.Init();
    }



    private void OnClickPlay()
    {
     
        SceneManager.LoadScene("GamePlay");
    }
}
