using UnityEngine;
using System.Collections;

abstract public class CutscenePuppeteer : MonoBehaviour {

	protected int CurrentScene = 0;
	protected float elapsedTime = 0;
	protected bool timerRunning = false;


	public virtual void Update() {
		if(timerRunning)
			elapsedTime += Time.deltaTime;
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
		AudioSource.PlayClipAtPoint(sound, Camera.main.transform.position);
	}

	
}
