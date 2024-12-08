using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace FantasyRpg.Combat
{
    public class AttributesManager : MonoBehaviour
    {
        public string characterName = "Magician";

        public int maxHealth;
        public int currentHealth;
        public int maxMana;
        public int currentMana;
        public int maxXp;
        public int currentXp;
        public int currentLevel;

        public int xpValue = 10;

        public int healthRegen = 1;
        public int manaRegen = 1;
        public int attack;
        public int armor;   

        public float critDamage = 1.5f;
        public float critChance = 0.1f;

        public event System.Action<int> OnTakeDamage;
        public event System.Action<int> OnLevelUp;

        private float timeSinceLastHit = 0f;
        private const float regenDelay = 3f;

        public SkinnedMeshRenderer[] skinnedMeshes;
        public float dissolveRate = 0.0125f;
        public float refreshRate = 0.025f;

        private Material[] skinnedMaterials;

        private void Start()
        {
            StartCoroutine(RegenerateHealthMana());
            GameObject.Find("CharacterName").GetComponent<TMPro.TextMeshProUGUI>().text = characterName;
            GameObject.Find("LevelText").GetComponent<TMPro.TextMeshProUGUI>().text = currentLevel.ToString();

            if (skinnedMeshes == null || skinnedMeshes.Length == 0)
            {
                skinnedMeshes = GetComponentsInChildren<SkinnedMeshRenderer>();
            }

            if (skinnedMeshes != null && skinnedMeshes.Length > 0)
            {
                foreach (var skinnedMesh in skinnedMeshes)
                {
                    skinnedMaterials = skinnedMesh.materials;
                }
            }
            else
            {
                Debug.LogWarning("No SkinnedMeshRenderers found on the GameObject or its children.");
            }
        }
        private void Update()
        {
            timeSinceLastHit += Time.deltaTime;
        }

        private IEnumerator RegenerateHealthMana()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f); // Wait for 1 second
                if (timeSinceLastHit >= regenDelay)
                {
                    currentHealth = Mathf.Clamp(currentHealth + healthRegen, 0, maxHealth);
                }
                currentMana = Mathf.Clamp(currentMana + manaRegen, 0, maxMana);
            }
        }

        public void TakeDamage(int amount, Color? color = null)
        {
            currentHealth -= amount - (amount * armor / 100);

            // Get the NavMeshAgent's position if possible
            NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
            Vector3 popUpPosition = navMeshAgent != null ? navMeshAgent.transform.position : transform.position;
            Vector3 randomOffset = new Vector3(Random.Range(0f, 0.25f), Random.Range(0f, 0.25f), Random.Range(0f, 0.25f));

            StatusPopUpGenerator.instance.CreatePopUp(popUpPosition + randomOffset, amount.ToString(), color ?? Color.yellow);
            OnTakeDamage?.Invoke(amount);

            // Reset the timer since the player was hit
            timeSinceLastHit = 0f;

            if (currentHealth <= 0)
            {
                HandleDeath();
            }
        }

        private void HandleDeath()
        {
            StartCoroutine(Dissolve());  // Start dissolve effect
        }

        private IEnumerator Dissolve()
        {
            float counter = 0;
            while (counter < 1)
            {
                counter += dissolveRate;

                foreach (var skinnedMesh in skinnedMeshes)
                {
                    if (skinnedMesh != null)
                    {
                        Material[] materials = skinnedMesh.materials;

                        for (int i = 0; i < materials.Length; i++)
                        {
                            materials[i].SetFloat("_Dissolve", counter);  // Apply dissolve effect
                        }
                    }
                }
                yield return new WaitForSeconds(refreshRate);
            }

            gameObject.SetActive(false);  // Deactivate the game object after death
        }

        public void Attack(GameObject target, float? damage = null, Color? color = null)
        {
            var atm = target.GetComponent<AttributesManager>();
            if (atm == null) return;
            float actualDamage = damage ?? attack;
            atm.TakeDamage((int)(Random.Range(0f, 1f) < critChance ? actualDamage * critDamage : actualDamage), color);

            if (atm.currentHealth <= 0)
            {
                GainExperience(atm.xpValue);
                target.tag = "Corpse";
            }
        }

        public void GainExperience(int amount)
        {
            currentXp += amount;
            if (currentXp >= maxXp)
            {
                currentLevel++;
                currentXp -= maxXp;

                // Calculate next level Xp required to level up
                maxXp = Mathf.CeilToInt(maxXp * 1.15f);

                // Increase attributes
                maxHealth += 10;
                currentHealth = maxHealth;
                maxMana += 5;
                currentMana = maxMana;
                attack += 2;
                armor += 1;

                OnLevelUp?.Invoke(currentLevel);

                // Update UI
                GameObject.Find("LevelText").GetComponent<TMPro.TextMeshProUGUI>().text = currentLevel.ToString();
            }
        }
    }
}