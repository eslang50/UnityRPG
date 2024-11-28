using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FantasyRpg.Combat
{
    public class AttackController : MonoBehaviour
    {
        private Animator animator;
        public GameObject basicAttackOnePrefab;
        public GameObject specialAttackOnePrefab;
        public GameObject specialAttackTwoPrefab;
        public GameObject specialAttackThreePrefab;
        public AttributesManager attributesManager;

        private int basicAttackOneLevel = 1;
        private int specialAttackOneLevel = 1;
        private int specialAttackTwoLevel = 1;
        private int specialAttackThreeLevel = 1;

        private float basicAttackOneCd = 1f;
        private float specialAttackOneCd = 8f;
        private float specialAttackTwoCd = 12f;
        private float specialAttackThreeCd = 15f;

        private Dictionary<string, float> abilityCooldowns = new Dictionary<string, float>();
        private Dictionary<string, int> manaCosts = new Dictionary<string, int>();

        private float _groundOffset = 0.75f;

        [Header("UI items for Spell Cooldown")]
        [SerializeField]
        private Image basicAttackOneCooldownImage;
        [SerializeField]
        private TMP_Text basicAttackOneCooldownText;

        [SerializeField]
        private Image specialAttackOneCooldownImage;
        [SerializeField]
        private TMP_Text specialAttackOneCooldownText;

        [SerializeField]
        private Image specialAttackTwoCooldownImage;
        [SerializeField]
        private TMP_Text specialAttackTwoCooldownText;

        [SerializeField]
        private Image specialAttackThreeCooldownImage;
        [SerializeField]
        private TMP_Text specialAttackThreeCooldownText;
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
            abilityCooldowns["SpecialAttackThree"] = 0f;

            manaCosts["BasicAttackOne"] = 10;
            manaCosts["SpecialAttackOne"] = 20;
            manaCosts["SpecialAttackTwo"] = 35;
            manaCosts["SpecialAttackThree"] = 40;

            GameObject.Find("AbilityOneText").GetComponent<TMPro.TextMeshProUGUI>().text = basicAttackOneLevel.ToString();
            GameObject.Find("AbilityTwoText").GetComponent<TMPro.TextMeshProUGUI>().text = specialAttackOneLevel.ToString();
            GameObject.Find("AbilityThreeText").GetComponent<TMPro.TextMeshProUGUI>().text = specialAttackTwoLevel.ToString();
            GameObject.Find("AbilityFourText").GetComponent<TMPro.TextMeshProUGUI>().text = specialAttackThreeLevel.ToString();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                animator.SetTrigger("MeleeAttack");
            }
            else if (Input.GetMouseButtonDown(0))
            {
                if (Time.time >= abilityCooldowns["BasicAttackOne"])
                {
                    StartCoroutine(BasicAttackOne());
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
            else if (Input.GetKeyDown(KeyCode.F))
            {
                if (Time.time >= abilityCooldowns["SpecialAttackThree"])
                {
                    StartCoroutine(SpecialAttackThree());
                }
            }
            UpdateCooldownUI();
        }

        private void UpdateCooldownUI()
        {
            UpdateCooldown("BasicAttackOne", basicAttackOneCooldownImage, basicAttackOneCooldownText, basicAttackOneCd);
            UpdateCooldown("SpecialAttackOne", specialAttackOneCooldownImage, specialAttackOneCooldownText, specialAttackOneCd);
            UpdateCooldown("SpecialAttackTwo", specialAttackTwoCooldownImage, specialAttackTwoCooldownText, specialAttackTwoCd);
            UpdateCooldown("SpecialAttackThree", specialAttackThreeCooldownImage, specialAttackThreeCooldownText, specialAttackThreeCd);
        }

        private void UpdateCooldown(string ability, Image cooldownImage, TMP_Text cooldownText, float cooldownTime)
        {
            float cooldownTimer = abilityCooldowns[ability] - Time.time;
            if (cooldownTimer < 0.0f)
            {
                cooldownText.gameObject.SetActive(false);
                cooldownImage.gameObject.SetActive(false);
                cooldownImage.fillAmount = 0.0f;
            }
            else
            {
                cooldownText.gameObject.SetActive(true);
                cooldownImage.gameObject.SetActive(true);
                cooldownText.text = Mathf.RoundToInt(cooldownTimer).ToString();
                cooldownImage.fillAmount = cooldownTimer / cooldownTime;
            }
        }

        protected IEnumerator BasicAttackOne()
        {
            if (attributesManager.currentMana < manaCosts["BasicAttackOne"])
            {
                yield break;
            }
            attributesManager.currentMana -= manaCosts["BasicAttackOne"];
            abilityCooldowns["BasicAttackOne"] = Time.time + basicAttackOneCd;

            animator.SetTrigger("BasicAttackOne");
            yield return new WaitForSeconds(0.25f);

            Vector3 mousePosition = GetMouseWorldPosition();
            Vector3 direction = (mousePosition - transform.position).normalized;
            direction.y = 0;

            Vector3 spawnPosition = transform.position + new Vector3(0, _groundOffset, 0) + direction;

            GameObject projectileInstance = Instantiate(basicAttackOnePrefab, spawnPosition, Quaternion.LookRotation(direction));
            projectileInstance.GetComponent<ProjectileCollider>().Initialize(attributesManager, (int)(attributesManager.attack * 1.25f));
        }

        protected IEnumerator SpecialAttackOne()
        {
            if (attributesManager.currentMana < manaCosts["SpecialAttackOne"])
            {
                yield break;
            }
            attributesManager.currentMana -= manaCosts["SpecialAttackOne"];
            abilityCooldowns["SpecialAttackOne"] = Time.time + specialAttackOneCd;

            Vector3 position = GetMouseWorldPosition();
            animator.SetTrigger("SpecialAttackOne");
            yield return new WaitForSeconds(0.5f);

            GameObject animationInstance = Instantiate(specialAttackOnePrefab, position, Quaternion.identity);

            StartCoroutine(ApplyAreaOfEffect(position, 3f, 5, 1f, (int)(attributesManager.attack * 1.5f)));

            Destroy(animationInstance, 5f);
        }

        protected IEnumerator SpecialAttackTwo()
        {
            if (attributesManager.currentMana < manaCosts["SpecialAttackTwo"])
            {
                yield break;
            }
            attributesManager.currentMana -= manaCosts["SpecialAttackTwo"];
            abilityCooldowns["SpecialAttackTwo"] = Time.time + specialAttackTwoCd;

            animator.SetTrigger("BasicAttackOne");
            yield return new WaitForSeconds(0.5f);

            Vector3 mousePosition = GetMouseWorldPosition();
            Vector3 direction = (mousePosition - transform.position).normalized;
            direction.y = 0;

            GameObject specialAttackInstance = Instantiate(specialAttackTwoPrefab, transform.position, Quaternion.LookRotation(direction));

            // Create a rectangular collider
            BoxCollider boxCollider = specialAttackInstance.AddComponent<BoxCollider>();
            boxCollider.size = new Vector3(3.5f, 1f, 12.5f);
            boxCollider.center = new Vector3(0, 0, boxCollider.size.z / 2);
            boxCollider.isTrigger = true;

            specialAttackInstance.AddComponent<WaveAttackCollider>().Initialize(attributesManager, (int)(attributesManager.attack * 3.0f));
        }

        protected IEnumerator SpecialAttackThree()
        {
            if (attributesManager.currentMana < manaCosts["SpecialAttackThree"])
            {
                yield break;
            }
            attributesManager.currentMana -= manaCosts["SpecialAttackThree"];
            abilityCooldowns["SpecialAttackThree"] = Time.time + specialAttackThreeCd;

            animator.SetTrigger("SpecialAttackOne");
            yield return new WaitForSeconds(0.5f);

            Vector3 position = GetMouseWorldPosition();
            GameObject animationInstance = Instantiate(specialAttackThreePrefab, position, Quaternion.identity);

            yield return new WaitForSeconds(3f); // Delay before applying damage

            Collider[] colliders = Physics.OverlapSphere(position, 3f);
            foreach (var other in colliders)
            {
                if (other.CompareTag("Enemy"))
                {
                    attributesManager.Attack(other.gameObject, (int)(attributesManager.attack * 5.0f));
                    StartCoroutine(ApplyDamageOverTime(other, 5, 1f, (int)(attributesManager.attack * 0.5f)));
                }
            }

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
                        attributesManager.Attack(other.gameObject, tickDamage);
                    }
                }
                yield return new WaitForSeconds(interval);
            }
        }

        private IEnumerator ApplyDamageOverTime(Collider collider, int duration, float interval, int tickDamage)
        {
            for (int i = 0; i < duration; i++)
            {
                if (collider.CompareTag("Enemy"))
                {
                    attributesManager.Attack(collider.gameObject, tickDamage);
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