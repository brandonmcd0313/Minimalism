using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheHolyText : MonoBehaviour
{

    public string[] TheHolyWords;
    public string[] Peace;
    public string[] War;
    public string[] OkIGuess;
    public string[] PropheticTexts;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            TextController.Instance.ShowTextBox(TheHolyWords, TextController.TextType.Pollito);
            switch(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>())
            {
                //Text for not killing anyone
                TextController.Instance.ShowTextBox(Peace, TextController.TextType.Pollito);
            }
        }
    }
}
