using UnityEngine;
using System.Collections;

public class SetOrthoSortMode : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Camera.main.transparencySortMode = TransparencySortMode.Orthographic;
	}

}
