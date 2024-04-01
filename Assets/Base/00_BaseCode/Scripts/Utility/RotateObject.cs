using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour {

    [SerializeField] float speed = 1;
	void Update () {
        if (Time.timeScale == 0) return;
        this.transform.Rotate(new Vector3(0, 0, 1) * speed);
	}
}
