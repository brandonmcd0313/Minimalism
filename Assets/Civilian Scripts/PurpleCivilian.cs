using System.Collections;
using UnityEngine;

//Blue = Defender (touching them is knock back)
public class PurpleCivilian : Civilian, IInteractable
{
    private Rigidbody2D rb;
    public GameObject ParticlePrefab;
    public string[] PromptTexts;
    public string[] DeathTexts;
    bool hasBeenApproached = false;
    


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Interact()
    {
        //spawn particle and set its color to the civilian's original color
        GameObject particle = Instantiate(ParticlePrefab, transform.position, Quaternion.identity);
        ParticleEffect particleEffect = particle.GetComponent<ParticleEffect>();
        particleEffect.SetColor(GetComponent<SpriteRenderer>().color);
        FadeColor();

        //disable constraints on the rigidbody so that the civilian can be pushed around
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        TextController.Instance.ShowTextBox(DeathTexts, TextController.TextType.Purple);
    }

    public void ShowInteractionPrompt()
    {
        Debug.Log("Show interaction prompt");
        if(!hasBeenApproached)
        {
            hasBeenApproached = true;
            //show the interaction prompt
            TextController.Instance.ShowTextBox(PromptTexts, TextController.TextType.Purple);
        }
    }

    public void HideInteractionPrompt()
    {
        Debug.Log("Hide interaction prompt");
    }

    void Update()
    {

    }

   
}
