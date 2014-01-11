using UnityEngine;
using System.Collections;

public class SparksFly : MonoBehaviour {
	
	public float scaleSpeed = 0.01f;
	public float rotateSpeed = 100f;
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale = transform.localScale + new Vector3(scaleSpeed, scaleSpeed, 1);

		Vector3 tempAngle = transform.localEulerAngles;
		tempAngle.z += rotateSpeed;
		transform.localEulerAngles = tempAngle;

	}
}
