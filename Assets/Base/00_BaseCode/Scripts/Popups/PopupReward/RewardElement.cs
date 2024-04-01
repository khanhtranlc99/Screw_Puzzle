using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RewardElement : MonoBehaviour
{
    public bool smallRewardElement;
    public Transform animParent;
    public Image branchIcon;
    public Image iconImg;
    [SerializeField] private Text valueTxt;
    [SerializeField] private GameObject rotateObj;
    public Transform aura;
    private int value;

    protected virtual void Update()
    {
        aura.localEulerAngles += new Vector3(0, 0, 30) * Time.deltaTime;
    }
    public virtual void Init(Sprite iconSpr, int value, GameObject rewardAnim = null, bool isAnim = true)
    {
        smallRewardElement = false;
        foreach (Transform child in animParent)
        {
            SimplePool.Despawn(child.gameObject);
        }
        iconImg.gameObject.SetActive(false);

        this.value = value;

        if (rewardAnim != null)
        {
            GameObject rw = SimplePool.Spawn(rewardAnim);
            Vector3 originalScale = rewardAnim.transform.localScale;
            rw.transform.SetParent(animParent);

        }
        else if (iconImg != null && iconSpr != null)
        {
            iconImg.gameObject.SetActive(true);
            iconImg.sprite = iconSpr;
        }

        if (valueTxt != null)
        {
            valueTxt.gameObject.SetActive(true);
            if (value == 0)
            {
                valueTxt.text = "";
            }
            else
                valueTxt.text = value.ToString();
        }
        
        if (isAnim)
        {
            this.transform.localScale = Vector3.zero;
            this.transform.DOKill();
            this.transform.DOScale(1, 0.3f).SetUpdate(true).SetEase(Ease.InBack).OnComplete(() => { if (rotateObj != null) rotateObj.gameObject.SetActive(true); });
        }
        else
        {
            this.transform.localScale = Vector3.one;
        }
    }

    public void Init(int playerId)
    {
        iconImg.gameObject.SetActive(false);
        valueTxt.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void MakeX2()
    {

        StartCoroutine(DoX2());
    }

    IEnumerator DoX2()
    {
        int curVal = value;
        int tarVal = 2 * value;

        int delta = ((tarVal - curVal) / 10);

        if (delta == 0)
        {
            delta = 1;
        }

        while (curVal < tarVal)
        {
            curVal += delta;
            if (curVal > tarVal) curVal = tarVal;

            DOTween.Sequence().Append(valueTxt.gameObject.GetComponent<RectTransform>().DOScale(1.1f, 0.01f)).Append(valueTxt.gameObject.GetComponent<RectTransform>().DOScale(1f, 0.01f));
            valueTxt.text = curVal.ToString();

            yield return new WaitForSeconds(0.02f);

        }

    }

}

public enum RewardType
{
    Bird = 0,
    Branch = 1,
    Theme = 2,
    Item = 3
}