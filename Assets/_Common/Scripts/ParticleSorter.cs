using UnityEngine;
using System.Collections;

public class ParticleSorter : MonoBehaviour {

	void Start () {
		particleSystem.renderer.sortingLayerName = "ParticleFX";
	}
}
