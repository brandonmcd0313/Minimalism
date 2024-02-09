using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2.0f; // Adjust speed as needed
    public float maxHeight = 5.0f; // Maximum height for the platform
    public float minHeight = 0.0f; // Minimum height for the platform
    private int direction = 1; // 1 for moving up, -1 for moving down

    // Update is called once per frame
    void Update()
    {
        // Move the platform up and down
        transform.Translate(Vector3.up * speed * direction * Time.deltaTime);

        // Change direction if reaching max height or min height
        if (transform.position.y >= maxHeight)
        {
            direction = -1; // Move down
        }
        else if (transform.position.y <= minHeight)
        {
            direction = 1; // Move up
        }
    }
}