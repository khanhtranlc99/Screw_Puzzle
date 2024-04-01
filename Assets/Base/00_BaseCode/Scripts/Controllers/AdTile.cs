using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdTile : MonoBehaviour
{
    [SerializeField] SpriteRenderer icon;
    [SerializeField] GameObject boder;
    [SerializeField] List<Sprite> spriteList; 
    private void OnMouseDown()
    {
        boder.SetActive(true);
        AdMode.instance.TileSelected(type, gameObject);
    }

    public void ChangeIcon(Sprite _icon)
    {
        icon.sprite = _icon;
    }
    public int type;
    public void InIt(int _type)
    {
        type = _type;
        ChangeIcon(spriteList[_type]);
    }
    public void UnSelected()
    {
        boder.SetActive(false);
    }
}
