using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Datastore : MonoBehaviour
{
    public int id, age;

    public string nickname, color, totaltime,ipaddress,address,town;

    public bool isAdmin;

    //string url = "https://gerrysaid.000webhostapp.com/";

    string url = "https://connectedgamingassign.000webhostapp.com/";

    public void displayNickname()
    {
        Debug.Log(nickname);
    }

    public IEnumerator LoadHighScores()
    {
        //checking the url that is being sent
        Debug.Log(url + "?operation=highscore");
        //generate the url and send it to the website
        UnityWebRequest www = UnityWebRequest.Get(url + "?operation=highscore");

        yield return www.SendWebRequest();

        if (www.isHttpError || www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //this reads the text from the website
            //Debug.Log(www.downloadHandler.text);	

            //split the data we get from the website. 
            string[] players = www.downloadHandler.text.Split('|');

            //remove the hanging split
            Array.Resize(ref players, players.Length - 1);


            for (int index = 0; index < players.Length; index++)
            {
                Debug.Log(index + " " + players[index]);
                //split again

                string[] eachplayer = players[index].Split(',');

                //remove the hanging comma

                foreach (string field in eachplayer)
                {
                    Debug.Log(field);


                }
                //id,name,xpos,ypos,
                int id = int.Parse(eachplayer[0]);
                string name = eachplayer[1];
                float xpos = float.Parse(eachplayer[2]);
                float ypos = float.Parse(eachplayer[3]);
                int hits = int.Parse(eachplayer[5]);


                //the \ is an escape string, this line removes the single quote from the nicknames in the database if they have single quotes
                name = name.Trim('\'');

                GameObject.Find("highscorestext").GetComponent<Text>().text += name + " " + hits + "\n";

            }
        }


    }

}
