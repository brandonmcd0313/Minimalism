using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextController : MonoBehaviour
{
    //All text is sent through this script


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
        playerBox.SetActive(false);
        pollitoBox.SetActive(false);
        purpleBox.SetActive(false);

    }

    public void ShowTextBox(string[] texts, TextType type)
    {


        switch (type)
        {
            case TextType.Player:
                currentBox = playerBox;
                currentText = playerText;
                break;
            case TextType.Pollito:
                currentBox = pollitoBox;
                currentText = pollitoText;
                break;
            case TextType.Purple:
                currentBox = purpleBox;
                currentText = purpleText;
                break;
        }
        //disable all text boxes
        playerBox.SetActive(false);
        pollitoBox.SetActive(false);
        purpleBox.SetActive(false);
        //enable the current text box
        currentBox.SetActive(true);
        currentText.SetActive(true);

        //start the coroutine to display the text
        StartCoroutine(DisplayText(texts));
    }

    IEnumerator DisplayText(string[] texts)
    {
        //disable all movement
        //find every object implementing ICanMove
        ToggleMovement(false);
        //disable player movement
        player.GetComponent<PlayerController>().canMove = false;
        for (int k = 0; k < texts.Length; k++)
        {
            for (int i = 0; i < texts.Length; i++)
            {
                for (int j = 0; j <= texts[i].Length; j++)
                {
                    //display the text one character at a time
                    currentText.GetComponent<TextMeshProUGUI>().text = texts[i].Substring(0, j);
                    Debug.Log(texts[i]);
                    yield return new WaitForSeconds(0.05f);
                }
                //wait until the player presses the space key
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            }
        }
        currentBox.SetActive(false);
        //enable player movement
        player.GetComponent<PlayerController>().canMove = true;
        ToggleMovement(true);
    }

    // Method to find all GameObjects implementing ICanMove and toggle their movement
    public void ToggleMovement(bool enable)
    {
        // Find all GameObjects in the scene
        foreach (GameObject obj in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            // Check if the GameObject has a component that implements ICanMove
            ICanMove movable = obj.GetComponent<ICanMove>();
            if (movable != null)
            {
                // If the bool is true, enable movement, otherwise disable movement
                if (enable)
                {
                    movable.EnableMovement();
                }
                else
                {
                    movable.DisableMovement();
                }
            }
        }
    }
}
