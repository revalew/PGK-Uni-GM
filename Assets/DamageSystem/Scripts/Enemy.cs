using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform hero;

    public GameObject spawnPoint;

    public LayerMask whatIsGround, whatIsHero;

    //ARTUR
    // Zmienne health i damage przeniesiono do klasy Attributes.
    public Attributes heroAttributes;

    public Attributes enemyAttributes;

    public ThirdPersonCharacter2 character;

    private Animator animator;
    //ARTUR

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    bool isAttacking;
    public GameObject projectile;

    //States
    public bool heroInSightRange, heroInAttackRange;

    private void Awake()
    {
        hero = GameObject.Find("Hero").transform;
        agent = GetComponent<NavMeshAgent>();
        //ARTUR
        animator = GetComponent<Animator>();
        //ARTUR
    }

    private void Update()
    {
        //Check for sight and attack range
        heroInSightRange = Physics.CheckSphere(transform.position, enemyAttributes.sightRange, whatIsHero);
        heroInAttackRange = Physics.CheckSphere(transform.position, enemyAttributes.attackRange, whatIsHero);

        if (!heroInSightRange && !heroInAttackRange) Patroling();
        if (heroInSightRange && !heroInAttackRange) ChaseHero();
        if (heroInAttackRange && heroInSightRange) AttackHero();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        //ARTUR
        //Usunięto nieużywany kod.
        //ARTUR

        // Idle.
        walkPoint = transform.position;

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChaseHero()
    {
        //ARTUR
        agent.SetDestination(hero.position);

        if (enemyAttributes.health > 0
            && heroAttributes.health > 0)
        {
            character.Move(agent.desiredVelocity, false, false);
        }
        else
        {
            character.Move(Vector3.zero, false, false);
        }
        //ARTUR
    }

    private void AttackHero()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(hero);

        if (!isAttacking
            && enemyAttributes.health > 0
            && heroAttributes.health > 0)
        {
            ///Attack code here
            //Rigidbody rb = Instantiate(projectile, spawnPoint.transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            //ARTUR
            GameObject instance = Instantiate(projectile, spawnPoint.transform.position, Quaternion.identity);
            Rigidbody rb = instance.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 15f, ForceMode.Impulse);
            rb.AddForce(transform.up * 2f, ForceMode.Impulse);

            DetectCollision detector = instance.GetComponent<DetectCollision>();
            if (detector != null)
            {
                detector.Initialize(heroAttributes, enemyAttributes);
            }

            Destroy(instance, 3);
            isAttacking = true;
            Invoke(nameof(ResetAttack), enemyAttributes.timeBetweenAttacks);
            //ARTUR
            ///End of attack code
        }
    }

    //ARTUR
    // Usunięto funkcję DealDamage.
    // Funkcje TakeDamage i DestroyEnemy przeniesiono do klasy Attributes.
    //ARTUR
    private void ResetAttack()
    {
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyAttributes.attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, enemyAttributes.sightRange);
    }
}
