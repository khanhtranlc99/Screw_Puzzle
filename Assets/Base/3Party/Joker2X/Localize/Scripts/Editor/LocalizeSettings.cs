using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

//===============================================================
//Developer:  CuongCT
//Company:    ONESOFT
//Date:       2017
//================================================================

public class LocalizeSettings
{
#region Generic Get and Set methods

	static public void SetBool (string name, bool val) { EditorPrefs.SetBool(name, val); }

	static public bool GetBool (string name, bool defaultValue) { return EditorPrefs.GetBool(name, defaultValue); }

	static public string GetString (string name, string defaultValue) { return EditorPrefs.GetString(name, defaultValue); }

#endregion

#region Convenience accessor properties

	static public bool minimalisticLook
	{
		get { return GetBool("Minimalistic", false); }
		set { SetBool("Minimalistic", value); }
	}

#endregion

}
