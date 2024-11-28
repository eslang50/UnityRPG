using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

        private void Start()
        {
            StartCoroutine(RegenerateHealthMana());
            GameObject.Find("CharacterName").GetComponent<TMPro.TextMeshProUGUI>().text = characterName;
            GameObject.Find("LevelText").GetComponent<TMPro.TextMeshProUGUI>().text = currentLevel.ToString();
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

        public void TakeDamage(int amount)
        {
            currentHealth -= amount - (amount * armor / 100);
            Vector3 randomOffset = new Vector3(Random.Range(0f, 0.25f), Random.Range(0f, 0.25f), Random.Range(0f, 0.25f));
            StatusPopUpGenerator.instance.CreatePopUp(transform.position + randomOffset, amount.ToString(), Color.yellow);
            OnTakeDamage?.Invoke(amount);

            // Reset the timer since the player was hit
            timeSinceLastHit = 0f;
        }

        public void Attack(GameObject target)
        {
            var atm = target.GetComponent<AttributesManager>();
            if (atm == null) return;
            atm.TakeDamage((int)(Random.Range(0f, 1f) < critChance ? attack * critDamage : attack));

            if (atm.currentHealth <= 0)
            {
                GainExperience(atm.xpValue);
                target.tag = "Corpse";
            }
        }

        public void Attack(GameObject target, float damage)
        {
            var atm = target.GetComponent<AttributesManager>();
            if (atm == null) return;
            atm.TakeDamage((int)(Random.Range(0f, 1f) < critChance ? damage * critDamage : damage));

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