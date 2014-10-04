using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FBCombatant : EnemyCombatant {

	public AudioClip LaserChargeSound, LaserFireSound, throwSound, hitSound;
	public GameObject ImmunityNotificationPrefab;

	private BBallShieldCombatant BBallShield;

	private GameObject immunityNotification;

	public int InitialHealth = 5000;

	private ParticleSystem LaserCharge, LaserFire;

	private AttackAnimationState attackAnimationState = AttackAnimationState.Off;
	private enum AnimationSequence { None, LaserCharge, LaserFire, LameBounce }

	private AnimationSequence currentAnimation = AnimationSequence.None;

	private List<BattleCombatant> targets;

	private int chargesRequred = 2;
	private int currentChargeCount = 2;

	// Use this for initialization
	public override void Start () {
		base.Start ();

		//set up basic stats
		MaxHitPoints = InitialHealth;
		HitPoints = InitialHealth;

		LaserCharge = transform.GetComponentsInChildren<ParticleSystem>()[0];
		LaserFire = GameObject.Find ("Laser_Fire").particleSystem;
		BBallShield = GameObject.Find ("BBall_Shield").GetComponent<BBallShieldCombatant>();

	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();

		isShielded = !BBallShield.isSleeping && BBallShield.HitPoints > 0;

		switch(currentAnimation) {
		case AnimationSequence.LaserCharge:
			animLaserCharge();

			break;
		case AnimationSequence.LaserFire:
			animLaserFire();

			break;
		case AnimationSequence.LameBounce:
			animLameBounce();

			break;
		}
		
	}



	private void animLaserCharge() {
		switch(attackAnimationState) {
		case AttackAnimationState.NeedsToStart:
			LaserCharge.Play();

			attackAnimationState = AttackAnimationState.InProgress;
			playSound(LaserChargeSound);
		
			break;
		case AttackAnimationState.InProgress:
			if(LaserCharge.time >= 3.0) {
				currentAnimation = AnimationSequence.LaserFire;
				attackAnimationState = AttackAnimationState.NeedsToStart;
				LaserCharge.Stop();
			}
			
			break;
		}
	}

	private void animLaserFire() {
		switch(attackAnimationState) {
		case AttackAnimationState.NeedsToStart:

			LaserFire.enableEmission = true;

			playSound(LaserFireSound);
			attackAnimationState = AttackAnimationState.InProgress;

			startTimer();
			
			break;
		case AttackAnimationState.InProgress:

			if(timerIsGreaterThan(2.5f)) {

				LaserFire.enableEmission = false;
				currentAnimation = AnimationSequence.None;
				attackAnimationState = AttackAnimationState.Off;

				targets.ForEach(t => t.Damage(t.MaxHitPoints - 1));

				AnimationInProgress = false;
			}

			break;
		}
	}

	private Vector3 lameBounceVelocity;
	private Vector3 initialPosition;

	private void animLameBounce() {
		switch(attackAnimationState) {
		case AttackAnimationState.NeedsToStart:

			if(timerIsGreaterThan(1.0f)) {

				initialPosition = transform.position;

				lameBounceVelocity = (transform.position - Vector3.right/2) - transform.position;
				lameBounceVelocity *= 3;

				playSound(hitSound);
				target.Damage(1);

				startTimer();
				attackAnimationState = AttackAnimationState.InProgress;
			}

			break;
		case AttackAnimationState.InProgress:
			transform.position = transform.position + lameBounceVelocity * Time.fixedDeltaTime;
			
			if(timerIsGreaterThan(0.1f)) {
				attackAnimationState = AttackAnimationState.Complete;
				startTimer();
			}


			
			break;
		case AttackAnimationState.Complete:
			transform.position = initialPosition;

			if(timerIsGreaterThan(0.5f)) {
				currentAnimation = AnimationSequence.None;
				attackAnimationState = AttackAnimationState.Off;
				AnimationInProgress = false;
			}

			break;
		}
	}

	public override void PutToSleep (int numberOfTurns) {
		// evil basketball entities are not sold by your sales pitches
		if(immunityNotification == null) {
			immunityNotification = Instantiate(ImmunityNotificationPrefab, 
			                           new Vector3(0.76f,
				            					   0.7f,
				            					   0),
			                           Quaternion.identity) as GameObject;
		}

		immunityNotification.GetComponent<TextFadeOutScript>().ShowText();
	}

	public override string getName() {
		return "Flying Basketball";
	}

	public override void AutoAttack (List<BattleCombatant> targetList) {

		if(currentChargeCount == chargesRequred) {
			// do crazy attack

			targets = targetList;
			AnimationInProgress = true;
			currentAnimation = AnimationSequence.LaserCharge;
			attackAnimationState = AttackAnimationState.NeedsToStart;

			currentChargeCount = 0;
		} else {
			currentChargeCount++;

			// do a simple attack
			target = getWeakestTarget(targetList);

			AnimationInProgress = true;
			currentAnimation = AnimationSequence.LameBounce;
			attackAnimationState = AttackAnimationState.NeedsToStart;
			startTimer();


		}


	}
	
}
