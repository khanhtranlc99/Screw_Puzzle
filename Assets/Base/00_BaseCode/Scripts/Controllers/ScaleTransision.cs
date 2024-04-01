using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTransision : MonoBehaviour
{
    public bool scaleOnEnable = true;
    void Start()
    {
        
    }
    private void OnEnable()
    {
        if(scaleOnEnable)
            StartCoroutine(ScaleUp(1f));
    }
    public IEnumerator ScaleUp(float duration)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime*5;
            transform.localScale = Vector3.one * elapsed;
            yield return null;
        }
        transform.localScale = Vector3.one;
    }
    public void Disable()
    {
        StartCoroutine(ScaleDown(1f));
    }
    public IEnumerator ScaleDown(float duration)
    {
        float elapsed = duration;
        while (elapsed > 0)
        {
            elapsed -= Time.deltaTime*5;
            transform.localScale = Vector3.one * elapsed;
            yield return null;
        }
        transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
