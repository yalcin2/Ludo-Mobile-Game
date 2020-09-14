using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Connection : MonoBehaviour
{
    GameObject player, loadedplayer, otherplayer;
    //change according to the name you gave to YOUR website

    string playername, color;
    float xpos, ypos;
    string url = "https://connectedgamingyalcin.000webhostapp.com/";
    // Use this for initialization
    void Start()
    {
        //start the process of asking the website for the data

        //StartCoroutine(getData());
    }


    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //method that is called as soon as the scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = Resources.Load("Player") as GameObject;
        playername = GameObject.Find("Datastore").GetComponent<Datastore>().nickname;
        color = GameObject.Find("Datastore").GetComponent<Datastore>().color;
        loadedplayer = Instantiate(player, Vector3.zero, Quaternion.identity);
        loadedplayer.GetComponentInChildren<TextMesh>().text = playername;
        loadedplayer.AddComponent<Player>();

        switch (color)
        {
            case "Red":
                {
                    loadedplayer.GetComponent<SpriteRenderer>().color = Color.red;
                    break;
                }
            case "Green":
                {
                    loadedplayer.GetComponent<SpriteRenderer>().color = Color.green;
                    break;
                }
            case "Blue":
                {
                    loadedplayer.GetComponent<SpriteRenderer>().color = Color.blue;
                    break;
                }
            case "Yellow":
                {
                    loadedplayer.GetComponent<SpriteRenderer>().color = Color.yellow;
                    break;
                }

        }
        StartCoroutine(joinGame(loadedplayer));
        StartCoroutine(timeLimit());

    }

    IEnumerator timeLimit()
    {
        //after 200 seconds, load the highscores scene
        yield return new WaitForSeconds(200f);
        SceneManager.LoadScene("highscores");
    }

    IEnumerator joinGame(GameObject loadedplayer)
    {
        string joinurl = url + "?operation=join&playername=" + playername + "&xposition=" + xpos + "&yposition=" + ypos +
        "&objectcolor=" + color;
        Debug.Log(joinurl);

        UnityWebRequest www = UnityWebRequest.Get(joinurl);
        //the player has joined the server
        yield return www.SendWebRequest();


        if (www.isHttpError || www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string[] returnedvalues = www.downloadHandler.text.Split('|');
            //the ID of the joined player is returned from the database
            loadedplayer.name = returnedvalues[1];
            StartCoroutine(sendposition(loadedplayer));
            //I need to start the coroutine that also gets the positions of OTHER players
            StartCoroutine(getData());
        }

    }

    IEnumerator sendposition(GameObject loadedplayer)
    {
        while (true)
        {
            string moveurl =
            url + "?operation=move&playerid=" + loadedplayer.name + "&xposition=" + loadedplayer.transform.position.x +
            "&yposition=" + loadedplayer.transform.position.y + "&objectcolor=" + color + "&hits=" + loadedplayer.GetComponent<Player>().hits;

            Debug.Log(moveurl);

            UnityWebRequest www = UnityWebRequest.Get(moveurl);

            yield return www.SendWebRequest();
        }


    }

    IEnumerator moveBox(GameObject box, float x, float y)
    {

        //if ( isMoving ) yield break; // exit function
        //isMoving = true;
        if (box.GetComponent<Othersquarecontroller>().isMoving) yield break;
        box.GetComponent<Othersquarecontroller>().isMoving = true;
        Vector3 from = box.transform.position;
        Vector3 to = new Vector3(x, y);
        for (float t = 0f; t < 1f; t += Time.deltaTime * 5f)
        {
            box.transform.position = Vector3.Lerp(from, to, t);
            yield return null;
        }
        box.transform.position = to;
        box.GetComponent<Othersquarecontroller>().isMoving = false;


    }

    IEnumerator getData()
    {
        while (true)
        {
            //checking the url that is being sent
            Debug.Log(url + "?operation=poll");
            //generate the url and send it to the website
            UnityWebRequest www = UnityWebRequest.Get(url + "?operation=poll");

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
                    Array.Resize(ref eachplayer, eachplayer.Length - 1);

                    foreach (string field in eachplayer)
                    {
                        Debug.Log(field);


                    }
                    //id,name,xpos,ypos,
                    int id = int.Parse(eachplayer[0]);
                    string name = eachplayer[1];
                    float xpos = float.Parse(eachplayer[2]);
                    float ypos = float.Parse(eachplayer[3]);


                    //the \ is an escape string, this line removes the single quote from the nicknames in the database if they have single quotes
                    name = name.Trim('\'');

                    if (GameObject.Find(eachplayer[0]) == null)
                    {
                        //create the other player
                        otherplayer = Instantiate(player,
                            new Vector3(xpos, ypos), Quaternion.identity);

                        otherplayer.GetComponentInChildren<TextMesh>().text = name;

                        otherplayer.name = eachplayer[0];

                        otherplayer.AddComponent<Othersquarecontroller>();
                    }
                    else
                    {
                        if (eachplayer[0] != loadedplayer.name)
                        {
                            //update other player position
                            otherplayer = GameObject.Find(eachplayer[0]);
                            // otherplayer.transform.position = new Vector3(xpos, ypos);

                            StartCoroutine(moveBox(otherplayer, xpos, ypos));
                        }
                    }


                }


            }
        }




    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.V))
        {
            color = "Red";
            loadedplayer.GetComponent<SpriteRenderer>().color = Color.red;

        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            color = "Green";
            loadedplayer.GetComponent<SpriteRenderer>().color = Color.green;
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            color = "Blue";
            loadedplayer.GetComponent<SpriteRenderer>().color = Color.blue;
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            color = "Yellow";
            loadedplayer.GetComponent<SpriteRenderer>().color = Color.yellow;
        }

    }
}
