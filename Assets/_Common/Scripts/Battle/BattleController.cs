using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleController : MonoBehaviour {
	public GUISkin guiSkin;

	/// <summary>
	/// The width of the screen at the desired resolution.
	/// Used to scale the GUI
	/// </summary>
	public int TargetScreenWidth = 640;

	public List<BattleCombatant> PlayerCombatants;
	public List<BattleCombatant> EnemyCombatants;

	public bool EnabledAtStart = true;

	[HideInInspector]
	public delegate void BattleEventHandler(BattleEvent eventType);
	[HideInInspector]
	public static event BattleEventHandler OnBattleEvent;
	
	private int totalCombatants;

	// battle state globals
	private bool battleStarted = false;
	private int currentTurn = 0;
	private BattleTurnState turnState;

	// target selection globals
	private PlayerAttack selectedAttack;
	private BattleCombatant selectedTarget;


	// Use this for initialization
	void Start () {

		if(EnabledAtStart)
			StartBattle();

	}

	public void StartBattle() {
		totalCombatants = PlayerCombatants.Count + EnemyCombatants.Count;
		currentTurn = 0;
		turnState = BattleTurnState.Attacking;
		battleStarted = true;

		//notify any listeners that the battle started
		OnBattleEvent(BattleEvent.Started);

	}

	void OnGUI () {
		// if the battle has started...
		if(battleStarted) {

			//set theme and scale gui to match resolution
			GUI.skin = guiSkin;
			scaleGUI(guiSkin);
			
			// draw the player combatants' data
			drawPlayerInfo();

			if(isPlayerTurn()) {
				PlayerCombatant currentPlayer = (PlayerCombatant)PlayerCombatants[currentTurn];

				switch(turnState) {
				case BattleTurnState.Attacking: 	
					selectedAttack = getSelectedAttack();

					if(selectedAttack != null) {
						turnState = BattleTurnState.Targeting;
					}
					
					break;
				case BattleTurnState.Targeting:
					selectedTarget = getSelectedTarget(selectedAttack);

					if(selectedTarget != null) {
						print (selectedTarget);
						currentPlayer.Attack (selectedAttack, selectedTarget);

						turnState = BattleTurnState.WaitingForAnimation;
					}
					
					break;
				case BattleTurnState.WaitingForAnimation:

					//TODO figure out why this is needed
					//if(Event.current.type == EventType.Repaint &&

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
					currentEnemy.AutoAttack(PlayerCombatants);
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

			}
		}
	}

	private bool isPlayerTurn() {
		return currentTurn < PlayerCombatants.Count;
	}

	private void scaleGUI(GUISkin guiSkin) {
		//fonts
		guiSkin.label.fontSize = scalePx (16);
		guiSkin.customStyles[1].fontSize = scalePx (16);
		guiSkin.customStyles[2].fontSize = scalePx (14);
		guiSkin.customStyles[3].fontSize = scalePx (12);
		guiSkin.button.fontSize = scalePx (16);
		
		//padding for label styles
		guiSkin.label.padding.top = scalePx (5);
		guiSkin.customStyles[1].padding.top = scalePx (5);
		guiSkin.customStyles[0].padding.top = scalePx (5);
		guiSkin.customStyles[0].padding.left = scalePx (10);
		guiSkin.customStyles[0].padding.right = scalePx (10);

		guiSkin.customStyles[2].padding.left = scalePx (10);
		guiSkin.customStyles[2].padding.top = scalePx (10);
		
		//padding for buttons
		guiSkin.button.margin.left = scalePx (20);
		guiSkin.button.margin.top = scalePx (4);
		guiSkin.button.padding.left = scalePx (10);
		guiSkin.button.padding.top = scalePx (3);
		guiSkin.button.padding.bottom = scalePx (3);
	}

	private int scalePx(int targetSize) {
		return (int)((targetSize * Screen.width) / TargetScreenWidth);
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
		areaHeight = scalePx (50 + 30 * numAttacks);
		GUILayout.BeginArea (new Rect (0, 0, scalePx (210), areaHeight), guiSkin.customStyles [0]);
		GUILayout.Label ("ATTACK", guiSkin.customStyles [3]);
		PlayerAttack attack;

		// display the attacks for the selected player
		for (int i = 0; i < numAttacks - 1; i++) {
			attack = ((PlayerCombatant)PlayerCombatants [currentTurn]).Attacks [i];
			if (GUILayout.Button (attack.Name)) {
				chosenAttack = attack;
			}
			attackButtons [i] = GUILayoutUtility.GetLastRect ();
		}

		// for now, assume that the last move is the healing move
		// (this can be generalized later, if necessary)
		GUILayout.Label ("HEAL", guiSkin.customStyles [3]);
		attack = ((PlayerCombatant)PlayerCombatants [currentTurn]).Attacks [numAttacks - 1];

		if (GUILayout.Button (attack.Name)) {
			chosenAttack = attack;
		}

		attackButtons [numAttacks - 1] = GUILayoutUtility.GetLastRect ();
		GUILayout.EndArea();

		// now draw the attack description tooltip
		for (int i = 0; i < numAttacks; i++) {
			if (Event.current.type == EventType.Repaint && attackButtons [i].Contains (Event.current.mousePosition)) {
				GUI.Label (new Rect (scalePx (220), Event.current.mousePosition.y - scalePx (30), scalePx (315), scalePx (55)), ((PlayerCombatant)PlayerCombatants [currentTurn]).Attacks [i].Description, guiSkin.customStyles [2]);
			}
		}

		return chosenAttack;
	}

	private BattleCombatant getSelectedTarget (PlayerAttack chosenAttack) {
		BattleCombatant chosenTarget = null;
		bool targetingCancelled = false;

		List<BattleCombatant> availableTargets;
		
		if (chosenAttack.Type == AttackType.Heal)
			// if it's a healing move, then we show the list of allies, but
			availableTargets = PlayerCombatants;
		else
			// if it's an attack move, show the list of enemies
			availableTargets = EnemyCombatants;

		int areaHeight = scalePx (60 + 30 * availableTargets.Count);

		GUILayout.BeginArea (new Rect (0, 0, scalePx (270), areaHeight), guiSkin.customStyles [0]);
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("<b>" + chosenAttack.Name + "</b>");

		if (GUILayout.Button ("Cancel", GUILayout.ExpandWidth (false))) {
			targetingCancelled = true;

			//set the turn state back to attacking, which will take effect on the next loop
			turnState = BattleTurnState.Attacking;
		}

		GUILayout.EndHorizontal();

		GUILayout.Label ("SELECT TARGET", guiSkin.customStyles [3]);
		BattleCombatant availableTarget;
		int percentHP;

		for (int i = 0; i < availableTargets.Count; i++) {
			availableTarget = availableTargets [i];
			percentHP = (int)Mathf.Round (availableTarget.HitPoints / (float)availableTarget.MaxHitPoints * 100);

			// grey out the button if the target is already dead
			if (availableTarget.HitPoints == 0)
				GUI.enabled = false;

			if (GUILayout.Button ("<b>" + availableTarget.name + "</b> (" + percentHP + "%)")) {

				chosenTarget = availableTarget;
			}

			if (Event.current.type == EventType.Repaint &&
			    GUILayoutUtility.GetLastRect ().Contains (Event.current.mousePosition)) {
				((SpriteRenderer)availableTarget.renderer).color = new Color (1, 1, 1, 1);
			}
			else {
				((SpriteRenderer)availableTarget.renderer).color = new Color (1, 1, 1, 0.5f);
			}

			// if the target is dead, undo the greyout state we enabled above
			if (availableTarget.HitPoints == 0)
				GUI.enabled = true;
		}

		GUILayout.EndArea();

		if(chosenTarget != null || targetingCancelled) {
			// if a target was selected or the cancel button was pressed, restore opacity of targets
			foreach (BattleCombatant c in availableTargets) {
				((SpriteRenderer)c.renderer).color = new Color (1, 1, 1, 1);
			}
		}

		return chosenTarget;
	}

	/// <summary>
	/// Draws the player characters' names and HP to the bottom left
	/// </summary>
	private void drawPlayerInfo () {
		int areaHeight = scalePx (30 * PlayerCombatants.Count + 10);
		GUILayout.BeginArea (new Rect (0, Screen.height - areaHeight, scalePx (180), areaHeight), guiSkin.customStyles [0]);

		for (int i = 0; i < PlayerCombatants.Count; i++) {
			GUILayout.BeginHorizontal ();
			string startTag = "<b>";
			string endTag = "</b>";

			if (i != currentTurn) {
				startTag = "<color=#ffffff55><b>";
				endTag = "</b></color>";
			}

			GUILayout.Label (startTag + PlayerCombatants [i].name + endTag);
			GUILayout.Label (PlayerCombatants [i].HitPoints + "/" + PlayerCombatants [i].MaxHitPoints, guiSkin.customStyles [1], GUILayout.Width (scalePx (75)));
			GUILayout.EndHorizontal ();
		}

		GUILayout.EndArea ();
	}

	private void checkForVictory () {
		if (PlayerCombatants.Find ((BattleCombatant c) => c.HitPoints > 0) == null) {
			//this means the players are all dead
			playersDefeated ();
		} else if (EnemyCombatants.Find ((BattleCombatant c) => c.HitPoints > 0) == null) {
			// this means the enemies are all dead
			enemiesDefeated ();
		}
	}

	private void incrementTurn() {
		//increase to the next sequential turn number
		currentTurn++;
		currentTurn %= totalCombatants;

		//if the player at that turn is dead, skip their turn
		if(currentTurn < PlayerCombatants.Count) {
			if(PlayerCombatants[currentTurn].HitPoints == 0) {
				incrementTurn();
			}
		} else {
			if(EnemyCombatants[currentTurn - PlayerCombatants.Count].HitPoints == 0) {
				incrementTurn();
			}
		}

	}

	private void playersDefeated() {
		// TODO go to game over screen or lose lives or something
	}

	private void enemiesDefeated() {
		battleStarted = false;

		// notify registered listeners
		OnBattleEvent(BattleEvent.Finished);
	}
	
}
