using UnityEngine;
using System.Collections;

public class ShoelaceBehavior : MonoBehaviour {

	public GameObject AttachedShoe;

	private LineRenderer r;
	private DistanceJoint2D j;

	// Use this for initialization
	void Start () {
		r = GetComponent<LineRenderer>();
		j = AttachedShoe.GetComponent<DistanceJoint2D>();
	}
	
	// Update is called once per frame
	void Update () {
		r.SetPosition(0, j.connectedBody.transform.position);
		r.SetPosition(1, AttachedShoe.transform.position);
	}
}
