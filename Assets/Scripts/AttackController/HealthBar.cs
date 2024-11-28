using FantasyRpg.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public AttributesManager attributesManager; // Reference to the player's attributes manager
    public Color flinchColor = Color.red;
    public float flinchDuration = 0.1f;

    private Color originalColor;
    private Image healthBarImage;

    private void Start()
    {
        // Initialize the slider values
        healthSlider.maxValue = attributesManager.maxHealth;
        healthSlider.value = attributesManager.currentHealth;

        // Get the health bar image component
        healthBarImage = healthSlider.fillRect.GetComponent<Image>();
        originalColor = healthBarImage.color;

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