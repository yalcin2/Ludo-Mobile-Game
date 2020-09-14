using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;

using UnityEngine;



public class FrameSettings : MonoBehaviour{




	public bool showFrameStatsInGame;
	public bool enableVsync;
	public float maxTime;

	

	public float frameRate;
	private void Awake(){
		
		QualitySettings.vSyncCount = enableVsync ? 1 : 0;
//		Application.targetFrameRate = 100;	
		Time.maximumDeltaTime = maxTime;
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update (){
		frameRate = 1.0f/Time.deltaTime;


//		Debug.Log("time taken is "+Time.deltaTime*1000);
	}
	
	private void OnGUI(){
		if (showFrameStatsInGame){
			GUI.Label(new Rect(new Vector2(Screen.width / 2.0f, 0.8f * Screen.height), new Vector2(200, 200)),
				"Frametime: " + Time.deltaTime);
			GUI.Label(new Rect(new Vector2(Screen.width / 2.0f, .9f * Screen.height), new Vector2(200, 200)),
				"Framerate: " + 1.0f / Time.deltaTime);
		}

	}
	
	
}
