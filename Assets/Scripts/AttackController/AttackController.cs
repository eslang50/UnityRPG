using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Burst.Intrinsics;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

namespace FantasyRpg.Combat
{
    public class AttackController : MonoBehaviour
    {
        private Animator animator;
        public GameObject specialAttackOnePrefab;
        public GameObject specialAttackTwoPrefab;
        public AttributesManager atm;
        private float _groundOffset = 0.75f;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                animator.SetTrigger("MeleeAttack");
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(SpecialAttackOne());
            }
        }

        private void UpdateUI() { }

        protected IEnumerator SpecialAttackOne()
        {
            Vector3 position = GetMouseWorldPosition();
            animator.SetTrigger("SpecialAttackOne");
            yield return new WaitForSeconds(0.5f);

            GameObject animationInstance = Instantiate(specialAttackOnePrefab, position, Quaternion.identity);

            StartCoroutine(ApplyAreaOfEffect(position, 3f, 5, 1f, (int)(atm.attack * 1.5f)));

            Destroy(animationInstance, 5f);
        }

        private IEnumerator ApplyAreaOfEffect(Vector3 position, float radius, int duration, float interval, int tickDamage)
        {
            for (int i = 0; i < duration; i++)
            {
                Collider[] colliders = Physics.OverlapSphere(position, radius);
                foreach (var other in colliders)
                {
                    if (other.CompareTag("Enemy"))
                    {
                        other.GetComponent<AttributesManager>().TakeDamage(tickDamage);
                    }
                }
                yield return new WaitForSeconds(interval);
            }
        }

        private IEnumerator ApplyDamageOverTime(Collider[] colliders, int duration, float interval, int tickDamage)
        {
            for (int i = 0; i < duration; i++)
            {
                foreach (var other in colliders)
                {
                    if (other.CompareTag("Enemy"))
                    {
                        other.GetComponent<AttributesManager>().TakeDamage(tickDamage);
                    }
                }
                yield return new WaitForSeconds(interval);
            }
        }

        private Vector3 GetMouseWorldPosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                return hit.point;
            }
            // Default to zero if no hit
            return Vector3.zero; 
        }
    }
}