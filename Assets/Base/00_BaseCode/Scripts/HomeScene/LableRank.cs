using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LableRank : MonoBehaviour
{
    public Text tvRank;
    public Text tvName;
    public Text tvScore;
    public Image avatar;
    public int score;


    public void Init(string paramRank, string paramName, int score , Sprite icon)
    {
        tvRank.text = paramRank;
        tvName.text = paramName;
        tvScore.text = "" + score;
        avatar.sprite = icon;
        this.score = score;
    }
}
