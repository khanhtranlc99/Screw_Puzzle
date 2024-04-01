using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Datas/LevelChestData", fileName = "LevelChestData.asset")]

public class LevelChestData : ScriptableObject
{
    public List<levelChest> lsLevelChest;
    public levelChest CurrentLevelChest
    {
        get
        {
            if (UseProfile.LevelOfLevelChest > lsLevelChest.Count - 1)
            {
                return null;
            }
            return lsLevelChest[UseProfile.LevelOfLevelChest];

        }
    }

    public void PlusLevelOfLevelChest()
    {
        UseProfile.LevelOfLevelChest += 1;
        if (UseProfile.LevelOfLevelChest > lsLevelChest[lsLevelChest.Count - 1].level)
        {
            UseProfile.LevelOfLevelChest = 0;
        }
    }




}
[System.Serializable]
public class levelChest
{
    public int level;
    public GiftType giftType;
    public int amount;
}
