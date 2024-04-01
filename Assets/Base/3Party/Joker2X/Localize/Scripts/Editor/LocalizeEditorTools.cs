using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

//===============================================================
//Developer:  CuongCT
//Company:    ONESOFT
//Date:       2017
//================================================================

public static class LocalizeEditorTools
{
	static Texture2D mBackdropTex;
	static Texture2D mContrastTex;
	static Texture2D mGradientTex;
	static GameObject mPrevious;
    
	static public bool DrawHeader (string text) { return DrawHeader(text, text, false, LocalizeSettings.minimalisticLook); }

	/// <summary>
	/// Draw a distinctly different looking header label
	/// </summary>

	static public bool DrawHeader (string text, string key, bool forceOn, bool minimalistic)
	{
		bool state = EditorPrefs.GetBool(key, true);

		if (!minimalistic) GUILayout.Space(3f);
		if (!forceOn && !state) GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
		GUILayout.BeginHorizontal();
		GUI.changed = false;

		if (minimalistic)
		{
			if (state) text = "\u25BC" + (char)0x200a + text;
			else text = "\u25BA" + (char)0x200a + text;

			GUILayout.BeginHorizontal();
			GUI.contentColor = EditorGUIUtility.isProSkin ? new Color(1f, 1f, 1f, 0.7f) : new Color(0f, 0f, 0f, 0.7f);
			if (!GUILayout.Toggle(true, text, "PreToolbar2", GUILayout.MinWidth(20f))) state = !state;
			GUI.contentColor = Color.white;
			GUILayout.EndHorizontal();
		}
		else
		{
			text = "<b><size=11>" + text + "</size></b>";
			if (state) text = "\u25BC " + text;
			else text = "\u25BA " + text;
			if (!GUILayout.Toggle(true, text, "dragtab", GUILayout.MinWidth(20f))) state = !state;
		}

		if (GUI.changed) EditorPrefs.SetBool(key, state);

		if (!minimalistic) GUILayout.Space(2f);
		GUILayout.EndHorizontal();
		GUI.backgroundColor = Color.white;
		if (!forceOn && !state) GUILayout.Space(3f);
		return state;
	}

	/// <summary>
	/// Begin drawing the content area.
	/// </summary>

	static public void BeginContents () { BeginContents(LocalizeSettings.minimalisticLook); }

	static bool mEndHorizontal = false;

	/// <summary>
	/// Begin drawing the content area.
	/// </summary>

	static public void BeginContents (bool minimalistic)
	{
		if (!minimalistic)
		{
			mEndHorizontal = true;
			GUILayout.BeginHorizontal();
			EditorGUILayout.BeginHorizontal("AS TextArea", GUILayout.MinHeight(10f));
		}
		else
		{
			mEndHorizontal = false;
			EditorGUILayout.BeginHorizontal(GUILayout.MinHeight(10f));
			GUILayout.Space(10f);
		}
		GUILayout.BeginVertical();
		GUILayout.Space(2f);
	}

	/// <summary>
	/// End drawing the content area.
	/// </summary>

	static public void EndContents ()
	{
		GUILayout.Space(3f);
		GUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();

		if (mEndHorizontal)
		{
			GUILayout.Space(3f);
			GUILayout.EndHorizontal();
		}

		GUILayout.Space(3f);
	}

	/// <summary>
	/// Helper function that draws a serialized property.
	/// </summary>

	static public SerializedProperty DrawProperty (string label, SerializedObject serializedObject, string property, params GUILayoutOption[] options)
	{
		return DrawProperty(label, serializedObject, property, false, options);
	}
    
	static public SerializedProperty DrawProperty (string label, SerializedObject serializedObject, string property, bool padding, params GUILayoutOption[] options)
	{
		SerializedProperty sp = serializedObject.FindProperty(property);

		if (sp != null)
		{
			if (LocalizeSettings.minimalisticLook) padding = false;

			if (padding) EditorGUILayout.BeginHorizontal();
			
			if (label != null) EditorGUILayout.PropertyField(sp, new GUIContent(label), options);
			else EditorGUILayout.PropertyField(sp, options);

			if (padding) 
			{
				LocalizeEditorTools.DrawPadding();
				EditorGUILayout.EndHorizontal();
			}
		}
		return sp;
	}

	/// <summary>
	/// Unity 4.3 changed the way LookLikeControls works.
	/// </summary>

	static public void SetLabelWidth (float width)
	{
		EditorGUIUtility.labelWidth = width;
	}
    	

	class MenuEntry
	{
		public string name;
		public GameObject go;
		public MenuEntry (string name, GameObject go) { this.name = name; this.go = go; }
	}
    
	/// <summary>
	/// Load the asset at the specified path.
	/// </summary>

	static public Object LoadAsset (string path)
	{
		if (string.IsNullOrEmpty(path)) return null;
		return AssetDatabase.LoadMainAssetAtPath(path);
	}

	/// <summary>
	/// Convenience function to load an asset of specified type, given the full path to it.
	/// </summary>

	static public T LoadAsset<T> (string path) where T: Object
	{
		Object obj = LoadAsset(path);
		if (obj == null) return null;

		T val = obj as T;
		if (val != null) return val;

		if (typeof(T).IsSubclassOf(typeof(Component)))
		{
			if (obj.GetType() == typeof(GameObject))
			{
				GameObject go = obj as GameObject;
				return go.GetComponent(typeof(T)) as T;
			}
		}
		return null;
	}


	static public void DrawPadding ()
	{
		if (!LocalizeSettings.minimalisticLook)
			GUILayout.Space(18f);
	}
}
