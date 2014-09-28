using UnityEngine;
using System.Collections;

public class LMBBallBehavior : MonoBehaviour {


	public float RotationSpeed = 1f;

	private Vector3 rotation;

	// Use this for initialization
	void Start () {
		rotation = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		rotation.z += RotationSpeed * Time.deltaTime;
		transform.eulerAngles = rotation;
	}

}
