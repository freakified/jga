using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

	private Camera mainCamera;
	private float initialSize;
	private Vector3 initialPos;
	
	void OnEnable () {
		mainCamera = Camera.main;
		initialSize = mainCamera.orthographicSize;
		initialPos = mainCamera.transform.position;
		mainCamera.orthographicSize = initialSize * 0.95f;
	}

	void OnDisable () {
		mainCamera.orthographicSize = initialSize;
		mainCamera.transform.position = initialPos;
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 newPos = mainCamera.transform.position;
		newPos.x += (Random.value - 0.5f) * 0.2f;
		newPos.y += (Random.value - 0.5f) * 0.2f;

		newPos.x = Mathf.Clamp(newPos.x, -0.17f, 0.17f);
		newPos.y = Mathf.Clamp(newPos.x, -0.08f, 0.08f);


		mainCamera.transform.position = newPos;
	}
}
