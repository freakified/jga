using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BattleController : BaseGUI {

	public List<BattleCombatant> PlayerCombatants;
	public List<BattleCombatant> EnemyCombatants;

	public bool EnabledAtStart = true;
	public float DelayBeforeAutoStart = 0.5f;
	public bool EnemiesGoFirst = false;

	[HideInInspector]
	public delegate void BattleEventHandler(BattleEvent eventType);
	[HideInInspector]
	public static event BattleEventHandler OnBattleEvent;
	
	private int totalCombatants;

	// battle state globals
	public bool battleEnabled = false;
	public int currentTurn { get; private set; }
	private BattleTurnState turnState;

	// target selection globals
	private PlayerAttack selectedAttack;
	private BattleCombatant selectedTarget;

	protected bool buttonKeyDown = false;

	// Use this for initialization
	public override void Start () {
		base.Start();

		if(EnabledAtStart)
			Invoke("StartBattle", DelayBeforeAutoStart);

	}

	public void StartBattle() {
		totalCombatants = PlayerCombatants.Count + EnemyCombatants.Count;

		turnState = BattleTurnState.Attacking;
		battleEnabled = true;
		enableGuiControl();

		// ignore collision with other battlers
		Physics2D.IgnoreLayerCollision(10, 10, true); 

		// let the enemies go first, if necessary

		if(!EnemiesGoFirst) {
			currentTurn = 0;
		} else {
			currentTurn = PlayerCombatants.Count;
		}

		// reset input delay counter
		elapsedTime = 0;

		//notify any listeners that the battle started
		if(OnBattleEvent != null) {
			OnBattleEvent(BattleEvent.Started);
		}

	}

	public void PauseBattle() {
		battleEnabled = false;
		disableGuiControl();

		Physics2D.IgnoreLayerCollision(10, 10, false); 
	}

	public void ResumeBattle() {
		battleEnabled = true;
		enableGuiControl();

		Physics2D.IgnoreLayerCollision(10, 10, true); 
	}

	public override void OnGUI () {
		base.OnGUI();

		// if the battle has started...
		if(battleEnabled) {


			// enforce 16:9 aspect ratio
			GUILayout.BeginArea(AspectUtility.screenRect);
			
			// draw the player combatants' data
			drawPlayerInfo();

			if(isPlayerTurn()) {
				PlayerCombatant currentPlayer = (PlayerCombatant)PlayerCombatants[currentTurn];

				switch(turnState) {
				case BattleTurnState.Attacking: 	
					selectedAttack = getSelectedAttack();

					if(selectedAttack != null) {
						currentButtonSelection = 0;
						turnState = BattleTurnState.Targeting;
					}

					checkKeyControlFocus();
					
					break;
				case BattleTurnState.Targeting:
					selectedTarget = getSelectedTarget(selectedAttack);

					if(selectedTarget != null) {
						currentButtonSelection = 0;
						currentPlayer.Attack (selectedAttack, selectedTarget);
						currentPlayer.IncrementTurnCounter();

						turnState = BattleTurnState.WaitingForAnimation;
					}

					checkKeyControlFocus();
					
					break;
				case BattleTurnState.WaitingForAnimation:

					// has the player's animation finished?
					if(!PlayerCombatants[currentTurn].AnimationInProgress) {
						turnState = BattleTurnState.TurnComplete;
					}

					break;
				}

			} else { // otherwise it is an enemy's turn
				EnemyCombatant currentEnemy =
					(EnemyCombatant)EnemyCombatants[currentTurn - PlayerCombatants.Count];

				switch(turnState) {
				case BattleTurnState.Attacking:
					if(!currentEnemy.isSleeping) {
						currentEnemy.AutoAttack(PlayerCombatants);
					}

					currentEnemy.IncrementTurnCounter();
					turnState = BattleTurnState.WaitingForAnimation;
					
					break;
				case BattleTurnState.WaitingForAnimation:
					// if the enemy attack has finished...
					if(Event.current.type == EventType.Repaint && !currentEnemy.AnimationInProgress) {
						turnState = BattleTurnState.TurnComplete;
					}
					
					break;
				}
			}

			if(turnState == BattleTurnState.TurnComplete) {

				// if the turn has completed, check if anyone won
				checkForVictory();

				// reset turn state
				turnState = BattleTurnState.Attacking;

				//...and then increment the turn.
				incrementTurn();

				// notify registered listeners of the turn change
				// note: this is not inside incrementTurn due to recursion
				if(OnBattleEvent != null) {
					OnBattleEvent(BattleEvent.TurnChange);
				}

			}

			GUILayout.EndArea();
		}
	}

	private bool isPlayerTurn() {
		return currentTurn < PlayerCombatants.Count;
	}

/// <summary>
/// Draws the attack selection window. Should only be called within the OnGUI function
/// </summary>
/// <returns>The selected attack. If no attack has been chosen, returns null.</returns>
	private PlayerAttack getSelectedAttack() {
		PlayerAttack chosenAttack = null;

		int numAttacks = ((PlayerCombatant)PlayerCombatants [currentTurn]).Attacks.Count;
		int areaHeight;

		Rect[] attackButtons = new Rect[numAttacks];

		// draw the attack selection box
		areaHeight = scalePx (55 + 30 * numAttacks);
		GUILayout.BeginArea (new Rect (0, 0, scalePx (210), areaHeight), guiSkin.customStyles [0]);
		GUILayout.Label ("ATTACK", guiSkin.customStyles [3]);
		PlayerAttack attack;

		// display the attacks for the selected player
		for (int i = 0; i < numAttacks - 1; i++) {
			attack = ((PlayerCombatant)PlayerCombatants [currentTurn]).Attacks [i];
			GUI.SetNextControlName (i.ToString());
			if (GUILayout.Button (attack.Name)) {
				chosenAttack = attack;
			}
			attackButtons [i] = GUILayoutUtility.GetLastRect ();
		}
		
		// for now, assume that the last move is the healing move
		// (this can be generalized later, if necessary)
		GUILayout.Label ("HEAL", guiSkin.customStyles [3]);

		GUI.SetNextControlName ((numAttacks - 1).ToString());

		numberOfButtonsVisible = numAttacks;

		attack = ((PlayerCombatant)PlayerCombatants [currentTurn]).Attacks [numAttacks - 1];

		if (GUILayout.Button (attack.Name)) {
			chosenAttack = attack;
		}

		attackButtons [numAttacks - 1] = GUILayoutUtility.GetLastRect ();
		GUILayout.EndArea();

		if(Event.current.keyCode == KeyCode.Space) {
			buttonKeyDown = true;
		} else if(input1IsDown && buttonKeyDown == false) {
			chosenAttack =
				((PlayerCombatant)PlayerCombatants [currentTurn]).Attacks [currentButtonSelection];
			currentButtonSelection = 0;
			buttonKeyDown = true;
		} else {
			buttonKeyDown = false;
		}

		return chosenAttack;
	}

	private BattleCombatant getSelectedTarget (PlayerAttack chosenAttack) {
		BattleCombatant chosenTarget = null;
		bool targetingCancelled = false;

		List<BattleCombatant> availableTargets;

		//used for the keyboard controls
		List<BattleCombatant> attackableTargets = new List<BattleCombatant>();
		
		if (chosenAttack.Type == AttackType.Heal) {
			// if it's a healing move, then target the current combatant
			// but also offer revivals to dead combatants
			availableTargets = new List<BattleCombatant>();
			availableTargets.Add((BattleCombatant)PlayerCombatants[currentTurn]);
			availableTargets.AddRange(PlayerCombatants.FindAll((BattleCombatant c) => c.HitPoints == 0));
			
		} else {
			// if it's an attack/status move, show the list of enemies
			availableTargets = EnemyCombatants.FindAll((BattleCombatant c) => c.participatingInBattle);
		}

		int areaHeight = scalePx (120 + 30 * availableTargets.Count);

		GUILayout.BeginArea (new Rect (0, 0, scalePx (330), areaHeight), guiSkin.customStyles [0]);
		GUILayout.Label ("<b>" + chosenAttack.Name + "</b>");

		GUILayout.Label (chosenAttack.Description, guiSkin.customStyles[2]);

		GUILayout.Label ("SELECT TARGET", guiSkin.customStyles [3]);
		BattleCombatant availableTarget;
		string status;

		int currentButtonNum = 0;

		for (int i = 0; i < availableTargets.Count; i++) {

			availableTarget = availableTargets [i];
			status = ((int)Mathf.Round (availableTarget.HitPoints / (float)availableTarget.MaxHitPoints * 100)) + "%";

			bool isTargetable = true;

			if(availableTarget.HitPoints == 0 ||
			   availableTarget.isShielded ||
			   (chosenAttack.Type == AttackType.Damage && availableTarget.immuneToDamage) ||
			   (chosenAttack.Type == AttackType.Heal && availableTarget.HitPoints == availableTarget.MaxHitPoints)) {
				isTargetable = false;

				if(availableTarget.isShielded) {
					status = "Shielded";
				} else if(availableTarget.immuneToDamage) {
					status = "Immune";
				} else if(availableTarget.HitPoints == 0 && chosenAttack.Type == AttackType.Heal) {
					status = "Revive";
					isTargetable = true;
				}
			}

			if(availableTarget.isSleeping) {
				status += ", Sleeping";
			}

			if(isTargetable) {
				attackableTargets.Add(availableTarget);
			}

			// grey out the button if the target is already dead
			if (!isTargetable) {
				GUI.enabled = false;
			} else {
				GUI.SetNextControlName (currentButtonNum.ToString());
				currentButtonNum++;
			}



			if (GUILayout.Button ("<b>" + availableTarget.getName() + "</b> (" + status + ")")) {

				chosenTarget = availableTarget;
			}

			// now highlight the selected target
			foreach(BattleCombatant t in availableTargets) {
				SpriteRenderer r = t.GetComponent<SpriteRenderer>();

				if(r != null) {
					r.color = new Color (1, 1, 1, 0.5f);
				}
			}

			if (currentButtonSelection < attackableTargets.Count) {
				SpriteRenderer r = attackableTargets[currentButtonSelection].GetComponent<SpriteRenderer>();

				if(r != null) {
					r.color = new Color (1, 1, 1, 1f);
				}
			}

			// if the target is dead, undo the greyout state we enabled above
			if (!isTargetable)
				GUI.enabled = true;
		}

		GUI.SetNextControlName (currentButtonNum.ToString());

		if (GUILayout.Button ("Cancel", GUILayout.ExpandWidth (false))) {
			targetingCancelled = true;
			
			//set the turn state back to attacking, which will take effect on the next loop
			turnState = BattleTurnState.Attacking;
			currentButtonSelection = 0;
		}

		numberOfButtonsVisible = currentButtonNum + 1;

		GUILayout.EndArea();

		// unity doesn't count gamepad presses as "clicks", so we need to fake it:
		if(Event.current.keyCode == KeyCode.Space) {
			buttonKeyDown = true;
		} else if(input1IsDown && buttonKeyDown == false ) {
			if(currentButtonSelection == attackableTargets.Count) {
				// if the cancel button is selected
				targetingCancelled = true;
				turnState = BattleTurnState.Attacking;
				
			} else {
				chosenTarget = attackableTargets[currentButtonSelection];
			}
			
			buttonKeyDown = true;
			currentButtonSelection = 0;
		} else {
			buttonKeyDown = false;
		}

		//now check for the escape key
		if(input2IsDown) {
			targetingCancelled = true;
			turnState = BattleTurnState.Attacking;
			currentButtonSelection = 0;
		}

		if(chosenTarget != null || targetingCancelled) {
			foreach(BattleCombatant t in availableTargets) {
				SpriteRenderer r = t.GetComponent<SpriteRenderer>();
				
				if(r != null) {
					r.color = new Color (1, 1, 1, 1f);
				}
			}
		}

		return chosenTarget;
	}

	/// <summary>
	/// Draws the player characters' names and HP to the bottom left
	/// </summary>
	private void drawPlayerInfo () {
		int areaHeight = scalePx (30 * PlayerCombatants.Count + 10);
		GUILayout.BeginArea (new Rect (0, AspectUtility.screenRect.height - areaHeight, scalePx (180), areaHeight), guiSkin.customStyles [0]);

		for (int i = 0; i < PlayerCombatants.Count; i++) {
			GUILayout.BeginHorizontal ();
			string startTag = "<b>";
			string endTag = "</b>";
			string hpStartTag = "";
			string hpEndTag = "";

			if (i != currentTurn) {
				startTag = "<color=#ffffff55><b>";
				endTag = "</b></color>";
			}

			if(PlayerCombatants[i].HitPoints / (float)PlayerCombatants[i].MaxHitPoints < 0.25f) {
				hpStartTag = "<color=#ff0000>";
				hpEndTag = "</color>";
			}

			GUILayout.Label (startTag + PlayerCombatants [i].getName() + endTag);
			GUILayout.Label (hpStartTag + PlayerCombatants [i].HitPoints + "/" + PlayerCombatants [i].MaxHitPoints + hpEndTag, guiSkin.customStyles [1], GUILayout.Width (scalePx (75)));
			GUILayout.EndHorizontal ();
		}

		GUILayout.EndArea ();
	}
	
	private void checkForVictory () {
		if (PlayerCombatants.Find ((BattleCombatant c) => c.HitPoints > 0 && c.participatingInBattle) == null) {
			//this means the players are all dead and/or not participating
			playersDefeated ();
		} else if (EnemyCombatants.Find ((BattleCombatant c) => c.HitPoints > 0 && c.participatingInBattle) == null) {
			// this means the enemies are all dead and/or not participating
			enemiesDefeated ();
		}
	}

	private void incrementTurn() {
		//increase to the next sequential turn number
		currentTurn++;
		currentTurn %= totalCombatants;

		//if the player at that turn is dead, skip their turn
		if(currentTurn < PlayerCombatants.Count) {
			if(PlayerCombatants[currentTurn].HitPoints == 0 || 
			   !PlayerCombatants[currentTurn].participatingInBattle) {
				incrementTurn();
			}
		} else {
			if(EnemyCombatants[currentTurn - PlayerCombatants.Count].HitPoints == 0 ||
			   !EnemyCombatants[currentTurn - PlayerCombatants.Count].participatingInBattle) {
				incrementTurn();
			}
		}
	}

	private bool gameOverInProgress = false;

	private void playersDefeated() {
		if(!gameOverInProgress) {
			Camera.main.GetComponent<CameraFade>().FadeAndNext(Color.black, 5, "x-01 Game Over", true);
			gameOverInProgress = true;
		}
	}

	private void enemiesDefeated() {
		battleEnabled = false;
		disableGuiControl();

		// notify registered listeners
		if(OnBattleEvent != null) {
			OnBattleEvent(BattleEvent.Finished);
		}

		// if the enemies are defeated, bring everyone back to life:
		PlayerCombatants.ForEach (c => c.Heal(c.MaxHitPoints));

		// don't re-enable battler collision to avoid collisions with corpses
		//Physics2D.IgnoreLayerCollision(10, 10, false); 
	}

}
