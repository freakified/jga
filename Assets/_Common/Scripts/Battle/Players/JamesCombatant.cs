using UnityEngine;
using System.Collections;

public class JamesCombatant : PlayerCombatant {

	public AudioClip GasBlastSound, HealSound;

	private PlayerAttack currentAttack;
	private BattleCombatant currentAttackTarget;

	private enum AnimationSequence { None, FlyingTowardsTarget, FlyingBack }

	private AnimationSequence currentAnimation = AnimationSequence.None;
	private AttackAnimationState attackAnimationState = AttackAnimationState.Off;

	private Vector2 initialPosition;

	// Use this for initializationl
	public override void Start () {
		base.Start ();

		//set up basic stats
		MaxHitPoints = 120;
		HitPoints = 120;

		//set up the list of attacks
		PlayerAttack attack1 = new PlayerAttack();
		attack1.Name = "Gas Blast";
		attack1.Description = "Unleashes a blast of foul-smelling gas upon the target, dealing damage.";
		attack1.Power = 240;
		attack1.Accuracy = 100;
		attack1.Type = AttackType.Damage;

		PlayerAttack attack2 = new PlayerAttack();
		attack2.Name = "Can of Beans";
		attack2.Description = "A delicious can of baked beans, scientifically proven to promote good heart health.";
		attack2.Power = MaxHitPoints;
		attack2.Accuracy = 100;
		attack2.Type = AttackType.Heal;

		Attacks.Add (attack1);
		Attacks.Add (attack2);

	
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();
		
	}

	void FixedUpdate () {
		if(currentAnimation == AnimationSequence.FlyingTowardsTarget) {
			animFlyTowardsTarget();
		} else if(currentAnimation == AnimationSequence.FlyingBack) {
			animFlyingBack();
		}
	}

	private void animFlyTowardsTarget () {
		switch (attackAnimationState) {
		case AttackAnimationState.NeedsToStart:
			initialPosition = transform.position;

			GetComponent<JamesGasBehavior>().EnableFlightMode();

			attackAnimationState = AttackAnimationState.InProgress;

			break;
		case AttackAnimationState.InProgress:

			rigidbody2D.AddForce(new Vector2(0, 400) * Time.fixedDeltaTime);

			
			if(transform.position.y > 0.1f) {
				playSound(GasBlastSound);
				attackAnimationState = AttackAnimationState.Complete;
			}


			break;
		case AttackAnimationState.Complete:

			rigidbody2D.AddForce(currentAttackTarget.transform.position - transform.position
			                     * 250 * Time.fixedDeltaTime);

			if(transform.rotation.eulerAngles.z < 170 && transform.rotation.eulerAngles.z > 163) {
				currentAttackTarget.Damage(currentAttack.Power);


				currentAnimation = AnimationSequence.FlyingBack;
				attackAnimationState = AttackAnimationState.NeedsToStart;
			}


			break;
		}
	}

	private void animFlyingBack () {
		switch (attackAnimationState) {
		case AttackAnimationState.NeedsToStart:

			rigidbody2D.AddForce(new Vector2(-400, 100) * Time.deltaTime);

			if(transform.position.x < initialPosition.x + 0.1f) {
				attackAnimationState = AttackAnimationState.InProgress;
			}
			
			break;
		case AttackAnimationState.InProgress:

			GetComponent<JamesGasBehavior>().DisableFlightMode();
			rigidbody2D.velocity = Vector2.zero;

			attackAnimationState = AttackAnimationState.Complete;


			break;
		case AttackAnimationState.Complete:

			if(transform.position.y < initialPosition.y + 0.2) {
				currentAnimation = AnimationSequence.None;
				attackAnimationState = AttackAnimationState.Off;
				AnimationInProgress = false;
			}

			break;
		}
	}

	public override void Attack(PlayerAttack attack, BattleCombatant target) {

		currentAttack = attack;
		currentAttackTarget = target;
		
		if(attack.Name == "Gas Blast") {
			initialPosition = transform.position;

			AnimationInProgress = true;
			currentAnimation = AnimationSequence.FlyingTowardsTarget;
			attackAnimationState = AttackAnimationState.NeedsToStart;

		} else if(attack.Name == "Can of Beans") {
			playSound(HealSound);
			currentAttackTarget.Heal();
			
		}

		
	}
}
