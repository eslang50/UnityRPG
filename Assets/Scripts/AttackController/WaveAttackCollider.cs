using FantasyRpg.Combat;
using UnityEngine;

public class WaveAttackCollider : MonoBehaviour
{
    private AttributesManager attributesManager;
    private int damage;

    public void Initialize(AttributesManager attributesManager, int damage)
    {
        this.attributesManager = attributesManager;
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            attributesManager.Attack(other.gameObject, damage);
        }
    }
}