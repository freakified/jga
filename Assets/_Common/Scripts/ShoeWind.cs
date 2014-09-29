using UnityEngine;
using System.Collections;

public class ShoeWind : MonoBehaviour {

	public Rigidbody2D shoe1, shoe2;

	public Vector2 WindForce = Vector2.right;
	public float WindPeriod = 1;

	private float elapsedTime = 0.0f;

	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void FixedUpdate () {
		elapsedTime += Time.deltaTime;

		float windMultiplier = Mathf.Abs(Mathf.Sin((elapsedTime / WindPeriod * Mathf.PI) / 2));

		shoe1.AddForce(WindForce * windMultiplier * Time.fixedDeltaTime);
		shoe2.AddForce(WindForce * windMultiplier * Time.fixedDeltaTime);

	}
}
