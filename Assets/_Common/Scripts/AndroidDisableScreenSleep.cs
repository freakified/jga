using UnityEngine;
using System.Collections;

public class AndroidDisableScreenSleep : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
