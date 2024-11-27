using UnityEngine;

public class AttackController : MonoBehaviour
{
    private Animator animator;
    public GameObject projectilePrefab; // Assign the projectile prefab in the Inspector
    public Transform projectileSpawnPoint; // The spawn point under the wand or hand

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Basic attack
        {
            animator.SetTrigger("BasicAttackOne");
            FireProjectile(); // Fire the projectile when BasicAttackOne is triggered
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) // Special attack
        {
            animator.SetTrigger("SpecialAttackOne");
        }
        else if (Input.GetKeyDown(KeyCode.Q)) // Melee attack
        {
            animator.SetTrigger("MeleeAttack");
        }
    }

    void FireProjectile()
    {
        // Get the player's forward direction (the direction the player is facing)
        Vector3 forwardDirection = transform.forward; // or use a specific bone like "projectileSpawnPoint.forward" if needed

        // Instantiate the projectile at the spawn point and make it face the correct direction
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

        // Optional: If you want to ensure the projectile's forward direction is aligned with the player's facing direction:
        projectile.transform.rotation = Quaternion.LookRotation(forwardDirection);
    }
}
