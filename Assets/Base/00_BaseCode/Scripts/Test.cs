using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class Test : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Color[] _groupColors;

   [Button]
    private void ChangeColor()
    {
        _renderer.material.color = _groupColors[0];
    }    
}
