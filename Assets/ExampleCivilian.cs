using System.Collections;
using UnityEngine;

public class ExampleCivilian : Civilian, IInteractable, ICanMove
{
    private Rigidbody2D rb;
    public ParticleSystem particle;
    private float dirX = -1f;
    private float moveSpeed = 3f;
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


    protected override void FadeColor()
    {
        //can add stuff like an inital attack here...
        base.FadeColor(); // Call the base interaction logic, which changes the color to gray
        base.StealColor();
    }
    void Update()
    {
        Move(); // Call Move every frame

        // Calculate the angle of the ray based on the current direction of movement
        // Angle the ray slightly in the direction the character is moving
        Vector2 rayDirection = dirX > 0 ? new Vector2(0.5f, -1).normalized : new Vector2(-0.5f, -1).normalized;

        // Cast the ray slightly angled based on the movement direction
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, 1f);
        Debug.DrawRay(transform.position, rayDirection, Color.red);

        // If the ray does not hit anything, it's time to flip
        if (!hit.collider)
        {
            Flip(); // Call Flip if needed
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
