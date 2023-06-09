using Character_Common;
using UnityEngine;
using UnityEngine.UI;

namespace Player.Stats
{
    public class PlayerStats : Character
    {
        //Needed references
        [Header("References")]
        [SerializeField] private Slider healthBar;

        private void Start()
        {
            //Initialize slider max value
            healthBar.maxValue = MaxHealth;
        }

        private void Update()
        {
            //Update bar value depending on current health
            UpdateBar();
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateBar()
        {
            healthBar.value = CurrentHealth;
        }
    }
}