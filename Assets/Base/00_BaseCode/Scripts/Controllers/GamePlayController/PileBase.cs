using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using MoreMountains.NiceVibrations;
public class PileBase : SerializedMonoBehaviour
{
    public int id;
    public PileData pileData;
    public Transform firstPost;
    public List<Transform> lsPost;
    public Stack<ScewBase> stackScew;
    public ParticleSystem vfxConfetti;
    public List<GameObject> lsVfxStar;
    public SpriteRenderer spriteRender;
    public Sprite vIcon;
    public Sprite xIcon;
    public Sprite fullIcon;
    public ScewBase[] scewOnArray
    {
        get
        {
            return stackScew.ToArray();
        }
    }
    public int GetIndexScewInFirstPost
    {
        get
        {
            for (int i = 0; i < scewOnArray.Length; i++)
            {
                if (scewOnArray[i].inFirstPost == true)
                {
                    return i;
                }
            }
            return -1;
        }
    }

    private bool isComPleted;
    public bool IsCompleted
    {
        get
        {
            return isComPleted;
        }
        set
        {
            isComPleted = value;
            if (isComPleted)
            {
                foreach (var item in lsVfxStar)
                {
                    item.gameObject.SetActive(true);
                }
            }
            else
            {
                foreach (var item in lsVfxStar)
                {
                    item.gameObject.SetActive(false);
                }
            }
        }
    }



