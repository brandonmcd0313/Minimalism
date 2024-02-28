using System.Collections;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, ICanMove
{
    Rigidbody2D rb2d;
    public float speed = 15;
    public float jump = 400;
    public GameObject platTest1, platTest2;
    public bool faceRight;
    private IInteractable interactableInRange;
    public bool canDoubleJump = true;
    int doubleJump = 0;
    public GameObject background, textBoxBack;
    public TextMeshProUGUI textBox;
    public bool canMove = true;
    public bool seeOrange = false;
    public bool seePurple = false;
    public bool seeBlue = false;
    public bool seeRed = false;
    public bool seeYellow = false;
    public bool seePollito = false;
   
    string[] pollitoText = { "MY CHILD, IT IS I... POLLITO...", "YOU ARE A CLUCKING MENACE TO SOCIETY", "BOOM, EXPLOSION, YOU DEAD"};

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        faceRight = true;
       Invoke("ShowStartText", 0.5f);
    }
    void ShowStartText()
    {
        TextController.Instance.ShowTextBox(new string[] { "This color... The world is too noisey! I must minimalize this chaos...\n\t\t[SPACE] " }, TextController.TextType.Player);
    }
    void Update()
    {
        Move();
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

        if (collision.tag == "Pollito")
        {
            if (seePollito)
                SceneManager.LoadScene("Playground");
            else
            {
                seePollito = true;
                //turn off player movement
                canMove = false;

                //Kill all player movement
                rb2d.velocity = Vector2.zero;



                //Change the background to pollitoBackground
                background.gameObject.SetActive(false);
                StartCoroutine(Pollito());
            }
        }
        if(collision.tag == "Gate to Heaven")
        {
            //find pollito game object
            GameObject personAliveness = GameObject.Find("PollitoTrigger").GetComponent<TheHolyText>().personAliveness;
            float alivePercent = personAliveness.GetComponent<PersonAlivenessTracker>().GetAlivePercentage();


            switch (alivePercent)
            {
                case float n when n == 100:
                    //Text for not killing anyone
                    TextController.Instance.ShowTextBox(new string[] { "Congratulations, you have lived your great life. Come, join me in eternal bread crumbs..." }, TextController.TextType.Pollito);
                    break;
                case float n when n == 0:
                    //Text for killing all of the people
                    TextController.Instance.ShowTextBox(new string[] { "You have killed everyone. You are a clucking menace to society. You are not worthy of eternal bread crumbs." }, TextController.TextType.Pollito);
                    break;
                default:
                    //Text for killing some people
                    TextController.Instance.ShowTextBox(new string[] { "Hmm... There are worse... I shall allow you to join me, but you shall not have any bread crumbs..." }, TextController.TextType.Pollito);
                    break;
            }
            StartCoroutine(EndGame());
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
        textBox.color = Color.black;
        textBoxBack.transform.localScale = new Vector3(50f, 50f, 50f);
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(7);
        SceneManager.LoadScene("StartMenu");
    }

    public void Move()
    {
        if (!canMove)
        {
            return;
        }
        HandleMovement();
        HandleJump();
        HandleInteraction();
    }

    void ICanMove.Flip()
    {
        throw new System.NotImplementedException();
    }

    public void DisableMovement()
    {
        canMove = false;
    }

    public void EnableMovement()
    {
        canMove = true;
    }
}
