using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleController : MonoBehaviour {
	public GUISkin guiSkin;

	public int targetScreenWidth = 640;

	// TODO: this should be replaced by the real combatant script
	private int numCombatants = 3;
	private int numAttacks = 4;
	private int currentAttackHover = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () {
		GUI.skin = guiSkin;

		//scale the sizes of elements to match the actual resolution
		scaleGUI(guiSkin);

		// draw the attack selection box
		int areaHeight = scalePx (50 + 30 * numAttacks);
		GUILayout.BeginArea (new Rect (0, 0, scalePx (210), areaHeight), 
		                     guiSkin.customStyles[0]);
		GUILayout.Label ("ATTACK", guiSkin.customStyles[3]);
		GUILayout.Button("All-Purpose Slice");

		Rect b1rect = GUILayoutUtility.GetLastRect();


		GUILayout.Button("Rock N' Chop");

		Rect b2rect = GUILayoutUtility.GetLastRect();


		GUILayout.Button("Sales Pitch");

		Rect b3rect = GUILayoutUtility.GetLastRect();



		GUILayout.Label ("HEAL", guiSkin.customStyles[3]);
		GUILayout.Button("Fried Chicken Smoothie");

		GUILayout.EndArea ();

		
	

		// now draw the attack description tooltip

		if(Event.current.type == EventType.Repaint && 
		   b1rect.Contains(Event.current.mousePosition )) {
			GUI.Label (new Rect (scalePx (220), scalePx (10), scalePx (315), scalePx (55)), 
					  "Stabs a single target with the Miracle Blade™ All-Purpose Slicer™.", 
			           guiSkin.customStyles[2]);
		}

		if(Event.current.type == EventType.Repaint && 
		   b2rect.Contains(Event.current.mousePosition )) {
			GUI.Label (new Rect (scalePx (220), scalePx (10 + 30), scalePx (315), scalePx (55)), 
			           "Chops up all enemies, dealing damage to all targets.", 
			           guiSkin.customStyles[2]);
		}

		if(Event.current.type == EventType.Repaint && 
		   b3rect.Contains(Event.current.mousePosition )) {
			GUI.Label (new Rect (scalePx (220), scalePx (10 + 30 + 30), scalePx (315), scalePx (55)), 
			           "Puts target to sleep with a lecture on the benefits of the Miracle Blade™ III Perfection Series™.", 
			           guiSkin.customStyles[2]);
		}




		// draw the player combatants' data
		areaHeight = scalePx (30 * numCombatants);
		GUILayout.BeginArea (new Rect (0, Screen.height - areaHeight, scalePx (180), areaHeight), 
		                     guiSkin.customStyles[0]);

		// chef tony's data
		GUILayout.BeginHorizontal();
			GUILayout.Label ("<b>Chef Tony</b>");
			GUILayout.Label ("100/100", guiSkin.customStyles[1], GUILayout.Width(scalePx (75)));
		GUILayout.EndHorizontal();
		// end chef tony

		// james health
		GUILayout.BeginHorizontal();
			GUILayout.Label ("<color=#ffffff55><b>James</b></color>");
			GUILayout.Label ("<color=red>23</color>/142", guiSkin.customStyles[1], GUILayout.Width(scalePx (75)));
		GUILayout.EndHorizontal();
		// end james health

		// lm health
		GUILayout.BeginHorizontal();
			GUILayout.Label ("<color=#ffffff55><b>Like Mike</b></color>");
			GUILayout.Label ("<color=red>23</color>/142", guiSkin.customStyles[1], GUILayout.Width(scalePx (75)));
		GUILayout.EndHorizontal();
		// end lm health


		GUILayout.EndArea ();
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
		return (int)((targetSize * Screen.width) / targetScreenWidth);
	}
	
}
