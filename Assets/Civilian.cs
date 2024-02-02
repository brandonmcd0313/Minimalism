using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Civilian : MonoBehaviour
{
    bool isInteractable = true;
    private Rigidbody2D rb;
    private float dirX = -1f;
    private float moveSpeed = 3f;
    private bool facingRight = false;
    private Vector3 localScale;
    public GameObject rightTest, leftTest;



    // Start is called before the first frame update
    void Start()
    {
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Raycast to check if the civilian is still on the platform
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f);
        Debug.DrawRay(transform.position, Vector2.down, Color.red);
        if (!hit.collider)
        {
            dirX = -dirX;
        }
    }

    private void FixedUpdate()
    {
        

        //Check if the civilian is still on the platform
        if (Physics2D.OverlapArea(rightTest.transform.position, leftTest.transform.position) == null)
        {
            dirX = -dirX;
        }

        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
    }

    protected virtual void FadeColor()
    {
        if (!isInteractable)
        {
            return;
        }
        isInteractable = false;

        Debug.Log("Interacting with civilian");

        //change the color of the civilian to be gray
        StartCoroutine(ChangeSpriteColorToGray());
    }

    IEnumerator ChangeSpriteColorToGray()
    {
        // Get the SpriteRenderer component of the object
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        // Store the original color of the sprite
        Color originalColor = spriteRenderer.color;

        // Target gray color
        Color grayColor = Color.gray;

        // Duration of the color transition
        float duration = 2.0f;

        // Track the time passed
        float timePassed = 0f;

        while (timePassed < duration)
        {
            // Calculate the fraction of the duration that has passed
            float lerpFraction = timePassed / duration;

            // Interpolate between the original color and gray based on the fraction
            Color newColor = Color.Lerp(originalColor, grayColor, lerpFraction);

            // Apply the interpolated color to the sprite
            spriteRenderer.color = newColor;

            // Increment the time passed by the time of the last frame
            timePassed += Time.deltaTime;

            // Wait until the next frame before continuing the loop
            yield return null;
        }

        // Ensure the final color is set to gray in case of any rounding errors
        spriteRenderer.color = grayColor;
    }

}
