using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//Red = Fighter(run at player, touching is knock back)
public class RedCivilian : Civilian, IInteractable, ICanMove
{

    private Rigidbody2D rb;
    public GameObject ParticlePrefab;
    private float dirX = -1f;
    [SerializeField] float moveSpeed = 3f;
    public float moveSpeedStorage;
    [SerializeField] float attackDistance = 5f;
    bool isChasingPlayer = false;
    private bool facingRight = false;
    private Vector3 localScale;
    GameObject player;
    [SerializeField] float knockbackForce = 10f;
    bool alive = true;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
    }

    public void Interact()
    {
        if (!alive) return;
        DisableMovement();
        //spawn particle and set its color to the civilian's original color
        GameObject particle = Instantiate(ParticlePrefab, transform.position, Quaternion.identity);
        ParticleEffect particleEffect = particle.GetComponent<ParticleEffect>();
        particleEffect.SetColor(GetComponent<SpriteRenderer>().color);
        FadeColor();
        alive = false;

    }

    public void ShowInteractionPrompt()
    {
        if (!player.GetComponent<PlayerController>().seeRed)
        {
            player.GetComponent<PlayerController>().seeRed = true;
            TextController.Instance.ShowTextBox(new string[] { "This red warrior is much too aggressive for this peaceful world... \n[E to interact]" }, TextController.TextType.Player);
        }
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
                RaycastHit2D hit = Physics2D.Raycast(transform.position, wallHit, 0.75f);
                Debug.DrawRay(transform.position, wallHit * 0.75f, Color.red);

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
        //if player is within 5 units of the civilian, move towards the player
        if (Vector2.Distance(transform.position, player.transform.position) < attackDistance)
        {
            isChasingPlayer = true;
     
        }

        if(isChasingPlayer)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
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

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!alive) return;

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

    public void EnableMovement()
    {
        if (alive)
            moveSpeed = moveSpeedStorage;
    }
}
