using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    public Transform playerTransform; // Reference to the player's transform
    public float speed = 5f; // Speed at which the particle moves towards the player
    private bool hasReachedPlayer = false;
    public float accelerationFactor = 0.1f; // Factor to control how much the speed increases with distance

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Player").transform; // Find the player's transform
        StartCoroutine(MoveTowardsPlayer());
    }


    IEnumerator MoveTowardsPlayer()
    {
        // Keep moving towards the player until close enough
        while (!hasReachedPlayer)
        {
            // Calculate dynamic speed based on distance to the player
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            float dynamicSpeed = speed + (distanceToPlayer * accelerationFactor);

            // Move towards the player with dynamic speed
            float step = dynamicSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, step);

            // Check if the particle has reached the player (or is very close)
            if (distanceToPlayer < 0.25f)
            {
                hasReachedPlayer = true;
            }

            yield return null; // Wait for the next frame before continuing the loop
        }

        //has reached the player, keep following but only for hal a second
        for(float i = 0; i < 0.5f; i += Time.deltaTime)
        {
            transform.position = playerTransform.position;
            yield return null;
        }

        // Destroy the particle effect
        Destroy(gameObject);
    }



    // Method to set the color of the particle effect
    public void SetColor(Color color)
    {
        // Find the Particle System component and set its start color
        var particleSystem = GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            var mainModule = particleSystem.main;
            mainModule.startColor = color;
        }
    }
}
