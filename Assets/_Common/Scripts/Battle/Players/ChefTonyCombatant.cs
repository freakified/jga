using UnityEngine;
using System.Collections;

public class ChefTonyCombatant : PlayerCombatant {

	public bool EnableSleepAttack = true;

	public AudioClip knifeSlash;
	public AudioClip healSound;

	public ParticleSystem SalesPitchParticlePrefab;
	private ParticleSystem salesPitchParticles;

	private PlayerAttack currentAttack;
	private BattleCombatant currentAttackTarget;

	private enum AnimationSequence { None, JumpForward, JumpBackward, SalesPitch }

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
		attack1.Power = 76;
		attack1.Accuracy = 100;
		attack1.Type = AttackType.Damage;
		Attacks.Add (attack1);

		if (EnableSleepAttack) {
			PlayerAttack attack2 = new PlayerAttack ();
			attack2.Name = "Sales Pitch";
			attack2.Description = "Puts target to sleep with a lecture on the " +
					"benefits of the Miracle Blade™ III Perfection Series™.";
			attack2.Power = 4;
			attack2.Accuracy = 100;
			attack2.Type = AttackType.Sleep;
			Attacks.Add (attack2);
		}

		PlayerAttack attack3 = new PlayerAttack();
		attack3.Name = "Fried Chicken Smoothie";
		attack3.Description = "90% chicken and 10% lettuce, blended to perfection using the Ultimate Chopper™. Restores health.";
		attack3.Power = 100;
		attack3.Accuracy = 100;
		attack3.Type = AttackType.Heal;


		Attacks.Add (attack3);

		salesPitchParticles = Instantiate(SalesPitchParticlePrefab) as ParticleSystem;
		salesPitchParticles.transform.position = transform.position;
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();
	
	}

	void FixedUpdate () {
		if(currentAnimation == AnimationSequence.JumpForward) {
			animJumpForward();
		} else if(currentAnimation == AnimationSequence.JumpBackward) {
			animJumpBackward();
		} else if(currentAnimation == AnimationSequence.SalesPitch) {
			animSalesPitch();
		}
	}

	private void animJumpForward () {
		switch (attackAnimationState) {
		case AttackAnimationState.NeedsToStart:
			Vector2 launchVelocity = new Vector2 (7f, 0f);

			setDragEnabled(false);

			//calculate the needed initial vertical speed to reach the target
			float dist = currentAttackTarget.transform.position.x - transform.position.x - 0.3f;
			float time = dist / launchVelocity.x;
			launchVelocity.y = Mathf.Abs (Physics2D.gravity.y) / 2 * time;

			// cheap hack to hit floating bosses
			if(currentAttackTarget.transform.position.y > transform.position.y) {
				launchVelocity.y += 4f;
			}

			rigidbody2D.velocity = launchVelocity;
			attackAnimationState = AttackAnimationState.InProgress;
			GetComponent<Animator> ().SetBool ("IsAttacking", true);
			break;
		case AttackAnimationState.InProgress:
			if (transform.position.x > currentAttackTarget.transform.position.x - 1f) {
				playSound (knifeSlash);
				currentAttackTarget.Damage (currentAttack.Power);
				attackAnimationState = AttackAnimationState.Complete;
			}
			break;
		case AttackAnimationState.Complete:
			if (rigidbody2D.velocity == Vector2.zero) {
				currentAnimation = AnimationSequence.JumpBackward;
				attackAnimationState = AttackAnimationState.NeedsToStart;
			}
			break;
		}
	}

	private void setDragEnabled(bool enabled) {
		if(!enabled) {
			// save chef tony's initial linear drag stat
			initialDrag = rigidbody2D.drag;
			
			// set drag to 0
			rigidbody2D.drag = 0;
		} else {
			rigidbody2D.drag = initialDrag;
		}

	}

	private void animJumpBackward() {
		switch(attackAnimationState) {
		case AttackAnimationState.NeedsToStart:
			Vector2 launchVelocity = new Vector2(-7f, 0f);

			setDragEnabled(false);
			
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
				setDragEnabled(true);
			}
			break;
		}
	}

	private void animSalesPitch() {
		switch(attackAnimationState) {
		case AttackAnimationState.NeedsToStart:
			
			GetComponent<Animator>().SetFloat("Speed", 10);
			
			GetComponent<ConstantVelocity>().enabled = true;
			GetComponent<ConstantVelocity>().velocity =  Vector2.right * 3;
			
			attackAnimationState = AttackAnimationState.InProgress;
			break;
			
		case AttackAnimationState.InProgress:
			if(transform.position.x > currentAttackTarget.transform.position.x - 0.5f) {
				GetComponent<ConstantVelocity>().enabled = false;
				GetComponent<Animator>().SetFloat("Speed", 0);

				attackAnimationState = AttackAnimationState.Complete;
				salesPitchParticles.transform.position = transform.position;
				salesPitchParticles.time = 0;
				salesPitchParticles.Play();
				startTimer();
			}
			break;
		case AttackAnimationState.Complete:
			if (timerIsGreaterThan(1.0f)) {
				currentAnimation = AnimationSequence.JumpBackward;
				attackAnimationState = AttackAnimationState.NeedsToStart;

				currentAttackTarget.PutToSleep (currentAttack.Power);
			}
			
			break;
		}
	}

	public override void Attack(PlayerAttack attack, BattleCombatant target) {
		currentAttack = attack;
		currentAttackTarget = target;

		if(attack.Name == "Fried Chicken Smoothie") {
			anim.Play("Jumping");
			playSound(healSound);
			currentAttackTarget.Heal();
		} else if(attack.Name == "All-Purpose Slice") {
			initialPosition = transform.position;
			AnimationInProgress = true;
			currentAnimation = AnimationSequence.JumpForward;
			attackAnimationState = AttackAnimationState.NeedsToStart;
		} else if(attack.Name == "Sales Pitch") {
			initialPosition = transform.position;
			AnimationInProgress = true;
			currentAnimation = AnimationSequence.SalesPitch;
			attackAnimationState = AttackAnimationState.NeedsToStart;
		}

	}
	
}
