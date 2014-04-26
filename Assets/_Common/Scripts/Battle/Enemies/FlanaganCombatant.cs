using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FlanaganCombatant : EnemyCombatant {

	public ParticleSystem OrphanRushPrefab;

	private ParticleSystem orphanRush;

	private AttackAnimationState attackAnimationState = AttackAnimationState.Off;

	// Use this for initialization
	public override void Start () {
		base.Start ();

		//set up basic stats
		MaxHitPoints = 100;
		HitPoints = 100;

	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();

		switch(attackAnimationState) {
		case AttackAnimationState.NeedsToStart:
			if(timerIsGreaterThan(0.5f)) {

				orphanRush = Instantiate(OrphanRushPrefab) as ParticleSystem;
				orphanRush.transform.parent = transform;

				attackAnimationState = AttackAnimationState.InProgress;

				//playSound(gunshotSound);

				startTimer();
			}
			break;
		case AttackAnimationState.InProgress:
			if(timerIsGreaterThan(0.5f)) {
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
