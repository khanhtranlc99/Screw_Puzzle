using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
[System.Serializable]
public class PossitionType
{
    public int sumPile;
    public Transform parent;
    public float plusUp;
    public List<Transform> lsPostPile;
}
public enum PileType
{
    Slot_4 = 0,//Có 4 Slot
    Slot_5 = 1,//Có 5 Slot
    Slot_6 = 2,//Có 6 Slot
    Slot_7 = 3,//Có 7 Slot
    Slot_8 = 4//Có 8 Slot
}
public enum ExtralPlayType
{
    None = 0,
    Boom = 1,
    LockBird = 2,
    EggLock = 3,
    SleepBird = 4,
    LockStand = 5
}
[System.Serializable]
public class LevelConfig
{

    public int id;
    public List<PileConfig> standConfig;
    public List<ExtralPlayData> extralsConfig;
    public int min;
    public int second;


    public bool Equals2(LevelConfig level)
    {
        if (standConfig.Count != level.standConfig.Count)
            return false;

        if (extralsConfig == null && level.extralsConfig != null)
            return false;

        if (extralsConfig != null && level.extralsConfig == null)
            return false;

        if (extralsConfig.Count != level.extralsConfig.Count)
            return false;

        for (int i = 0; i < standConfig.Count; i++)
        {
            if (standConfig[i].idBirds == null && level.standConfig[i].idBirds != null)
                return false;
            if (standConfig[i].idBirds != null && level.standConfig[i].idBirds == null)
                return false;

            if (standConfig[i].idBirds.Count != level.standConfig[i].idBirds.Count)
                return false;

            for (int t = 0; t < standConfig[i].idBirds.Count; t++)
            {
                if (standConfig[i].idBirds[t] != level.standConfig[i].idBirds[t])
                {
                    return false;
                }
            }
        }

        for (int i = 0; i < extralsConfig.Count; i++)
        {
            if (extralsConfig[i].type != level.extralsConfig[i].type)
                return false;

            if (extralsConfig[i].positionData.indexScew != level.extralsConfig[i].positionData.indexScew)
                return false;
            if (extralsConfig[i].positionData.indexPile != level.extralsConfig[i].positionData.indexPile)
                return false;

            if (extralsConfig[i].extralValues == null && level.extralsConfig[i].extralValues != null)
                return false;
            if (extralsConfig[i].extralValues != null && level.extralsConfig[i].extralValues == null)
                return false;

            for (int t = 0; t < extralsConfig[i].extralValues.Length; t++)
            {
                if (extralsConfig[i].extralValues[t] != level.extralsConfig[i].extralValues[t])
                    return false;
            }
        }

        return true;
    }
}
[System.Serializable]
public class ExtralPlayData
{
    public ExtralPlayType type;
    public ExtralPositionData positionData;
    public float[] extralValues;


    public ScewBase scewParent;
    [HideInInspector] public PileBase pileParent;
}
[System.Serializable]
public class PileConfig
{
    public List<int> idBirds = new List<int>();
    public PileData standData;
    public int side;
}
[System.Serializable]
public class PileData
{
    public PileType type;
    public int numSlot;
}

public enum ExtralPlayPosition
{
    InBird = 0,
    InStand = 1,
    InGamePlay = 2
}

[System.Serializable]
public class ExtralPositionData
{
    public ExtralPlayPosition posType;
    public int indexPile;
    public int indexScew;
    public Vector3 localPosition;
}
public class PlayerContain : MonoBehaviour
{
    public ScewBase scewBase;
    public List<PileBase> lsPileHolds;
    public List<Transform> lsPostPileHolds;
    public List<PossitionType> lsPossitionType;
    public LevelConfig levelConfig;
  
    public List<PileBase> currentPileInGame;
    PileType typeStandInLevel;
    public PileBase currentPile;
    public bool wasTouchPile;
    public int sumScewCurrent;
    public int sumScewInit;
    public bool allScewInitDone;
 
    public PossitionType GetPossitionType(int param)
    {
        foreach(var item in lsPossitionType)
        {
            if(item.sumPile == param)
            {
                return item;
            }
        }
        return null;
    }

    [Button]
    public void Init()
    {
        sumScewCurrent = 0;
        sumScewInit = 0;
        countDownTap = 0;
        wasTouchPile = false;
        SpawnLevel();
    }

