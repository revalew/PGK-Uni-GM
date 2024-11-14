//ARTUR
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class Hero : MonoBehaviour
{

    public PathChecking pathChecking;
    public NavMeshAgent agent;

    public ThirdPersonCharacter2 character;

    public Transform enemy;

    public Transform treasure;

    public Animator animator;

    public Attributes heroAttributes;

    public Attributes enemyAttributes;

    public LayerMask whatIsGround, whatIsEnemy;

    //Attacking
    bool isAttacking;

    //States
    public bool enemyInSightRange, enemyInAttackRange;

    private void Awake()
    {
        // enemy = GameObject.Find("Enemy").transform;
        //enemy = GameObject.FindWithTag("Enemy").transform;

        ObjPlacer.OnBuildingPlaced += FindEnemy;
        agent = GetComponent<NavMeshAgent>();
    }

    void FindEnemy(){
        enemy = FindObjectOfType<Enemy>().transform;    
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {

        if(enemy == null)
            return;
        
        enemyInSightRange = Physics.CheckSphere(transform.position, heroAttributes.sightRange, whatIsEnemy);
        enemyInAttackRange = Physics.CheckSphere(transform.position, heroAttributes.attackRange, whatIsEnemy);

        if (enemyInSightRange && !enemyInAttackRange) 
        {
            ChaseEnemy();
            return;
        }
        if (enemyInAttackRange && enemyInSightRange) 
        {
            AttackEnemy();
            return;
        }

        if (!enemyInSightRange && !enemyInAttackRange && pathChecking._pathAvailable) 
        {
            ChaseTreasure();
            return;
        }
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
    }
    //ARTUR

    private void AttackEnemy()
    {
        //Przeciwnik nie porusza się podczas ataku.
        //agent.SetDestination(transform.position);
        //character.Move(Vector3.zero, false, false);
        transform.LookAt(enemy);

        if (!isAttacking
            && enemyAttributes.health > 0
            && heroAttributes.health > 0)
        {
            isAttacking = true;
            DealDamage(enemyAttributes);
            Invoke(nameof(ResetAttack), heroAttributes.timeBetweenAttacks);
        }
    }

    public void DealDamage(Attributes target)
    {
        if (target != null)
        {
            animator.SetTrigger("AttackTrigger");
            target.TakeDamage(heroAttributes.damage);
        }
    }

    private void ResetAttack()
    {
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
