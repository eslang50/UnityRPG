using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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

        [Header("UI items for Ability Levels")]
        [SerializeField]
        private TMP_Text basicAttackOneLevelText;
        [SerializeField]
        private TMP_Text specialAttackOneLevelText;
        [SerializeField]
        private TMP_Text specialAttackTwoLevelText;
        [SerializeField]
        private TMP_Text specialAttackThreeLevelText;

        [Header("UI items for Skill Points")]
        [SerializeField]
        private Button basicAttackOneUpgradeButton;
        [SerializeField]
        private Button specialAttackOneUpgradeButton;
        [SerializeField]
        private Button specialAttackTwoUpgradeButton;
        [SerializeField]
        private Button specialAttackThreeUpgradeButton;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            InitializeAbilities();

            basicAttackOneUpgradeButton.onClick.AddListener(LevelUpBasicAttackOne);
            specialAttackOneUpgradeButton.onClick.AddListener(LevelUpSpecialAttackOne);
            specialAttackTwoUpgradeButton.onClick.AddListener(LevelUpSpecialAttackTwo);
            specialAttackThreeUpgradeButton.onClick.AddListener(LevelUpSpecialAttackThree);
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

            UpdateAbilityLevelUI();
        }

        private void UpdateAbilityLevelUI()
        {
            basicAttackOneLevelText.text = basicAttackOneLevel.ToString();
            specialAttackOneLevelText.text = specialAttackOneLevel.ToString();
            specialAttackTwoLevelText.text = specialAttackTwoLevel.ToString();
            specialAttackThreeLevelText.text = specialAttackThreeLevel.ToString();
        }

        public void LevelUpBasicAttackOne()
        {
            if (attributesManager.skillPoints > 0)
            {
                basicAttackOneLevel++;
                attributesManager.skillPoints--;
                UpdateAbilityLevelUI();
                UpdateSkillPointUI();
            }
        }

        public void LevelUpSpecialAttackOne()
        {
            if (attributesManager.skillPoints > 0)
            {
                specialAttackOneLevel++;
                attributesManager.skillPoints--;
                UpdateAbilityLevelUI();
                UpdateSkillPointUI();
            }
        }

        public void LevelUpSpecialAttackTwo()
        {
            if (attributesManager.skillPoints > 0)
            {
                specialAttackTwoLevel++;
                attributesManager.skillPoints--;
                UpdateAbilityLevelUI();
                UpdateSkillPointUI();
            }
        }

        public void LevelUpSpecialAttackThree()
        {
            if (attributesManager.skillPoints > 0)
            {
                specialAttackThreeLevel++;
                attributesManager.skillPoints--;
                UpdateAbilityLevelUI();
                UpdateSkillPointUI();
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                animator.SetTrigger("MeleeAttack");
            }
            else if (Input.GetMouseButtonDown(0))
            {
                if (Time.time >= abilityCooldowns["BasicAttackOne"] && !EventSystem.current.IsPointerOverGameObject())
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
            UpdateSkillPointUI();
        }

        private void UpdateCooldownUI()
        {
            UpdateCooldown("BasicAttackOne", basicAttackOneCooldownImage, basicAttackOneCooldownText, basicAttackOneCd);
            UpdateCooldown("SpecialAttackOne", specialAttackOneCooldownImage, specialAttackOneCooldownText, specialAttackOneCd);
            UpdateCooldown("SpecialAttackTwo", specialAttackTwoCooldownImage, specialAttackTwoCooldownText, specialAttackTwoCd);
            UpdateCooldown("SpecialAttackThree", specialAttackThreeCooldownImage, specialAttackThreeCooldownText, specialAttackThreeCd);
        }

        private void UpdateSkillPointUI()
        {
            bool hasSkillPoints = attributesManager.skillPoints > 0;
            basicAttackOneUpgradeButton.gameObject.SetActive(hasSkillPoints);
            specialAttackOneUpgradeButton.gameObject.SetActive(hasSkillPoints);
            specialAttackTwoUpgradeButton.gameObject.SetActive(hasSkillPoints);
            specialAttackThreeUpgradeButton.gameObject.SetActive(hasSkillPoints);
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

            float distanceMultiplier = 1.5f;
            Vector3 spawnPosition = transform.position + new Vector3(0, _groundOffset, 0) + direction * distanceMultiplier;

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

            Collider[] colliders = Physics.OverlapSphere(position, 3.5f);
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
                yield return new WaitForSeconds(interval);

                if (collider.CompareTag("Enemy"))
                {
                    attributesManager.Attack(collider.gameObject, tickDamage, new Color(0.5f, 0f, 0.5f));
                }
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