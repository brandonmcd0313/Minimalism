using System.Collections;
using UnityEngine;

//Blue = Defender (touching them is knock back)
public class BlueCivilian : Civilian, IInteractable, ICanMove
{
    private Rigidbody2D rb;
    public GameObject ParticlePrefab;
    private float dirX = -1f;
    [SerializeField] float moveSpeed = 3f;
    private bool facingRight = false;
    private Vector3 localScale;


    // Start is called before the first frame update
    void Start()
    {
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
    }

    public void Interact()
    {
        DisableMovement();
        //spawn particle and set its color to the civilian's original color
        GameObject particle = Instantiate(ParticlePrefab, transform.position, Quaternion.identity);
        ParticleEffect particleEffect = particle.GetComponent<ParticleEffect>();
        particleEffect.SetColor(GetComponent<SpriteRenderer>().color);
        FadeColor();
    }

    public void ShowInteractionPrompt()
    {
        Debug.Log("Show interaction prompt");
    }

    public void HideInteractionPrompt()
    {
        Debug.Log("Hide interaction prompt");
    }

    void Update()
    {
        // Cast a ray straight down to check for ground
        RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down, 1f);
        Debug.DrawRay(transform.position, Vector2.down, Color.green);

        if (groundHit.collider)
        {
            // The player is on the ground, proceed with movement calculations
            Vector2 rayDirection = dirX > 0 ? new Vector2(0.5f, -1).normalized : new Vector2(-0.5f, -1).normalized;

            // Cast the ray slightly angled based on the movement direction
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, 1f);
            Debug.DrawRay(transform.position, rayDirection, Color.red);

            // If the ray does not hit anything, it's time to flip
            if (!hit.collider)
            {
                Flip(); // Call Flip if needed
            }

            Move(); // Call Move only if the player is on the ground
        }
        else
        {
            // The player is not on the ground, so they should just fall due to gravity
            // Ensure that gravity is properly set in the Rigidbody2D component to allow falling
            // No need to call Move or Flip in this case
        }
    }

    public void Move() // Implementing Move method from ICanMove
    {
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
    }

    public void Flip() // Implementing Flip method from ICanMove
    {
        dirX = -dirX; // Reverse direction
        facingRight = !facingRight;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public void DisableMovement() // Implementing DisableMovement method from ICanMove
    {
        moveSpeed = 0;
    }
}
