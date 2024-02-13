using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    public float rotationInterval = 1f; // Time interval in seconds
    private float timeElapsed = 0f;

    void Update()
    {
        // Increment the time elapsed
        timeElapsed += Time.deltaTime;

        // If the time elapsed exceeds the rotation interval, rotate the platform
        if (timeElapsed >= rotationInterval)
        {
            // Rotate the platform by 90 degrees around its Y-axis
            transform.Rotate(0f, 90f, 0f);

            // Reset the time elapsed
            timeElapsed = 0f;
        }
    }
}
