using UnityEngine;
using System.Collections;

public class ChefTonyCombatant : PlayerCombatant {

//	public bool EnableBasicAttack = true;
//	public bool EnableHealAttack = true;

	public AudioClip knifeSlash;

	private PlayerAttack currentAttack;
	private EnemyCombatant currentAttackTarget;

	private enum AnimationSequence { None, JumpForward, JumpBackward }

	private AnimationSequence currentAnimation = AnimationSequence.None;
	private AttackAnimationState attackAnimationState = AttackAnimationState.Off;

	private Vector2 initialPosition;

	
	// Use this for initialization
	public override void Start () {
		base.Start ();

		//save initial position
		initialPosition = transform.position;

		//set up basic stats
		MaxHitPoints = 100;
		HitPoints = 100;

		//set up the list of attacks
		PlayerAttack attack1 = new PlayerAttack();
		attack1.Name = "All-Purpose Slice";
		attack1.Description = "Stabs a single target with the Miracle Blade™ All-Purpose Slicer™.";
		attack1.BasePower = 25;
		attack1.IsHealingMove = false;

		PlayerAttack attack2 = new PlayerAttack();
		attack2.Name = "Fried Chicken Smoothie";
		attack2.Description = "Much healthier than whipping cream.  Restores health.";
		attack2.BasePower = 25;
		attack2.IsHealingMove = true;

		Attacks.Add (attack1);
		Attacks.Add (attack2);
	
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();
	
	}

	void FixedUpdate () {
		if(currentAnimation == AnimationSequence.JumpForward) {
			switch(attackAnimationState) {
			case AttackAnimationState.NeedsToStart:
				Vector2 launchVelocity = new Vector2(7f, 0f);

				//calculated the needed initial vertical speed to reach the target
				float dist = currentAttackTarget.transform.position.x - transform.position.x - 0.5f;
				float time = dist / launchVelocity.x;
				launchVelocity.y = Mathf.Abs(Physics2D.gravity.y) / 2 * time;

				rigidbody2D.velocity = launchVelocity;

				attackAnimationState = AttackAnimationState.InProgress;
				GetComponent<Animator>().SetBool("IsAttacking", true);

				break;
			case AttackAnimationState.InProgress:
				if(transform.position.x > currentAttackTarget.transform.position.x - 1f) {
					playSound(knifeSlash);
					currentAttackTarget.Damage(currentAttack.BasePower);
					attackAnimationState = AttackAnimationState.Complete;
				}
				break;
			case AttackAnimationState.Complete:
				if(rigidbody2D.velocity == Vector2.zero) {
					currentAnimation = AnimationSequence.JumpBackward;
					attackAnimationState = AttackAnimationState.NeedsToStart;
				}
				break;
			}
		} else if(currentAnimation == AnimationSequence.JumpBackward) {
			switch(attackAnimationState) {
			case AttackAnimationState.NeedsToStart:
				Vector2 launchVelocity = new Vector2(-7f, 0f);
				
				//calculated the needed initial vertical speed to reach the target
				float dist = initialPosition.x - transform.position.x;
				float time = dist / launchVelocity.x;
				launchVelocity.y = Mathf.Abs(Physics2D.gravity.y) / 2 * time;
				
				rigidbody2D.velocity = launchVelocity;
				
				attackAnimationState = AttackAnimationState.InProgress;
				GetComponent<Animator>().SetBool("IsAttacking", false);
				
				break;
			case AttackAnimationState.InProgress:
				if(transform.position.x < initialPosition.x + 0.1f) {
					rigidbody2D.velocity = Vector2.zero;
					currentAnimation = AnimationSequence.None;
					attackAnimationState = AttackAnimationState.Off;
					AnimationInProgress = false;
				}
				break;
			}
		}
	}

	public override void Attack(PlayerAttack attack, EnemyCombatant target) {
		currentAttack = attack;
		currentAttackTarget = target;
		AnimationInProgress = true;
		currentAnimation = AnimationSequence.JumpForward;
		attackAnimationState = AttackAnimationState.NeedsToStart;

	}
	
}