    public bool GetPileSuccess
    {
        get
        {
            var item = stackScew.Peek();
            for (int i = 0; i < scewOnArray.Length; i++)
            {
                if (scewOnArray[i].id != item.id)
                {
                    return false;
                }
            }

            if (stackScew.Count >= pileData.numSlot)
            {
                for (int i = 0; i < scewOnArray.Length; i++)
                {
                    scewOnArray[i].IsCompleted = true;
                }
                vfxConfetti.Play();
                audio.PlayOneShot(audioClipConfeti);
                MMVibrationManager.Haptic(HapticTypes.Selection, false,true,this);
                return true;
            }

            return false;
        }
    }
    public bool GetScewComplete
    {
        get
        {
            for (int i = 0; i < scewOnArray.Length; i++)
            {
                if (scewOnArray[i].IsCompleted == false)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public List<ScewBase> GetScewSameEnd(bool isGetLockMove = true)
    {
        List<ScewBase> result = new List<ScewBase>();
        int numBrid = stackScew.Count;
        var lst_ScewOn = stackScew.ToArray();
        if (numBrid > 0)
        {
            var ScewTop = stackScew.Peek();

            int idScewEnd = ScewTop.id;
            if (isGetLockMove)
                result.Add(ScewTop);
            else
            {
                if (!ScewTop.isLockMove)
                    result.Add(ScewTop);
                else
                    return result;
            }
            for (int i = 1; i < lst_ScewOn.Length; i++)
            {
                int index = i;
                if (idScewEnd == lst_ScewOn[index].id)
                {
                    if (isGetLockMove)
                        result.Add(lst_ScewOn[index]);
                    else
                    {
                        if (!lst_ScewOn[index].isLockMove)
                            result.Add(lst_ScewOn[index]);
                        else
                            break;
                    }
                }
                else
                    break;
            }
        }
        return result;
    }

    public List<ExtralPlayBase> extrals;
    public AudioSource audio;
    public AudioClip audioClipConfeti;


    public bool CanTouchMove
    {
        get
        {
            foreach(var item in scewOnArray)
            {
                if(item.isMoving == true)
                {
                    return false;
                }
            }

            return true;
        }
    }
    public IEnumerator Init(List<int> idScew)
    {
        stackScew = new Stack<ScewBase>();
        yield return new WaitForSeconds(1);

        for (int i = 0; i < idScew.Count; i++)
        {
            int index = i;
            var ScewPrefab = GamePlayController.Instance.playerContain.scewBase;
            var Scew = Instantiate(ScewPrefab, firstPost).GetComponent<ScewBase>();
            Scew.Init(idScew[i]);
            Scew.gameObject.name = idScew[i].ToString();
            stackScew.Push(Scew);
            GamePlayController.Instance.playerContain.sumScewCurrent += 1;
        }
        if (GamePlayController.Instance.tutLevel_1.tutorials[0].IsCanShowTut())
        {
            var temp1 = lsPost[0];
            var temp2 = lsPost[1];
            var temp3 = lsPost[2];
            var temp4 = lsPost[3];
            lsPost[0] = temp3;
            lsPost[1] = temp4;
            lsPost[2] = temp1;
            lsPost[3] = temp2;
        }

        for (int i = 0; i < scewOnArray.Length; i++)
        {
            int index = i;
            float time = 0;
            if (index > 0)
            {
                time = (float)(1 / (float)index);

            }
            else
            {
                time = (float)(1 / 0.8f);
            }
            scewOnArray[index].Scale(time, delegate
            {
                scewOnArray[index].transform.parent = lsPost[index];
                scewOnArray[index].LocalMove(0.5f, delegate { GamePlayController.Instance.playerContain.CheckScewWasInit(); });

            });
        }
        HandleIDScew();
        IsCompleted = false;

    }
    public void HandleTutLevel1Step2()
    {
        var temp1 = lsPost[0];
        var temp2 = lsPost[1];
        var temp3 = lsPost[2];
        var temp4 = lsPost[3];
        lsPost[0] = temp3;
        lsPost[1] = temp4;
        lsPost[2] = temp1;
        lsPost[3] = temp2;

    }
    public void HandleIDScew()
    {
        for (int i = 0; i < scewOnArray.Length; i++)
        {
            scewOnArray[i].idIndex = i;
        }
    }
   
    public void HandleIDScewReturn()
    {
        for (int i = 0; i < scewOnArray.Length; i++)
        {
          for(int j = 0; j < lsPost.Count; j++)
            {
                scewOnArray[i].idIndex = j;
            }    
        }
    }

    public void HandleSortScew()
    {
        var temp = GetIndexScewInFirstPost;
     //   Debug.LogError("GetIndexScewInFirstPost " + GetIndexScewInFirstPost);
       if (temp != -1)
        {
            scewOnArray[temp].inFirstPost = false;
            scewOnArray[temp].transform.parent = lsPost[scewOnArray[temp].idIndex];
            scewOnArray[temp].ShowAnim(scewOnArray[temp].ROTATE_LEFT);
            scewOnArray[temp].LocalMove(0.5f, delegate {  });
        
        }
    }    

    public void OnMouseDown()
    {
        if (!GamePlayController.Instance.playerContain.allScewInitDone)
        {
            return;
        }
        if (GamePlayController.Instance.playerContain.wasTouchPile)
        {
            return;
        }
        if(!CanTouchMove)
        {
            return;
        }
        if (IsCompleted)
        {
            return;
        }
     

        GamePlayController.Instance.playerContain.wasTouchPile = true;
         var currentPile = GamePlayController.Instance.playerContain.currentPile;
        if (currentPile != null)
        {
            GamePlayController.Instance.playerContain.HandleShowTutXV(false);
            if (currentPile != this) // nếu pile được bấm không giống với currentpile
            {
          
                StartCoroutine(HandleCheckScewChangePile(currentPile));
  
            }
            else //nếu giống
            {

                if(UseProfile.CurrentLevel == 1)
                {
                    return;
                }
            
                HandleSortScew();
                GamePlayController.Instance.playerContain.currentPile = null;

                GameController.Instance.musicManager.PlayScrewInSound();
              
            }
     
        }
        else
        {
            OnChoicePile(); // lần đầu chạm vào pile
            GamePlayController.Instance.playerContain.HandleShowTutXV(true);
            GameController.Instance.musicManager.PlayScrewOutSound();
       
        }
      
        GamePlayController.Instance.tutLevel_1.NextTut();
        GamePlayController.Instance.tutLevel_2.NextTut();
    }

    private void OnChoicePile()
    {
      
        List<ScewBase> scewEnd = GetScewSameEnd();
        if(scewEnd.Count > 0)
        {
            if(scewEnd[0].isLockMove)
            {
                scewEnd[0].extrals[0].transform.DOShakeRotation(1,9);
                return;
            }
         
            scewEnd[0].inFirstPost = true;
            scewEnd[0].ShowAnim(scewEnd[0].ROTATE_RIGHT);  
            scewEnd[0].MoveScew(this.firstPost, 0.3f, delegate
            {
          
            });
            GamePlayController.Instance.playerContain.currentPile = this;
        
        }
    }
    private IEnumerator HandleCheckScewChangePile(PileBase pileBase)
    {
        if(pileBase != null)
        {
         if(stackScew.Count >= pileData.numSlot)
            {
                GamePlayController.Instance.playerContain.HandleCallAllSort();
                GamePlayController.Instance.playerContain.currentPile = this;
                OnChoicePile();
                GamePlayController.Instance.playerContain.HandleShowTutXV(true);
                GameController.Instance.musicManager.PlayScrewOutSound();
                yield return null;
            }
         else
          {
                
                GamePlayController.Instance.tut_BoosterRedo.StartTut();
                GamePlayController.Instance.tut_RemoveLock_Booster.StartTut();
                float delayMove = 0.08f;
                List<ScewBase> ScewEnd_currentPile = pileBase.GetScewSameEnd(isGetLockMove: false); // lấy ra những scew cùng màu cuối Currentpile
                int numScewEnd_currentStand = ScewEnd_currentPile.Count;

                List<ScewBase> ScewEnd_this = this.GetScewSameEnd(isGetLockMove: false); // lấy ra những scew cùng màu cuối pile mới
                int numScewEnd_this = ScewEnd_this.Count;

           


                if (numScewEnd_this > 0) // nếu pilethis đang có scew
                {

                    if(numScewEnd_currentStand > 0) // nếu currentPile đang có scew mang theo
                    {
                        if (pileData.numSlot > stackScew.Count) // nếu còn chỗ chứa 
                        {
                            if (ScewEnd_this[0].id == ScewEnd_currentPile[0].id) // nếu 2 scew của 2 pile giống nhau
                            {
                             
                                int numSlotLeft = pileData.numSlot - stackScew.Count; // số lượng scew có thể lắp
                                int numScewMove = 0;
                                if(numScewEnd_currentStand >= numSlotLeft)
                                {
                                    numScewMove = numSlotLeft;
                                }
                                else
                                {
                                    numScewMove = numScewEnd_currentStand;
                                }

                                // viết return vào đây
                                DataReturn dataReturn = new DataReturn();
                                List<ScewBase> scewMove = new List<ScewBase>();
                                for (int i = 0; i < numScewMove; i++)
                                {
                                    int index = i;
                                    if (index >= numScewEnd_currentStand)
                                        yield return null;
                                    var scew = pileBase.stackScew.Pop();
                                    this.stackScew.Push(scew);
                                    scewMove.Add(scew);

                                    DataReturnModle retu = new DataReturnModle();
                                    retu.scew = scew;
                                    retu.oldStand = pileBase;
                                    retu.idOldSlot = pileBase.stackScew.Count;
                                    retu.newStand = this;
                                    retu.idIndexOld = scew.idIndex;
                                    dataReturn.data.Add(retu);
                                }
                                GamePlayController.Instance.returnController.HandleAddStack(dataReturn);
                                GamePlayController.Instance.playerContain.currentPile = null;
                                GamePlayController.Instance.returnController.IsLockReturn = true;
                                //FindFor
                                GameController.Instance.musicManager.PlayScrewInSound();
                                for (int i = 0; i < scewMove.Count; i++)
                                {
                                    int index = i;
                                    int post = (lsPost.Count - 1) - (pileData.numSlot - numSlotLeft + index);
                                    if (i == scewMove.Count - 1)
                                    {
                                        scewMove[index].isMoving = true;
                                        scewMove[index].ShowAnim(scewMove[index].ROTATE_RIGHT);
                                        scewMove[index].MoveScew(pileBase.firstPost, i*0.2f, delegate {
                                            scewMove[index].MoveScew(this.firstPost, 0.3f, delegate {
                                                scewMove[index].ShowAnim(scewMove[index].ROTATE_LEFT);
                                                scewMove[index].MoveScew(lsPost[post], 0.3f, delegate {
                                                    scewMove[index].idIndex = post;
                                                    scewMove[index].inFirstPost = false;
                                                    IsCompleted = GetPileSuccess;
                                                    GamePlayController.Instance.playerContain.HandleCheckWin();
                                                    HandleCheckExtral();
                                                    scewMove[index].vfxScew.Play();
                                                    scewMove[index].isMoving = false;
                                                    //  Debug.LogError("EndMove");
                                                });
                                            });
                                        });
                                    }
                                    else
                                    {
                                        scewMove[index].isMoving = true;
                                        scewMove[index].ShowAnim(scewMove[index].ROTATE_RIGHT);
                                        scewMove[index].MoveScew(pileBase.firstPost, i * 0.2f, delegate {
                                            scewMove[index].MoveScew(this.firstPost, 0.3f, delegate {
                                                scewMove[index].ShowAnim(scewMove[index].ROTATE_LEFT);
                                                scewMove[index].MoveScew(lsPost[post], 0.3f, delegate {
                                                    scewMove[index].idIndex = post;
                                                    scewMove[index].inFirstPost = false;
                                                    scewMove[index].vfxScew.Play();
                                                    scewMove[index].isMoving = false;
                                                });
                                            });

                                        });
                                        yield return new WaitForSeconds(delayMove);

                                    }                                               
                                }                                                  
                            }
                            else
                            {
                              
                                GamePlayController.Instance.playerContain.HandleCallAllSort();
                                HandleErrorSort();
                                yield return null;
                            }
                        }
                        else
                        {
                            Debug.LogError("khongconchochua");
                            GamePlayController.Instance.playerContain.HandleCallAllSort();
                            HandleErrorSort();
                            yield return null;
                        }
                    }    
                    else
                    {
                        Debug.LogError("gidaykhonghieu");
                        GamePlayController.Instance.playerContain.HandleCallAllSort();
                        HandleErrorSort();
                        yield return null;
                    }

                }   
                else
                {
        
                    if (this.stackScew.Count > 0)
                    {
                        Debug.LogError("co scew");
                        List<ScewBase> scews_c = pileBase.GetScewSameEnd();
                        List<ScewBase> scews_t = this.GetScewSameEnd();
                        bool isCanChangePile = false;

                        if (scews_c.Count > 0 && scews_t.Count > 0)
                        {
                            if (scews_c[0].id == scews_t[0].id)
                            {
                                isCanChangePile = true;
                            }
                        }
                        if (isCanChangePile)
                        {
                            if (numScewEnd_currentStand > 0)
                            {
                              
                                int numLeftThis = pileData.numSlot - this.stackScew.Count;
                                if (numLeftThis >= numScewEnd_currentStand)
                                {
                                    DataReturn dataReturn = new DataReturn();
                                    List<ScewBase> scewMove = new List<ScewBase>();
                                    for (int i = 0; i < numScewEnd_currentStand; i++)
                                    {
                                        int index = i;
                                        var scew = pileBase.stackScew.Pop();
                                        this.stackScew.Push(scew);
                                        scewMove.Add(scew);


                                        DataReturnModle retu = new DataReturnModle();
                                        retu.scew = scew;
                                        retu.oldStand = pileBase;
                                        retu.idOldSlot = pileBase.stackScew.Count;
                                        retu.newStand = this;
                                        retu.idIndexOld = scew.idIndex;
                                        dataReturn.data.Add(retu);
                                    }
                                    GamePlayController.Instance.returnController.HandleAddStack(dataReturn);
                                    GamePlayController.Instance.playerContain.currentPile = null;
                                    GamePlayController.Instance.returnController.IsLockReturn = true;
                                    GameController.Instance.musicManager.PlayScrewInSound();
                                    //FindFor
                                    for (int i = 0; i < scewMove.Count; i++)
                                    {
                                        int index = i;
                                        int post = (lsPost.Count - 1) - (pileData.numSlot - numLeftThis + index); 
                                        if (i == scewMove.Count - 1)
                                        {
                                            scewMove[index].isMoving = true;
                                            scewMove[index].ShowAnim(scewMove[index].ROTATE_RIGHT);
                                            scewMove[index].MoveScew(pileBase.firstPost, i * 0.2f, delegate {
                                                scewMove[index].MoveScew(this.firstPost, 0.3f, delegate {
                                                    scewMove[index].ShowAnim(scewMove[index].ROTATE_LEFT);
                                                    scewMove[index].MoveScew(lsPost[post], 0.3f, delegate {
                                                        scewMove[index].inFirstPost = false;
                                                        scewMove[index].idIndex = post;
                                                        IsCompleted = GetPileSuccess;
                                                        GamePlayController.Instance.playerContain.HandleCheckWin();
                                                        HandleCheckExtral();
                                                        scewMove[index].vfxScew.Play();
                                                        scewMove[index].isMoving = false;
                                                        //Debug.LogError("EndMove");

                                                    });
                                                });
                                            });
                                        }    
                                        else
                                        {
                                            scewMove[index].isMoving = true;
                                            scewMove[index].ShowAnim(scewMove[index].ROTATE_RIGHT);
                                            scewMove[index].MoveScew(pileBase.firstPost, i * 0.2f, delegate {
                                                scewMove[index].MoveScew(this.firstPost, 0.3f, delegate {
                                                    scewMove[index].ShowAnim(scewMove[index].ROTATE_LEFT);
                                                    scewMove[index].MoveScew(lsPost[post], 0.3f, delegate {
                                                        scewMove[index].idIndex = post;
                                                        scewMove[index].inFirstPost = false;
                                                        scewMove[index].vfxScew.Play();
                                                        scewMove[index].isMoving = false;
                                                    });
                                                });

                                            });
                                            yield return new WaitForSeconds(delayMove);
                                        }    
                                    }

                                }
                                else // ko đủ chỗ chứa
                                {
                                    DataReturn dataReturn = new DataReturn();
                                    List<ScewBase> scewMove = new List<ScewBase>();
                                    for (int i = 0; i < numLeftThis; i++)
                                    {
                                        int index = i;
                                        var scew = pileBase.stackScew.Pop();
                                        this.stackScew.Push(scew);
                                        scewMove.Add(scew);

                                        DataReturnModle retu = new DataReturnModle();
                                        retu.scew = scew;
                                        retu.oldStand = pileBase;
                                        retu.idOldSlot = pileBase.stackScew.Count;
                                        retu.newStand = this;
                                        retu.idIndexOld = scew.idIndex;
                                        dataReturn.data.Add(retu);
                                    }
                                    GamePlayController.Instance.returnController.HandleAddStack(dataReturn);
                                    GamePlayController.Instance.playerContain.currentPile = null;
                                    GamePlayController.Instance.returnController.IsLockReturn = true;
                                    GameController.Instance.musicManager.PlayScrewInSound();
                                    //FindFor
                                    for (int i = 0; i < scewMove.Count; i++)
                                    {
                                        int index = i;
                                        int post = (lsPost.Count - 1) - (pileData.numSlot - numLeftThis + index);
                                        if (i == scewMove.Count - 1)
                                        {
                                            scewMove[index].isMoving = true;
                                            scewMove[index].ShowAnim(scewMove[index].ROTATE_RIGHT);
                                            scewMove[index].MoveScew(pileBase.firstPost, i*0.2f, delegate {
                                                scewMove[index].MoveScew(this.firstPost, 0.3f, delegate {
                                                    scewMove[index].ShowAnim(scewMove[index].ROTATE_LEFT);
                                                    scewMove[index].MoveScew(lsPost[post], 0.3f, delegate {
                                                        scewMove[index].idIndex = post;
                                                        scewMove[index].inFirstPost = false;
                                                        IsCompleted = GetPileSuccess;
                                                        GamePlayController.Instance.playerContain.HandleCheckWin();
                                                        HandleCheckExtral();
                                                        scewMove[index].vfxScew.Play();
                                                        scewMove[index].isMoving = false;
                                                        //      Debug.LogError("EndMove");
                                                    });
                                                });
                                            });
                                        }                                         
                                        else
                                        {
                                            scewMove[index].isMoving = true;
                                            scewMove[index].ShowAnim(scewMove[index].ROTATE_RIGHT);
                                            scewMove[index].MoveScew(pileBase.firstPost, i * 0.2f, delegate {
                                                scewMove[index].MoveScew(this.firstPost, 0.3f, delegate {
                                                    scewMove[index].ShowAnim(scewMove[index].ROTATE_LEFT);
                                                    scewMove[index].MoveScew(lsPost[post], 0.3f, delegate {
                                                        scewMove[index].idIndex = post;
                                                        scewMove[index].inFirstPost = false;
                                                        scewMove[index].vfxScew.Play();
                                                        scewMove[index].isMoving = false;
                                                    });
                                                });

                                            });
                                            yield return new WaitForSeconds(delayMove);
                                        }   
                                    }
                                }
                            }
                            else
                            {
                                HandleErrorSort();
                                yield return null;
                            }
                        }
                        else
                        {
                            Debug.LogError("gidaykhonghieu");
                            HandleErrorSort();
                            yield return null;
                        }

                    }
                    else
                    {
                    
                        if (numScewEnd_currentStand > 0)
                        {                     
                            if (pileData.numSlot >= numScewEnd_currentStand)
                            {
                                DataReturn dataReturn = new DataReturn();
                                List<ScewBase> scewMove = new List<ScewBase>();
                                for (int i = 0; i < numScewEnd_currentStand; i++)
                                {
                                    int index = i;
                                    var scew = pileBase.stackScew.Pop();
                                    this.stackScew.Push(scew);
                                    scewMove.Add(scew);

                                    DataReturnModle retu = new DataReturnModle();
                                    retu.scew = scew;
                                    retu.oldStand = pileBase;
                                    retu.idOldSlot = pileBase.stackScew.Count;
                                    retu.newStand = this;
                                    retu.idIndexOld = scew.idIndex;
                                    dataReturn.data.Add(retu);
                                }
                                GamePlayController.Instance.returnController.HandleAddStack(dataReturn);
                                GamePlayController.Instance.playerContain.currentPile = null;
                                GamePlayController.Instance.returnController.IsLockReturn = true;
                                GameController.Instance.musicManager.PlayScrewInSound();
                                //FindFor
                                for (int i = 0; i < scewMove.Count; i++)
                                {
                                    int index = i;
                                    int post = (lsPost.Count -1) - index;
                                  
                                    if (i == numScewEnd_currentStand - 1)
                                    {
                                        scewMove[index].isMoving = true;
                                        scewMove[index].ShowAnim(scewMove[index].ROTATE_RIGHT);
                                        scewMove[index].MoveScew(pileBase.firstPost, i *0.2f, delegate {
                                            scewMove[index].MoveScew(this.firstPost, 0.3f, delegate {
                                                scewMove[index].ShowAnim(scewMove[index].ROTATE_LEFT);
                                                scewMove[index].MoveScew(lsPost[post], 0.3f, delegate {
                                                    scewMove[index].idIndex = post;
                                                    scewMove[index].inFirstPost = false;
                                                    IsCompleted = GetPileSuccess;
                                                    GamePlayController.Instance.playerContain.HandleCheckWin();
                                                    HandleCheckExtral();
                                                    scewMove[index].vfxScew.Play();
                                                    scewMove[index].isMoving = false;
                                                });
                                            });
                                        });
                                    }                                       
                                    else
                                    {
                                        scewMove[index].isMoving = true;
                                        scewMove[index].ShowAnim(scewMove[index].ROTATE_RIGHT);
                                        scewMove[index].MoveScew(pileBase.firstPost, i * 0.2f, delegate {
                                            scewMove[index].MoveScew(this.firstPost, 0.3f, delegate {
                                                scewMove[index].ShowAnim(scewMove[index].ROTATE_LEFT);
                                                scewMove[index].MoveScew(lsPost[post], 0.3f, delegate {
                                                    scewMove[index].idIndex = post;
                                                    scewMove[index].inFirstPost = false;
                                                    scewMove[index].vfxScew.Play();
                                                    scewMove[index].isMoving = false;
                                                });
                                            });

                                        });
                                        yield return new WaitForSeconds(delayMove);
                                    }    
                                      
                                }

                            }    
                            else
                            {
                                Debug.LogError("2222");
                                DataReturn dataReturn = new DataReturn();
                                List<ScewBase> scewMove = new List<ScewBase>();
                                for (int i = 0; i < numScewEnd_currentStand; i++)
                                {
                                    int index = i;
                                    var scew = pileBase.stackScew.Pop();
                                    this.stackScew.Push(scew);
                                    scewMove.Add(scew);

                                    DataReturnModle retu = new DataReturnModle();
                                    retu.scew = scew;
                                    retu.oldStand = pileBase;
                                    retu.idOldSlot = pileBase.stackScew.Count;
                                    retu.newStand = this;
                                    retu.idIndexOld = scew.idIndex;
                                    dataReturn.data.Add(retu);
                                }
                                GamePlayController.Instance.returnController.HandleAddStack(dataReturn);
                                GamePlayController.Instance.playerContain.currentPile = null;
                                GamePlayController.Instance.returnController.IsLockReturn = true;
                                GameController.Instance.musicManager.PlayScrewInSound();
                                //FindFor
                                for (int i = 0; i < scewMove.Count; i++)
                                {
                                    int index = i;
                                    int post = (lsPost.Count - 1) - index;
                                    Debug.LogError("post " + post);
                                    if (i == pileData.numSlot - 1)
                                    {
                                        scewMove[index].isMoving = true;
                                        scewMove[index].ShowAnim(scewMove[index].ROTATE_RIGHT);
                                        scewMove[index].MoveScew(pileBase.firstPost, i * 0.2f, delegate {
                                            scewMove[index].MoveScew(this.firstPost, 0.3f, delegate {
                                                scewMove[index].ShowAnim(scewMove[index].ROTATE_LEFT);
                                                scewMove[index].MoveScew(lsPost[post], 0.3f, delegate {
                                                    scewMove[index].idIndex = post;
                                                    scewMove[index].inFirstPost = false;
                                                    IsCompleted = GetPileSuccess;
                                                    GamePlayController.Instance.playerContain.HandleCheckWin();
                                                    HandleCheckExtral();
                                                    scewMove[index].vfxScew.Play();
                                                    scewMove[index].isMoving = false;
                                                    // Debug.LogError("EndMove");
                                                });
                                            });
                                        });
                                    }                                        
                                    else
                                    {
                                        scewMove[index].isMoving = true;
                                        scewMove[index].ShowAnim(scewMove[index].ROTATE_RIGHT);
                                        scewMove[index].MoveScew(pileBase.firstPost, i * 0.2f, delegate {
                                            scewMove[index].MoveScew(this.firstPost, 0.3f, delegate {
                                                scewMove[index].ShowAnim(scewMove[index].ROTATE_LEFT);
                                                scewMove[index].MoveScew(lsPost[post], 0.3f, delegate {
                                                    scewMove[index].idIndex = post;
                                                    scewMove[index].inFirstPost = false;
                                                    scewMove[index].vfxScew.Play();
                                                    scewMove[index].isMoving = false;
                                                });
                                            });
                                        });
                                        yield return new WaitForSeconds(delayMove);
                                    }    
                                }
                            }    
                        } 
                        else
                        {
                            Debug.LogError("gidaykhonghieu");
                            HandleErrorSort();
                        
                        }    
                    }    


                }
            }    
        }
    }
    public void BackTurn(ScewBase scew, bool isOldPile, int idIndexOld, PileBase pileOld, PileBase pileCurrent)
    {
        HandleSortScew();
        if (isOldPile)
        {
            pileCurrent.IsCompleted = false;
            stackScew.Push(scew);
            float time = 0;
            if (idIndexOld > 0)
            {
                time = (float)(1 / (float)idIndexOld);

            }
            else
            {
                time = (float)(1 / 1.2);
            }

            scew.ShowAnim(scew.ROTATE_RIGHT);
            scew.MoveScew(pileCurrent.firstPost, time, delegate
            {
                scew.MoveScew(pileOld.firstPost, 0.3f, delegate
                {
                    scew.ShowAnim(scew.ROTATE_RIGHT);
                    scew.MoveScew(lsPost[idIndexOld], 0.3f, delegate {
                        scew.inFirstPost = false;
                        scew.idIndex = idIndexOld;
                    });
                });
            });

        

        }
        else
        {
            if (stackScew.Count > 0)
            {
                stackScew.Pop();

            }

        }
    
    }


    public void HandleCheckExtral()
    {
        bool iCanMove = IsCompleted;

        if(iCanMove)
        {
            GamePlayController.Instance.extralPlayController.CheckExtrals(bird: scewOnArray[0], stand: this, isFlyOut: true);
        }
        else
        {
            GamePlayController.Instance.extralPlayController.CheckExtrals(bird: null, stand: this, isFlyOut: false);
        }
      //  Debug.LogError("iCanMove " + iCanMove);
         
    }
   
    public void HandleErrorSort()
    {
        GamePlayController.Instance.playerContain.currentPile = this;
        OnChoicePile();
        GamePlayController.Instance.playerContain.HandleShowTutXV(true);
        GameController.Instance.musicManager.PlayScrewOutSound();
    }

    public void HandleOnOffSprite( bool onOff)
    {
        if (UseProfile.CurrentLevel > 4)
        {
            return;
        }
        if (onOff)
        {
            if (pileData.numSlot == stackScew.Count)
            {
                spriteRender.color = new Color32(255, 255, 255, 255);
                spriteRender.sprite = fullIcon;
            }
            if (pileData.numSlot > stackScew.Count)
            {
                if (GamePlayController.Instance.playerContain.currentPile == null)
                {
                    return;
                }
                   List<ScewBase> ScewEnd_currentPile = GamePlayController.Instance.playerContain.currentPile.GetScewSameEnd(isGetLockMove: false);                  
                   List<ScewBase> ScewEnd_this = this.GetScewSameEnd(isGetLockMove: false);
                    if (ScewEnd_this.Count == 0)
                    {
                        spriteRender.color = new Color32(255, 255, 255, 255);
                        spriteRender.sprite = vIcon;
                        return;
                    }
                    if (ScewEnd_this[0].id == ScewEnd_currentPile[0].id)
                    {
                        spriteRender.color = new Color32(255, 255, 255, 255);
                        spriteRender.sprite = vIcon;
                    }
                    else
                    {
                        spriteRender.color = new Color32(255, 255, 255, 255);
                        spriteRender.sprite = xIcon;
                    }
               
            
            }
        }
        else
        {
            spriteRender.color = new Color32(0, 0, 0, 0);
        }
       


    }
}
