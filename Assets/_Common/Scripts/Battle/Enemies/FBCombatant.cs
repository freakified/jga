using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FBCombatant : EnemyCombatant {

	public AudioClip LaserChargeSound, LaserFireSound;

	private int InitialHealth = 1000;
	private int AttackPower = 99;

	private ParticleSystem LaserCharge, LaserFire;

	private AttackAnimationState attackAnimationState = AttackAnimationState.Off;
	private enum AnimationSequence { None, LaserCharge, LaserFire }

	private AnimationSequence currentAnimation = AnimationSequence.None;

	private List<BattleCombatant> targets;

	private int chargesRequred = 1;
	private int currentChargeCount = 1;

	// Use this for initialization
	public override void Start () {
		base.Start ();

		//set up basic stats
		MaxHitPoints = InitialHealth;
		HitPoints = InitialHealth;

		LaserCharge = transform.GetComponentsInChildren<ParticleSystem>()[0];
		LaserFire = transform.GetComponentsInChildren<ParticleSystem>()[1];

	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();

		switch(currentAnimation) {
		case AnimationSequence.LaserCharge:
			animLaserCharge();

			break;
		case AnimationSequence.LaserFire:
			animLaserFire();

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

				//EVERYONE DIES HAHAHAHAHAHAH
				targets.ForEach(t => t.Damage(t.MaxHitPoints - 1));

				AnimationInProgress = false;
			}

			break;
		}
	}

	public override string getName() {
		return "Flying Basketball";
	}

	public override void AutoAttack (List<BattleCombatant> targetList) {

		if(currentChargeCount == chargesRequred) {
			targets = targetList;
			AnimationInProgress = true;
			currentAnimation = AnimationSequence.LaserCharge;
			attackAnimationState = AttackAnimationState.NeedsToStart;

			currentChargeCount = 0;
		} else {
			currentChargeCount++;
		}


	}
	
}
