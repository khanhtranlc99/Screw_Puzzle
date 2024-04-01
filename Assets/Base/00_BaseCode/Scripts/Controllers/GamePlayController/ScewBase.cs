using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using System;
public class ScewBase : MonoBehaviour
{
    public int idIndex;
    public bool inFirstPost= false;
    public string ROTATE_LEFT = "ScewRotate_Left";
    public string ROTATE_RIGHT = "ScewRotate_Right";
    public int id;
    [SerializeField] private Renderer _renderer;
 
    [SerializeField] private Material[] lsMaterials;
    public Animator animation;
    protected int m_isLockMove;
    public ParticleSystem vfxScew;
    public bool isMoving;
    public bool isLockMove
    {
        get
        {
            return m_isLockMove > 0;
        }
        set
        {
            if (value)
                m_isLockMove++;
            else
            {
                m_isLockMove--;
                if (m_isLockMove < 0)
                    m_isLockMove = 0;
            }

            if (m_isLockMove <= 0)
            {
             

            }
        }
    }
 
    private  bool iscompleted;
    public bool IsCompleted
    {
        get
        {
            return iscompleted;
        }
        set
        {
            iscompleted = value;
           
        }
    }

    public List<ExtralPlayBase> extrals;


    public void Init(int paramId)
    {

        _renderer.material = lsMaterials[paramId];
        this.transform.localScale = new Vector3(0, 0, 0);
        id = paramId;
        IsCompleted = false;
        

    }
    
    public void Scale(float time, Action callBack)
    {
   
        this.transform.DOScale(new Vector3(1, 1, 1), time).OnComplete(delegate { ShowAnim(ROTATE_LEFT); callBack?.Invoke(); });

    }    

    public void MoveScew(Transform parent, float time, Action callBack)
    {
        this.transform.parent = parent;
      
        this.transform.DOLocalMove(Vector3.zero, time).OnComplete(delegate
        {
            callBack?.Invoke();
        });
     

    }

    public void LocalMove( float time, Action callBack)
    {
        this.transform.DOLocalMove(Vector3.zero, time).OnComplete(delegate
        {
            callBack?.Invoke();
        });
    }
    public void ShowAnim(string param)
    {
        animation.Play(param);
    }    


}
