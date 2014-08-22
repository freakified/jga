using UnityEngine;
using System.Collections;

public class LikeMikeCombatant : PlayerCombatant {

//	public bool EnableBasicAttack = true;
//	public bool EnableHealAttack = true;
	
	// Use this for initialization
	public override void Start () {
		base.Start ();

		//set up basic stats
		MaxHitPoints = 120;
		HitPoints = 120;

		//set up the list of attacks
		PlayerAttack attack1 = new PlayerAttack();
		attack1.Name = "Slam Dunk";
		attack1.Description = "Scores a totally radical slam dunk, damaging a single target.";
		attack1.Power = 102;
		attack1.Accuracy = 100;
		attack1.Type = AttackType.Damage;

		PlayerAttack attack2 = new PlayerAttack();
		attack2.Name = "Gatorade\u2122 Thirst Quencher";
		attack2.Description = "<i>Real</i> athletes literally sweat this substance.  Restores health.";
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

	public override void Attack(PlayerAttack attack, BattleCombatant target) {
		
	}
}
