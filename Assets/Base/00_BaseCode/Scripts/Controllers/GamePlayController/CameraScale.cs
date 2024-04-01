using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScale : MonoBehaviour
{
    public Camera cam;
    public float Speed;
    public Transform transformLeftPost;
    public Transform transformRightPost;
    public void Init()
    {
        StartCoroutine(FixScrean(transformLeftPost.position, transformRightPost.position));
    }    

    public IEnumerator FixScrean(Vector3 left, Vector3 right)
    {
        float speed = Speed;
        while (cam.WorldToViewportPoint(left).x < 0)
        {
            //cam.orthographicSize = cam.orthographicSize + 0.1f;
            if (cam.orthographic)
            {
                cam.orthographicSize = cam.orthographicSize + speed;
            }
            else
            {
                cam.fieldOfView = cam.fieldOfView + speed;
            }
            speed += 0.001f;
            yield return null;
        }

        while (cam.WorldToViewportPoint(right).x > 1)
        {
            if (cam.orthographic)
            {
                cam.orthographicSize = cam.orthographicSize + speed;
            }
            else
            {
                cam.fieldOfView = cam.fieldOfView + speed;
            }
            speed += 0.005f;
            yield return null;
        }
        Debug.Log("DONE");

    }
}
