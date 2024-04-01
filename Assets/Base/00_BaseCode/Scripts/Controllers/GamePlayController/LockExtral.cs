using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LockExtral : ExtralPlayBase
{
    public int idScewHaskey;
    public ScewBase scewHaskey;
    public PileBase pileHaskey;

    [SerializeField] private GameObject key;
    [SerializeField] private GameObject aura;

    public override void Init(ExtralPlayData data, int id)
    {
        base.Init(data, id);
        data.scewParent.isLockMove = true;
        idScewHaskey = (int)data.extralValues[0];




        if (data.extralValues.Length > 1)
        {
            int idBirdKey = (int)data.extralValues[1];
            int idStandKey = (int)data.extralValues[2];

            //Hiện chìa khoá ở chân chim idBirdHasKey
            var stand = GamePlayController.Instance.playerContain.currentPileInGame[idStandKey];

            scewHaskey = stand.scewOnArray[idBirdKey];
            key.transform.parent = scewHaskey.transform;
            key.transform.localPosition = new Vector3(0.1f, 0.07f, -0.245f);
        }
        else
        {
            //Hiện chìa khoá ở chân chim idBirdHasKey
            var birdsInGame = GameObject.FindObjectsOfType<ScewBase>();
            if (birdsInGame != null)
            {
                for (int i = 0; i < birdsInGame.Length; i++)
                {
                    if (birdsInGame[i].id != idScewHaskey)
                        continue;

                    scewHaskey = birdsInGame[i];
                    key.transform.parent = scewHaskey.transform;
                    key.transform.localPosition = new Vector3(0, -1.5f, 0);
                    break;
                }
            }
        }

        body.gameObject.transform.localScale = Vector3.zero;
        body.gameObject.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.7f);
        key.gameObject.transform.localScale = Vector3.zero;
        key.gameObject.transform.DOScale(new Vector3(0.08f, 0.08f, 0.08f), 0.7f);
        //body.gameObject.SetActive(false);
        //key.gameObject.SetActive(false);

    }

    public override void ReAppearExtral(int idBird, int idStand)
    {
 
    }

    public override void ResetExtral()
    {

    }

    protected override void CheckExtralHandle(ScewBase bird, PileBase stand, bool isFlyOut)
    {
        if (isFlyOut)
        {
            if (bird.id == idScewHaskey)
            {
                pileHaskey = stand;
                state = StateExtral.Success;
                if (m_ExtralSuccess != null)
                    StopCoroutine(m_ExtralSuccess);
                m_ExtralSuccess = ExtralSuccessHandle();
                StartCoroutine(m_ExtralSuccess);
               
                Debug.Log("=============== KeyOK ");
                //GamePlayController.Instance.tutorialController.DoneTutExtral();
            }
        }
    }

    protected override void ReturnExtralHandle()
    {
       
    }
    public override void PlayAnimEnd()
    {
      

        key.transform.DOScale(new Vector3(0, 0, 0), 0.7f);
        body.transform.DOScale(new Vector3(0, 0, 0), 0.7f).OnComplete(delegate {

            key.gameObject.SetActive(false);
            body.gameObject.SetActive(false);
            data.scewParent.isLockMove = false;
        });
        GamePlayController.Instance.gameScene.btnRemoveCage.interactable = false;
    }

    private IEnumerator ExtralSuccessHandle()
    {
        // this.transform.parent = GamePlayController.Instance.level.birdParent;
        //yield return new WaitUntil(() => data.scewParent.isFlyOut);
        yield return new WaitForSeconds(0.6f);


        key.transform.parent = pileHaskey.firstPost.transform;
        key.transform.DOLocalMove(new Vector3(0,1,1), 1).OnComplete(delegate {

            key.transform.parent = body.transform;
            key.transform.DOLocalMove(Vector3.zero, 0.7f).SetEase(Ease.InOutBack).OnComplete(delegate {

                key.gameObject.SetActive(false);
                body.gameObject.SetActive(false);
                data.scewParent.isLockMove = false;

            });


        });
        //    body.gameObject.transform.DOScale(new Vector3(0, 0, 0), 0.7f);
        //  PlayAnimEnd();

     
    }

    private void Update()
    {
        aura.transform.localEulerAngles += new Vector3(0, 0, 1);
    }
}
