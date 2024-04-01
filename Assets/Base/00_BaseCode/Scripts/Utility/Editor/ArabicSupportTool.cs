using ArabicSupport;
using UnityEditor;
using UnityEngine;

public class ArabicSupportTool : EditorWindow
{
	private string rawText;
	private string fixedText;

	private bool showTashkeel = true;
	private bool useHinduNumbers = true;

	// Add menu item named "Arabic Support Tool" to the Tools menu
	[MenuItem("Tools/Arabic Support Tool")]
	public static void ShowWindow()
	{
		//Show existing window instance. If one doesn't exist, make one.
		GetWindow(typeof(ArabicSupportTool));
	}

	private void OnGUI()
	{
		fixedText = string.IsNullOrEmpty(rawText) ? "" : ArabicFixer.Fix(rawText, showTashkeel, useHinduNumbers);

		GUILayout.Label("Options:", EditorStyles.boldLabel);
		showTashkeel = EditorGUILayout.Toggle("Use Tashkeel", showTashkeel);
		useHinduNumbers = EditorGUILayout.Toggle("Use Hindu Numbers", useHinduNumbers);

		GUILayout.Label("Input (Not Fixed)", EditorStyles.boldLabel);
		rawText = EditorGUILayout.TextArea(rawText);

		GUILayout.Label("Output (Fixed)", EditorStyles.boldLabel);
		fixedText = EditorGUILayout.TextArea(fixedText);
		if (GUILayout.Button("Copy")) {
			var tempTextEditor = new TextEditor {text = fixedText};
			tempTextEditor.SelectAll();
			tempTextEditor.Copy();
		}

	}

}