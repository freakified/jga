using UnityEngine;
using System.Collections;

abstract public class BattleCombatant : MonoBehaviour {

	public int HitPoints {get; protected set; }
	public int MaxHitPoints {get; protected set; }
	public bool AnimationInProgress { get; protected set; }

	protected Animator anim;

	protected float elapsedTime = 0;
	protected bool timerRunning = false;
	
	/// <summary>
	/// The particle sprayer to activate on hits, usually blood.
	/// Optional.
	/// </summary>
	public ParticleSystem DamageParticlesPrefab;

	private ParticleSystem damageParticles;

	// Use this for initialization
	public virtual void Start () {
		AnimationInProgress = false;

		anim = GetComponent<Animator>();

		//check if it's null, since people don't NEED to spew blood if they don't want to
		if(DamageParticlesPrefab != null) {
			damageParticles = Instantiate(DamageParticlesPrefab) as ParticleSystem;
			damageParticles.transform.parent = transform;
			damageParticles.transform.localPosition = Vector2.zero;
		}
	}
	
	// Update is called once per frame
	public virtual void Update () {
		if(timerRunning)
			elapsedTime += Time.deltaTime;
	}

	public void Heal (int amount) {
		HitPoints = (int)Mathf.Clamp(HitPoints + amount, 0, MaxHitPoints);

		if(anim != null) {
			anim.SetInteger("HP", HitPoints);
		}
	}

	public void Damage (int amount) {
		HitPoints = (int)Mathf.Clamp(HitPoints - amount, 0, MaxHitPoints);

		if(DamageParticlesPrefab != null) {
			damageParticles.time = 0;
			damageParticles.Play();
		}

		if(anim != null) {
			anim.SetInteger("HP", HitPoints);
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
