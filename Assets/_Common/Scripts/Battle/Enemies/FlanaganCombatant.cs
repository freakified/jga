using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FlanaganCombatant : EnemyCombatant {

	public ParticleSystem OrphanRushPrefab;
	private ParticleSystem orphanRushParticles;

	public OrphanCombatant orphanShield;
	
	private AttackAnimationState attackAnimationState = AttackAnimationState.Off;

	// Use this for initialization
	public override void Start () {
		base.Start ();

		//set up basic stats
		MaxHitPoints = 500;
		HitPoints = 500;

		//init orphan particles
		orphanRushParticles = Instantiate(OrphanRushPrefab) as ParticleSystem;
		orphanRushParticles.transform.parent = transform;
		orphanRushParticles.transform.localPosition = new Vector3(2.0f, 0, 0);
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();

		// FF is shielded so long as the orphan is awake
		isShielded = !orphanShield.isSleeping;

		switch(attackAnimationState) {
		case AttackAnimationState.NeedsToStart:
			GetComponent<Animator>().SetTrigger("FistPump");
			if(timerIsGreaterThan(0.5f)) {

				orphanRushParticles.time = 0;
				orphanRushParticles.Play();

				attackAnimationState = AttackAnimationState.InProgress;

				//playSound(gunshotSound);

				startTimer();
			}
			break;
		case AttackAnimationState.InProgress:
			if(timerIsGreaterThan(1.4f)) {
				target.Damage(23);

				stopTimer();
				attackAnimationState = AttackAnimationState.Off;
				AnimationInProgress = false;
			}
			break;
		}
		
	}

	public override void AutoAttack (List<BattleCombatant> targetList) {
		//select the player with the lowest HP as the target
		target = targetList.OrderByDescending(t => t.HitPoints).First();
		AnimationInProgress = true;
		attackAnimationState = AttackAnimationState.NeedsToStart;
		startTimer();

	}
	
}
