using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextController : MonoBehaviour
{
    //All text is sent through this script

   public GameObject Canvas;
   public GameObject TextBox;
    public static TextController Instance;
    public GameObject player;

    public GameObject playerBox, pollitoBox, purpleBox;
    public GameObject playerText, pollitoText, purpleText;

    GameObject currentBox;
    GameObject currentText;
    public enum TextType
    {
        Player,
        Pollito,
        Purple
    }
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
       //disable the canvas and text box
       Canvas.SetActive(false);
        TextBox.SetActive(false);
      
    }

    public void ShowTextBox(string[] texts, TextType type)
    {
       

        switch (type)
        {
            case TextType.Player:
                Debug.Log("HIT");
                currentBox = playerBox;
                currentText = playerText;
                playerText.GetComponent<TextMeshProUGUI>().color = Color.white;
                break;
            case TextType.Pollito:
                currentBox = pollitoBox;
                currentText = pollitoText;
                pollitoText.GetComponent<TextMeshProUGUI>().color = Color.black;
                break;
            case TextType.Purple:
                currentBox = purpleBox;
                currentText = purpleText;
                purpleText.GetComponent<TextMeshProUGUI>().color = Color.white;
                break;
        }   

        Canvas.SetActive(true);
        //disable all text boxes
        playerBox.SetActive(false);
        pollitoBox.SetActive(false);
        purpleBox.SetActive(false);
        //enable the current text box
        currentBox.SetActive(true);

        //start the coroutine to display the text
        StartCoroutine(DisplayText(texts));
    }

    IEnumerator DisplayText(string[] texts)
    {
        //disable player movement
        player.GetComponent<PlayerController>().canMove = false;
        for (int i = 0; i < texts.Length; i++)
        {
            for(int j = 0; j < texts[i].Length; j++)
            {
                //display the text one character at a time
                TextBox.GetComponent<TextMeshProUGUI>().text = texts[i].Substring(0, j);
                yield return new WaitForSeconds(0.05f);
            }
           //wait until the player presses the space key
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }
        //disable the canvas and text box
        Canvas.SetActive(false);
        currentBox.SetActive(false);
        //enable player movement
        player.GetComponent<PlayerController>().canMove = true;
    }
}
