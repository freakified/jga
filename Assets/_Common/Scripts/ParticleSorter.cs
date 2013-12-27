using UnityEngine;
using System.Collections;

public class ParticleSorter : MonoBehaviour {

	void Start () {
		//Change Foreground to the layer you want it to display on 
		//You could prob. make a public variable for this
		particleSystem.renderer.sortingLayerName = "ParticleFX";
		//particleSystem.renderer.sortingOrder = 2;
	}
}
