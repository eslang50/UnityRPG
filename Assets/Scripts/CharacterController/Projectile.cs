using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 3f;
    public float speed = 10f;

    private Rigidbody rb;

    void Start()
    {
        // Cache the Rigidbody reference
        rb = GetComponent<Rigidbody>();

        // Ensure the Rigidbody exists
        if (rb != null)
        {
            // Set the projectile's initial velocity
            rb.velocity = transform.forward * speed;
        }

        // Destroy the projectile after the lifetime expires
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Handle collision logic, e.g., apply damage or effects
        Debug.Log($"Projectile hit {collision.gameObject.name}");

        // Destroy the projectile on collision
        Destroy(gameObject);
    }
}
