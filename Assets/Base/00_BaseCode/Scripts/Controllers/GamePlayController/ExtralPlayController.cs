using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtralPlayController : MonoBehaviour
{
    [SerializeField] private List<ExtralPlayBase> extralsPrefab;
    public List<ExtralPlayBase> extralInGame;
    private Dictionary<ExtralPlayType, List<ExtralPlayBase>> extralsInited;
    public bool isSpawnedExtral;

    public ExtralPlayBase GetExtraPlayBase(ExtralPlayType extralPlayType)
    {
        foreach(var item in extralInGame)
        {
            if(extralPlayType == item.data.type)
            {
                return item;
            }
        }
        return null;
    }

    public void Init()
    {
        extralInGame = new List<ExtralPlayBase>();
        extralsInited = new Dictionary<ExtralPlayType, List<ExtralPlayBase>>();
        isSpawnedExtral = false;
    }

    public bool IsExtralsReady()
    {
        for (int i = 0; i < extralInGame.Count; i++)
        {
            if (!extralInGame[i].isReady)
                return false;
        }

        return true;
    }

    private ExtralPlayBase GetExtralPrefab(ExtralPlayType type)
    {
        for (int i = 0; i < extralsPrefab.Count; i++)
        {
            if (extralsPrefab[i].data.type == type)
                return extralsPrefab[i];
        }
        return null;
    }

    public void SpawnExtrals(List<ExtralPlayData> extralsData)
    {
        if (isSpawnedExtral)
            return;
        for (int i = 0; i < extralsData.Count; i++)
        {
            int index = i;
            Transform parentExtral = this.transform;
            if (extralsData[index].positionData.posType == ExtralPlayPosition.InBird)
            {
                int indexStand = extralsData[index].positionData.indexPile;
                int indexBird = extralsData[index].positionData.indexPile;
                var bird = GamePlayController.Instance.playerContain.currentPileInGame[indexStand].scewOnArray[indexBird];
                extralsData[index].scewParent = bird;
                parentExtral = bird.transform;
            }
            else if (extralsData[index].positionData.posType == ExtralPlayPosition.InStand)
            {
                int indexStand = extralsData[index].positionData.indexPile;
                var stand = GamePlayController.Instance.playerContain.currentPileInGame[indexStand];
                extralsData[index].pileParent = stand;
                parentExtral = stand.transform;
            }

            var extralPrefab = GetExtralPrefab(extralsData[index].type);
            if (extralPrefab != null)
            {
                var extral = Instantiate(extralPrefab, parentExtral);
                extral.transform.localPosition = extralsData[index].positionData.localPosition;
                extral.Init(extralsData[index], index);

                extralInGame.Add(extral);
            }

        }
        GamePlayController.Instance.gameScene.CheckExtralInLevel();
    }
    /// <summary>
    /// Gọi khi tất cả các chim đã đậu vào cành (lúc start game)
    /// </summary>
    /// <param name="extralsData"></param>
    public void AppearExtral()
    {
        if (isSpawnedExtral)
            return;
        for (int i = 0; i < extralInGame.Count; i++)
        {
            extralInGame[i].ApearExtral();
        }
        isSpawnedExtral = true;
    }

    public void CheckExtrals(ScewBase bird, PileBase stand, bool isFlyOut)
    {
        for (int i = 0; i < extralInGame.Count; i++)
        {
            extralInGame[i].CheckExtral(bird, stand, isFlyOut);
        }
    
    }

    public void ReturnExtrals()
    {
        for (int i = 0; i < extralInGame.Count; i++)
        {
            extralInGame[i].ReturnExtral();
        }
    }

    public void ReAppearExtrals(int idBird, int idStand)
    {
        for (int i = 0; i < extralInGame.Count; i++)
        {
            extralInGame[i].ReAppearExtral(idBird, idStand);
        }
    }

    public void DestroyExtral(ExtralPlayType type)
    {
        for (int i = 0; i < extralInGame.Count; i++)
        {
            if (extralInGame[i].data.type == type)
                extralInGame[i].DestroyExtrals();
        }
    }
    public void DestroyAllExtral()
    {
        for (int i = 0; i < extralInGame.Count; i++)
        {


            extralInGame[i].ResetExtral();

            //Destroy(extralInGame[i].gameObject);


        }
        extralInGame.Clear();
        isSpawnedExtral = false;
    }
    public void ResetExtra()
    {
        for (int i = 0; i < extralInGame.Count; i++)
        {
            extralInGame[i].ResetExtral();
        }
    }

    public void InitDestroyExtralButton(ExtralPlayType type, ExtralPlayBase extral)
    {
        Debug.LogError("InitDestroyExtralButton(type)");
        if (extralsInited.ContainsKey(type))
        {
            Debug.LogError("ContainsKey(type)");
            extralsInited[type].Add(extral);
        
            return;
        }
        Debug.LogError("extral");
        List<ExtralPlayBase> lst = new List<ExtralPlayBase>();
        lst.Add(extral);
        extralsInited.Add(type, lst);
        GamePlayController.Instance.gameScene.InitExtral(type, extral.iconItem, () =>
        {
            DestroyExtral(type);
        });
    
    }
    public void CheckAllExtralSuccess()
    {
        bool isAllSuccess = true;
        for (int i = 0; i < extralInGame.Count; i++)
        {
            if (extralInGame[i].state != StateExtral.Success)
            {
                isAllSuccess = false;
            }
            else
            {
                if (CheckAllExtralSameTypeSuccess(extralInGame[i].data.type))
                {
                    GamePlayController.Instance.gameScene.HandleCompleteExtral(extralInGame[i].data.type);
                }
            }
        }
        if (isAllSuccess)
        {
            GamePlayController.Instance.gameScene.isExtralSuccess = true;
            GamePlayController.Instance.gameScene.ActiveExtral(false);
        }
    }
    private bool CheckAllExtralSameTypeSuccess(ExtralPlayType type)
    {
        bool result = true;
        for (int i = 0; i < extralInGame.Count; i++)
        {
            if (extralInGame[i].data.type == type && extralInGame[i].state != StateExtral.Success)
            {
                result = false;
                break;
            }
        }
        return result;
    }



}
