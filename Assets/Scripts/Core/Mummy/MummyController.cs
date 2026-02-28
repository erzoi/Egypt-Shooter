using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class MummyController : MonoBehaviour, ISpawnable
{
    [Header("Settings")]
    [SerializeField] private float attackDistance = 2f;
    [SerializeField] private float rotationSpeed = 8f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private int baseScoreReward = 10;

    [Header("Attack Settings")]
    [SerializeField] private float baseDamage = 3;
    [SerializeField] private float attackRadius = 0.8f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask playerLayer;

    private Transform target;
    private NavMeshAgent agent;
    private Animator animator;
    private MummyHealth health;
    private ObjectSpawner spawner;

    private float currentDamage;
    private float lastAttackTime;
    private bool blockMovement;
    private bool isDead;

    private void Awake()
    {
        health = GetComponent<MummyHealth>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;

        agent.stoppingDistance = attackDistance / 2f;
        agent.speed = baseSpeed;
        currentDamage = baseDamage;
        
        health.OnDead += Die;
    }

    private void Start()
    {
        ApplyDifficulty();
    }

    private void Update()
    {
        // якщо поточний стан гри в≥др≥зна€Їтьс€ в≥д активного, завершуЇмо виконанн€
        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing)
            return;

        if (target == null || isDead) return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > attackDistance && !blockMovement)
        {
            Move();
        }
        else
        {
            RotateToTarget();
            Attack();
        }

        UpdateAnimator();
    }

    void Move()
    {
        agent.isStopped = false;
        agent.updateRotation = true;
        agent.SetDestination(target.position);
    }

    void Attack()
    {
        agent.isStopped = true;
        agent.updateRotation = false;
        agent.velocity = Vector3.zero;
        agent.ResetPath();

        blockMovement = true;

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            animator.SetBool("Attack", true);
            lastAttackTime = Time.time;
        }
    }

    void RotateToTarget()
    {
        Vector3 direction = (target.position - transform.position);
        direction.y = 0;

        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    void UpdateAnimator()
    {
        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed);
    }

    public void OnAttackFinish()
    {
        blockMovement = false;
    }

    public void DealDamage()
    {
        Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRadius, playerLayer);

        foreach (var hit in hits)
        {
            DamageReceiver playerHealth = hit.GetComponent<DamageReceiver>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(currentDamage);
            }
        }
    }

    public void ApplyDifficulty()
    {
        int stage = Mathf.FloorToInt(GameTimeManager.GameTime / 60f);

        float damageMultiplier = 1f + stage * 0.25f;
        float speedMultiplier = 1f + stage * 0.1f;

        currentDamage = baseDamage * damageMultiplier;
        agent.speed = baseSpeed * speedMultiplier;
    }

    public void Initialize(ObjectSpawner spawner)
    {
        this.spawner = spawner;
    }

    public void Die()
    {
        isDead = true;
        agent.enabled = false;
        GetComponent<Collider>().enabled = false;

        ScoreManager.Instance.AddScore(baseScoreReward);
        spawner?.OnObjectDestroy();

        animator.SetBool("IsDead", true);
        animator.SetTrigger("DieForward");

        Destroy(gameObject, 5f);
    }
}
