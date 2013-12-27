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

	public List<PlayerCombatant> PlayerCombatants;
	public List<EnemyCombatant> EnemyCombatants;

	public bool EnabledAtStart = true;
	
	private int totalCombatants;

	private bool BattleStarted = false;
	private int CurrentTurn = 0;
	private bool TargetSelection = false;
	private PlayerAttack SelectedAttack;
	private bool turnNearlyFinished = false;	

	// Use this for initialization
	void Start () {

		if(EnabledAtStart)
			StartBattle();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartBattle() {
		totalCombatants = PlayerCombatants.Count + EnemyCombatants.Count;
		CurrentTurn = 0;
			
		BattleStarted = true;

	}

	void OnGUI () {
		//print (CurrentTurn);
		if(BattleStarted) {
			GUI.skin = guiSkin;

			//scale the sizes of elements to match the actual resolution
			scaleGUI(guiSkin);

			int areaHeight;

			// are we waiting for an animation to finish, and it finished?

			
			// is it the player's turn?
			if(CurrentTurn < PlayerCombatants.Count) {
				if(Event.current.type == EventType.Repaint && turnNearlyFinished &&
				   !PlayerCombatants[CurrentTurn].AnimationInProgress) {
					IncrementTurn();
					turnNearlyFinished = false;
					return;
				}

				int numAttacks = PlayerCombatants[CurrentTurn].Attacks.Count;
				Rect[] attackButtons = new Rect[numAttacks];

				// draw the attack selection box

				if(!TargetSelection) {
					areaHeight = scalePx (50 + 30 * numAttacks);
					GUILayout.BeginArea (new Rect (0, 0, scalePx (210), areaHeight), 
					                     guiSkin.customStyles[0]);
					GUILayout.Label ("ATTACK", guiSkin.customStyles[3]);

					PlayerAttack attack;

					// display the attacks for the selected player
					for(int i = 0; i < numAttacks - 1; i++) {
						attack = PlayerCombatants[CurrentTurn].Attacks[i];

						if(GUILayout.Button(attack.Name)) {
							TargetSelection = true;
							SelectedAttack = attack;
						}

						attackButtons[i] = GUILayoutUtility.GetLastRect();

					}

					// for now, assume that the last move is the healing move
					// (this can be generalized later, if necessary)
					GUILayout.Label ("HEAL", guiSkin.customStyles[3]);

					attack = PlayerCombatants[CurrentTurn].Attacks[numAttacks - 1];

					if(GUILayout.Button(attack.Name)) {
						//TargetSelection = true;
						SelectedAttack = attack;
					}

					attackButtons[numAttacks - 1] = GUILayoutUtility.GetLastRect();

					GUILayout.EndArea();
				}


				// now draw the attack description tooltip

				if(!TargetSelection) {
					for(int i = 0; i < numAttacks; i++) {
						if(Event.current.type == EventType.Repaint && 
						   attackButtons[i].Contains(Event.current.mousePosition )) {
							GUI.Label (new Rect (scalePx (220), Event.current.mousePosition.y - scalePx (30), scalePx (315), scalePx (55)), 
							           PlayerCombatants[CurrentTurn].Attacks[i].Description, 
							           guiSkin.customStyles[2]);
						}
					}
				}

				// target selection box
				if(TargetSelection && !turnNearlyFinished) {
					areaHeight = scalePx (60 + 30 * EnemyCombatants.Count);
					GUILayout.BeginArea (new Rect (0, 0, scalePx (270), areaHeight), 
					                    guiSkin.customStyles[0]);

					GUILayout.BeginHorizontal();
					GUILayout.Label ("<b>" + SelectedAttack.Name + "</b>");
					if(GUILayout.Button("Cancel", GUILayout.ExpandWidth (false))) {
						TargetSelection = false;
					}
					GUILayout.EndHorizontal();

					GUILayout.Label ("SELECT TARGET", guiSkin.customStyles[3]);

					EnemyCombatant enemy;
					int percentHP;
					Rect[] targetButtons = new Rect[EnemyCombatants.Count];

					for(int i = 0; i < EnemyCombatants.Count; i++) {
						enemy = EnemyCombatants[i];
						percentHP = (int)Mathf.Round(enemy.HitPoints / (float)enemy.MaxHitPoints * 100);
						
						if(GUILayout.Button("<b>" + enemy.name + "</b> (" + percentHP + "%)" )) {
							PlayerCombatants[CurrentTurn].Attack(SelectedAttack, enemy);

							//restore opacity of enemies
							foreach(EnemyCombatant e in EnemyCombatants) {
								((SpriteRenderer)e.renderer).color = new Color(1, 1, 1, 1);
							}

							TargetSelection = false;
							turnNearlyFinished = true;
						}

						// add mouseover effect to enemies
						if(!turnNearlyFinished) {
							if(Event.current.type == EventType.Repaint && 
							   GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition )) {
								((SpriteRenderer)enemy.renderer).color = new Color(1, 1, 1, 1);
							} else {
								((SpriteRenderer)enemy.renderer).color = new Color(1, 1, 1, 0.5f);
							}
						}
					}


					
					GUILayout.EndArea();
				}

				
			} else { //the turn is not a player's

				// get which enemy's turn it is
				int enemyIndex = CurrentTurn - PlayerCombatants.Count;

				if(!turnNearlyFinished) {
					EnemyCombatants[enemyIndex].AutoAttack(PlayerCombatants);
					turnNearlyFinished = true;
				} else {
					// has the enemy finished their attack/animation?
					if(Event.current.type == EventType.Repaint && 
					   !EnemyCombatants[enemyIndex].AnimationInProgress) {

						turnNearlyFinished = false;
						IncrementTurn();
						return;
					}
				}
			}

			
			// draw the player combatants' data
			areaHeight = scalePx (30 * PlayerCombatants.Count + 10);

			GUILayout.BeginArea (new Rect (0, Screen.height - areaHeight, scalePx (180), areaHeight), 
			                     guiSkin.customStyles[0]);


			for(int i = 0; i < PlayerCombatants.Count; i++) {
				GUILayout.BeginHorizontal();

				string startTag = "<b>";
				string endTag = "</b>";

				if(i != CurrentTurn) {
					startTag = "<color=#ffffff55><b>";
					endTag = "</b></color>";
				}

				GUILayout.Label (startTag + PlayerCombatants[i].name + endTag);
				GUILayout.Label (PlayerCombatants[i].HitPoints + "/" + PlayerCombatants[i].MaxHitPoints, 
				                 guiSkin.customStyles[1], 
				                 GUILayout.Width(scalePx (75)));
				GUILayout.EndHorizontal();
			}

			GUILayout.EndArea ();


		}
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

	private void IncrementTurn() {
		CurrentTurn++;
		CurrentTurn %= totalCombatants;
	}
	
}
