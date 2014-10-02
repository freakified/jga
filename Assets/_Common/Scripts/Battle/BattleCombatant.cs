using UnityEngine;
using System.Collections;

abstract public class BattleCombatant : MonoBehaviour {

	public int HitPoints {get; protected set; }
	public int MaxHitPoints {get; protected set; }
	public bool AnimationInProgress { get; protected set; }
	public bool isSleeping {get; protected set; }
	public bool isShielded {get; protected set; }
	public bool immuneToDamage {get; protected set; }
	public bool participatingInBattle {get; set; }

	protected Animator anim;

	protected float elapsedTime = 0;
	protected bool timerRunning = false;
	protected int sleepTurnCounter;
	
	/// <summary>
	/// The particle sprayer to activate on hits, usually blood.
	/// Optional.
	/// </summary>
	public ParticleSystem DamageParticlesPrefab;
	private ParticleSystem damageParticles;

	public ParticleSystem SleepParticlesPrefab;
	private ParticleSystem sleepParticles;
	
	// Use this for initialization
	public virtual void Start () {
		AnimationInProgress = false;
		isSleeping = false;
		isShielded = false;
		immuneToDamage = false;
		participatingInBattle = true;

		anim = GetComponent<Animator>();

		//check if it's null, since people don't NEED to spew blood if they don't want to
		if(DamageParticlesPrefab != null) {
			damageParticles = Instantiate(DamageParticlesPrefab) as ParticleSystem;
			damageParticles.transform.parent = transform;
			damageParticles.transform.localPosition = Vector2.zero;
		}

		if(SleepParticlesPrefab != null) {
			sleepParticles = Instantiate(SleepParticlesPrefab) as ParticleSystem;
			sleepParticles.transform.parent = transform;
			sleepParticles.transform.localPosition = Vector2.zero;
		}
	}

	// Update is called once per frame
	public virtual void Update () {
		if(timerRunning)
			elapsedTime += Time.deltaTime;
	}

	/// <summary>
	/// Heal to 100% HP, or revive to 50% HP
	/// </summary>
	public void Heal () {
		HitPoints = HitPoints != 0 ? MaxHitPoints : MaxHitPoints / 2;

		if(anim != null) {
			anim.SetInteger("HP", HitPoints);
		}
	}

	public void Heal (int amount) {
		HitPoints = (int)Mathf.Clamp(HitPoints + amount, 0, MaxHitPoints);

		if(anim != null) {
			anim.SetInteger("HP", HitPoints);
		}
	}

	public virtual void Damage (int amount) {

		HitPoints = (int)Mathf.Clamp(HitPoints - Mathf.Max(amount, 0), 0, MaxHitPoints);

		if(DamageParticlesPrefab != null) {
			damageParticles.time = 0;
			damageParticles.Play();
		}

		if(anim != null) {
			anim.SetInteger("HP", HitPoints);
		}

		//ensure that the sleep particles don't continue to play after people die
		if(HitPoints == 0) {
			WakeUp();
		}
	}


	/// <summary>
	/// Put the combatant to "sleep" for the specified number of turns.
	/// </summary>
	/// <param name="numberOfTurns">The number of turns to sleep.</param>
	public virtual void PutToSleep (int numberOfTurns) {
		isSleeping = true;

		sleepTurnCounter = numberOfTurns;

		if(SleepParticlesPrefab != null) {
			sleepParticles.time = 0;
			sleepParticles.Play();
		}
	}

	/// <summary>
	/// Forces the combatant to wake up immediately if they are sleeping
	/// </summary>
	public void WakeUp() {
		isSleeping = false;

		if(SleepParticlesPrefab != null) {
			sleepParticles.Stop();
		}
	}

	public void IncrementTurnCounter() {
		if(isSleeping) {
			sleepTurnCounter--;

			if(sleepTurnCounter == 0) {
				WakeUp ();
			}

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

	public virtual string getName() {
		return name;
	}



}
