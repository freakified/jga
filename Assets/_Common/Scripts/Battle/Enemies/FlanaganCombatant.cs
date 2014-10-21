using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FlanaganCombatant : EnemyCombatant {

	public ParticleSystem OrphanRushPrefab;
	private ParticleSystem orphanRushParticles;

	public AudioClip OrphanRushSound;

	public OrphanCombatant orphanShield;
	
	private AttackAnimationState attackAnimationState = AttackAnimationState.Off;
	private bool poweredUp = false;

	private bool isFirstPoweredUpAttack = true;

	// Use this for initialization
	public override void Start () {
		base.Start ();

		//set up basic stats
		MaxHitPoints = 350;
		HitPoints = 350;

		//init orphan particles
		orphanRushParticles = Instantiate(OrphanRushPrefab) as ParticleSystem;
		orphanRushParticles.transform.parent = transform;
		orphanRushParticles.transform.localPosition = new Vector3(2.0f, 0, 0);
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();

		// FF loses his shield when powered up
		if(!poweredUp) {
			// FF is shielded so long as the orphan is awake
			isShielded = !orphanShield.isSleeping;
		} else {
			isShielded = false;
		}

		switch(attackAnimationState) {
		case AttackAnimationState.NeedsToStart:
			GetComponent<Animator>().SetTrigger("FistPump");
			if(timerIsGreaterThan(0.5f)) {


				orphanRushParticles.time = 0;
				orphanRushParticles.Play();

				attackAnimationState = AttackAnimationState.InProgress;

				playSound(OrphanRushSound);

				orphanRushParticles = Instantiate(OrphanRushPrefab) as ParticleSystem;
				orphanRushParticles.transform.parent = transform;
				orphanRushParticles.transform.localPosition = new Vector3(2.0f, 0, 0);

				startTimer();
			}
			break;
		case AttackAnimationState.InProgress:
			if(timerIsGreaterThan(1.4f)) {
				if(target != null) {
					if(poweredUp == true) {
						// don't kill him on the first attack
						if(isFirstPoweredUpAttack) {
							target.Damage(target.HitPoints - 2);
							isFirstPoweredUpAttack = false;
						} else {
							target.Damage(52);
						}
					} else {
						target.Damage(23);
					}
				}

				stopTimer();
				attackAnimationState = AttackAnimationState.Off;
				AnimationInProgress = false;
			}
			break;
		}
		
	}

	public override void AutoAttack (List<BattleCombatant> targetList) {
		//select the player with the lowest HP as the target

		// allows you to call autoattack without a target
		// this is so we can trigger the orphanrush animation outside of the battle
		if(targetList != null) {
			target = targetList.OrderByDescending(t => t.HitPoints).First();
		}

		//if our HP is low, use the SUPER ORPHAN RUSH attack
		if(HitPoints / (float)MaxHitPoints < 0.2f) {
			orphanRushParticles.emissionRate = 15;
			poweredUp = true;
		} else {
			orphanRushParticles.emissionRate = 5;
			poweredUp = false;
		}

		AnimationInProgress = true;
		attackAnimationState = AttackAnimationState.NeedsToStart;
		startTimer();

	}
	
}
