using UnityEngine;

namespace Character_Common
{
    public class Character : MonoBehaviour, IDamageable
    {
        //Initial stats required
        [Header("Initial Stats")]
        [SerializeField] private int maxHealth;
        
        //Internally needed parameters
        private int _maxHealth, _currentHealth;
        private bool _hasDied;
        
        //Accessors
        public int MaxHealth
        {
            get => _maxHealth;
            set => _maxHealth = value > 0 ? value : 1;
        }
        
        public int CurrentHealth
        {
            get => _currentHealth;
            set
            {
                if (value >= 0 && value <= maxHealth)
                {
                    _currentHealth = value;
                }
                else if (value < 0)
                {
                    _currentHealth = 0;
                }
                else
                {
                    _currentHealth = maxHealth;
                }
            }
        }

        private void Awake()
        {
            //Health initialization
            MaxHealth = maxHealth;
            CurrentHealth = MaxHealth;
        }

        #region Methods

        /// <summary>
        /// This method controls health reduction. It takes a parameter and subtract it from current health.
        /// If current health reaches 0 and character hasn't die yet, it calls die method.
        /// </summary>
        /// <param name="damage">Subtracted parameter from current health</param>
        public void ReceiveDamage(int damage)
        {
            //Subtract from current health
            CurrentHealth -= damage;
            
            //Check for die condition
            if (CurrentHealth == 0 && !_hasDied) ((IDamageable)this).Die();
        }

        public void AddHealth(int healing)
        {
            CurrentHealth += healing;
        }

        void IDamageable.Die()
        {
            _hasDied = true;
        }

        #endregion 
    }
}
