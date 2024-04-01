using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using Newtonsoft.Json;

public class GetTypeLevel : MonoBehaviour
{
    public string docID;
    public int sheetID;
    public List<ScanLevel> lsScanLevels;
    public LevelType lsLevelType;



    public void LoadData(string dataID)
    {
        lsScanLevels  = new List<ScanLevel>();
        var data = CSVOnlineReader.ReadGSheet(dataID, sheetID);
        if (data != null && data.Count > 0)
        {
            string sData = JsonConvert.SerializeObject(data);
            foreach (var dict in data)
            {
                ScanLevel pu = new ScanLevel(dict);
                lsScanLevels.Add(pu);
            }
        }
        Soft();
    }



    public void Soft()
    {
        for(int i = 0; i < lsScanLevels.Count;i ++)
        {
            lsLevelType.boomLevel.Add(lsScanLevels[i].boomLevel);
            lsLevelType.cageLevel.Add(lsScanLevels[i].CageLevel);
            lsLevelType.sleepLevel.Add(lsScanLevels[i].Sleep);
            lsLevelType.eggLevel.Add(lsScanLevels[i].Egg);
            lsLevelType.lockStandLevel.Add(lsScanLevels[i].LockStandLevel);
        }    
    }    



}
[Serializable]
public class ScanLevel
{
    public int boomLevel;
    public int CageLevel;
    public int Sleep;
    public int Egg;
    public int LockStandLevel;
    public ScanLevel(Dictionary<string, string> dict)
    {
        try
        {
            boomLevel = int.Parse(dict["Boom"]);
            CageLevel = int.Parse(dict["CageLevel"]);
            Sleep = int.Parse(dict["Sleep Level"]);
            Egg = int.Parse(dict["Egg Level"]);
            LockStandLevel = int.Parse(dict["LockStandLevel"]);
        }
    catch
        {

        }
    }
}

[Serializable]
public class LevelType
{
    public List<int> boomLevel;
    public List<int> cageLevel;
    public List<int> sleepLevel;
    public List<int> eggLevel;
    public List<int> lockStandLevel;

}



    #region Custom Inspector   
#if UNITY_EDITOR
    [CustomEditor(typeof(GetTypeLevel))]
public class LoadToolGetSheet : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GetTypeLevel control = (GetTypeLevel)target;
        if (GUILayout.Button("Load dataPiecesAndCoin from GSheet"))
        {
            control.LoadData(control.docID);
        }
     
    }
}
#endif
#endregion