using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using Joker2X.Localize.Scripts.Editor;

//===============================================================
//Developer:  CuongCT
//Company:    ONESOFT
//Date:       2017
//================================================================
[CanEditMultipleObjects]
[CustomEditor(typeof(UILocalize), true)]
public class UILocalizeEditor : Editor
{
    static List<string> mKeys;

    void OnEnable() {
        ReloadDictionary();
    }

    private static void ReloadDictionary() {
        var dict = Localization.dictionary;

        if (dict.Count > 0) {
            mKeys = new List<string>();

            foreach (var pair in dict) {
                if (pair.Key == "KEY") continue;
                mKeys.Add(pair.Key);
            }

            mKeys.Sort((left, right) => String.Compare(left, right, StringComparison.Ordinal));
        }
    }

    static IEnumerator testRoutine()
    {
        //string url = "https://docs.google.com/spreadsheets/d/16mLAT8_u2_FuPTp93H9JLC3b4sPFtOqvSXZigblFC1A/export?format=csv&gid=345164090";
        //var url = "https://docs.google.com/spreadsheets/d/16mLAT8_u2_FuPTp93H9JLC3b4sPFtOqvSXZigblFC1A/export?format=csv&id=16mLAT8_u2_FuPTp93H9JLC3b4sPFtOqvSXZigblFC1A&gid=345164090";
        var url = "https://docs.google.com/spreadsheets/d/1zBNXFEs12IONAyDt-3EmWAodHze0cSPTKY283cg4aKU/export?format=csv&gid=0";
        var www = new WWW(url);
        float time = 0;
        while (!www.isDone)
        {
            time += 0.001f;
            if (time > 10000)
            {
                yield return null;
                Debug.Log("Downloading...");

                time = 0;
            }
        }

        string data = string.Empty;
        if (www.isDone && string.IsNullOrEmpty(www.error))
        {
            data = www.text.Replace("\n\n\"","\"").Replace("\n\"","\"");
        }

        yield return null;
        EditorUtility.DisplayDialog("Notify", "Sync Data from google Success!", "OK");
        AssetDatabase.SaveAssets ();
        AssetDatabase.Refresh();
        Localization.Reload();
        ParseArabicData(data);
        ReloadDictionary();
    }

    [MenuItem("Tools/Localize/Reload")]
    public static void ReloadData()
    {
        EditorCoroutine.start(testRoutine());
    }

    //[MenuItem("Tools/Localize/ParseArabic")]
    public static void ParseArabicData(string data)
    {
        new LocalizeParse().Parse(data);
        EditorUtility.DisplayDialog("title", "Parse Done", "OK");
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Reload"))
        {
            //EditorCoroutine.start(testRoutine());
            ReloadDictionary();

        }


        serializedObject.Update();

        GUILayout.Space(6f);
        LocalizeEditorTools.SetLabelWidth(80f);

        GUILayout.BeginHorizontal();
        // Key not found in the localization file -- draw it as a text field
        var sp = LocalizeEditorTools.DrawProperty("Key", serializedObject, "key");

        var myKey = sp.stringValue;
        var isPresent = (mKeys != null) && mKeys.Contains(myKey);
        GUI.color = isPresent ? Color.green : Color.red;
        GUILayout.BeginVertical(GUILayout.Width(22f));
        GUILayout.Space(2f);
        GUILayout.Label(isPresent ? "\u2714" : "\u2718", "TL SelectionButtonNew", GUILayout.Height(20f));
        GUILayout.EndVertical();
        GUI.color = Color.white;
        GUILayout.EndHorizontal();

        if (isPresent)
        {
            if (LocalizeEditorTools.DrawHeader("Preview"))
            {
                LocalizeEditorTools.BeginContents();

                var keys = Localization.knownLanguages;
                string[] values;

                if (Localization.dictionary.TryGetValue(myKey, out values))
                {
                    if (keys.Length != values.Length)
                    {
                        EditorGUILayout.HelpBox("Number of keys doesn't match the number of values! Did you modify the dictionaries by hand at some point?", MessageType.Error);
                    }
                    else
                    {
                        for (var i = 0; i < keys.Length; ++i)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label(keys[i], GUILayout.Width(66f));

                            if (GUILayout.Button(values[i], "AS TextArea", GUILayout.MinWidth(80f), GUILayout.MaxWidth(Screen.width - 110f)))
                            {
                                ((UILocalize) target).value = values[i];
                                GUIUtility.hotControl = 0;
                                GUIUtility.keyboardControl = 0;
                            }
                            GUILayout.EndHorizontal();
                        }
                    }
                }
                else GUILayout.Label("No preview available");

                LocalizeEditorTools.EndContents();
            }
        }
        else if (mKeys != null && !string.IsNullOrEmpty(myKey))
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(80f);
            GUILayout.BeginVertical();
            GUI.backgroundColor = new Color(1f, 1f, 1f, 0.35f);

            var matches = 0;

            for (int i = 0, imax = mKeys.Count; i < imax; ++i)
            {
                if (mKeys[i].StartsWith(myKey, StringComparison.OrdinalIgnoreCase) || mKeys[i].Contains(myKey))
                {
                    if (GUILayout.Button(mKeys[i] + " \u25B2", "CN CountBadge"))
                    {
                        sp.stringValue = mKeys[i];
                        GUIUtility.hotControl = 0;
                        GUIUtility.keyboardControl = 0;
                    }

                    if (++matches == 8)
                    {
                        GUILayout.Label("...and more");
                        break;
                    }
                }
            }
            GUI.backgroundColor = Color.white;
            GUILayout.EndVertical();
            GUILayout.Space(22f);
            GUILayout.EndHorizontal();
        }

        serializedObject.ApplyModifiedProperties();
    }
}