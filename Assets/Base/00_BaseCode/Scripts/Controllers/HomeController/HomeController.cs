using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeController : Singleton<HomeController>
{
    public HomeScene homeScene;

    protected override void OnAwake()
    {
      //  GameController.Instance.currentScene = SceneType.MainHome;

    }

    private void Start()
    {
        homeScene.Init();
    }

}
