using UnityEngine;
using System.Collections;

abstract public class CutscenePuppeteer : MonoBehaviour {

	protected int CurrentScene = 0;
	protected float elapsedTime = 0;
	protected bool timerRunning = false;
	private CameraFade fader;

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

		fader = Camera.main.GetComponent<CameraFade>();

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

	protected void stopSound() {
		soundSource.Stop();
	}

	protected IEnumerator FadeAndNext(Color fadeTo, float seconds, string nextScene) {
		fader.SetScreenOverlayColor (new Color(fadeTo.r, fadeTo.g, fadeTo.b, 0));
		fader.StartFade(fadeTo, seconds);
		yield return new WaitForSeconds(seconds);
		if(nextScene != null)
			Application.LoadLevel(nextScene);
	}

	
}
