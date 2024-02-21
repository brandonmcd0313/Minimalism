using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public abstract class Civilian : MonoBehaviour
{

    bool isInteractable = true;
    bool isAlive = true;

    protected virtual void FadeColor()
    {
        if (!isInteractable)
        {
            return;
        }

        isInteractable = false;
        isAlive = false;
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
