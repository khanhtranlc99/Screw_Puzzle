using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public GameObject shadow;
    private bool isClickable = false;
    public bool isActive = true;
    public float x, y;
    public int tileType;
    public int floorIndex = 0;
    private void Awake()
    {
        string nameFloor = gameObject.transform.parent.name;
        SetFloorIndex(nameFloor);
    }
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClicked);
        x = transform.localPosition.x;
        y = transform.localPosition.y;
        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnClicked()
    {
        if (isClickable && isActive)
        {
            transform.parent.parent.GetComponent<LevelController>().PutTileToBar(gameObject);
        }
    }
    public void SetClickAble(bool clickable)
    {
        isClickable = clickable;
        if (isClickable){
            shadow.SetActive(false);
        }
        else
        {
            shadow.SetActive(true);
        }
    }

    public void SetFloorIndex(string nameFloor) {
        if (nameFloor.Equals("Floor1")) {
            floorIndex = 1;
        }
        else if (nameFloor.Equals("Floor2"))
        {
            floorIndex = 2;
        }
        else if (nameFloor.Equals("Floor3"))
        {
            floorIndex = 3;
        }
        else if (nameFloor.Equals("Floor4"))
        {
            floorIndex = 3;
        }
        else if (nameFloor.Equals("Floor5"))
        {
            floorIndex = 3;
        }
    }
}
