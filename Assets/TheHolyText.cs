using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TheHolyText : MonoBehaviour
{

    public string[] TheHolyWords;
    public string[] Peace;
    public string[] War;
    public string[] OkIGuess;
    public string[] PropheticTexts;
    public string[] TheGreatJourneySpeechOfTheHighestBeing;
    public GameObject personAliveness;

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
            TheGreatJourneySpeechOfTheHighestBeing = TheHolyWords.Concat(TheHolyWords).ToArray();
            switch (personAliveness.GetComponent<PersonAlivenessTracker>().GetAlivePercentage())
            {
                case float n when n == 100:
                    //Text for not killing anyone
                    TheGreatJourneySpeechOfTheHighestBeing = TheHolyWords.Concat(Peace).ToArray();
                    break;
                case float n when n == 0:
                    //Text for killing all of the people
                    TheGreatJourneySpeechOfTheHighestBeing = TheHolyWords.Concat(War).ToArray();
                    break;
                default:
                    //Text for killing some people
                    TheGreatJourneySpeechOfTheHighestBeing = TheHolyWords.Concat(OkIGuess).ToArray();
                    break;
            }
            //add the prophetic texts to thegreatjourneyspeechofthehighestbeing
            TheGreatJourneySpeechOfTheHighestBeing = TheHolyWords.Concat(PropheticTexts).ToArray();
            TextController.Instance.ShowTextBox(TheGreatJourneySpeechOfTheHighestBeing, TextController.TextType.Pollito);
        }
    }
}
