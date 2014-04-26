using UnityEngine;
using System.Collections;

public class OrphanBehavior : MonoBehaviour {

	public float StartDelay = 0;
	public int JumpForce = 100;
	public float Interval = 0.5f;

	private float time = 0;

	void Start () {

	}

	void FixedUpdate() {
		time += Time.fixedDeltaTime;

		if(time > StartDelay) {
			if(time > Interval) {
				rigidbody2D.AddForce(Vector2.up * JumpForce);

				time = 0;
			}
		}
	}

//	public float OrbitRadius = 0.5f;
//	public float OrbitSpeed = 10f;
//
//	private Vector2 targetPos = Vector2.zero;
//
//	private Transform father; 
//
//	private float tTemp;
//
//	// Use this for initialization
//	void Start () {
//		father = transform.parent.transform;
//		targetPos.x = OrbitRadius;
//		OrbitSpeed /= 100;
//	}
//	
//	// Update is called once per frame
//	void FixedUpdate () {
//		if(tTemp < 0.5 * OrbitSpeed) { // target has not yet been reached
//			tTemp += Time.fixedDeltaTime * OrbitSpeed;
//		} else {
//			tTemp = 0;
//			targetPos.x = -targetPos.x;
//		}
//
//		print (tTemp);
//		transform.localPosition = Vector2.Lerp(transform.localPosition, targetPos, tTemp);
//
//	}
}
