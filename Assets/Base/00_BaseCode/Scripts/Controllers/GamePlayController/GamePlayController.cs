using Crystal;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum StateGame
{
    Loading = 0,
    Playing = 1,
    Win = 2,
    Lose = 3,
    Pause = 4
}

public class GamePlayController : Singleton<GamePlayController>
{
    public PlayerContain playerContain;
    public GameScene gameScene;
    public ReturnController returnController;
    public ExtralPlayController extralPlayController;
    public CameraScale cameraScale;
    public TutorialFunController tutLevel_1;
    public TutorialFunController tutLevel_2;
    public TutorialFunController tut_BoosterRedo;
    public TutorialFunController tut_BoosterAddPile;
    public TutorialFunController tut_Extral_Boom;
    public TutorialFunController tut_Extral_Lock;
    public TutorialFunController tut_RemoveBoom_Booster;
    public TutorialFunController tut_RemoveLock_Booster;
    protected override void OnAwake()
    {
   

        Init();

    }

    public void Init()
    {

        returnController.Init();
        playerContain.Init();
        gameScene.Init();
        extralPlayController.Init();
        cameraScale.Init();
        tutLevel_1.Init();
        tutLevel_2.Init();
        tut_BoosterRedo.Init();
        tut_BoosterAddPile.Init();
        tut_Extral_Boom.Init();
        tut_Extral_Lock.Init();
        tut_RemoveBoom_Booster.Init();
        tut_RemoveLock_Booster.Init();
        GameController.Instance.AnalyticsController.LoadingComplete();
        GameController.Instance.AnalyticsController.StartLevel(UseProfile.CurrentLevel);
        GameController.Instance.admobAds.canShowOpenAppAds = true;     
     
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            GameController.Instance.admobAds.ShowOpenAppAdsInGame();
        }
        else
        {
            NotiBox.Setup(delegate { NotiBox.Setup(null).Close(); }).Show();
        }

      
      //  StartCoroutine(TestLevel());
    }
    public IEnumerator TestLevel()
    {
        yield return new WaitForSeconds(3);
        UseProfile.CurrentLevel += 1;
        SceneManager.LoadScene("GamePlay");
    }
}
