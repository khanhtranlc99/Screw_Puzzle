using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Datas/BirdChestData", fileName = "BirdChestData.asset")]
public class BirdChestData : ScriptableObject
{
    public List<BirdChest> lsBirdChests;



}
[System.Serializable]
public class BirdChest
{
    [Header("============================================================================")]
    public int level;
    public List<int> idBird;
}