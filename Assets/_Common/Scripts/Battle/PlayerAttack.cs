using UnityEngine;
using System.Collections;

/// <summary>
/// Represents an attack that can be performed by a player character.
/// </summary>
public class PlayerAttack {

	public string Name { get; set; }
	public string Description { get; set; }
	public bool IsHealingMove { get; set; }
	public int BasePower { get; set; }
	
//	public PlayerAttack(string name, string description,
//	                    bool isHealingMove, int basePower) {
//		Name = name;
//		Description = description;
//		IsHealingMove = isHealingMove;
//		BasePower = basePower;
//	}
}
