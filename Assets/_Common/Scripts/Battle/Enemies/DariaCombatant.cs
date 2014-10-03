using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DariaCombatant : EnemyCombatant {

	public AudioClip LaserChargeSound, LaserFireSound;

	public GameObject ImmunityNotificationPrefab;
	private GameObject immunityNotification;

	public int InitialHealth = 100;
	private int AttackPower = 99;

	private ParticleSystem LaserCharge, LaserFire, StaffGlow;
	private Quaternion initialLaserRot;

	private AttackAnimationState attackAnimationState = AttackAnimationState.Off;
	private enum AnimationSequence { None, LaserCharge, LaserFire }

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

		StaffGlow = transform.GetComponentsInChildren<ParticleSystem>()[0];
		LaserCharge = transform.GetComponentsInChildren<ParticleSystem>()[1];
		LaserFire = transform.GetComponentsInChildren<ParticleSystem>()[2];

		initialLaserRot = LaserFire.transform.localRotation;

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
			if(LaserCharge.time >= 2.5) {
				currentAnimation = AnimationSequence.LaserFire;
				attackAnimationState = AttackAnimationState.NeedsToStart;
				LaserFire.Stop();
			}
			
			break;
		}
	}

	private void animLaserFire() {
		switch(attackAnimationState) {
		case AttackAnimationState.NeedsToStart:
			LaserFire.Play();
			playSound(LaserFireSound);
			attackAnimationState = AttackAnimationState.InProgress;
			
			break;
		case AttackAnimationState.InProgress:
			LaserFire.transform.Rotate (new Vector3(0, 0, 10) * Time.deltaTime);

			if(LaserFire.transform.rotation.eulerAngles.z > 9 &&
			   LaserFire.transform.rotation.eulerAngles.z < 10) {


				LaserFire.Stop();
				LaserFire.Clear();

				LaserFire.transform.localRotation = initialLaserRot;
				currentAnimation = AnimationSequence.None;
				attackAnimationState = AttackAnimationState.Off;

				//EVERYONE DIES HAHAHAHAHAHAH
				targets.ForEach(t => t.Damage(AttackPower));

				AnimationInProgress = false;
			}

			break;
		}
	}

	public override void PutToSleep (int numberOfTurns) {
		// evil basketball entities are not sold by your sales pitches
		if(immunityNotification == null) {
			immunityNotification = Instantiate(ImmunityNotificationPrefab, 
			                                   new Vector3(0.79f,
												            0.42f,
												            0),
			                                   Quaternion.identity) as GameObject;
		}
		
		immunityNotification.GetComponent<TextFadeOutScript>().ShowText();
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

		StaffGlow.gravityModifier = Mathf.Lerp (-0.001f, -0.02f, currentChargeCount / (float)chargesRequred);


	}
	
}
