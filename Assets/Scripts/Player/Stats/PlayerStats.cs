using System.Collections;
using Character_Common;
using Player.Movement;
using UnityEngine;
using UnityEngine.UI;

namespace Player.Stats
{
    [RequireComponent(typeof(SimpleMovement))]
    public class PlayerStats : Character
    {
        //Needed references
        [Header("References")]
        [SerializeField] private Slider healthBar;
        
        //Internally needed parameters
        private Vector2 _defaultStats;
        private SimpleMovement _simpleMovement;

        private void Start()
        {
            //Initialize slider max value
            healthBar.maxValue = MaxHealth;

            _simpleMovement = GetComponent<SimpleMovement>();
        }

        private void Update()
        {
            //Update bar value depending on current health
            UpdateBar();
        }

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="boostRatio"></param>
        public void SpeedBoost(float boostRatio, float boostDuration)
        {
            _defaultStats = _simpleMovement.SpeedAxis;
            
            _simpleMovement.SpeedAffected(_defaultStats.x + boostRatio, _defaultStats.y + boostRatio);

            StartCoroutine(BoostDuration(boostDuration));
        }

        #endregion
        
        #region Utility

        /// <summary>
        /// 
        /// </summary>
        private void UpdateBar()
        {
            healthBar.value = CurrentHealth;
        }

        #endregion

        #region Coroutines

        private IEnumerator BoostDuration(float duration)
        {
            yield return new WaitForSeconds(duration);
            
            _simpleMovement.SpeedAffected(_defaultStats.x, _defaultStats.y);
        }

        #endregion
    }
}