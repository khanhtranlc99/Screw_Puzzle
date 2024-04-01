using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdMode : MonoBehaviour
{
    private GameObject[,] adTiles;
    [SerializeField] private GameObject tilePrb;
    public static AdMode instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private List<GameObject> SelectedTiles;

    public void TileSelected(int type, GameObject tile)
    {
        SelectedTiles.Add(tile);
        
        int count = 0;
        for (int i = 0; i < SelectedTiles.Count; i++)
        {
            if(SelectedTiles[i].GetComponent<AdTile>().type != type)
            {
                for (int j = 0; j < SelectedTiles.Count; j++)
                {
                    SelectedTiles[j].GetComponent<AdTile>().UnSelected();
                }
                SelectedTiles = new List<GameObject>();
                break;
            }
            else
            {
                count++;
                Debug.Log("count " + count);
            }
        }
        if(count >= 3)
        {
            for (int j = 0; j < SelectedTiles.Count; j++)
            {
                SelectedTiles[j].SetActive(false);

            }
            SelectedTiles = new List<GameObject>();
        }
        
    }
    
    void Start()
    {
        adTiles = new GameObject[6,6];
        SelectedTiles = new List<GameObject>();


        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                GameObject tile = Instantiate(tilePrb, transform);
                adTiles[i, j] = tile;
                int tileType = Random.Range(0,7);
                Debug.Log(tileType);
                tile.GetComponent<AdTile>().InIt(tileType);
                tile.transform.localScale = new Vector3(0.8f,0.8f,0.8f);
                tile.transform.position = new Vector3(-5f + i*1.75f, 7f - j * 1.75f, 0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = new Vector3(transform.position.x, transform.position.y+Time.deltaTime, transform.position.z);
    }
}

