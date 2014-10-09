using UnityEngine;
using System.Collections;

abstract public class CutscenePuppeteer : MonoBehaviour {

	protected int CurrentScene = 0;
	protected float elapsedTime = 0;
	protected bool timerRunning = false;
	private AudioSource soundSource;


	public virtual void Update() {
		if(timerRunning)
			elapsedTime += Time.deltaTime;
	}

	public void Awake() {
		soundSource = gameObject.AddComponent<AudioSource>();
	}
		
	public virtual void OnEnable() {
		CutsceneController.OnCutsceneChange += UpdateSceneNumber;
	}
	
	
	public virtual void OnDisable() {
		CutsceneController.OnCutsceneChange -= UpdateSceneNumber;
	}

	public void UpdateSceneNumber(int SceneNumber) {
		CurrentScene = SceneNumber;

		HandleSceneChange();
	}

	abstract public void HandleSceneChange();

	/* Various convenience methods for subclasses */

	protected void flipObject(GameObject g) {
		Vector3 theScale = g.transform.localScale;
		theScale.x *= -1;
		g.transform.localScale = theScale;
	}

	protected void nextScene() {
		GetComponent<CutsceneController>().playNext();
	}

	protected void stopTimer() {
		timerRunning = false;
		elapsedTime = 0;
	}

	protected void startTimer() {
		timerRunning = true;
		elapsedTime = 0;
	}

	protected bool timerIsGreaterThan(float seconds) {
		return timerRunning && elapsedTime > seconds;
	}

	protected void playSound(AudioClip sound) {
		soundSource.clip = sound;
		soundSource.Play();
	}

	protected void playSound(AudioClip sound, bool shouldLoop) {
		soundSource.loop = shouldLoop;

		playSound(sound);
	}

	protected void stopSound() {
		soundSource.loop = false;
		soundSource.Stop();
	}

	protected void FadeAndNext(Color fadeTo, float seconds, string nextScene, bool fadeMusic) {
		Camera.main.GetComponent<CameraFade>().FadeAndNext(fadeTo, seconds, nextScene, fadeMusic);
	}
	
}
