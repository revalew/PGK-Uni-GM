// ARTUR
using UnityEngine;
using UnityEngine.AI;

public class Attributes: MonoBehaviour
{
    public Animator animator;

    public float health, damage, sightRange, attackRange, timeBetweenAttacks;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float value)
    {
        health -= value;
        if (health <= 0)
        {
            animator.SetTrigger("DeathTrigger");
            Invoke(nameof(DestroySelf), 3.5f);
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
// ARTUR