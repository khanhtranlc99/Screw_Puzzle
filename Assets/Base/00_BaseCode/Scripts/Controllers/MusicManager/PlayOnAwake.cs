using UnityEngine;

public class PlayOnAwake : MonoBehaviour {
	
    public AudioClip audioClip;
	
	private void OnEnable () {
        GameController.Instance.musicManager.PlayOneShot(audioClip);
    }
	
}
