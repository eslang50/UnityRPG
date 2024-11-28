using FantasyRpg.Combat;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace FantasyRpg.Combat
{
    public class XpBar : MonoBehaviour
    {
        public Slider xpSlider;
        public AttributesManager attributesManager;

        public Color levelUpColor = Color.green;
        public float blinkDuration = 0.1f;

        private Color originalColor;
        private Image xpBarImage;

        private bool xpBarEnabled = true;

        private void Start()
        {
            // Initialize the slider values
            xpSlider.maxValue = attributesManager.maxXp;
            xpSlider.value = attributesManager.currentXp;

            // Get the mana bar image component
            xpBarImage = xpSlider.fillRect.GetComponent<Image>();
            originalColor = xpBarImage.color;

            // Subscribe to the TakeDamage event
            attributesManager.OnLevelUp += HandleLevelUp;
        }

        private void Update()
        {
            if (xpBarEnabled)
            {
                // Sync the slider value with the current 
                xpSlider.value = attributesManager.currentXp;
            }
        }

        private void HandleLevelUp(int level)
        {
            xpSlider.maxValue = attributesManager.maxXp;
            StartCoroutine(BlinkEffect());
        }

        private IEnumerator BlinkEffect()
        {
            xpBarEnabled = false;
            // Temporarily set the XP bar to the maximum value to ensure the blink effect is visible
            xpSlider.value = xpSlider.maxValue;

            int t = 0;
            while (t < 5)
            {
                // Change the color to the blink color
                xpBarImage.color = levelUpColor;

                yield return new WaitForSeconds(blinkDuration);

                // Revert the color back to the original color
                xpBarImage.color = originalColor;

                yield return new WaitForSeconds(blinkDuration);

                t++;
            }

            // Reset the XP bar value to the current XP after blinking
            xpSlider.value = attributesManager.currentXp;
            xpBarEnabled = true;
        }

        private void OnDestroy()
        {
            // Unsubscribe from the OnLevelUp event
            attributesManager.OnLevelUp -= HandleLevelUp;
        }
    }
}