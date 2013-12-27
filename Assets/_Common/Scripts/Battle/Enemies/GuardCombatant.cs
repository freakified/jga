using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GuardCombatant : EnemyCombatant {

	public AudioClip gunshotSound;

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

				GetComponent<Animator>().SetBool("IsAttacking", true);
				attackAnimationState = AttackAnimationState.InProgress;

				playSound(gunshotSound);
				target.Damage(25);

				startTimer();
			}
			break;
		case AttackAnimationState.InProgress:
			if(timerIsGreaterThan(0.5f)) {
				GetComponent<Animator>().SetBool("IsAttacking", false);

				stopTimer();
				attackAnimationState = AttackAnimationState.Off;
				AnimationInProgress = false;
			}
			break;
		}
		
	}

	public override void AutoAttack (List<PlayerCombatant> playerList) {
		//select the player with the lowest HP as the target
		target = playerList.OrderByDescending(player => player.HitPoints).First();
		AnimationInProgress = true;
		attackAnimationState = AttackAnimationState.NeedsToStart;
		startTimer();

	}
	
}
