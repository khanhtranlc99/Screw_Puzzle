using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;
using UnityEngine.SceneManagement;
using MoreMountains.NiceVibrations;

public class DialogueRate : BaseBox
{

    private static DialogueRate instance;
    public static DialogueRate Setup()
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load<DialogueRate>(PathPrefabs.RATE_GAME_BOX));
            instance.Init();
        }
        //ChickenDataManager.CountTillShowRate = 0;
        instance.gameObject.SetActive(true);
        return instance;
    }
    private const int MIN_API_LEVEL_REVIEW = 21;
    //[SerializeField] private ReviewInAppController reviewInAppController;


    [SerializeField] private Button btnClose;
    [SerializeField] private Button btnConfirm;
    [SerializeField] private List<Sprite> lstSprStar;
    [SerializeField] private List<Button> lstBtnStar;
    [SerializeField] private List<Image> lstImgStar;
    private int countStar;
    public GameObject rotateAura;
    public void Init()
    {
        btnConfirm.onClick.AddListener(RateAction);
        btnClose.onClick.AddListener(CloseAction);
    }
    public void InitState()
    {
        for (int i = 0; i < lstBtnStar.Count; i++)
        {
            int index = i + 1;
            // lstBtnStar[i].onClick.AddListener(() => { ClickStar(index); });
            lstImgStar[i].sprite = lstSprStar[0];
        }
        countStar = 0;
    }
    public void ClickStar(int index)
    {
        countStar = index;
        for (int i = 0; i < lstImgStar.Count; i++)
        {
            lstImgStar[i].sprite = lstSprStar[0];
        }
        for (int i = 0; i < index; i++)
        {
            lstImgStar[i].sprite = lstSprStar[1];
        }
        //GameController.Instance.musicManager.Pla();
    }
    public void RateAction()
    {
        GameController.Instance.musicManager.PlayClickSound();
        if (countStar <= 0)
            return;
        if (countStar == 5)
        {
            UseProfile.CanShowRate = false;
            DirectlyOpen();
            NextLevel();


            //
        }
        else
        {
            NextLevel();

        }
    }
    private void NextLevel()
    {
        GameController.Instance.AnalyticsController.WinLevel(UseProfile.CurrentLevel);
        GameController.Instance.admobAds.HandleHideMerec();
        MMVibrationManager.Haptic(HapticTypes.Selection, false, true, this);
        GameController.Instance.musicManager.PlayClickSound();
        GamePlayController.Instance.tutLevel_1.EndTut();
        UseProfile.CurrentLevel += 1;
        SceneManager.LoadScene("GamePlay");
    }
    public void CloseAction()
    {
        NextLevel();


    }

    public void Next()
    {
        if (PlayerPrefs.GetInt("Level", 1) >= SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(UnityEngine.Random.Range(0, SceneManager.sceneCountInBuildSettings - 1));
            PlayerPrefs.SetInt("Level", (PlayerPrefs.GetInt("Level", 1) + 1));
        }
        else
        {

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("Level", (PlayerPrefs.GetInt("Level", 1) + 1));
        }
        PlayerPrefs.SetInt("levelnumber", PlayerPrefs.GetInt("levelnumber", 1) + 1);
    }

    public void ShowTextThankRate()
    {
        //StartCoroutine(Helper.StartAction(() =>
        //{
        GameController.Instance.moneyEffectController.SpawnEffectText_FlyUp
    (
       btnConfirm.transform.position,
        "Thank you for the review!",
        Color.white,
        isSpawnItemPlayer: true
    );
        // }, 0.5f));
    }
    public void DirectlyOpen()
    {
        string link = RemoteConfigController.GetStringConfig(FirebaseConfig.LINK_RATE,$"https://play.google.com/store/apps/details?id=com.gk.workoutfitness.mmaclub");
        Application.OpenURL(link);
    }

    private void Update()
    {
        rotateAura.transform.localEulerAngles += new Vector3(0, 0, 1);
    }
}