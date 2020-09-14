using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateRandom : MonoBehaviour
{
    public Sprite[] symbolsSides;
    public Image[] rend;
    public Sprite imagePressed;
    public Sprite imageUnpressed;
    public Text checkSymbolsText;
    public Text roundNumberText;

    int[] preset;
    bool isPlaying;
    int roundNumber;

    private void Start()
    {
        roundNumber = 0;
        isPlaying = false;
        roundNumberText.text = "Round: " + roundNumber;
        checkSymbolsText.text = "Press PLAY!";
    }

    public void OnSelected()
    {
        if(!isPlaying){
            isPlaying = true;

            roundNumber++;
            roundNumberText.text = "Round: " + roundNumber;

            checkSymbolsText.text = "";
            StartCoroutine(RollTheDice(roundNumber));
            this.gameObject.GetComponent<Image>().sprite = imagePressed;
        }
    }
    public IEnumerator RollTheDice(int roundNum) 
    {
        int randomDiceSide = 0;
        preset = new int[3];

        for(int y = 0; y < 3; y++){
            if(roundNum == 3)
                preset[y] = 3; // SET ROUND 3 WINNING SPRITE
            else
                preset[y] = UnityEngine.Random.Range(0,4);
        }
        
        for(int x = 0; x < 3; x++){
           for (int i = 0; i <= 40; i++)
           {
               randomDiceSide = UnityEngine.Random.Range(0, 4);
               rend[x].sprite = symbolsSides[randomDiceSide];

               if (i == 40) {
                   print(x + " -------  " + i);
                   rend[x].sprite = symbolsSides[preset[x]];
                }

                if(i < 20)
                    yield return new WaitForSeconds(0.05f);
                else if(i > 20 && i > 30)
                    yield return new WaitForSeconds(0.1f);
                else if(i > 30 && i > 40)
                   yield return new WaitForSeconds(0.2f);  
            }
        }

        checkSymbols();

        isPlaying = false;
        this.gameObject.GetComponent<Image>().sprite = imageUnpressed;
    }

    private void checkSymbols(){
        string box1 = rend[0].sprite.name;
        string box2 = rend[1].sprite.name;
        string box3 = rend[2].sprite.name;

        if(box1 == box2 && box1 == box3){
            if(box1 == "Lucky7_rainbow")
              checkSymbolsText.text = "Lucky 7 Bonus Points";
            else
              checkSymbolsText.text = "Matched in 3 Symbols";
        }
        else if(box1 == box2)
            checkSymbolsText.text = "Matched in 2 Symbols";
        else if(box2 == box3)
            checkSymbolsText.text = "Matched in 2 Symbols";
        else if(box1 == box3)
            checkSymbolsText.text = "Matched in 2 Symbols";
        else
            checkSymbolsText.text = "No match!";
  }
}