    private void SpawnLevel()
    {
        string pathLevel = StringHelper.PATH_CONFIG_LEVEL;
        pathLevel = StringHelper.PATH_CONFIG_LEVEL_TEST;
        TextAsset lvJson = Resources.Load<TextAsset>(string.Format(pathLevel, UseProfile.CurrentLevel));
        Debug.LogError(lvJson.name);
        levelConfig = JsonUtility.FromJson<LevelConfig>(lvJson.ToString());
        lsPostPileHolds = new List<Transform>();
        foreach(var item in GetPossitionType(levelConfig.standConfig.Count).lsPostPile)
        {
            lsPostPileHolds.Add(item);
        }
        if (levelConfig != null)
        {
            for (int i = 0; i < levelConfig.standConfig.Count; i++)
            {
                int index = i;
                typeStandInLevel = levelConfig.standConfig[index].standData.type;
                int idStand = (int)levelConfig.standConfig[index].standData.type;
                int sidePosStand = levelConfig.standConfig[index].side;

                //  Debug.LogError("levelConfig");

                PileBase stand = Instantiate(lsPileHolds[Mathf.Clamp(idStand, 0, lsPileHolds.Count - 1)], lsPostPileHolds[i].transform);
                StartCoroutine(stand.Init(levelConfig.standConfig[index].idBirds));
                
                currentPileInGame.Add(stand);
              //  sumScewInLevel += sidePosStand;
            }
        }

        GameController.Instance.musicManager.PlayInGameSound();
        StartCoroutine(PlaySoundScew()) ;

        if(typeStandInLevel == PileType.Slot_6)
        {
            Camera.main.orthographicSize += 0.5f;
        }
    }

    private IEnumerator PlaySoundScew()
    {
        yield return new WaitForSeconds(1);
        GameController.Instance.musicManager.PlayScrewInSound();
    }
    public void CheckScewWasInit()
    {
        sumScewInit += 1;
        if (sumScewInit >= sumScewCurrent)
        {
            allScewInitDone = true;
            GamePlayController.Instance.extralPlayController.SpawnExtrals(levelConfig.extralsConfig);
            GamePlayController.Instance.tutLevel_1.StartTut();
            GamePlayController.Instance.tutLevel_2.StartTut();
           
        
        }
        else
        {
            allScewInitDone = false;
        }
          
    }
    public void HandleCallAllSort()
    {
        foreach(var item in currentPileInGame)
        {
            item.HandleSortScew();
        }    
    }
    public float countDownTap;
    public void Update()
    {
        if(wasTouchPile)
        {
            countDownTap += Time.unscaledDeltaTime;
            if(countDownTap >= 0.3f)
            {
                wasTouchPile = false;
                countDownTap = 0;
            }    
        }    
        if(Input.GetKeyDown(KeyCode.K))
        {
            SpawnPileBooster();
        }    

    }
    public void SpawnPileBooster()
    {
        Debug.LogError("Count " + (currentPileInGame.Count - 1));
        int idStand = (int)levelConfig.standConfig[0].standData.type;
        PileBase stand = Instantiate(lsPileHolds[Mathf.Clamp(idStand, 0, lsPileHolds.Count - 1)], lsPostPileHolds[currentPileInGame.Count ]);
        var temp = new List<int>();
        StartCoroutine(stand.Init(temp));
        currentPileInGame.Add(stand);
        
    }
    bool checkWin;
    public void HandleCheckWin()
    {
        checkWin = true;

        foreach (var item in currentPileInGame)
        {
          if(!item.GetScewComplete)
            {
                checkWin = false;
            }

        }

        if(checkWin)
        {
            StartCoroutine(ShowPopupWin());
         
        }

        //Debug.LogError("checkWin " + checkWin);
    }

    private IEnumerator ShowPopupWin()
    {
        yield return new WaitForSeconds(2);

        Winbox.Setup().Show();

    }

    public void HandleShowTutXV(bool onOff)
    {
      if(onOff)
        {
            foreach (var item in currentPileInGame)
            {
                if (item != currentPile)
                {
                    item.HandleOnOffSprite(onOff);
                }

            }
        }
      else
        {
            foreach (var item in currentPileInGame)
            {               
               item.HandleOnOffSprite(onOff);               
            }
        }
       

    }
}
[Serializable]
public class PileHold
{
    public PileType holdType;
    public PileBase pileBase;
}