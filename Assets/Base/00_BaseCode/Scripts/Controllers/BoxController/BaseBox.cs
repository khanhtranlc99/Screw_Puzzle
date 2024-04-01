using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseBox : MonoBehaviour
{
    [SerializeField] protected RectTransform mainPanel;

    [HideIf("isPopup", false)] public RectTransform contentPanel;

    [Header("========= CONFIG BOX ===========")]
    public bool isNotStack;
    public bool isPopup;
    [SerializeField] protected bool isAnim = true;
    protected Canvas popupCanvas;
    protected CanvasGroup canvasGroupPanel;
    [HideInInspector] public bool isBoxSave;

    protected string iDPopup;
    protected string GetIDPopup()
    {
        return iDPopup;
    }
    protected virtual void SetIDPopup()
    {
        iDPopup = this.GetType().ToString();
    }

    //Call Back
    public UnityAction OnCloseBox;
    public UnityAction<int> OnChangeLayer;
    protected UnityAction actionOpenSaveBox;

    public virtual T SetupBase<T>(bool isSaveBox = false, UnityAction actionOpenBoxSave = null) where T : BaseBox
    {
        InitBoxSave(isSaveBox, actionOpenBoxSave);
        return null;
    }


    private void Awake()
    {
        popupCanvas = this.GetComponent<Canvas>();
        if (popupCanvas != null && isPopup)
        {
            popupCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            popupCanvas.worldCamera = Camera.main;
            popupCanvas.sortingLayerID = SortingLayer.NameToID("Popup");
        }

        if (this.mainPanel != null)
        {
            var tweenAnimation = this.mainPanel.GetComponent<DOTweenAnimation>();
            if (tweenAnimation != null)
            {
                tweenAnimation.isIndependentUpdate = true;//Không phục thuộc vào time scale
                isAnim = false;
            }
        }

        OnAwake();
    }

    protected virtual void OnAwake()
    {

    }

    public void InitBoxSave(bool isBoxSave, UnityAction actionOpenSaveBox)
    {
        this.isBoxSave = isBoxSave;
        this.actionOpenSaveBox = actionOpenSaveBox;
    }

    #region Init Open Handle 
    protected virtual void OnEnable()
    {
        if (!isNotStack)
        {
            if (!isPopup)
                if (canvasGroupPanel != null)
                    canvasGroupPanel.blocksRaycasts = false;

            BoxController.Instance.AddNewBackObj(this);
        }
        SetIDPopup();
        DoAppear();
        OnStart();
    }

    protected virtual void DoAppear()
    {
        if (isAnim)
        {
            if (mainPanel != null)
            {
                mainPanel.localScale = Vector3.zero;
                mainPanel.DOScale(1, 0.5f).SetUpdate(true).SetEase(Ease.OutBack);
            }
        }
    }

    protected virtual void OnStart()
    {

    }
    #endregion

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Đưa popup vào trong Stack save
    /// </summary>
    public virtual void SaveBox()
    {
        if (isBoxSave)
            BoxController.Instance.AddBoxSave(GetIDPopup(), actionOpenSaveBox);
    }

    /// <summary>
    /// Chủ động gọi Remove Save Box theo trường hợp cụ thể
    /// </summary>
    public virtual void RemoveSaveBox()
    {
        BoxController.Instance.RemoveBoxSave(GetIDPopup());
    }

    #region Close Box
    public virtual void Close()
    {
        if (!isNotStack)
            BoxController.Instance.Remove();
        DoClose();
    }
    public virtual void CloseToPoint(Transform point)
    {
        if (!isNotStack)
            BoxController.Instance.Remove();
        DoCloseToPoint(point);
    }

    protected virtual void DoClose()
    {
        //if (isAnim)
        //{
        //    if (mainPanel != null)
        //    {
        //        mainPanel.localScale = Vector3.one;
        //        mainPanel.DOScale(0, 0.5f).SetUpdate(true).SetEase(Ease.InBack).OnComplete(() =>
        //        {

        //            this.gameObject.SetActive(false);
        //        });
        //    }
        //    else
        //    {

        //        this.gameObject.SetActive(false);
        //    }
        //}
        //else
        //{

            this.gameObject.SetActive(false);
        //}

        if (!isPopup)
        {
            if (canvasGroupPanel != null)
                canvasGroupPanel.blocksRaycasts = true;
        }
    }
    protected virtual void DoCloseToPoint(Transform point)
    {
        //if (isAnim)
        //{
        //    if (mainPanel != null)
        //    {
        //        Vector3 firstPos = mainPanel.position;
        //        mainPanel.localScale = Vector3.one;
        //        Vector3 target = new Vector3(point.position.x, point.position.y, mainPanel.position.z);

        //        //mainPanel.DOMove(target, 0.5f).SetUpdate(true).SetEase(Ease.InBack).OnComplete(() =>
        //        //{
        //        //});
        //        mainPanel.DOScale(0, 0.5f).SetUpdate(true).SetEase(Ease.InBack).OnComplete(() =>
        //        {
        //            this.transform.position = firstPos;
        //            this.gameObject.SetActive(false);
        //        });

        //    }
        //    else
        //    {

        //        this.gameObject.SetActive(false);
        //    }
        //}
        //else
        //{

            this.gameObject.SetActive(false);
        //}

        if (!isPopup)
        {
            if (canvasGroupPanel != null)
                canvasGroupPanel.blocksRaycasts = true;
        }
    }

    protected virtual void OnDisable()
    {
        if (OnCloseBox != null)
        {
            OnCloseBox();
            OnCloseBox = null;
        }

        if (BoxController.Instance.actionOnClosedOneBox != null)
            BoxController.Instance.actionOnClosedOneBox();
    }

    protected void DestroyBox()
    {
        if (OnCloseBox != null)
            OnCloseBox();
        Destroy(gameObject);
    }
    #endregion

    #region Change layer Box
    public void ChangeLayerHandle(ref int indexInStack)
    {
        if (isPopup)
        {
            if (popupCanvas != null)
            {
                popupCanvas.sortingOrder = BoxController.BASE_INDEX_LAYER + indexInStack;
                popupCanvas.planeDistance = 5;

                if (OnChangeLayer != null)
                    OnChangeLayer(popupCanvas.sortingLayerID);

                indexInStack += 40;
            }
        }
        else
        {
            if (contentPanel != null)
                this.transform.SetAsLastSibling();
        }
    }
    #endregion

    public virtual bool IsActive()
    {
        return true;
    }
}
