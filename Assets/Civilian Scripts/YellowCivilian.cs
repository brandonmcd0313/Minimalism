using System.Collections;
using UnityEngine;

//Yellow = Coward (Run from player, but slower than player, can fall off platforms)
public class YellowCivilian : Civilian, IInteractable, ICanMove
{
    private Rigidbody2D rb;
    public GameObject ParticlePrefab;
    private float dirX = -1f;
    [SerializeField] float moveSpeed;
    public float moveSpeedStorage;
    bool isFleeingPlayer = false;
    [SerializeField] float attackDistance = 5f;
    private bool facingRight = false;
    private Vector3 localScale;
    GameObject player;
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
        if (!player.GetComponent<PlayerController>().seeYellow)
        {
            player.GetComponent<PlayerController>().seeYellow = true;
            TextController.Instance.ShowTextBox(new string[] { "Oh, poor yellow... you are much to cowardly for this kind world... \n[E to interact]" }, TextController.TextType.Player);
        }
    }

    public void HideInteractionPrompt()
    {
        Debug.Log("Hide interaction prompt");
    }

    void Update()
    {
        Move();
    }

    public void Move()
    {
        //if player is within 5 units of the civilian, move towards the player
        if (Vector2.Distance(transform.position, player.transform.position) < attackDistance)
        {
            isFleeingPlayer = true;

        }

        if (isFleeingPlayer)
        {
            //move away from the player
            transform.position = Vector2.MoveTowards(transform.position, transform.position * 2 - player.transform.position, moveSpeed * Time.deltaTime);

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

    public void EnableMovement() // Implementing EnableMovement method from ICanMove
    {
        if (alive)
            moveSpeed = moveSpeedStorage;
    }
}