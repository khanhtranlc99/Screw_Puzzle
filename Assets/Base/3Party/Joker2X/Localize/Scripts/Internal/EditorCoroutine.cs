using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EditorCoroutine
{
    public static EditorCoroutine start(IEnumerator _routine)
    {
        var coroutine = new EditorCoroutine(_routine);
        coroutine.start();
        return coroutine;
    }

    readonly IEnumerator routine;
    EditorCoroutine(IEnumerator _routine)
    {
        routine = _routine;
    }

    void start()
    {
        //Debug.Log("start");
    #if UNITY_EDITOR
        EditorApplication.update += update;
    #endif
    }
    public void stop()
    {
        //Debug.Log("stop");
    #if UNITY_EDITOR
        EditorApplication.update -= update;
    #endif
    }

    void update()
    {
        /* NOTE: no need to try/catch MoveNext,
         * if an IEnumerator throws its next iteration returns false.
         * Also, Unity probably catches when calling EditorApplication.update.
         */

        //Debug.Log("update");
        if (!routine.MoveNext())
        {
            stop();
        }
    }
}