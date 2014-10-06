using UnityEngine;
using System.Collections;

public class ChapterSave : MonoBehaviour {

	public int ChapterNumber;

	// Use this for initialization
	void Start () {
		int currentSave = PlayerPrefs.GetInt("HighestCompletedChapter", -1);

		if(ChapterNumber > currentSave) {
			PlayerPrefs.SetInt("HighestCompletedChapter", ChapterNumber);

			PlayerPrefs.Save();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
