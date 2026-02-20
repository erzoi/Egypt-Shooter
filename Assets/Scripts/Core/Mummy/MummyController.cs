using UnityEngine;
using UnityEngine.AI;

public class MummyController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private Transform player;
    private MummyHealth health;

    [SerializeField] private float attackDistance = 2f;
    [SerializeField] private float attackCooldown = 1.5f;

    private float attackTimer;
    private bool isDead;

    private void Awake()
    {
        health = GetComponent<MummyHealth>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;

        agent.stoppingDistance = attackDistance;
        agent.updateRotation = false;

        health.OnDead += Die;
    }

    private void Update()
    {
        if (isDead) return;

        agent.SetDestination(player.position);

        UpdateMovement();
        HandleAttack();
    }

    private void UpdateMovement()
    {
        if (agent.pathPending) return;

        float distance = agent.remainingDistance;

        if (distance > agent.stoppingDistance)
        {
            // Двигаемся
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }
        else
        {
            // Остановились
            animator.SetFloat("Speed", 0f);
        }

        RotateTowardsPlayer();
    }

    private void RotateTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            10f * Time.deltaTime
        );
    }

    private void HandleAttack()
    {
        if (agent.pathPending) return;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackCooldown)
            {
                attackTimer = 0f;
                animator.SetTrigger("Attack");
            }
        }
        else
        {
            attackTimer = 0f;
        }
    }

    public void Die()
    {
        isDead = true;
        agent.enabled = false;
        GetComponent<Collider>().enabled = false;

        animator.SetBool("IsDead", true);
        animator.SetTrigger("DieForward");

        Destroy(gameObject, 5f);
    }
}
