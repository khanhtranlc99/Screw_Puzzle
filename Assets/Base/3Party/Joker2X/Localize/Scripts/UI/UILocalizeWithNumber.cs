using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILocalizeWithNumber : MonoBehaviour
{
    public Text txtText;
    public string key;
    public int number;

    private void OnEnable()
    {
        OnLocalize();
    }

    private void Start()
    {
        OnLocalize();
    }

    [Button]
    private void OnLocalize()
    {
        txtText.text = Localization.Get(key) + " " + number;
    }

#if UNITY_EDITOR
    private void Reset()
    {
        txtText = GetComponent<Text>();
    }
#endif
}
