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

	public AudioClip CursorMoveSound;

	public List<BattleCombatant> PlayerCombatants;
	public List<BattleCombatant> EnemyCombatants;

	public bool EnabledAtStart = true;
	

	[HideInInspector]
	public delegate void BattleEventHandler(BattleEvent eventType);
	[HideInInspector]
	public static event BattleEventHandler OnBattleEvent;

	
	private int totalCombatants;



	// battle state globals
	private bool battleEnabled = false;
	private int currentTurn = 0;
	private BattleTurnState turnState;

	// target selection globals
	private PlayerAttack selectedAttack;
	private BattleCombatant selectedTarget;

	// keyboard control globals
	private int numberOfButtonsVisible = 0;
	private int currentButtonSelection = 0;
	private bool dirKeyDown = false;

	// Use this for initialization
	void Start () {

		if(EnabledAtStart)
			StartBattle();

	}

	public void StartBattle() {
		totalCombatants = PlayerCombatants.Count + EnemyCombatants.Count;
		currentTurn = 0;
		turnState = BattleTurnState.Attacking;
		battleEnabled = true;

		// ignore collision with other battlers
		Physics2D.IgnoreLayerCollision(10, 10, true); 

		//notify any listeners that the battle started
		if(OnBattleEvent != null) {
			OnBattleEvent(BattleEvent.Started);
		}

	}

	public void PauseBattle() {
		battleEnabled = false;

		Physics2D.IgnoreLayerCollision(10, 10, false); 
	}

	public void ResumeBattle() {
		battleEnabled = true;
		Physics2D.IgnoreLayerCollision(10, 10, true); 
	}

	void OnGUI () {
		// if the battle has started...
		if(battleEnabled) {

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

					checkKeyControlFocus();
					
					break;
				case BattleTurnState.Targeting:
					selectedTarget = getSelectedTarget(selectedAttack);

					if(selectedTarget != null) {
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

		// if the mouse is over a button, select it:
		for (int i = 0; i < numAttacks; i++) {
			if (Event.current.type == EventType.Repaint && 
			    (attackButtons [i].Contains (Event.current.mousePosition))) {
				currentButtonSelection = i;
			}
		}


		// now draw the attack description tooltip
		for (int i = 0; i < numAttacks; i++) {
			if (Event.current.type == EventType.Repaint && (currentButtonSelection == i)) {
				GUI.Label (new Rect (scalePx (220), 
				                     attackButtons[i].y - scalePx(15), 
				                     scalePx (315), 
				                     scalePx (55)), 
				           ((PlayerCombatant)PlayerCombatants[currentTurn]).Attacks[i].Description, 
				           guiSkin.customStyles [2]);

			}
		}

		// unity doesn't count gamepad presses as "clicks", so we need to fake it:
		// TODO: check if this works with actual gamepads
		if(Event.current.type == EventType.KeyDown && Input.GetButtonDown("Select")) {
			chosenAttack =
				((PlayerCombatant)PlayerCombatants [currentTurn]).Attacks [currentButtonSelection];
		}

		return chosenAttack;
	}

	private BattleCombatant getSelectedTarget (PlayerAttack chosenAttack) {
		BattleCombatant chosenTarget = null;
		bool targetingCancelled = false;

		List<BattleCombatant> availableTargets;
		
		if (chosenAttack.Type == AttackType.Heal)
			// if it's a healing move, then we show the list of allies
			availableTargets = PlayerCombatants.FindAll((BattleCombatant c) => c.participatingInBattle);
		else
			// if it's an attack/status move, show the list of enemies
			availableTargets = EnemyCombatants.FindAll((BattleCombatant c) => c.participatingInBattle);

		int areaHeight = scalePx (60 + 30 * availableTargets.Count);

		GUILayout.BeginArea (new Rect (0, 0, scalePx (270), areaHeight), guiSkin.customStyles [0]);
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("<b>" + chosenAttack.Name + "</b>");

		GUI.SetNextControlName ("0");
		numberOfButtonsVisible = availableTargets.Count + 1;

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
			GUI.SetNextControlName ((i+1).ToString());
			availableTarget = availableTargets [i];
			percentHP = (int)Mathf.Round (availableTarget.HitPoints / (float)availableTarget.MaxHitPoints * 100);

			bool isTargetable = true;

			if(availableTarget.HitPoints == 0 ||
			   availableTarget.isShielded ||
			   (chosenAttack.Type == AttackType.Damage && availableTarget.immuneToDamage)) {
				isTargetable = false;
			}

			// grey out the button if the target is already dead
			if (!isTargetable)
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
			if (!isTargetable)
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

	/// <summary>
	/// Checks for the keys controlling the focus (input up/down)
	/// </summary>
	private void checkKeyControlFocus() {
		float v = Input.GetAxis("Vertical");

		if(!dirKeyDown) { 
			if(v != 0) {
				if(v < 0) {
					currentButtonSelection++;
				} else {
					currentButtonSelection--;
				}

				if(currentButtonSelection < numberOfButtonsVisible && currentButtonSelection >= 0) {
					AudioSource.PlayClipAtPoint(CursorMoveSound, Camera.main.transform.position);
				} else {
					currentButtonSelection = Mathf.Clamp(currentButtonSelection, 0, numberOfButtonsVisible - 1);
				}

				dirKeyDown = true;
			}
		} else {
			if(v == 0) {
				dirKeyDown = false;
			}
		}

		GUI.FocusControl(currentButtonSelection.ToString());

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

	private void playersDefeated() {
		// TODO go to game over screen or lose lives or something
	}

	private void enemiesDefeated() {
		battleEnabled = false;

		// notify registered listeners
		if(OnBattleEvent != null) {
			OnBattleEvent(BattleEvent.Finished);
		}

		Physics2D.IgnoreLayerCollision(10, 10, false); 
	}
	
}
