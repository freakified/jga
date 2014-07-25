using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S402Puppeteer : CutscenePuppeteer {

	private GameObject ChefTony;
	private GameObject ExamineInfo;
	private GUIText ExamineNextText;
	private MusicPlayer mus;

	//private Animator ctanim;

	public List<SpriteRenderer> BackgroundLayers;
	
	public SpriteRenderer CoffinGlow;

	public AudioClip RessurectionMusic;

	public GameObject ShoesPrefab;
	private GameObject shoesInstance;

	public GameObject KnifePrefab;
	private GameObject knifeInstance;

	private Animator ctanim;

	// Use this for initialization
	void Start () {
		// get all the objects we'll need for the cutscene 
		ChefTony = GameObject.Find ("Chef Tony");
		ExamineInfo = GameObject.Find ("ExamineInfo");
		ExamineNextText = GameObject.Find ("ExamineNextText").GetComponent<GUIText>();
		ExamineNextText.enabled = false;


		mus = GameObject.Find ("BGM").GetComponent<MusicPlayer>();
		ctanim = ChefTony.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	public void FixedUpdate () {
		if(CurrentScene == 3) {
			if(ChefTony.transform.position.x > 0.537f) {
				ChefTony.GetComponent<PlayerFreeze>().Freeze();
				nextScene();
			}
		} else if(CurrentScene == 5) {
			if(!ExamineNextText.enabled && timerIsGreaterThan(1)) {
				ExamineNextText.enabled = true;
				stopTimer();
			} else if(Input.GetButtonDown("Select")) {
				GameObject.Destroy(ExamineInfo);
				nextScene();
			}

		} else if(CurrentScene == 15) {
			Color colorFade = new Color(Mathf.Lerp(1.0f, 0.5f, elapsedTime / 2),
			                           Mathf.Lerp(1.0f, 0.0f, elapsedTime / 2),
			                           Mathf.Lerp(1.0f, 0.0f, elapsedTime / 2));
			BackgroundLayers.ForEach(layer => layer.color = colorFade);


			
			if(elapsedTime / 2 > 1.0f) {
				nextScene();
			}
		} else if (CurrentScene == 17) {
			Color coffinFade = new Color(1.0f, 1.0f, 1.0f,
			                             Mathf.Lerp(0.0f, 1.0f, elapsedTime / 2));
			CoffinGlow.color = coffinFade;

			if(elapsedTime / 2 > 1.0f) {

				//hide the foreground pews since they don't layer well with the particles
				GameObject.Find("00 - temple-interior-fg").renderer.enabled = false;

				Instantiate(ShoesPrefab, new Vector3(1.35f, -0.32f, 0), Quaternion.identity);
				shoesInstance = GameObject.Find ("ShoesTie");
				shoesInstance.rigidbody2D.gravityScale = 0;
				shoesInstance.rigidbody2D.isKinematic = false;
				shoesInstance.GetComponent<ShoeWind>().enabled = false;
				shoesInstance.GetComponent<Shoesplosion>().enabled = true;

				startTimer();
				nextScene();
			}
		} else if(CurrentScene == 19) {
			//just wait a bit and then kill the textbox

			if(knifeInstance.transform.position.x > shoesInstance.transform.position.x) {
				startTimer();
				nextScene();
			}
		} else if(CurrentScene == 20) {
			//home in on the shoes
			Vector3 temp = GameObject.Find("Shoe").transform.position;
			
			knifeInstance.rigidbody2D.AddForce((shoesInstance.transform.position -
			                                    knifeInstance.transform.position) *
			                                   20 * Time.fixedDeltaTime);

			//basically, when the music's about to end
			if(timerIsGreaterThan (6.0f)) {
				StartCoroutine(FadeAndNext(Color.white, 4.0f, null));
				nextScene();
			}
		}
	}

	public override void HandleSceneChange() {

		//temp code to accelerate cutscene
		if(CurrentScene < 14) {
			nextScene();
		}

		if(CurrentScene == 3) {
			ChefTony.GetComponent<PlayerFreeze>().UnFreeze();
		} else if(CurrentScene == 5) {
			//show the coffin examination thing
			startTimer();

			ExamineInfo.transform.position = Vector3.zero;
		} else if(CurrentScene == 15) {
			startTimer();

		} else if(CurrentScene == 17) {
			mus.PlayMusic(RessurectionMusic, false);

			startTimer();
		} else if (CurrentScene == 18) {

			ctanim.SetBool("LostKnife", true);
			knifeInstance = (GameObject)Instantiate(KnifePrefab, 
			                                        ChefTony.transform.position + new Vector3(0.27f, -0.06f, 0), 
			                                        Quaternion.identity);
			knifeInstance.rigidbody2D.AddForce(new Vector2(50.0f, 10.0f));
			knifeInstance.rigidbody2D.AddTorque(40.0f);
			startTimer();
			nextScene();
		}
	}

}
