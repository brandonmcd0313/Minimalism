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
    public GameObject personAliveness, throneBack;

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
            throneBack.SetActive(false);
            //disable saturation
            SaturationController.instance.EnableSaturation(false);

            // Concatenate the arrays
            List<string> speech = new List<string>(TheHolyWords);
            switch (personAliveness.GetComponent<PersonAlivenessTracker>().GetAlivePercentage())
            {
                case float n when n == 100:
                    //Text for not killing anyone
                    speech.AddRange(Peace);
                    break;
                case float n when n == 0:
                    //Text for killing all of the people
                    speech.AddRange(War);
                    break;
                default:
                    //Text for killing some people
                    speech.AddRange(OkIGuess);
                    break;
            }
            // Add the prophetic texts to the speech
            speech.AddRange(PropheticTexts);

            // Convert List<string> to string array
            TheGreatJourneySpeechOfTheHighestBeing = speech.ToArray();

            // Show the text
            TextController.Instance.ShowTextBox(TheGreatJourneySpeechOfTheHighestBeing, TextController.TextType.Pollito);
        }
    }
}
