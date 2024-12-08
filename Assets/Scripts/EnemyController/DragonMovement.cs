using UnityEngine;
using UnityEngine.AI;

public class DragonMovement : MonoBehaviour
{
    public enum MovementState
    {
        Idle,
        Follow,
        Attack
    }

    public MovementState currentState = MovementState.Idle;

    public float followRange = 15f; // Range to start following
    public float attackRange = 5f;  // Range for attacking
    public float attackCooldown = 2f; // Cooldown between attacks
    public Transform player;

    private Animator animator;
    private NavMeshAgent agent;
    private float attackTimer; // Timer to handle attack cooldown

    void Start()
    {
        player = GameObject.Find("PlayerArmature").transform; // Find the player
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        attackTimer = 0f; // Initialize attack cooldown timer
    }

    void Update()
    {
        attackTimer -= Time.deltaTime; // Reduce attack cooldown timer
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Update state based on distance to the player
        if (currentState != MovementState.Attack && distanceToPlayer <= attackRange)
        {
            currentState = MovementState.Attack;
        }
        else if (currentState != MovementState.Attack && distanceToPlayer <= followRange)
        {
            currentState = MovementState.Follow;
        }
        else if (currentState == MovementState.Follow && distanceToPlayer > followRange)
        {
            currentState = MovementState.Idle;
        }

        // Handle the current state
        switch (currentState)
        {
            case MovementState.Idle:
                HandleIdleState();
                break;

            case MovementState.Follow:
                HandleFollowState();
                break;

            case MovementState.Attack:
                HandleAttackState();
                break;
        }

        UpdateAnimator();
    }

    void HandleIdleState()
    {
        agent.ResetPath(); // Stop movement
    }

    void HandleFollowState()
    {
        agent.SetDestination(player.position); // Move toward the player
    }

    void HandleAttackState()
    {
        agent.ResetPath(); // Stop moving to attack

        // If attack cooldown has passed, perform attack logic
        if (attackTimer <= 0)
        {
            attackTimer = attackCooldown; // Reset the cooldown
            PerformAttack(); // Perform the attack action
        }

        // Exit attack state if player moves out of range
        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            currentState = MovementState.Follow;
        }
    }

    void PerformAttack()
    {
        // Logic to handle attack (e.g., deal damage, play animation)
        Debug.Log("Dragon attacks the player!");
    }

    void UpdateAnimator()
    {
        float speed = agent.velocity.magnitude / agent.speed; // Normalize speed
        animator.SetFloat("speed", speed); // Update Speed parameter

        // Set "isAttacking" parameter for animation transitions
        animator.SetBool("attack01", currentState == MovementState.Attack);
    }
}
