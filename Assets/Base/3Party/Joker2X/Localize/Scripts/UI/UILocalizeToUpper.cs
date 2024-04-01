using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILocalizeToUpper : UILocalize
{
	public override void OnLocalize()
	{
		// If no localization key has been specified, use the label's text as the key
		if (string.IsNullOrEmpty(key))
		{
			Text lbl = GetComponent<Text>();
			if (lbl != null) key = lbl.text;
		}

		// If we still don't have a key, leave the value as blank
		if (!string.IsNullOrEmpty(key)) value = Localization.Get(key).ToUpper();
	}
}