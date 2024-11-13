//ARTUR
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class Hero : MonoBehaviour
{
    public NavMeshAgent agent;

    public ThirdPersonCharacter2 character;

    public Transform enemy;

    public Transform treasure;

    private Animator animator;

    public Attributes heroAttributes;

    public Attributes enemyAttributes;

    public LayerMask whatIsGround, whatIsEnemy;

    //Attacking
    bool isAttacking;

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
        enemyInSightRange = Physics.CheckSphere(transform.position, heroAttributes.sightRange, whatIsEnemy);
        enemyInAttackRange = Physics.CheckSphere(transform.position, heroAttributes.attackRange, whatIsEnemy);

        //ARTUR
        if (!enemyInSightRange && !enemyInAttackRange) ChaseTreasure();
        //ARTUR
        if (enemyInSightRange && !enemyInAttackRange) ChaseEnemy();
        if (enemyInAttackRange && enemyInSightRange) AttackEnemy();
    }

    //ARTUR
    private void Idle()
    {
        agent.SetDestination(transform.position);
        character.Move(Vector3.zero, false, false);
    }

    private void ChaseTreasure()
    {
        agent.SetDestination(treasure.position);
        character.Move(agent.desiredVelocity, false, false);
    }

    private void ChaseEnemy()
    {
        if (enemyAttributes.health > 0
            && heroAttributes.health > 0)
        {
            agent.SetDestination(enemy.position);
            character.Move(agent.desiredVelocity, false, false);
        }
        else
            Idle();
    }
    //ARTUR

    private void AttackEnemy()
    {
        //Przeciwnik nie porusza się podczas ataku.
        agent.SetDestination(transform.position);
        character.Move(Vector3.zero, false, false);
        transform.LookAt(enemy);

        if (!isAttacking
            && enemyAttributes.health > 0
            && heroAttributes.health > 0)
        {
            isAttacking = true;
            DealDamage(enemyAttributes.gameObject);

            Invoke(nameof(ResetAttack), heroAttributes.timeBetweenAttacks);
        }
    }

    public void DealDamage(GameObject target)
    {
        enemyAttributes = target.GetComponent<Attributes>();
        if (enemyAttributes != null)
        {
            animator.SetBool("AttackTrigger", true);
            enemyAttributes.TakeDamage(heroAttributes.damage);
        }
    }

    private void ResetAttack()
    {
        animator.SetBool("AttackTrigger", false);
        isAttacking = false;
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
