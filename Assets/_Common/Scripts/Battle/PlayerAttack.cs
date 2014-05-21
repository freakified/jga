using UnityEngine;
using System.Collections;

/// <summary>
/// Represents an attack that can be performed by a player character.
/// </summary>
public class PlayerAttack {

	public string Name { get; set; }
	public string Description { get; set; }
	public AttackType Type { get; set; }
	public int Power { get; set; }
	public int Accuracy { get; set; }

}
