using UnityEngine;
using System.Collections;
using System.Collections.Generic;

abstract public class PlayerCombatant : BattleCombatant {
	
	public List<PlayerAttack> Attacks { get; protected set; }

	public override void Start () {
		base.Start ();

		Attacks = new List<PlayerAttack>();
	}

	/// <summary>
	/// Attack using the specified attack and target.  
	/// Performs any needed animations and sounds, and also
	/// applies damage to target.
	/// </summary>
	/// <param name="attack">The attack to use</param>
	/// <param name="attack">The target</param>
	abstract public void Attack (PlayerAttack attack, BattleCombatant target);
}
