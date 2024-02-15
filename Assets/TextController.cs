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

    public void ShowTextBox(string[] texts)
    {
        //enable the canvas and text box
        Canvas.SetActive(true);
        TextBox.SetActive(true);


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
        TextBox.SetActive(false);
        //enable player movement
        player.GetComponent<PlayerController>().canMove = true;
    }
}
