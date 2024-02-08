using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb2d;
    public float speed = 15;
    public float jump = 400;
    public GameObject platTest1, platTest2;
    public bool faceRight;
    public TextMeshProUGUI interactText;
    private IInteractable interactableInRange;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        faceRight = true;
    }

    void Update()
    {

        HandleMovement();
        HandleJump();
        HandleInteraction();
    }



private void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        if (moveHorizontal > 0 && !faceRight || moveHorizontal < 0 && faceRight)
        {
            Flip();
        }

        // Apply a force for movement
        rb2d.AddForce(new Vector2(moveHorizontal * speed * Time.deltaTime, 0));

        // Optional: Limit the maximum speed to prevent the player from accelerating indefinitely
        rb2d.velocity = new Vector2(Mathf.Clamp(rb2d.velocity.x, -10f, 10f), rb2d.velocity.y);
    }

    private void Flip()
    {
        faceRight = !faceRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    private void HandleJump()
    {
        bool canJump = Physics2D.OverlapArea(platTest1.transform.position, platTest2.transform.position) != null;
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            rb2d.AddForce(new Vector3(0, jump));
        }
    }

    private void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E) && interactableInRange != null)
        {
            interactableInRange.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check the collided object and its parent for the IInteractable component
        IInteractable interactable = collision.GetComponent<IInteractable>() ?? collision.transform.parent.GetComponent<IInteractable>();

        // Proceed only if an IInteractable component is found
        if (interactable != null)
        {
            interactableInRange = interactable; // Set the current interactable object
            interactableInRange.ShowInteractionPrompt(); // Show interaction prompt

            // Activate the interaction text UI, if it exists and is assigned
            /*
            if (interactText != null)
            {
                interactText.gameObject.SetActive(true);
                // Adjust the position of the interaction text UI based on the interactable object's position
                interactText.gameObject.transform.position = Camera.main.WorldToScreenPoint(collision.transform.position + new Vector3(0, 2.0f, 0));
            }
            */
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check the collided object and its parent for the IInteractable component
        IInteractable interactable = collision.GetComponent<IInteractable>() ?? collision.transform.parent.GetComponent<IInteractable>();

        // Proceed only if an IInteractable component is found and matches interactableInRange
        if (interactable != null && interactableInRange == interactable)
        {
            interactableInRange.HideInteractionPrompt(); // Hide interaction prompt
            interactableInRange = null; // Clear the current interactable object

            // Deactivate the interaction text UI, if it exists and is assigned
            if (interactText != null)
            {
                interactText.gameObject.SetActive(false);
            }
        }
    }

}
