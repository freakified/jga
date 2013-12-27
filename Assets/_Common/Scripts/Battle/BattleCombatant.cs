using UnityEngine;
using System.Collections;

abstract public class BattleCombatant : MonoBehaviour {

	public int HitPoints {get; protected set; }
	public int MaxHitPoints {get; protected set; }
	public bool AnimationInProgress { get; protected set; }

	protected float elapsedTime = 0;
	protected bool timerRunning = false;
	
	/// <summary>
	/// The particle sprayer to activate on hits, usually blood.
	/// Optional.
	/// </summary>
	public ParticleSystem damageParticles;

	private ParticleSystem particles;

	// Use this for initialization
	public virtual void Start () {
		AnimationInProgress = false;
		
		if(damageParticles != null) {
			particles = Instantiate(damageParticles) as ParticleSystem;
			particles.transform.position = transform.position;
		}
	}
	
	// Update is called once per frame
	public virtual void Update () {
		if(timerRunning)
			elapsedTime += Time.deltaTime;
	}

	public void Heal (int amount) {
		HitPoints = (int)Mathf.Clamp(HitPoints + amount, 0, MaxHitPoints);
	}

	public void Damage (int amount) {
		HitPoints = (int)Mathf.Clamp(HitPoints - amount, 0, MaxHitPoints);

		if(damageParticles != null) {

			particles.transform.position = transform.position;
			particles.time = 0;
			particles.Play();

		}
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
