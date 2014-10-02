using UnityEngine;
using System.Collections;

public class LMBBallBehavior : MonoBehaviour {


	public float RotationSpeed = 1f;

	private Vector3 rotation;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(new Vector3(0, 0, 1) * RotationSpeed * Time.deltaTime);
	}

}
