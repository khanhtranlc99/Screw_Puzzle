using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class PopupRewardBase : BaseBox
{
    private static PopupRewardBase instance;

    [SerializeField] private RewardElement rewardPrefab;
    [SerializeField] private RewardElement smallRewardPrefab;
    protected bool smallReward;
    [SerializeField] private Transform contentPool;

    [SerializeField] private Button claimBtn;

    private List<RewardElement> _poolReward = new List<RewardElement>();
    private List<GiftRewardShow> _tempReward ;
    private List<GiftRewardShow> _reward;
    private Action _actionClaim;
    private bool isX2Reward;
    private bool isClickedClaim;
    private bool isClosing;
    private bool isAddValueItem;

    [SerializeField] private ParticleSystemRenderer[] parslayer;
    [SerializeField] private Canvas canvasRewardContent;

    [HideInInspector] public UnityAction actionMoreClaim;

    [SerializeField] private AudioClip rewardClip;
    public static PopupRewardBase Setup(bool smallReward = false, bool isSaveBox = false, Action actionOpenBoxSave = null)
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load<PopupRewardBase>(PathPrefabs.POPUP_REWARD_BASE));
        }
        //instance.Show();
        return instance;

    }

    public void Init()
    {

    }
    public void InitState(bool _smallReward)
    {
        smallReward = _smallReward;
    }
    IEnumerator showBtnClaimIE;
    public PopupRewardBase Show(List<GiftRewardShow> reward, Action actionClaim = null, float timeShowClaimNow = 0)
    {
        Debug.LogError("============= AAA ============== ");
        claimBtn.onClick.RemoveAllListeners();
        claimBtn.onClick.AddListener(Claim);
        claimBtn.interactable = true;
        _tempReward = new List<GiftRewardShow>();
        _tempReward = reward;
        _poolReward = new List<RewardElement>();
        if (isAnim)
        {
            if (mainPanel != null)
            {
                mainPanel.localScale = Vector3.zero;
                mainPanel.DOScale(1, 0.5f).SetUpdate(true).SetEase(Ease.OutBack).OnComplete(() => { });
            }
        }

        base.Show();
        _actionClaim = actionClaim;
        ClearPool();
        // BoxController.Instance.isLockEscape = true;
        canvasRewardContent.sortingLayerID = SortingLayer.NameToID("Popup");
        canvasRewardContent.sortingOrder = popupCanvas.sortingLayerID + 2;

        if(reward.Count > 9)
        {
            //canvasRewardContent.GetComponent<GridLayoutGroup>().padding.top = -216;
            canvasRewardContent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(240f, 240f);
            canvasRewardContent.GetComponent<GridLayoutGroup>().spacing = new Vector2(20f, 80f);

        }
        for (int i = 0; i < reward.Count; i++)
        {
            RewardElement elem = GetRewardElement();

            if (reward[i].icon == null)
            {
                if (reward[i].rewardAnim == null)
                {
                    elem.Init(GetIcon(reward[i].type), reward[i].amount, GetAnim(reward[i].type));
                }
                else
                {
                    elem.Init(GetIcon(reward[i].type), reward[i].amount, reward[i].rewardAnim);
                }
            }
            else

            {
                if (reward[i].rewardAnim != null)
                {
                    elem.Init(reward[i].icon, reward[i].amount, reward[i].rewardAnim);
                }
                else
                {
                    elem.Init(reward[i].icon, reward[i].amount, GetAnim(reward[i].type));
                }
            }

            if (GiftDatabase.IsCharacter(reward[i].type))
            {
                elem.iconImg.transform.localScale = 4f * Vector3.one;
            }
            else
            {
                elem.iconImg.transform.localScale = 1.5f * Vector3.one;
            }
        }
        this._reward = reward;

        isClickedClaim = false;
        isClosing = false;

        OnCloseBox = () =>
        {
            isClosing = true;
            Claim();
            if (actionMoreClaim != null)
            {
                actionMoreClaim();
                actionMoreClaim = null;
            }
        };

        claimBtn.gameObject.SetActive(true);
        claimBtn.transform.localScale = Vector3.one;
        isX2Reward = false;

        for (int i = 0; i < parslayer.Length; i++)
        {
            parslayer[i].sortingOrder = popupCanvas.sortingOrder + 1;
        }

        GameController.Instance.musicManager.PlayOneShot(rewardClip);

      
        return this;
    }

    private RewardElement GetRewardElement()
    {
        for (int i = 0; i < _poolReward.Count; i++)
        {
            if (!_poolReward[i].gameObject.activeSelf)
            {
                if(_poolReward[i].smallRewardElement == smallReward)
                {
                    _poolReward[i].gameObject.SetActive(true);
                    return _poolReward[i];
                }
            }
        }
        RewardElement element = null;
  
        if (smallReward)
        {
          
            element = Instantiate(smallRewardPrefab, contentPool);
        }
        else
        {
            element = Instantiate(rewardPrefab, contentPool);
        }

        _poolReward.Add(element);
      

        return element;
    }

    private void ClearPool()
    {
        foreach (var rewardElement in _poolReward)
        {
            rewardElement.gameObject.SetActive(false);
        }
    }

    public void Claim()
    {
        if (isClickedClaim)
            return;

        ClaimSuccess();
        GameController.Instance.musicManager.PlayClickSound();
    }

    IEnumerator ClaimWithDelay()
    {
        yield return new WaitForSeconds(0.8f);

        ClaimSuccess();
    }

    public void ClaimSuccess()
    {

        ClearPool();




        Close();



        //  BoxController.Instance.isLockEscape = false;


        if (_actionClaim != null)
            _actionClaim();
        claimBtn.interactable = false;




    }

    private IEnumerator Closeee()
    {
        yield return new WaitForSeconds(0.5f);
        if (!isClosing)
        Close();
    
    }


    private Sprite GetIcon(GiftType type)
    {
        return GameController.Instance.dataContain.giftDatabase.GetIconItem(type);
    }
    private GameObject GetAnim(GiftType type)
    {
        return GameController.Instance.dataContain.giftDatabase.GetAnimItem(type);
    }
}

[System.Serializable]
public class GiftRewardShow
{
    public GiftType type;
    public int amount = 0;
    public Sprite icon = null;
    public GameObject rewardAnim = null;
}
