using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FantasyRpg.Combat
{
    public class AttributesManager : MonoBehaviour
    {
        public int health;
        public int attack;
        public int armor;
        public float critDamage = 1.5f;
        public float critChance = 0.1f;

        public SkinnedMeshRenderer[] skinnedMeshes;
        public float dissolveRate = 0.0125f;
        public float refreshRate = 0.025f;

        private Material[] skinnedMaterials;

        private void Start()
        {
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

        public void TakeDamage(int amount)
        {
            int damageTaken = Mathf.Max(0, amount - (amount * armor / 100));  // Apply armor reduction
            health = Mathf.Max(0, health - damageTaken);  // Reduce health, ensuring it doesn't go below zero
            Vector3 randomOffset = new Vector3(Random.Range(0f, 0.25f), Random.Range(0f, 0.25f), Random.Range(0f, 0.25f));
            StatusPopUpGenerator.instance.CreatePopUp(transform.position + randomOffset, damageTaken.ToString(), Color.yellow);  // Show damage pop-up

            if (health <= 0)
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

        public void Attack(GameObject target)
        {
            var atm = target.GetComponent<AttributesManager>();
            if (atm == null) return;

            // Calculate damage with crit chance and damage
            int damage = (int)(Random.Range(0f, 1f) < critChance ? attack * critDamage : attack);
            atm.TakeDamage(damage);  // Apply the calculated damage to the target
        }

    }
}
