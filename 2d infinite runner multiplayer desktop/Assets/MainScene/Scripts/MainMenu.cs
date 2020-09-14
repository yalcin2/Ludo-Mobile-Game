using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public InputField username;
	public InputField password;
	
	public Button playButton;

    // Use this for initialization
    void Start () {
		playButton.enabled = false;
	}


	public void onPressStart(){
		SceneManager.LoadScene("MainScene");
	}

	public void onPressedQuit(){
		Application.Quit();
	}
	// Update is called once per frame
	void Update () {
		if(username!= null && password != null){
			if(username.text.ToString() != "" && password.text.ToString() != ""){
				playButton.enabled = true;
			}
			else{
				playButton.enabled = false;
			}
		}
	}

	private void OnDestroy(){
		Debug.Log("main menu destroyed ");
	}

    //this method is being run in the main scene
    public void click_displayNickname()
    {
        GameObject.Find("Datastore").GetComponent<Datastore>().displayNickname();
    }
}
