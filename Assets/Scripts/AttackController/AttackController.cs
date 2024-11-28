using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FantasyRpg.Combat
{
    public class AttackController : MonoBehaviour
    {
        private Animator animator;
        public GameObject basicAttackOnePrefab;
        public GameObject specialAttackOnePrefab;
        public GameObject specialAttackTwoPrefab;
        public AttributesManager attributesManager;

        private int basicAttackOneLevel = 1;
        private int meleeAttackLevel = 1;
        private int specialAttackOneLevel = 1;
        private int specialAttackTwoLevel = 1;

        private float basicAttackOneCd = 2f;
        private float specialAttackOneCd = 8f;
        private float specialAttackTwoCd = 2f;

        private Dictionary<string, float> abilityCooldowns = new Dictionary<string, float>();
        private Dictionary<string, int> manaCosts = new Dictionary<string, int>();

        private float _groundOffset = 0.75f;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            InitializeAbilities();
        }

        private void InitializeAbilities()
        {
            abilityCooldowns["BasicAttackOne"] = 0f;
            abilityCooldowns["SpecialAttackOne"] = 0f;
            abilityCooldowns["SpecialAttackTwo"] = 0f;

            manaCosts["BasicAttackOne"] = 10;
            manaCosts["SpecialAttackOne"] = 25;
            manaCosts["SpecialAttackTwo"] = 35;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                animator.SetTrigger("MeleeAttack");
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                if (Time.time >= abilityCooldowns["BasicAttackOne"])
                {
                    BasicAttackOne();
                }
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                if (Time.time >= abilityCooldowns["SpecialAttackOne"])
                {
                    StartCoroutine(SpecialAttackOne());
                }
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                if (Time.time >= abilityCooldowns["SpecialAttackTwo"])
                {
                    StartCoroutine(SpecialAttackTwo());
                }
            }
        }

        private void UpdateUI() { }

        protected void BasicAttackOne()
        {
            if (attributesManager.currentMana < manaCosts["BasicAttackOne"])
            {
                return;
            }

            Vector3 mousePosition = GetMouseWorldPosition();
            Vector3 direction = (mousePosition - transform.position).normalized;
            direction.y = 0;

            Vector3 spawnPosition = transform.position + new Vector3(0, _groundOffset, 0) + direction;

            animator.SetTrigger("BasicAttackOne");

            GameObject projectileInstance = Instantiate(basicAttackOnePrefab, spawnPosition, Quaternion.LookRotation(direction));
            projectileInstance.GetComponent<ProjectileCollider>().Initialize(attributesManager, (int)(attributesManager.attack * 1.25f));

            attributesManager.currentMana -= manaCosts["BasicAttackOne"];
            abilityCooldowns["BasicAttackOne"] = Time.time + basicAttackOneCd;
        }

        protected IEnumerator SpecialAttackOne()
        {
            if (attributesManager.currentMana < manaCosts["SpecialAttackOne"])
            {
                yield break;
            }

            Vector3 position = GetMouseWorldPosition();
            animator.SetTrigger("SpecialAttackOne");
            yield return new WaitForSeconds(0.5f);

            GameObject animationInstance = Instantiate(specialAttackOnePrefab, position, Quaternion.identity);

            StartCoroutine(ApplyAreaOfEffect(position, 3f, 5, 1f, (int)(attributesManager.attack * 1.5f)));

            Destroy(animationInstance, 5f);

            attributesManager.currentMana -= manaCosts["SpecialAttackOne"];
            abilityCooldowns["SpecialAttackOne"] = Time.time + specialAttackOneCd;
        }

        protected IEnumerator SpecialAttackTwo()
        {
            if (attributesManager.currentMana < manaCosts["SpecialAttackTwo"])
            {
                yield break;
            }

            Vector3 mousePosition = GetMouseWorldPosition();
            Vector3 direction = (mousePosition - transform.position).normalized;
            direction.y = 0;

            animator.SetTrigger("BasicAttackOne");
            yield return new WaitForSeconds(0.5f);

            GameObject specialAttackInstance = Instantiate(specialAttackTwoPrefab, transform.position, Quaternion.LookRotation(direction));

            // Create a rectangular collider
            BoxCollider boxCollider = specialAttackInstance.AddComponent<BoxCollider>();
            boxCollider.size = new Vector3(3.5f, 1f, 12.5f);
            boxCollider.center = new Vector3(0, 0, boxCollider.size.z / 2);
            boxCollider.isTrigger = true;

            specialAttackInstance.AddComponent<WaveAttackCollider>().Initialize(attributesManager, (int)(attributesManager.attack * 3.0f));

            attributesManager.currentMana -= manaCosts["SpecialAttackTwo"];
            abilityCooldowns["SpecialAttackTwo"] = Time.time + specialAttackTwoCd;
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
                        attributesManager.Attack(other.gameObject, tickDamage);
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
                        attributesManager.Attack(other.gameObject, tickDamage);
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