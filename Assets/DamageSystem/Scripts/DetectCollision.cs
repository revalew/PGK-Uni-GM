// ARTUR
using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    private Attributes targetAttributes;
    private Attributes objectAttributes;

    public void Initialize(Attributes targetAttributes, Attributes objectAttributes)
    {
        this.objectAttributes = objectAttributes;
        this.targetAttributes = targetAttributes;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == targetAttributes.gameObject)
        {
            targetAttributes.TakeDamage(objectAttributes.damage);
            Destroy(gameObject);
        }
    }
}
// ARTUR