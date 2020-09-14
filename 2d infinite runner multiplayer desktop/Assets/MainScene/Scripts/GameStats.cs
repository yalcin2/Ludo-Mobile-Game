using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStats : MonoBehaviour{

	Player player;
	public Text scoreText;
	public Text highScore;
	public GameObject platformHolder;
	public int platChildren;

    public int totalCurrentScore;

    private Transform holderTransform;
    private GameObject dataStore;
    private CameraScript camera;

    void Awake(){
        holderTransform = platformHolder.GetComponent<Transform>();
	}

	// Use this for initialization
	void Start (){
        dataStore = GameObject.Find("Datastore").gameObject;
        camera = GameObject.Find("Main Camera").GetComponent<CameraScript>();

        //player = CameraScript.publicLoadedPlayer.GetComponent<Player>();


        Coroutine c = StartCoroutine(scoreUpdater());
		highScore.text = "HighScore "+PlayerPrefs.GetInt("HighScore");	
	}

	public IEnumerator scoreUpdater(){
		int i = 0;
		scoreText.text = "Score " + i;
        totalCurrentScore++;

        yield return new WaitForSeconds(3f);

        while (CameraScript.publicLoadedPlayer.GetComponent<Player>().getAlive())
        {
			
			yield return new WaitForSeconds(1.0f);
			scoreText.text = "Score " + i;
            totalCurrentScore++;
            if (!CameraScript.publicLoadedPlayer.GetComponent<Player>().getAlive())
            {
				break;
			}
			++i;
		}

        PlayerPrefs.SetInt("HighScore",Math.Max(i,PlayerPrefs.GetInt("HighScore")));
        camera.playerDied(CameraScript.publicLoadedPlayer);

        //StartCoroutine(camera.leaveGame(CameraScript.publicLoadedPlayer));

    }
	// Update is called once per frame
	void Update () {
		platChildren = holderTransform.childCount;


		if (Input.GetKeyDown(KeyCode.Escape)){
			//TODO cleanup here
			Application.Quit();
		}	
	}


}
