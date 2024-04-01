using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Sirenix.OdinInspector;
using DG.Tweening;

public class ComboSystem : SerializedMonoBehaviour
{
    public List<ComboData> comboData;
    public List<ComboData> lsCurrentComboData;
    public Image progess;
    public Text tvCombo;

    public ComboData CurrentCombo
    {
        get
        {
            if(lsCurrentComboData.Count <= 0)
            {
                return null;
            }
            else
            {
                return lsCurrentComboData[lsCurrentComboData.Count - 1];
            }
        }
    }

    public void SetCombo()
    {
        if(lsCurrentComboData.Count <= 0)
        {
            lsCurrentComboData.Add(comboData[0]);
        }
        else
        {
            foreach(var item in comboData)
            {
                if(item != lsCurrentComboData[lsCurrentComboData.Count - 1] && !lsCurrentComboData.Contains(item))
                {
                    lsCurrentComboData.Add(item);
                    break;
                }
            }
        }
        SetProgessBar(lsCurrentComboData[lsCurrentComboData.Count - 1]);
    }

    private void SetProgessBar(ComboData comboData)
    {
        if(comboData.typeCombo != TypeCombo.x1)
        {
            var tempScale = tvCombo.transform.localScale;
            tvCombo.transform.DOScale(tempScale*1.3f,0.25f).OnComplete(delegate { tvCombo.transform.DOScale(tempScale, 0.25f); });
            tvCombo.text = "Combo x" + comboData.score;
        }
        else
        {
            tvCombo.text = "";
            var temp = tvCombo.transform.localScale;
          
        }
        progess.DOKill();
        progess.fillAmount = 1;
        progess.DOFillAmount(0, comboData.timeSpace).OnComplete(delegate{
            tvCombo.text = "";
            lsCurrentComboData.Clear();
        });

    }



}
public enum TypeCombo
{
    x1 = 1,
    x2 = 2,
    x3 = 3,
    x4 = 4,
    x5 = 5,
    x6 = 6,
    x7 = 7,
    x8 = 8,
    x9 = 9,
    x10 = 10,
    x11 = 11,
    x12 = 12,
    x13 = 13,
    x14 = 14,
    x15 = 15,
    x16 = 16,
    x17 = 17,
    x18 = 18,
    x19 = 19,
}

[Serializable]
public class ComboData
{
    public TypeCombo typeCombo;
    public int score;
    public float timeSpace;
  
}