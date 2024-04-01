using UnityEngine;
using UnityEngine.UI;

public class PlaySoundOnClick : MonoBehaviour
{
   // [SerializeField] private ActionClick actionClick;
    [SerializeField] private AudioClip sound;
    [SerializeField] private bool isEnableSoundToggle = true;

    private void Awake()
    {
        var button = GetComponent<Button>();
        if (button != null)
                button.onClick.AddListener(PlaySound);

        if (isEnableSoundToggle)
        {
            var toggle = GetComponent<Toggle>();
            if (toggle != null)
                toggle.onValueChanged.AddListener((v) => { PlaySound(); });
        }
    }

    public void PlaySound()
    {
        GameController.Instance.musicManager.PlayOneShot(sound);
       // AnalyticsController.LogActionClick(actionClick);
        //Debug.Log(StringHelper.StringColor(gameObject.name, ColorString.green));
    }
}
