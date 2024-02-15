using System.Collections;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb2d;
    public float speed = 15;
    public float jump = 400;
    public GameObject platTest1, platTest2;
    public bool faceRight;
    public TextMeshProUGUI interactText;
    private IInteractable interactableInRange;
    public bool canDoubleJump = true;
    int doubleJump = 0;
    public GameObject background, textBoxBack;
    public TextMeshProUGUI textBox;
    public bool canMove = true;
    public Sprite playerBox, pollitoBox;
    //fonts for tmpro
    public TMP_FontAsset regFont, pollitoFont;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        faceRight = true;
    }

    void Update()
    {
        if (!canMove)
        {
            return;
        }
            HandleMovement();
            HandleJump();
            HandleInteraction();
        
        if(Input.GetKeyDown(KeyCode.R))
        {
            transform.position = new Vector3(450, 190, 0);
        }
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

    public void HideText()
    {
        textBoxBack.SetActive(false);
        canMove = true;
    }

    public void Speak(string words, Sprite textBack)
    {
        textBoxBack.gameObject.SetActive(true);
        textBox.text = "";
        if (textBack == pollitoBox)
        {
            textBox.color = Color.black;
            textBox.font = pollitoFont;
            textBoxBack.transform.localScale = new Vector3(40f, 40f, 40f);
        }
        else
        {
            textBox.color = Color.white;
            textBox.font = regFont;
            textBoxBack.transform.localScale = new Vector3(60f, 60f, 60f);
        }

        textBoxBack.GetComponent<SpriteRenderer>().sprite = textBack;

        //turn off player movement and kill all player movement
        canMove = false;
        rb2d.velocity = Vector2.zero;


        //text scrolling effect
        StartCoroutine(ScrollText(words));
    }

    IEnumerator ScrollText(string words)
    {
        textBox.text = "";
        foreach (char letter in words.ToCharArray())
        {
            textBox.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
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
            //make a raycast down
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 2f);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Platform" || hit.collider.gameObject.tag == "Person" || hit.collider.gameObject.tag == "Soul")
                {
                    rb2d.AddForce(new Vector3(0, jump));
                    canDoubleJump = true;
                    doubleJump = 0;
                }
            }
            else
            {
                doubleJump++;
                if (doubleJump >= 2)
                {
                  canDoubleJump = false;
                }
                else
                {
                    rb2d.AddForce(new Vector3(0, jump));
                }
            }
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

        if(collision.tag == "Pollito")
        {
            //turn off player movement
            canMove = false;

            //Kill all player movement
            rb2d.velocity = Vector2.zero;

            


            //Change the background to pollitoBackground
            background.gameObject.SetActive(false);
            StartCoroutine(Pollito());        
        }
    }

    IEnumerator Pollito()
    {
        while (transform.position != new Vector3(485, 189.8448f, 0) || Camera.main.orthographicSize != 15) {
            //Gradually move player to 480 189.8448 using moveTowards
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(485, 189.8448f, 0), 0.1f);

            //gradually zoom camera to orthographic size 10
            Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, 15, 0.1f);
            yield return new WaitForSeconds(0.05f);
        }
        Speak("MY CHILD, IT IS I... POLLITO...", pollitoBox);
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
