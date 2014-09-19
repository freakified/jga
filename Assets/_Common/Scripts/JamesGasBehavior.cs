using UnityEngine;
using System.Collections;

public class JamesGasBehavior : MonoBehaviour {

	public bool EnableGas = false;
	public bool EmitGasWhenMoving = false;

	private ParticleSystem gas;

	// Use this for initialization
	void Start () {
		gas = GetComponentInChildren<ParticleSystem>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(EmitGasWhenMoving) {
			EnableGas = rigidbody2D.velocity.magnitude > 0;
		}

		if(EnableGas && !gas.isPlaying) {
			gas.Play();
		} else if(!EnableGas && gas.isPlaying) {
			gas.Stop();
		}
	}
}
