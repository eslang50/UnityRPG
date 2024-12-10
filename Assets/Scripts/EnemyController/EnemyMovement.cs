using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public enum MovementState
    {
        Idle,
        Walk,
        Follow,
        Attack, // New Attack state
    }

    public MovementState currentState = MovementState.Idle;

    public float idleDuration = 2f;
    public float walkDuration = 5f;
    public float followRange = 5f;
    public float attackRange = 1.5f; // Range for attacking
    public float attackCooldown = 2f; // Cooldown between attacks
    public Transform player;

    private Animator animator;
    private float stateTimer;
    private NavMeshAgent agent;
    private float attackTimer; // Timer to handle attack cooldown

    void Start()
    {
        player = GameObject.Find("PlayerArmature").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        stateTimer = 0f;
        attackTimer = 0f;
    }

    void Update()
    {

        stateTimer += Time.deltaTime;
        attackTimer -= Time.deltaTime; // Reduce attack cooldown timer

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Update state based on distance to player
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
            stateTimer = 0f;
        }


        // Handle states
        switch (currentState)
        {
            case MovementState.Idle:
                HandleIdleState();
                break;

            case MovementState.Walk:
                HandleWalkState();
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
        if (stateTimer >= idleDuration)
        {
            currentState = MovementState.Walk;
            stateTimer = 0f;
            SetRandomDestination();
        }
    }

    void HandleWalkState()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            currentState = MovementState.Idle;
            stateTimer = 0f;
        }
    }

    void HandleFollowState()
    {
        agent.SetDestination(player.position);
    }

    void HandleAttackState()
    {
        agent.ResetPath(); // Stop moving

        // If attack cooldown has passed, trigger attack
        if (attackTimer <= 0)
        {
            attackTimer = attackCooldown; // Reset attack cooldown
        }

        // Exit attack state if player moves out of range
        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            currentState = MovementState.Follow;
        }
    }

    void SetRandomDestination()
    {
        float randomAngle = Random.Range(0f, 360f);
        Vector3 randomDirection = new Vector3(Mathf.Cos(randomAngle), 0f, Mathf.Sin(randomAngle));
        Vector3 randomPoint = transform.position + randomDirection * Random.Range(5f, 10f);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    void UpdateAnimator()
    {
        float speed = agent.velocity.magnitude / agent.speed; // Normalize speed
        animator.SetFloat("speed", speed); // Update Speed parameter

        // Set "isAttacking" parameter for animation transitions
        animator.SetBool("attack01", currentState == MovementState.Attack);

    }

    // Optional: Damage the player (requires a player health script)
    public void DealDamage()
    {
        // Logic to deal damage to the player
        Debug.Log("Enemy dealt damage to the player!");
    }

}
