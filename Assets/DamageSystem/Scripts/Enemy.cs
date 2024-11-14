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
    public Animator animator;

    public ThirdPersonCharacter2 character;

    public Attributes heroAttributes;

    public Attributes enemyAttributes;
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
        heroInSightRange = Physics.CheckSphere(transform.position, enemyAttributes.sightRange, whatIsHero);
        heroInAttackRange = Physics.CheckSphere(transform.position, enemyAttributes.attackRange, whatIsHero);

        //ARTUR
        if (!heroInSightRange && !heroInAttackRange) Idle();
        //ARTUR
        if (heroInSightRange && !heroInAttackRange) ChaseHero();
        if (heroInAttackRange && heroInSightRange) AttackHero();
    }

    //ARTUR
    // Usunięto nieużywane funkcje Patroling oraz SearchWalkPoint.
    //ARTUR

    //ARTUR
    private void Idle()
    {
        agent.SetDestination(transform.position);
        character.Move(Vector3.zero, false, false);
    }
    //ARTUR

    private void ChaseHero()
    {
        //ARTUR
        if (enemyAttributes.health > 0
            && heroAttributes.health > 0)
        {
            agent.SetDestination(hero.position);
            character.Move(agent.desiredVelocity, false, false);
        }
        else
            Idle();
        //ARTUR
    }

    private void AttackHero()
    {
        //Przeciwnik nie porusza się podczas ataku.
        agent.SetDestination(transform.position);
        character.Move(Vector3.zero, false, false);
        transform.LookAt(hero);

        if (!isAttacking
            && enemyAttributes.health > 0
            && heroAttributes.health > 0)
        {
            //ARTUR
            GameObject instance = Instantiate(projectile, spawnPoint.transform.position, Quaternion.identity);
            Rigidbody rb = instance.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 10f, ForceMode.Impulse);
            rb.AddForce(transform.up * 1f, ForceMode.Impulse);

            DetectCollision detector = instance.GetComponent<DetectCollision>();
            if (detector != null)
            {
                detector.Initialize(heroAttributes, enemyAttributes);
            }

            Destroy(instance, 3);
            isAttacking = true;
            Invoke(nameof(ResetAttack), enemyAttributes.timeBetweenAttacks);
            //ARTUR
        }
    }

    //ARTUR
    // Usunięto nieużywaną funkcję DealDamage.
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
