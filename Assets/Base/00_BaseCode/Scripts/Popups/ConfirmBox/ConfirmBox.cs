using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConfirmBox : BaseBox
{
    private static GameObject instance;

    public UnityAction moreActionOff;


    private UnityAction actionCloseButton;
    public UnityAction actionHide;

    [SerializeField] private Button yesBtn;
    [SerializeField] private Text textYes;
    [SerializeField] private Button noBtn;
    [SerializeField] private Text textNo;
    [SerializeField] private Button closeBtn;


    [SerializeField] private Text titleText;
    [SerializeField] private Text messegerText;

    protected override void OnStart()
    {

        base.OnStart();
    }

    protected override void DoClose()
    {
        base.DoClose();
        if (moreActionOff != null)
            moreActionOff();

        mainPanel.localScale = Vector3.one;
        mainPanel.transform.DOScale(Vector3.zero, 0.5f).SetUpdate(true).SetEase(Ease.InBack).OnComplete(() =>
        {
            if (actionHide != null)
            {
                actionHide();
                actionHide = null;
            }
        });

    }

    public static ConfirmBox Setup()
    {
        if (instance == null)
        {
            // Create popup and attach it to UI
            instance = Instantiate(Resources.Load(PathPrefabs.CONFIRM_POPUP) as GameObject);
        }
        instance.SetActive(true);
        return instance.GetComponent<ConfirmBox>();
    }



    public void AddMessageYesNo(string str, string message, UnityAction actionYes, UnityAction actionNo, string stringYes = "", string stringNo = "")
    {
        titleText.text = str;
        messegerText.text = message;

        yesBtn.gameObject.SetActive(true);
        yesBtn.onClick.RemoveAllListeners();
        yesBtn.onClick.AddListener(() => { Close(); if (actionYes != null) actionYes(); });
       // yesBtn.onClick.AddListener(MusicManager.Instance.PlayClickBtnSound);
        if (textYes != null && stringYes != "")
            textYes.text = stringYes;
        else if (textYes != null)
            textYes.text = "Yes";

        noBtn.gameObject.SetActive(true);
        noBtn.onClick.RemoveAllListeners();
        noBtn.onClick.AddListener(() => { Close(); if (actionNo != null) actionNo(); });
        if (textNo != null && stringNo != "")
            textNo.text = stringNo;
        else if (textNo != null)
            textNo.text = "No";

        closeBtn.gameObject.SetActive(false);

    }

    public void AddMessageYes(string str, string message, UnityAction actionYes = null)
    {
        titleText.text = str;
        messegerText.text = message;

        yesBtn.gameObject.SetActive(true);
        yesBtn.onClick.RemoveAllListeners();
        yesBtn.onClick.AddListener(() => { Close(); if (actionYes != null) actionYes(); });
       // yesBtn.onClick.AddListener(MusicManager.Instance.PlayClickBtnSound);
        noBtn.gameObject.SetActive(false);
        closeBtn.gameObject.SetActive(false);
    }

    public void AddMessageYesHasCloseBtn(string str, string message, UnityAction actionYes)
    {
        titleText.text = str;
        messegerText.text = message;

        yesBtn.gameObject.SetActive(true);
        yesBtn.onClick.RemoveAllListeners();
        yesBtn.onClick.AddListener(() => { Close(); if (actionYes != null) actionYes(); });
       // yesBtn.onClick.AddListener(MusicManager.Instance.PlayClickBtnSound);
        noBtn.gameObject.SetActive(false);

        closeBtn.gameObject.SetActive(true);
    }
}
