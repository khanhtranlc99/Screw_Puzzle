using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Datas/StarChestData", fileName = "StarChestData.asset")]
public class StarChestData : ScriptableObject
{
    public List<StarChest> lsStarChests;
}
[System.Serializable]
public class StarChest
{
    [Header ("============================================================================") ]
    public int start;
    public GiftType giftType;
    public int amount;
}