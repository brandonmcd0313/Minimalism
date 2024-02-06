using System.Collections;
using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    public Transform playerTransform; // Reference to the player's transform
    public float speed = 5f; // Speed at which the particle moves towards the player
    private bool hasReachedPlayer = false;

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
            // Move towards the player
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, step);

            // Check if the particle has reached the player (or is very close)
            if (Vector3.Distance(transform.position, playerTransform.position) < 0.001f)
            {
                hasReachedPlayer = true;
            }

            yield return null; // Wait for the next frame before continuing the loop
        }

        // Wait for 0.5 seconds after reaching the player
        yield return new WaitForSeconds(0.5f);

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
