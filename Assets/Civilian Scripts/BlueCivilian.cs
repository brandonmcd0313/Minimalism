using System.Collections;
using UnityEngine;

//Blue = Defender (touching them is knock back)
public class BlueCivilian : Civilian, IInteractable
{
    private Rigidbody2D rb;
    public GameObject ParticlePrefab;
    [SerializeField] float knockbackForce = 10f;
    bool alive = true;
    private GameObject player;
    


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
    }

    public void Interact()
    {
        if (!alive) return;
        //spawn particle and set its color to the civilian's original color
        GameObject particle = Instantiate(ParticlePrefab, transform.position, Quaternion.identity);
        ParticleEffect particleEffect = particle.GetComponent<ParticleEffect>();
        particleEffect.SetColor(GetComponent<SpriteRenderer>().color);
        FadeColor();

        //disable constraints on the rigidbody so that the civilian can be pushed around
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        alive = false;
    }

    public void ShowInteractionPrompt()
    {
        if (!player.GetComponent<PlayerController>().seeBlue)
        {
            player.GetComponent<PlayerController>().seeBlue = true;
            TextController.Instance.ShowTextBox(new string[] { "This blue fellow... He is very defensive and stubborn, this isn't right... " }, TextController.TextType.Player);
        }
    }

    public void HideInteractionPrompt()
    {
        Debug.Log("Hide interaction prompt");
    }

    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(!alive) return;

        if (col.gameObject.tag.Equals("Player"))
        {
            //set the player's velocity to 0
            col.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //apply a knockback force to the player opposite to the direction of the collision
            Vector2 refrence = col.gameObject.transform.position - transform.position;
            //apply in the direction of refrence
            col.gameObject.GetComponent<Rigidbody2D>().AddForce(refrence.normalized * knockbackForce, ForceMode2D.Impulse);

        }
    }
}
