using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public List<Transform> tiles;
    void Start()
    {
        
    }

    // Update is called once per frame
    public void SetTiles()
    {
        tiles.Clear();
        foreach (Transform child in this.transform)
        {
            if (child.GetComponent<Tile>())
            {
                child.GetComponent<Tile>().SetClickAble(true);
                tiles.Add(child);
            }
        }
    }
    public int GetTilesCount()
    {
        return tiles.Count;
    }

    public IEnumerator MoveLeft(float duration)
    {
        float elapsed = duration;
        transform.localPosition = new Vector3(720, 0, 0);
        while (elapsed > 0)
        {
            elapsed -= Time.deltaTime * 5;
            transform.localPosition = new Vector3(720*elapsed, 0, 0);
            yield return null;
        }
        transform.localPosition = Vector3.zero;
    }
    public IEnumerator MoveRight(float duration)
    {
        float elapsed = 0;
        transform.localPosition = new Vector3(-720, 0, 0);
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime * 5;
            transform.localPosition = new Vector3(-720 + 720 * elapsed, 0, 0);
            yield return null;
        }
        transform.localPosition = Vector3.zero;
    }
}
