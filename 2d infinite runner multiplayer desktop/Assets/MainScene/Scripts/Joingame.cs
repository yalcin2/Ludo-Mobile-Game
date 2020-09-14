using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Joingame : MonoBehaviour
{

    string mynickname, mypassword, mycolor, myaddress, mytown;
    int myage;

    GameObject dataStore;

    private string adminUser = "admin123";
    private string adminPass = "admin123";

    public Text guitext;
    public Button playBtn;
    public InputField name;
    public InputField pass;
    public InputField address;
    public InputField town;
    public InputField age;

    private void Start()
    {
        myaddress = "";
        myage = 0;
        mytown = "";
    }

    private void Update()
    {
        if (name.text == "" || pass.text == "" || address.text == "" || town.text == "" || age.text == "")
        {
            playBtn.interactable = false;
        }
        else {
            playBtn.interactable = true;
        }
    }

    public void click_joinGamebutton()
    {
        Debug.Log("join clicked");

        //the value that was entered in the input field
        mynickname = GameObject.Find("NicknameInput").GetComponent<InputField>().text;
        mypassword = GameObject.Find("PasswordInput").GetComponent<InputField>().text;
        myaddress = GameObject.Find("addressInput").GetComponent<InputField>().text;
        mytown = GameObject.Find("townInput").GetComponent<InputField>().text;
        myage = int.Parse(GameObject.Find("ageInput").GetComponent<InputField>().text);

        if (mynickname == adminUser)
        {
            if (mypassword == adminPass)
            {
                startGame(true);
            }
            else
            {
                guitext.text = "Username already exists, please enter the correct password!";
            }
        }
        else {
            startGame(false);
        }
    }

    //this method is being run in the main scene
    public void click_displayNickname()
    {
        GameObject.Find("Datastore").GetComponent<Datastore>().displayNickname();
    }

    void startGame(bool isAdmin) {
        dataStore = GameObject.Find("Datastore");
        DontDestroyOnLoad(dataStore);

        Dropdown colordropdown = GameObject.Find("ColorDropdown").GetComponent<Dropdown>();
        mycolor = colordropdown.options[colordropdown.value].text;

        dataStore.GetComponent<Datastore>().isAdmin = isAdmin;
        dataStore.GetComponent<Datastore>().nickname = mynickname;
        dataStore.GetComponent<Datastore>().color = mycolor;
        dataStore.GetComponent<Datastore>().address = myaddress;
        dataStore.GetComponent<Datastore>().age = myage;
        dataStore.GetComponent<Datastore>().town = mytown;


        Debug.Log("Nickname: " + mynickname);

        //once the button is clicked, load the main scene
        SceneManager.LoadScene("MainScene");
    }
}
