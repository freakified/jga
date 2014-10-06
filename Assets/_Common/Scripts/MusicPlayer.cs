using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {

	public AudioClip music;
	public bool loop = false;
	public bool playAtStart = true;
	public bool OnlyPlayIfNoMusicPlaying = false;
	private AudioSource soundSource;

	private static MusicPlayer instance = null;

	//used for fade out function
	[HideInInspector]
	public bool fadingOut = false;
	private float fadeDuration;
	private float fadeElapsed;

	public static MusicPlayer Instance {
		get { return instance; }
	}

	void Awake () {
		// singleton example code
		if (instance != null && instance != this) {
			//if there is already a music player active
			//just tell *it* to play our song, and then self-destruct

			if(playAtStart) {
				if(OnlyPlayIfNoMusicPlaying) {
					if(!this.soundSource.isPlaying) {
						instance.PlayMusic(this.music, this.loop);
					}
				} else {
					instance.PlayMusic(this.music, this.loop);
				}


			} else {
				instance.music = this.music;
				instance.loop = this.loop;
			}

			Destroy(this.gameObject);
			return;
		} else {
			instance = this;
		}

		DontDestroyOnLoad(this.gameObject);

		soundSource = gameObject.AddComponent<AudioSource>();
		soundSource.loop = loop;

		if(playAtStart)
			PlayMusic (music, loop);
	}

	public void PlayMusic(AudioClip newMusic, bool shouldLoop) {
		soundSource.Stop();
		soundSource.volume = 1.0f;
		fadingOut = false;
		music = newMusic;
		soundSource.loop = shouldLoop;
		
		if(newMusic != null) {
			soundSource.clip = music;
			soundSource.Play();
		}
	}

	public void PlayMusic() {
		PlayMusic(this.music, this.loop);
	}

	//stops the current music immediately
	public void StopMusic() {
		soundSource.Stop();
	}

	//stops the current music, with a fade out
	public void StopMusic(float fadeLength) {
		fadingOut = true;
		fadeDuration = fadeLength;
		fadeElapsed = 0;
	}

	public void Update() {
		if(fadingOut) {
			fadeElapsed += Time.deltaTime;
			soundSource.volume = Mathf.Lerp(1.0f, 0.0f, fadeElapsed / fadeDuration);

			//if the fade is complete stop the audio altogether
			if(fadeElapsed >= fadeDuration) {
				fadingOut = false;
				soundSource.Stop();
			}
		}
	}

}
