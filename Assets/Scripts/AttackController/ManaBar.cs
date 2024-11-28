using FantasyRpg.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public Slider manaSlider;
    public AttributesManager attributesManager; // Reference to the player's attributes manager
    public Color flinchColor = Color.blue;
    public float flinchDuration = 0.1f;

    private Color originalColor;
    private Image manaBarImage;

    private void Start()
    {
        // Initialize the slider values
        manaSlider.maxValue = attributesManager.maxMana;
        manaSlider.value = attributesManager.currentMana;

        // Get the mana bar image component
        manaBarImage = manaSlider.fillRect.GetComponent<Image>();
        originalColor = manaBarImage.color;

        // Subscribe to the TakeDamage event
        attributesManager.OnTakeDamage += HandleTakeDamage;
    }

    private void Update()
    {
        // Sync the slider value with the current 
        manaSlider.value = attributesManager.currentMana;
    }

    private void HandleTakeDamage(int damage)
    {
        StartCoroutine(FlinchEffect());
    }

    private IEnumerator FlinchEffect()
    {
        // Change the color to the flinch color
        manaBarImage.color = flinchColor;

        // Wait for the flinch duration
        yield return new WaitForSeconds(flinchDuration);

        // Revert the color back to the original color
        manaBarImage.color = originalColor;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the TakeDamage event
        attributesManager.OnTakeDamage -= HandleTakeDamage;
    }
}