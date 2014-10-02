using UnityEngine;
using System.Collections;

public class LikeMikeCombatant : PlayerCombatant {

	public AudioClip healSound, throwSound, hitSound;

	private PlayerAttack currentAttack;
	private BattleCombatant currentAttackTarget;

	private enum AnimationSequence { None, PreparingToSlam, ReturningFromSlamming }

	private AnimationSequence currentAnimation = AnimationSequence.None;
	private AttackAnimationState attackAnimationState = AttackAnimationState.Off;

	private Transform originalBasketball, attacksketball;
	private Vector3 basketballVelocity;

	private Vector2 originalBasketballLocalPos;

	// Use this for initializationl
	public override void Start () {
		base.Start ();

		//set up basic stats
		MaxHitPoints = 120;
		HitPoints = 120;

		//set up the list of attacks
		PlayerAttack attack1 = new PlayerAttack();
		attack1.Name = "Slam Dunk";
		attack1.Description = "Scores a totally radical slam dunk, damaging a single target.";
		attack1.Power = 120;
		attack1.Accuracy = 100;
		attack1.Type = AttackType.Damage;

		PlayerAttack attack2 = new PlayerAttack();
		attack2.Name = "Gatorade\u2122 Sports Drink";
		attack2.Description = "Real athletes literally sweat this substance.  Restores health.";
		attack2.Power = MaxHitPoints;
		attack2.Accuracy = 100;
		attack2.Type = AttackType.Heal;

		Attacks.Add (attack1);
		Attacks.Add (attack2);

		originalBasketball = transform.GetChild (0);
		originalBasketballLocalPos = originalBasketball.transform.localPosition;

		attacksketball = Instantiate(originalBasketball) as Transform;
		attacksketball.renderer.enabled = false;
		attacksketball.transform.position = transform.position;
	
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();

		if(HitPoints == 0) {
			originalBasketball.renderer.enabled = false;
		} else {
			if(!AnimationInProgress) {
				originalBasketball.renderer.enabled = true;
			}
		}
	}

	void FixedUpdate () {
		if(currentAnimation == AnimationSequence.PreparingToSlam) {
			animPrepareToSlam();
		} else if(currentAnimation == AnimationSequence.ReturningFromSlamming) {
			animReturningFromSlamming();
		}
	}

	private void animPrepareToSlam () {
		switch (attackAnimationState) {
		case AttackAnimationState.NeedsToStart:
			Vector2 launchForce = new Vector2 (0f, 300f);

			originalBasketball.localPosition = new Vector2(0.18f, 0.21f);

			GetComponent<BoxCollider2D>().enabled = false;
			rigidbody2D.AddForce(launchForce);
			anim.SetTrigger ("Dunking");

			attackAnimationState = AttackAnimationState.InProgress;

			break;
		case AttackAnimationState.InProgress:
			if (rigidbody2D.velocity.magnitude < 0.8) {
				anim.SetTrigger ("Slamming");
				rigidbody2D.gravityScale = 0;

				originalBasketball.renderer.enabled = false;

				attacksketball.position = transform.position + new Vector3(0.18f, 0.21f, 0.0f);
				basketballVelocity = currentAttackTarget.transform.position - attacksketball.position;
				basketballVelocity *= 2;
				attacksketball.renderer.enabled = true;
				playSound(throwSound);

				attackAnimationState = AttackAnimationState.Complete;
			}
			break;
		case AttackAnimationState.Complete:
			attacksketball.position = attacksketball.position + basketballVelocity * Time.fixedDeltaTime;

			if(attacksketball.position.x > (currentAttackTarget.transform.position.x - 0.1) ) {
				attacksketball.renderer.enabled = false;
				currentAttackTarget.Damage(currentAttack.Power);
				playSound(hitSound);
				currentAnimation = AnimationSequence.ReturningFromSlamming;
				attackAnimationState = AttackAnimationState.NeedsToStart;
			}

			break;
		}
	}

	private void animReturningFromSlamming () {
		switch (attackAnimationState) {
		case AttackAnimationState.NeedsToStart:
			rigidbody2D.gravityScale = 1;
			GetComponent<BoxCollider2D>().enabled = true;

			attackAnimationState = AttackAnimationState.InProgress;
			
			break;
		case AttackAnimationState.InProgress:

			if(rigidbody2D.velocity.magnitude < 0.01f) {
				anim.SetTrigger("Finished");
				originalBasketball.transform.localPosition = originalBasketballLocalPos;
				originalBasketball.renderer.enabled = true;
				attackAnimationState = AttackAnimationState.Complete;
			}

			break;
		case AttackAnimationState.Complete:
			currentAnimation = AnimationSequence.None;
			attackAnimationState = AttackAnimationState.Off;
			AnimationInProgress = false;
			break;
		}
	}

	public override void Attack(PlayerAttack attack, BattleCombatant target) {

		currentAttack = attack;
		currentAttackTarget = target;
		
		if(attack.Name == "Slam Dunk") {
			AnimationInProgress = true;
			currentAnimation = AnimationSequence.PreparingToSlam;
			attackAnimationState = AttackAnimationState.NeedsToStart;

		} else if(attack.Name == "Gatorade\u2122 Sports Drink") {
			playSound(healSound);
			currentAttackTarget.Heal();
		}

		
	}
}
