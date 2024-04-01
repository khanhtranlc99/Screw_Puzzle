using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class DataReturn
{
    public List<DataReturnModle> data;

    public DataReturn()
    {
        data = new List<DataReturnModle>();
    }
}

public class DataReturnModle
{
    public ScewBase scew;
    public int idIndexOld;

    public PileBase oldStand;
    public int idOldSlot;
  

    public PileBase newStand;
    public int idNewSlot;
}



public class ReturnController : SerializedMonoBehaviour
{
    public Stack<DataReturn> stackReturn;
    public bool CanReturn
    {
        get
        {
          if (stackReturn.Count > 0)
            {
                return true;
            }
            return false;
        }
       
    }
    private int m_isLockReturn;
    public bool IsLockReturn
    {
        get
        {
            return m_isLockReturn > 0;
        }
        set
        {
            if (value)
            {
                m_isLockReturn++;
              
            }
            else
            {
                m_isLockReturn--;
                if (m_isLockReturn < 0)
                    m_isLockReturn = 0;
                if (m_isLockReturn == 0)
                {
               
                }
            }
        }

    }
    public void Init()
    {
        stackReturn = new Stack<DataReturn>();
    }

    public void HandleAddStack(DataReturn data)
    {
        stackReturn.Push(data);
    }

    public void HandleReturn()
    {
        if (stackReturn.Count <= 0)
        {
            return;
        }
        var data = stackReturn.Pop();
        if (data != null)
        {
            IsLockReturn = true;
            bool isReturnScore = false;
            int idBirdBack = -1;
            int idStandBack = -1;
            for(int i = data.data.Count -1 ; i >= 0; i --)
            {
                int index = i;
                if (data.data[index].newStand != null)
                {
                 if (data.data[index].newStand.stackScew.Count > 0)
                 {
                        var scewMove = data.data[index].newStand.stackScew.Pop();
                    
                        if (scewMove != null)
                        {
                            if (data.data[index].oldStand != null)
                                data.data[index].oldStand.BackTurn(scewMove, isOldPile: true, data.data[index].idIndexOld, data.data[index].oldStand, data.data[index].newStand);
                        }
                  }
                 else
                    {
                        if (data.data[index].oldStand != null)
                            data.data[index].oldStand.BackTurn(data.data[index].scew, isOldPile: true, data.data[index].idIndexOld, data.data[index].oldStand, data.data[index].newStand);
                    }    
                }
                else
                {
                    if (data.data[index].oldStand != null)
                    {
                        data.data[index].oldStand.BackTurn(data.data[index].scew, isOldPile: true, data.data[index].idIndexOld, data.data[index].oldStand, data.data[index].newStand);
                        isReturnScore = true;
                        idBirdBack = data.data[index].scew.id;
                        idStandBack = data.data[index].oldStand.id;
                    }
                }    
            }
        }

    }
    public void ResetReturn()
    {
        stackReturn.Clear();
        m_isLockReturn = 0;
    }
}
