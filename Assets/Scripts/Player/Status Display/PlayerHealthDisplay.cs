using System.Collections;
using System.Collections.Generic;
using AstroWorld.Common;
using UnityEngine;
using UnityEngine.UI;

namespace AstroWorld.Player.StatusDisplay
{
    public class PlayerHealthDisplay : MonoBehaviour
    {
        [Header("Colors")]
        public Color minHealthColor = Color.red;
        public Color halfHealthColor = Color.yellow;
        public Color maxHealthColor = Color.green;

        [Header("UI Display")]
        public Slider healthSlider;
        public Image healthFiller;

        [Header("Player Component")]
        public HealthSetter _healthSetter;

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update() => DisplayHealthToUI();

        private void DisplayHealthToUI()
        {
            if (_healthSetter == null)
                return;

            float currentHealth = _healthSetter.GetCurrentHealth();
            float maxHealth = _healthSetter.maxHealthAmount;

            float healthRatio = currentHealth / maxHealth;

            if (healthRatio > 0.5f)
                healthFiller.color = Color.Lerp(halfHealthColor, maxHealthColor, (healthRatio - 0.5f) * 2);
            else
                healthFiller.color = Color.Lerp(minHealthColor, halfHealthColor, healthRatio * 2);

            healthSlider.value = healthRatio;
        }
    }
}