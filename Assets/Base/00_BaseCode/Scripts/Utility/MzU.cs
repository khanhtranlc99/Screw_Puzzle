using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

public static class MzU
{

    public static System.Random rnd = new System.Random();
    public static readonly float IniValue = float.NegativeInfinity;

    public static T RandomEnumValue<T>()
    {

        var v = System.Enum.GetValues(typeof(T));
        return (T)v.GetValue(new System.Random().Next(v.Length));
    }


    /// <summary>
    /// Shuffle a list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static void Shuffle<T>(this IList<T> list)
    {
        for (var i = 0; i < list.Count; i++)
            list.Swap(i, rnd.Next(i, list.Count));
    }

    public static void Swap<T>(this IList<T> list, int i, int j)
    {
        var temp = list[i];
        list[i] = list[j];
        list[j] = temp;
    }
  

    /// <summary>
    /// Manual Invoke to time
    /// </summary>
    /// <param name="value">ref param to count</param>
    /// <param name="time">Time to invoke,time less than zero to alway run</param>
    /// <returns></returns>
    public static int ManualInvoke(ref float value, float time)
    {
        //value = 0;
        //		Debug.Log (value);
        if (time < 0)
            return 1;
        if (value == IniValue)
            return -1;
        value += Time.deltaTime;
        if (value >= time)
        {
            value = IniValue;
            return 1;
        }
        return 0;
    }

    public static int ManualCoutdown(ref float value)
    {

        if (value == IniValue)
            return -1;
        value -= Time.deltaTime;
        if (value <= 0)
        {
            value = IniValue;
            return 1;
        }
        return 0;
    }

    public class ValueObject
    {
        float min, max;
        public ValueObject(float min)
        {
            this.min = min;
            this.max = min;
        }
        public ValueObject(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public float GetValue()
        {
            if (min == max)
                return min;
            else
                return Random.Range(min, max);
        }
    }

    public static Vector2 VectorToInt(Vector2 input)
    {

        return new Vector2(Mathf.FloorToInt(input.x), Mathf.FloorToInt(input.y));
    }

    #region BoundChecker:
    public static Bounds GetBounds(RectTransform rect)
    {
       // Debug.Log(rect.sizeDelta.x + "/ 2 *" + rect.localScale.x + "=" + (rect.sizeDelta.x / 2 * rect.localScale.x));
        BoxCollider2D box = rect.gameObject.GetComponent<BoxCollider2D>();
        if (box != null)
            return box.bounds;
        return new Bounds(rect.position, new Vector3(rect.sizeDelta.x * rect.localScale.x, rect.sizeDelta.y * rect.localScale.y, 0));
    }

    public static bool isUIOverLaps(RectTransform rect1, RectTransform rect2)
    {
        return GetBounds(rect1).Intersects(GetBounds(rect2));
    }
    #endregion


}

