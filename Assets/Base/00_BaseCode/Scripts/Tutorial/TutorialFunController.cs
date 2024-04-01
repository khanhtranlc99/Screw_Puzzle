using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFunController : MonoBehaviour
{

    public List<TutorialBase> tutorials;
    public int currentIDTut; 
    public GameObject handTut;
    [SerializeField] private GameObject refHandTut;

    public TutorialBase currentTutorial
    {
        get
        {
            return tutorials[currentIDTut];
        }
    }

    public virtual void Init()
    {
     
        currentIDTut = 0;
        for (int i = 0; i < tutorials.Count; i++)
        {
            tutorials[i].Init(this);
        }
        
    }

 
    public void StartTut()
    {
        for (int i = 0; i < tutorials.Count; i++)
        {
            if (tutorials[i].IsCanShowTut())
            {
                tutorials[i].StartTut();
                currentIDTut = i;
                break;
            }
        }
      
    }

      

    public virtual void NextTut()
    {

        if (currentIDTut > tutorials.Count - 1)
        {
            return;
        }
     
        if (!tutorials[currentIDTut].IsCanShowTut())
            return;


        if (!tutorials[currentIDTut].IsCanEndTut())
            return;
 
        tutorials[currentIDTut].OnEndTut();
        currentIDTut++;

        if (currentIDTut >= tutorials.Count)
            return;
        if (tutorials[currentIDTut].IsCanShowTut())
        {
            tutorials[currentIDTut].StartTut();       
        }
    
    }    

    public virtual void EndTut()
    {

    }


}
