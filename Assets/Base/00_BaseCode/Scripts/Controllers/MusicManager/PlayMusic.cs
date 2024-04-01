using UnityEngine;

public class PlayMusic : MonoBehaviour {

	public AudioClip musicClip;

	private void Start()
	{
		GameController.Instance.musicManager.PlayMusic(musicClip);
	}

}
