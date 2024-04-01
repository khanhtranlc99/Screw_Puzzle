using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UILocalizeFirstUpper : UILocalize
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

		if (!string.IsNullOrEmpty(key))
		{
			string tempValue = Localization.Get(key).ToLower();
			value = tempValue.First().ToString().ToUpper() + tempValue.Substring(1);
		}
	}
}