using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public enum MovementState
    {
        Idle,
        Walk,
        Follow,
    }

    public MovementState currentState = MovementState.Idle;

    public float idleDuration = 2f;
    public float walkDuration = 5f;
    public float followRange = 5f;
    public Transform player;
    private Animator animator; // Reference to Animator

    private float stateTimer;
    private NavMeshAgent agent;

    void Start()
    {
        player = GameObject.Find("PlayerArmature").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // Get Animator component
        stateTimer = 0f;
    }

    void Update()
    {
        stateTimer += Time.deltaTime;

        switch (currentState)
        {
            case MovementState.Idle:
                if (stateTimer >= idleDuration)
                {
                    currentState = MovementState.Walk;
                    stateTimer = 0f;
                    SetRandomDestination();
                }
                break;

            case MovementState.Walk:
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    currentState = MovementState.Idle;
                    stateTimer = 0f;
                }
                break;
        }
        UpdateAnimator();

        if (Vector3.Distance(transform.position, player.position) <= followRange)
        {
            currentState = MovementState.Follow;
            agent.SetDestination(player.transform.position);
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
        float speed = agent.velocity.magnitude / agent.speed; 
        animator.SetFloat("speed", speed); 
    }


}