using UnityEngine;
using System.Collections;

public class ChefTonyCombatant : PlayerCombatant {

//	public bool EnableBasicAttack = true;
//	public bool EnableHealAttack = true;

	public AudioClip knifeSlash;
	public AudioClip healSound;

	private PlayerAttack currentAttack;
	private BattleCombatant currentAttackTarget;

	private enum AnimationSequence { None, JumpForward, JumpBackward, WalkForward, WalkBackward }

	private AnimationSequence currentAnimation = AnimationSequence.None;
	private AttackAnimationState attackAnimationState = AttackAnimationState.Off;

	private Vector2 initialPosition;

	private float initialDrag;

	
	// Use this for initialization
	public override void Start () {
		base.Start ();

		//set up basic stats
		MaxHitPoints = 100;
		HitPoints = 100;
		anim.SetInteger("HP", HitPoints);

		//set up the list of attacks
		PlayerAttack attack1 = new PlayerAttack();
		attack1.Name = "All-Purpose Slice";
		attack1.Description = "Stabs a single target with the Miracle Blade™ All-Purpose Slicer™.";
		attack1.BasePower = 75;
		attack1.Type = AttackType.Damage;

		PlayerAttack attack2 = new PlayerAttack();
		attack2.Name = "Sales Pitch";
		attack2.Description = "Puts target to sleep with a lecture on the " +
			"benefits of the Miracle Blade™ III Perfection Series™.";
		attack2.BasePower = 3;
		attack2.Type = AttackType.Sleep;

		PlayerAttack attack3 = new PlayerAttack();
		attack3.Name = "Fried Chicken Smoothie";
		attack3.Description = "Much healthier than whipping cream.  Restores health.";
		attack3.BasePower = 25;
		attack3.Type = AttackType.Heal;

		Attacks.Add (attack1);
		Attacks.Add (attack2);
		Attacks.Add (attack3);
	
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

				// save chef tony's initial linear drag stat
				initialDrag = rigidbody2D.drag;

				// set drag to 0
				rigidbody2D.drag = 0;

				//calculate the needed initial vertical speed to reach the target
				float dist = currentAttackTarget.transform.position.x - transform.position.x - 0.3f;
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
				
				//calculate the needed initial vertical speed to reach the initial position
				float dist = initialPosition.x - transform.position.x;
				float time = dist / launchVelocity.x;
				launchVelocity.y = Mathf.Abs(Physics2D.gravity.y) / 2 * time;


				rigidbody2D.velocity = launchVelocity;

				attackAnimationState = AttackAnimationState.InProgress;
				GetComponent<Animator>().SetBool("IsAttacking", false);
				
				break;
			case AttackAnimationState.InProgress:
				if(transform.position.x < initialPosition.x + 0.2f) {
					rigidbody2D.velocity = Vector2.zero;
					currentAnimation = AnimationSequence.None;
					attackAnimationState = AttackAnimationState.Off;
					AnimationInProgress = false;

					// reset drag to initial value
					rigidbody2D.drag = initialDrag;
				}
				break;
			}
		} else if(currentAnimation == AnimationSequence.WalkForward) {
			switch(attackAnimationState) {
				case AttackAnimationState.NeedsToStart:

				GetComponent<Animator>().SetFloat("Speed", 10);

				GetComponent<ConstantVelocity>().enabled = true;
				GetComponent<ConstantVelocity>().velocity =  Vector2.right * 3;

				attackAnimationState = AttackAnimationState.InProgress;
				break;

				case AttackAnimationState.InProgress:
				if(transform.position.x > currentAttackTarget.transform.position.x - 1f) {
					GetComponent<ConstantVelocity>().enabled = false;
					GetComponent<Animator>().SetFloat("Speed", 0);

					attackAnimationState = AttackAnimationState.Complete;
				}
				break;
				case AttackAnimationState.Complete:


				break;
			}
		}
	}

	public override void Attack(PlayerAttack attack, BattleCombatant target) {
		currentAttack = attack;
		currentAttackTarget = target;

		if(attack.Name == "Fried Chicken Smoothie") {
			anim.Play("Jumping");
			playSound(healSound);
			currentAttackTarget.Heal(attack.BasePower);
		} else if(attack.Name == "All-Purpose Slice") {
			initialPosition = transform.position;
			AnimationInProgress = true;
			currentAnimation = AnimationSequence.JumpForward;
			attackAnimationState = AttackAnimationState.NeedsToStart;
		} else if(attack.Name == "Sales Pitch") {
			initialPosition = transform.position;
			AnimationInProgress = true;
			currentAnimation = AnimationSequence.WalkForward;
			attackAnimationState = AttackAnimationState.NeedsToStart;
			//something
		}

	}
	
}
