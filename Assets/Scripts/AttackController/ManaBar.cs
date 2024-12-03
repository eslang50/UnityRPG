using FantasyRpg.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace FantasyRpg.Combat
{
    public class ManaBar : MonoBehaviour
    {
        public Slider manaSlider;
        public AttributesManager attributesManager;

        private void Start()
        {
            // Initialize the slider values
            manaSlider.maxValue = attributesManager.maxMana;
            manaSlider.value = attributesManager.currentMana;
        }

        private void Update()
        {
            // Sync the slider value with the current 
            manaSlider.value = attributesManager.currentMana;
        }
    }
}