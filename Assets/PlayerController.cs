using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; //loading levels
using UnityEngine.UI; //UI text and images

public class PlayerController : MonoBehaviour {

    Rigidbody2D rb2d; // reference to RigidBody2d
    public float speed = 15;
    public float jump = 400;
    public GameObject platTest1, platTest2;
    public bool faceRight;
    public TextMeshProUGUI interactText;



	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        faceRight = true;
        //aud = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        //Left-right movement
        //A = left, D = Right
        if (Input.GetKey(KeyCode.A))
        {
            if (faceRight)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                faceRight = !faceRight;
            }

            transform.position -= new Vector3(speed * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            if (!faceRight)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                faceRight = !faceRight;
            }

            transform.position += new Vector3(speed * Time.deltaTime, 0);
            faceRight = true;
        }

        //Thruster (F)
        if (Input.GetKey(KeyCode.F))
        {
            if (faceRight)
            {
                transform.position += new Vector3(speed * 3 * Time.deltaTime, 0);
            }
            else
            {
                transform.position -= new Vector3(speed * Time.deltaTime, 0);
            }

        }

        //Jump (= SPACE) when on a platform, not air
        bool canJump = true;
        if (Physics2D.OverlapArea(platTest1.transform.position, platTest2.transform.position))
            canJump = (Physics2D.OverlapArea(platTest1.transform.position, platTest2.transform.position).tag == "Platform") || (Physics2D.OverlapArea(platTest1.transform.position, platTest2.transform.position).tag == "Enemy Projectile");
        else canJump = false;

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            rb2d.AddForce(new Vector3(0, jump));
        }


        /*Fire (left click)
        if (Input.GetMouseButtonDown(0))
        {
            //create player projectile, player position, default rotation
            Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //"pickup" tag collision
        if(collision.tag == "Pickup")
        {

        }

        ///respawn zone collision
        if(collision.tag == "Respawn")
        {
            SceneManager.LoadScene(0);
        }

        //Enemy shot collision
        if(collision.tag == "Enemy Projectile")
        {
            //reload the scene due to death. He is Spartan 1 now
            SceneManager.LoadScene(0);
        }

        if(collision.tag == "Soul")
        {
            interactText.gameObject.SetActive(true);
            interactText.gameObject.transform.position = Camera.main.WorldToScreenPoint(collision.gameObject.transform.position + new Vector3(0, 2.0f, 0));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Soul")
        {
            interactText.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //"enemy" tag collision
        if(collision.gameObject.tag == "Enemy"){
            print("HYAH!");
        }
    }
}
