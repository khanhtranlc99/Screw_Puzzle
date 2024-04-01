using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public Transform[] floors;
    public Transform[] tilesInBar;
    List<int> TypeInBar;
    public Transform bar;

    [Header("Level Setup")]
    [SerializeField]
    List<GameObject> TypeOfLevel;
    [SerializeField]
    List<int> TileAmount;
    List<GameObject> TilesOfLevel;

    public Button undoBtn;
    private void Awake()
    {
        TilesOfLevel = new List<GameObject>();
        for (int i = 0; i < TypeOfLevel.Count; i++)
        {
            for (int j = 0; j < TileAmount[i]; j++)
            {
                TilesOfLevel.Add(TypeOfLevel[i]);
            }
        }
        ShufferTiles();
        InitTiles();
        tilesInBar = new Transform[7];
        TypeInBar = new List<int>();
    }
    void Start()
    {  
        FloorTransition();
        
        undoBtn.onClick.AddListener(Undo);
    }
    public void InitTiles()
    {
        int index = 0;
        for (int i = 0; i < floors.Length; i++)
        {
            Floor floor = floors[i].GetComponent<Floor>();
            Transform[] children = floors[i].GetComponentsInChildren<Transform>();
            for (int j = 0; j < children.Length; j++) { 
                if (children[j].GetComponent<Tile>())
                {
                    GameObject temp = Instantiate(TilesOfLevel[index], floor.transform);
                    temp.transform.localPosition = children[j].localPosition;
                    Destroy(children[j].gameObject);
                    index++;
                }
            }
        }
    }
    public void ShufferTiles()
    {
        for (int i = 0; i < TilesOfLevel.Count; i++)
        {
            GameObject temp = TilesOfLevel[i];
            int randomIndex = Random.Range(i, TilesOfLevel.Count);
            TilesOfLevel[i] = TilesOfLevel[randomIndex];
            TilesOfLevel[randomIndex] = temp;
        }
    }
    public void FloorTransition()
    {
        //AudioManager.instance.Play("FloorOpen");
        for (int i = 0; i < floors.Length; i++)
        {
            Floor floor = floors[i].GetComponent<Floor>();
            if (i % 2 == 0) StartCoroutine(floor.MoveLeft(1f));
            else StartCoroutine(floor.MoveRight(1f));
        }
        StartCoroutine(WaitSomeThing(0.5f));
    }
    IEnumerator WaitSomeThing(float duration)
    {
        float elapsed = duration;
        while (elapsed > 0)
        {
            elapsed -= Time.deltaTime;
            yield return null;
        }
        CheckClicked(); 
    }
    // Update is called once per frame
    void CheckClicked()
    {
        int tileCount = 0;
        for (int i = 0; i < floors.Length; i++)
        {
            Floor floor = floors[i].GetComponent<Floor>();
            floor.SetTiles();
            tileCount += floor.GetTilesCount();
        }
        if(tileCount == 0)
        {
           // GameManager.instance.WinPhase();
        }
        for (int i = 0; i < floors.Length; i++)
        {
            if (i < (floors.Length - 1)){
                Floor floor = floors[i].GetComponent<Floor>();
                for (int j = 0; j < floor.tiles.Count; j++)
                {
                    Tile tile = floor.tiles[j].GetComponent<Tile>();
                    List<Transform> tilesUp = floors[i + 1].GetComponent<Floor>().tiles;
                    if (tilesUp.Count > 0)
                    {
                        for (int k = 0; k < tilesUp.Count; k++)
                        {
                            tile.GetComponent<Tile>();
                            if(tilesUp[k].GetComponent<Tile>().isActive && Mathf.Abs(tilesUp[k].localPosition.x - tile.x) <= 75 && Mathf.Abs(tilesUp[k].localPosition.y - tile.y) <= 75)
                            {
                                tile.GetComponent<Tile>().SetClickAble(false);
                            }
                        }
                    }           
                }
            }
            
        }
    }
   
    public void PutTileToBar(GameObject tile)
    {
        for (int i = 0; i < tilesInBar.Length; i++)
        {
            if(tilesInBar[i] == null)
            {
                UndoPos = new Vector3(tile.transform.localPosition.x, tile.transform.localPosition.y, tile.transform.localPosition.z);
                oldParent = tile.transform.parent;
                tile.transform.parent = bar;
                tile.GetComponent<Tile>().isActive = false;
                tilesInBar[i] = tile.transform;
                undoIndex = i;
                StartCoroutine(MoveTileToBar(0.5f, tilesInBar[i], new Vector3(-450 + i * 150, 0)));
                if (!TypeInBar.Contains(tile.GetComponent<Tile>().tileType))
                {
                    TypeInBar.Add(tile.GetComponent<Tile>().tileType);
                }
                break;
            }
        }
        if(CheckMatchTile())
        {
            if(countWinVoice >= 2)
            {
                countWinVoice = 0;
                int voice = Random.Range(1, 3);
                //AudioManager.instance.Play("Voice" + voice);
            }
            countWinVoice++;
        }
        else
        {
            if (countVoice > 3)
            {
                countVoice = 0;
                int voice = Random.Range(3, 5);
                //AudioManager.instance.Play("Voice" + voice);
            }
            countVoice++;
        }    
    }

    Vector3 UndoPos;
    int countWinVoice = 0;
    int undoIndex;
    Transform oldParent;
    void Undo()
    {
        if (tilesInBar[undoIndex])
        {
            tilesInBar[undoIndex].parent = oldParent;
            StartCoroutine(MoveTileToBar(1f, tilesInBar[undoIndex], UndoPos));
            tilesInBar[undoIndex].GetComponent<Tile>().isActive = true;
            tilesInBar[undoIndex] = null;
        }
    }
    bool CheckMatchTile()
    {
        int[] typeCount = new int[TileAmount.Count];
        for (int i = 0; i < tilesInBar.Length; i++)
        {
            if (tilesInBar[i] != null)
            {
                int tileType = tilesInBar[i].GetComponent<Tile>().tileType;
                typeCount[TypeInBar.IndexOf(tileType)]++;
                if (typeCount[TypeInBar.IndexOf(tileType)] == 3)
                {
                    GameObject[] objs = new GameObject[3]; 
                    int count = 0;
                    for (int j = 0; j < tilesInBar.Length; j++)
                    {
                        if (count < 3 && tilesInBar[j] != null && tilesInBar[j].GetComponent<Tile>().tileType == tileType)
                        {
                            objs[count] = tilesInBar[j].gameObject;
                            tilesInBar[j] = null;
                            count++;
                        }
                    }
                    typeCount[TypeInBar.IndexOf(tileType)] = 0;
                    StartCoroutine(DisableTileInBar(0.3f, objs));
                    return true;
                }
            }   
        }
        return false;
    }

    int countVoice = 0;
    IEnumerator MoveTileToBar(float duration,Transform tile, Vector3 target)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            tile.localPosition = Vector3.Lerp(tile.localPosition, target, 0.5f);
            yield return null;
            tile.localPosition = target;
            CheckClicked();
        }
    }
    
    IEnumerator DisableTileInBar(float duration, GameObject[] objs)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        //AudioManager.instance.Play("TileDestroyed");
        foreach (GameObject item in objs)
        {
            item.GetComponent<ScaleTransision>().Disable();
        }
        List<Transform> ls = new List<Transform>();
        for (int i = 0; i < tilesInBar.Length; i++)
        {
            if (tilesInBar[i] != null)
            {
                ls.Add(tilesInBar[i]);
            }
        }
        for (int i = 0; i < tilesInBar.Length; i++)
        {
            if (i < ls.Count)
            {
                tilesInBar[i] = ls[i];
                StartCoroutine(MoveTileToBar(0.5f, tilesInBar[i], new Vector3(-450 + i * 150, 0)));
            }
            else
            {
                tilesInBar[i] = null;
            }
        }
    }
}
