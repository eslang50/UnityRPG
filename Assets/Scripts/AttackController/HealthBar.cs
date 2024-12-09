using FantasyRpg.Combat;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace FantasyRpg.Combat
{
    public class HealthBar : MonoBehaviour
    {
        public Slider healthSlider;
        public AttributesManager attributesManager;
        public Color flinchColor = Color.red;
        public float flinchDuration = 0.1f;
        public bool isPlayer = false;

        private Color originalColor;
        private Image healthBarImage;
        private CanvasGroup canvasGroup;

        private void Start()
        {
            // Initialize the slider values
            healthSlider.maxValue = attributesManager.maxHealth;
            healthSlider.value = attributesManager.currentHealth;

            // Get the health bar image component
            healthBarImage = healthSlider.fillRect.GetComponent<Image>();
            originalColor = healthBarImage.color;

            canvasGroup = GetComponent<CanvasGroup>();

            // Get the CanvasGroup component to control visibility   
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }

            if (!isPlayer)
            {
                // Set the health bar to be invisible initially
                canvasGroup.alpha = 0;
            }
            // Subscribe to the TakeDamage event
            attributesManager.OnTakeDamage += HandleTakeDamage;
        }

        private void Update()
        {
            // Sync the slider value with the current health
            healthSlider.value = attributesManager.currentHealth;
        }

        private void HandleTakeDamage(int damage)
        {
            // Make the health bar visible
            canvasGroup.alpha = 1;

            StartCoroutine(FlinchEffect());
        }

        private IEnumerator FlinchEffect()
        {
            // Change the color to the flinch color
            healthBarImage.color = flinchColor;

            // Wait for the flinch duration
            yield return new WaitForSeconds(flinchDuration);

            // Revert the color back to the original color
            healthBarImage.color = originalColor;
        }

        private void OnDestroy()
        {
            // Unsubscribe from the TakeDamage event
            attributesManager.OnTakeDamage -= HandleTakeDamage;
        }
    }
}