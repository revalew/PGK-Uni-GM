//ARTUR
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class Hero : MonoBehaviour
{
    public NavMeshAgent agent;

    public ThirdPersonCharacter2 character;

    private Animator animator;

    public Transform enemy;

    public LayerMask whatIsGround, whatIsEnemy;

    public Attributes heroAttributes;

    public Attributes enemyAttributes;

    //Attacking
    bool isAttacking;
    public GameObject sword;

    //States
    public bool enemyInSightRange, enemyInAttackRange;

    private void Awake()
    {
        enemy = GameObject.Find("Enemy").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //Check for sight and attack range
        enemyInSightRange = Physics.CheckSphere(transform.position, heroAttributes.sightRange, whatIsEnemy);
        enemyInAttackRange = Physics.CheckSphere(transform.position, heroAttributes.attackRange, whatIsEnemy);

        if (enemyInSightRange && !enemyInAttackRange) ChaseEnemy();
        if (enemyInAttackRange && enemyInSightRange) AttackEnemy();
    }

    private void ChaseEnemy()
    {
        if (enemyAttributes.health > 0
            && heroAttributes.health > 0)
        {
            agent.SetDestination(enemy.position);
        }
    }

    private void AttackEnemy()
    {
        if (!isAttacking
            && enemyAttributes.health > 0
            && heroAttributes.health > 0)
        {
            transform.LookAt(enemy);

            isAttacking = true;

            animator.SetBool("AttackTrigger", true);
            DealDamage(enemyAttributes.gameObject);

            Invoke(nameof(ResetAttack), heroAttributes.timeBetweenAttacks);
            
        }
    }

    public void DealDamage(GameObject target)
    {
        enemyAttributes = target.GetComponent<Attributes>();
        if (enemyAttributes != null)
        {
            enemyAttributes.TakeDamage(heroAttributes.damage);
        }
    }

    private void ResetAttack()
    {
        isAttacking = false;
        animator.SetBool("AttackTrigger", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, heroAttributes.attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, heroAttributes.sightRange);
    }
}
//ARTUR
