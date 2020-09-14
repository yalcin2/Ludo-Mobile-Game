using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
    public static CameraScript instance;

    string url = "https://connectedgamingassign.000webhostapp.com/";
    string playername, color;

    float xpos, ypos;

    private static string gameVersion = "1.0";

    public int totalJumps;

    private GameObject player, loadedplayer, otherplayer, dataStore;

    public static GameObject publicLoadedPlayer;
    public GameObject otherPlayer;
    public Text totalPlayers;
    public Button showStatsBtn;
    public GameObject statsPanel;
    public Text statsText;

    private GameStats gameStats;

    private int numOfPlayers;
    private string myIpAddress;
    private float timer = 0.0f;

    private Camera myCam;
    private float offsetX;
    private AudioClip audioClip;
    public AudioSource audioSource;
    private AssetBundle bundle;
    private AssetBundle bundle2;
    private AssetBundle bundle3;

    string address, town;
    int age;

    public float getCamLength()
    {
        return 2 * myCam.orthographicSize * myCam.aspect;
    }

    public float getRightBound()
    {
        return transform.position.x + getCamLength() / 2;
    }

    public float getlowerBound()
    {

        return transform.position.y - myCam.orthographicSize;
    }

    private void Start()
    {
        myCam = GetComponent<Camera>();


        address = dataStore.GetComponent<Datastore>().address;
        age = dataStore.GetComponent<Datastore>().age;
        town = dataStore.GetComponent<Datastore>().town;
        //offsetX = transform.position.x - loadedplayer.transform.position.x;

        if (dataStore.GetComponent<Datastore>().isAdmin)
            showStatsBtn.gameObject.SetActive(true);

        else 
            showStatsBtn.gameObject.SetActive(false);

    }

    private void Update()
    {
        timer += Time.deltaTime;
        int seconds = (int)(timer % 60);
        //print(seconds);

        totalPlayers.text = "PLAYERS: " + numOfPlayers + "/6";

    }

    private void LateUpdate()
    {
        try
        {
            transform.position = new Vector3(loadedplayer.transform.position.x + offsetX, transform.position.y, transform.position.z);
        }
        catch (Exception e) { }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        print("LOADED SCENE: " + scene.name);

        totalJumps = 0;

        gameStats = GameObject.Find("GameManager").GetComponent<GameStats>();

        myIpAddress = new WebClient().DownloadString("http://icanhazip.com");


        if (scene.name == "MainScene")
        {
            dataStore = GameObject.Find("Datastore");

            StartCoroutine(loadPlayerCharacter());
            StartCoroutine(loadMusic());    
        }
    }

    public void statsBtn() {
        if (statsPanel.activeSelf)
        {
            statsPanel.SetActive(false);
        }
        else if(!statsPanel.activeSelf) {
            statsPanel.SetActive(true);
        }
    }

    IEnumerator joinGame(GameObject loadedplayer)
    {
        string joinurl = url + "?operation=join&playername=" + playername + "&xposition=" + xpos + "&yposition=" + ypos +
            "&objectcolor=" + color + "&totaltime=" + timer + "&ipaddress=" + myIpAddress + "&address=" + address + "&age=" + age + "&town=" + town + 
                "&score=" + gameStats.totalCurrentScore + "&totaljumps=" + totalJumps + "&gameversion=" + gameVersion;

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
            dataStore.GetComponent<Datastore>().id = int.Parse(returnedvalues[1]);
            StartCoroutine(sendposition(loadedplayer));
            //I need to start the coroutine that also gets the positions of OTHER players
            StartCoroutine(getData());
        }
    }

    public void leaveGameBtn() {
        StartCoroutine(leaveGame(publicLoadedPlayer));
    }


    public IEnumerator leaveGame(GameObject loadedplayer)
    {
        string leaveurl = url + "?operation=leave&playerid=" + loadedplayer.name;
        Debug.Log(leaveurl);

        UnityWebRequest www = UnityWebRequest.Get(leaveurl);
        //the player has left the server
        yield return www.SendWebRequest();

        if (www.isHttpError || www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            print("PLAYER HAS LEFT THE GAME");
            Destroy(dataStore);
            SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
        }
    }


    public void playerDied(GameObject loadedplayer) {
        loadedplayer.transform.position = new Vector2(2, 1);
        loadedplayer.GetComponent<Player>().setAlive();
        StartCoroutine(gameStats.scoreUpdater());
    }


    IEnumerator sendposition(GameObject loadedplayer)
    {
        while (true)
        {
            string moveurl =
            url + "?operation=move&playerid=" + loadedplayer.name + "&xposition=" + loadedplayer.transform.position.x +
            "&yposition=" + loadedplayer.transform.position.y + "&objectcolor=" + color + "&totaltime=" + timer + "&ipaddress=" + myIpAddress
                + "&address=" + address + "&age=" + age + "&town=" + town + "&score=" + gameStats.totalCurrentScore + "&totaljumps=" + totalJumps
                + "&gameversion=" + gameVersion; //+ "&hits=" + loadedplayer.GetComponent<Player>().hits;

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
                try
                {
                    //split the data we get from the website. 
                    string[] players = www.downloadHandler.text.Split('|');

                    //remove the hanging split
                    Array.Resize(ref players, players.Length); // - 1

                    for (int index = 0; index < players.Length; index++)
                    {
                        Debug.Log(index + " " + players[index]);
                        //split again

                        string[] eachplayer = players[index].Split(',');

                        //remove the hanging comma
                        Array.Resize(ref eachplayer, eachplayer.Length); // - 1

                        foreach (string field in eachplayer)
                        {
                            print(field);
                        }

                        //id,name,xpos,ypos,
                        int id = int.Parse(eachplayer[0]);
                        string name = eachplayer[1];
                        float xpos = float.Parse(eachplayer[2]);
                        float ypos = float.Parse(eachplayer[3]);
                        string otherPlayerColor = eachplayer[4];
                        float totaltime = float.Parse(eachplayer[5]);
                        string ipaddress = eachplayer[6];
                        float score = float.Parse(eachplayer[7]);
                        float totalJumps = float.Parse(eachplayer[8]);
                        string gameVersion = eachplayer[9];

                        //the \ is an escape string, this line removes the single quote from the nicknames in the database if they have single quotes
                        name = name.Trim('\'');

                        if (GameObject.Find(eachplayer[0]) == null)
                        {
                            //create the other player
                            otherplayer = Instantiate(otherPlayer,
                                new Vector3(xpos, ypos), Quaternion.identity);
                           statsText.text = statsText.text + Environment.NewLine + "PLAYER NAME: " + name + " COLOR: " + color + " Elapsed Time: " + totaltime + " IP ADDRESS: " + ipaddress;
 

                            numOfPlayers++;

                            StartCoroutine(loadPlayerColour(otherplayer, otherPlayerColor));
                            otherplayer.GetComponentInChildren<TextMesh>().text = name;

                            //otherplayer.GetComponentInChildren<SpriteRenderer>().material.color.a = 0.5f;
                            //setPlayerColour(otherplayer, otherPlayerColor);

                            otherplayer.name = eachplayer[0];
                            otherplayer.AddComponent<Othersquarecontroller>();
                        }
                        else
                        {
                            if (eachplayer[0] != loadedplayer.name)
                            {
                                //update other player position
                                otherplayer = GameObject.Find(eachplayer[0]);

                                //otherplayer.transform.position = new Vector3(xpos, ypos);

                                float x = otherPlayer.transform.position.x;
                                float y = otherplayer.transform.position.y;

                                lastPosx = gameObject.transform.position.x;
                                lastPosy = gameObject.transform.position.y;

                                StartCoroutine(moveBox(otherplayer, xpos, ypos));

                                checkIfMoving(otherplayer, x, y);
                            }
                        }


                    }
                }
                catch (Exception e)
                {
                    print(e.Message);
                }

            }
        }
    }

    private float lastPosx;
    private float lastPosy;

    IEnumerator checkIfMoving(GameObject box, float x, float y)
    {
        yield return new WaitForSeconds(5f);

        if (lastPosy != 0 && lastPosx != 0)
        {
            if (lastPosx == x && lastPosy == y)
            {
                numOfPlayers--;
                Destroy(box);
            }
            else
            {
                lastPosy = x;
                lastPosy = y;
            }
        }
    }


    public IEnumerator loadPlayerCharacter()
    {
        while (!Caching.ready) yield return null;

        UnityWebRequest asset = UnityWebRequestAssetBundle.GetAssetBundle("https://connectedgamingassign.000webhostapp.com/assets/player");

        yield return asset.SendWebRequest();

        if (asset.isNetworkError || asset.isHttpError)
        {
            Debug.Log(asset.error);
        }
        else
        {
            bundle = DownloadHandlerAssetBundle.GetContent(asset);
        }

        player = bundle.LoadAsset<GameObject>("player");

        //player = Resources.Load("Player") as GameObject;
        playername = GameObject.Find("Datastore").GetComponent<Datastore>().nickname;
        color = GameObject.Find("Datastore").GetComponent<Datastore>().color;

        print("SPAWNED IN A PLAYER");
        loadedplayer = Instantiate(player, Vector3.zero, Quaternion.identity);
        numOfPlayers++;
        statsText.text = statsText.text + Environment.NewLine + " PLAYER NAME: " + playername + " COLOR: " + color + " Elapsed Time: " + timer + " IP ADDRESS: " + myIpAddress;
        bundle.Unload(false);

        setPlayerGameType(loadedplayer, color);
        StartCoroutine(loadPlayerColour(loadedplayer, color));
        loadedplayer.GetComponentInChildren<TextMesh>().text = playername;

        publicLoadedPlayer = loadedplayer;

        StartCoroutine(joinGame(loadedplayer));

        print("LOADED PLAYER PREFAB BUNDLE");
    }

    public IEnumerator loadMusic()
    {
        while (!Caching.ready) yield return null;

        UnityWebRequest asset = UnityWebRequestAssetBundle.GetAssetBundle("https://connectedgamingassign.000webhostapp.com/assets/music");

        yield return asset.SendWebRequest();

        if (asset.isNetworkError || asset.isHttpError)
        {
            Debug.Log(asset.error);
        }
        else
        {
            bundle2 = DownloadHandlerAssetBundle.GetContent(asset);
        }

        audioClip = bundle2.LoadAsset<AudioClip>("music");

        //audioSource.clip = audioClip;
        //audioSource.Play();

        bundle2.Unload(false);

        print("LOADED AUDIO SOURCE BUNDLE");
    }

    public IEnumerator loadPlayerColour(GameObject p, string color)
    {
        while (!Caching.ready) yield return null;

        UnityWebRequest asset = UnityWebRequestAssetBundle.GetAssetBundle("https://connectedgamingassign.000webhostapp.com/assets/" + color);

        yield return asset.SendWebRequest();

        if (asset.isNetworkError || asset.isHttpError)
        {
            Debug.Log(asset.error);
        }
        else
        {
            bundle3 = DownloadHandlerAssetBundle.GetContent(asset);
        }

        Sprite sprite = bundle3.LoadAsset<Sprite>(color);
        p.GetComponentInChildren<SpriteRenderer>().sprite = sprite;

        bundle3.Unload(false);

        print("LOADED PLAYER SPRITE RENDERER BUNDLE");
    }

    private void setPlayerGameType(GameObject p, string playerColor)
    {
        switch (playerColor)
        {
            case "Green": // Default speed 
                {
                    p.GetComponent<Player>().speed = 8f;
                    break;
                }
            case "Blue": // Medium speed
                {
                    p.GetComponent<Player>().speed = 11f;
                    break;
                }
            case "Red":  // Fast speed
                {
                    p.GetComponent<Player>().speed = 15f;
                    break;
                }
            //case "Yellow": 
                //{
                   // p.GetComponent<Player>().canDoubleJump = false;
                    //break;
                //}
        }
    }

    void OnDisable()
    {
        //Debug.Log("OnDisable");
        //StartCoroutine(leaveGame(loadedplayer));
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnApplicationQuit()
    {
        StartCoroutine(leaveGame(loadedplayer));
    }
}