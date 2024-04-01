using System.Collections.Generic;
using UnityEngine;

namespace Joker2X.Localize.Scripts.Internal
{
    public class LocalizeService : IService
    {
        public readonly List<SystemLanguage> SupportedLanguage = new List<SystemLanguage>
        {
            SystemLanguage.English,
            SystemLanguage.Arabic,
            SystemLanguage.Chinese,
            SystemLanguage.French,
            SystemLanguage.German,
            SystemLanguage.Indonesian,
            SystemLanguage.Italian,
            SystemLanguage.Japanese,
            SystemLanguage.Korean,
            SystemLanguage.Portuguese,
            SystemLanguage.Russian,
            SystemLanguage.Spanish,
            SystemLanguage.Thai,
            SystemLanguage.Turkish,
            SystemLanguage.Vietnamese,
        };

        public void Init()
        {
            Localization.loadFunction = LoadFunction;

            if (!PlayerPrefs.HasKey("Language"))
            {
                var language = SupportedLanguage.Contains(Application.systemLanguage)
                    ? Application.systemLanguage.ToString()
                    : SystemLanguage.English.ToString();
                Localization.LoadCSV(Localization.loadFunction.Invoke(language), true);
                Localization.language = language;
            }
        }

        private byte[] LoadFunction(string mode)
        {
            var language = string.CompareOrdinal(mode, "Localization") == 0
                ? PlayerPrefs.GetString("Language", "English")
                : mode;
            if (string.CompareOrdinal(language, SystemLanguage.English.ToString()) == 0)
            {
                var asset = Resources.Load<TextAsset>("Localization");
                return asset.bytes;
            }
            else
            {
                var asset = Resources.Load<TextAsset>($"Localization-{language}");
                return asset.bytes;
            }
        }
    }
}