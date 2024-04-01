using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelDataForUI : SerializedScriptableObject
{
    public List<LevelDataNeeded> data;
}
public class LevelDataNeeded
{
    public int id;
    public string description;
    public int goalPostCount;
}
