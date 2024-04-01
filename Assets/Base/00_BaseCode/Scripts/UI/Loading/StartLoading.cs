using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StartLoading : MonoBehaviour
{
    public Text txtLoading;
    public Slider progressBar;
    private string sceneName;
    public int countSecond;
    Coroutine coroutineLoad;
    public bool wasCoolDown;

    public void Init()
    {
        wasCoolDown = true;
        progressBar.value = 0f;
        countSecond = 0;
        coroutineLoad = StartCoroutine(LoadAdsToChangeScene());
        StartCoroutine(LoadingText());
    }
 
    // Use this for initialization
    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(1);
        progressBar.value = 0f;
        // we start loading the scene
        //scene_name = GameUtils.SceneName.HOME_SCENE;
        //if (UseProfile.IsFirstTimeInstall || PlayerPrefs.GetInt("Level_1") == 0)
        //{
        //    UseProfile.CurrentLevel = 1;
        //    sceneName = "GamePlay";
        //}
        //else
        //{
        //    sceneName = "HomeScene";
        //}
        // sceneName = "HomeScene";
        var _asyncOperation = SceneManager.LoadSceneAsync("GamePlay", LoadSceneMode.Single);
        //_asyncOperation.allowSceneActivation = false;
        //Debug.Log("_asyncOperation " + _asyncOperation.progress);
        //// while the scene loads, we assign its progress to a target that we'll use to fill the progress bar smoothly
        while (!_asyncOperation.isDone)
        {
            progressBar.value = Mathf.Clamp01(_asyncOperation.progress / 0.9f);
            yield return null;
     
        //// we switch to the new scene
        //_asyncOperation.allowSceneActivation = true;
    }
    }

    private IEnumerator LoadAdsToChangeScene()
    {
        yield return new WaitForSeconds(1);
        countSecond += 1;
        progressBar.value =  1 - (1 / (float)countSecond);
        if (GameController.Instance.admobAds.IsOpenAdsReady)
        {
            wasCoolDown = false;
        }
        if (countSecond >= 5)
        {

            wasCoolDown = false;
       
        }
        if(wasCoolDown == true )
        {
            coroutineLoad = StartCoroutine(LoadAdsToChangeScene());
        }
        else
        {
            if (coroutineLoad != null)
            {
                StartCoroutine(ChangeScene());
                StopCoroutine(coroutineLoad);
                coroutineLoad = null;
            }
        }
 
    }


    IEnumerator LoadingText()
    {
        var wait = new WaitForSeconds(1f);
        while (true)
        {
            txtLoading.text = "LOADING .";
            yield return wait;

            txtLoading.text = "LOADING ..";
            yield return wait;

            txtLoading.text = "LOADING ...";
            yield return wait;

        }
    }
}
