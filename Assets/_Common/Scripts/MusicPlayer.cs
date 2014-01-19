using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {

	public AudioClip music;
	public bool loop = false;
	public bool playAtStart = true;
	private AudioSource soundSource;

	private static MusicPlayer instance = null;

	public static MusicPlayer Instance {
		get { return instance; }
	}

	void Awake () {
		// singleton example code
		if (instance != null && instance != this) {
			//if there is already a music player active
			//just tell *it* to play our song, and then self-destruct

			if(playAtStart) {
				instance.PlayMusic(this.music, this.loop);
			}

			Destroy(this.gameObject);
			return;
		} else {
			instance = this;
		}

		DontDestroyOnLoad(this.gameObject);

		soundSource = gameObject.AddComponent<AudioSource>();
		soundSource.loop = loop;

		PlayMusic (music, loop);
	}

	public void PlayMusic(AudioClip newMusic, bool shouldLoop) {
		soundSource.Stop();
		music = newMusic;
		soundSource.loop = shouldLoop;
		
		if(newMusic != null) {
			soundSource.clip = music;
			soundSource.Play();
		}
	}

}
