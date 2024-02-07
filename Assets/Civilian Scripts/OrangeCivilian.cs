using System.Collections;
using UnityEngine;

//Orange = Basic (walks back and forth)
public class OrangeCivilian : Civilian, IInteractable, ICanMove
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
        RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down, 1f);
        Debug.DrawRay(transform.position, Vector2.down, Color.green);

        if (groundHit.collider)
        {
            Vector2 forwardRayStart = transform.position + transform.right * dirX * 0.5f;
            RaycastHit2D forwardHit = Physics2D.Raycast(forwardRayStart, Vector2.down, 1f);
            Debug.DrawRay(forwardRayStart, Vector2.down, Color.blue);

            if (!forwardHit.collider)
            {
                Flip();
            }

            else
            {
                Vector2 wallHit = transform.right * dirX;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, wallHit, 0.5f);
                Debug.DrawRay(transform.position, wallHit * 0.5f, Color.red);

                // Check if the hit object is not tagged "Player" before flipping
                if (hit.collider && hit.collider.tag != "Player")
                {
                    Flip();
                }
            }

            Move();
        }
    }

    public void Move()
    {
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
    }

    public void Flip()
    {
        dirX *= -1;
        facingRight = !facingRight;
        localScale.x *= -1;
        transform.localScale = localScale;
    }


    public void DisableMovement() // Implementing DisableMovement method from ICanMove
    {
        moveSpeed = 0;
    }
}