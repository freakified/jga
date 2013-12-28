using UnityEngine;
using System.Collections;
using System.Collections.Generic;

abstract public class EnemyCombatant : BattleCombatant {
	
	public List<PlayerAttack> Attacks { get; protected set; }

	protected BattleCombatant target;

	public override void Start () {
		base.Start ();

		Attacks = new List<PlayerAttack>();
	}

	/// <summary>
	/// Automatically selects a target and attacks it
	/// </summary>
	abstract public void AutoAttack (List<BattleCombatant> targetList);
}
