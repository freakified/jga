using UnityEngine;
using System.Collections;

public class LikeMikeCombatant : PlayerCombatant {

//	public bool EnableBasicAttack = true;
//	public bool EnableHealAttack = true;
	
	// Use this for initialization
	public override void Start () {
		base.Start ();

		//set up basic stats
		MaxHitPoints = 123;
		HitPoints = 100;

		//set up the list of attacks
		PlayerAttack attack1 = new PlayerAttack();
		attack1.Name = "Slam Dunk";
		attack1.Description = "Scores a totally radical slam dunk, damaging a single target.";
		attack1.BasePower = 25;
		attack1.IsHealingMove = false;

		PlayerAttack attack2 = new PlayerAttack();
		attack2.Name = "Gatorade™ Thirst Quencher™";
		attack2.Description = "<i>Real</i> athletes literally sweat this substance.  Restores health.";
		attack2.BasePower = 25;
		attack2.IsHealingMove = true;

		Attacks.Add (attack1);
		Attacks.Add (attack2);
	
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();
		
	}

	public override void Attack(PlayerAttack attack, EnemyCombatant target) {
		
	}
}
