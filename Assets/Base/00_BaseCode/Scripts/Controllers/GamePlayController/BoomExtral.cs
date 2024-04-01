using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class BoomExtral : ExtralPlayBase
{

    protected float timeCoolDown;
    [SerializeField] private TMP_Text coolDownTxt;
    [SerializeField] private GameObject coolDownObj;

    public GameObject vfxExplosion;
    public bool wasComplete;
    public AudioClip coolDownBoom;
    public AudioClip coolDownExplosion;
    public AudioSource audioSource;


    public override void Init(ExtralPlayData data, int id)
    {
        base.Init(data, id);
        body.gameObject.transform.localScale = Vector3.zero;
        body.gameObject.transform.DOScale(new Vector3(50, 50, 50), 0.7f).OnComplete(delegate {

          
        });

        timeCoolDown = this.data.extralValues[0];
        coolDownTxt.text = timeCoolDown.ToString();
        wasComplete = false;
    }

    public override void ReAppearExtral(int idBird, int idStand)
    {

    }

    public override void ResetExtral()
    {

    }

    protected override void CheckExtralHandle(ScewBase bird, PileBase stand, bool isFlyOut)
    {
        if (!isFlyOut)
        {
            ActiveCoolDown();
            Debug.LogError("ActiveCoolDown");
        }
        else
        {
            if (bird != null && data.scewParent.id == bird.id)
            {
                //Phá boom thành công, hiển thị effect phá boom
                PlayAnimEnd();
            }
            else
            {
                ActiveCoolDown();
            }
        }
    }

    protected override void ReturnExtralHandle()
    {

    }
    private void ActiveCoolDown()
    {
        if (wasComplete)
        {
            return;
        }
        timeCoolDown--;
        audioSource.PlayOneShot(coolDownBoom);
        if (timeCoolDown <= 3)
        {
            GamePlayController.Instance.tut_RemoveBoom_Booster.StartTut();
        }

        if (timeCoolDown <= 0)
        {
            timeCoolDown = 0;
            EndExtral(false);
        }

        coolDownTxt.text = timeCoolDown.ToString();
    }
    private IEnumerator ExtralSuccessHandle()
    {
        // this.transform.parent = GamePlayController.Instance.level.birdParent;
        //yield return new WaitUntil(() => data.scewParent.isFlyOut);
        yield return new WaitForSeconds(0.6f);

        body.gameObject.transform.DOScale(new Vector3(0, 0, 0), 0.7f);
  
    }
    public override void PlayAnimEnd()
    {

        state = StateExtral.Success;
        if (m_ExtralSuccess != null)
            StopCoroutine(m_ExtralSuccess);
        m_ExtralSuccess = ExtralSuccessHandle();
        StartCoroutine(m_ExtralSuccess);
        wasComplete = true;
        base.PlayAnimEnd();
        GamePlayController.Instance.gameScene.btnRemoveBoom.interactable = false;

    }
    public override void EndExtral(bool isDone)
    {
        base.EndExtral(isDone);
        StartCoroutine(ShowVFX());
    }
    private IEnumerator ShowVFX()
    {
        yield return new WaitForSeconds(0.5f);
        body.gameObject.SetActive(false);
        vfxExplosion.gameObject.SetActive(true);
        audioSource.PlayOneShot(coolDownExplosion);
        StartCoroutine(ShowPopupLose());
    }
    private IEnumerator ShowPopupLose()
    {
        yield return new WaitForSeconds(0.6f);
        LoseBox.Setup().Show();
    }
}
