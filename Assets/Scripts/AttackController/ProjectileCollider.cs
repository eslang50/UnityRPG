using FantasyRpg.Combat;
using UnityEngine;


namespace FantasyRpg.Combat
{
    public class ProjectileCollider : MonoBehaviour
    {
        private AttributesManager attributesManager;
        private int damage;
        private float speed = 25f;

        public void Initialize(AttributesManager attributesManager, int damage)
        {
            this.attributesManager = attributesManager;
            this.damage = damage;
            Destroy(gameObject, 3f);
        }

        private void Update()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                attributesManager.Attack(other.gameObject, damage);
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, GetComponent<Collider>().bounds.size);
        }
    }
}