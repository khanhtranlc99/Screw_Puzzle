using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RewardPackElement : MonoBehaviour
{
    public Transform animParent;
    public Image iconImg;
    [SerializeField] private Text valueTxt;
    [SerializeField] private GameObject rotateObj;
    public Transform aura;
    private int value;

    private void Update()
    {
        aura.localEulerAngles += new Vector3(0, 0, 30) * Time.deltaTime;
    }
    public void Init(Sprite iconSpr, int value, GameObject rewardAnim = null, bool isAnim = true)
    {
        foreach (Transform child in animParent)
        {
            SimplePool.Despawn(child.gameObject);
        }
        iconImg.gameObject.SetActive(false);

        this.value = value;

        if (iconImg != null && iconSpr != null)
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
        if (rewardAnim != null)
        {

            GameObject rw = SimplePool.Spawn(rewardAnim);
            rw.transform.SetParent(animParent);
            rw.transform.localScale = Vector3.one;
            rw.transform.position = Vector3.zero;
            rw.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

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
