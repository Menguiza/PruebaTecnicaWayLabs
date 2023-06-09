using Character_Common;
using UnityEngine;

namespace Enemies.Following
{
    public class EnemyAttack : MonoBehaviour
    {
        //Attack characteristics
        [Header("Attack Parameters")]
        [SerializeField] [Range(0.5f, 10f)] private float timeBetweenAttacks;
        [SerializeField] [Range(0f, 100f)] private int damage;
        [SerializeField] [Range(0.1f, 2f)] private float damageZoneRadius = 0.5f;
        [SerializeField] private bool attackWithAnimationEvents;

        //Needed references
        [Header("References")]
        [SerializeField] private Transform attackPoint;
    
        //Accessors
        public float TimeBetweenAttacks => timeBetweenAttacks;
        public bool AttackWithAnimationEvents => attackWithAnimationEvents;

        #region Methods

        /// <summary>
        /// This method generates an overlapped sphere at "attackPoint" position with a "damageZoneRadius"
        /// radius and register all colliders hit. Then it loops through all of them and check if they
        /// implement "IDamageable" if so, it checks if it refers to the player and call it's method
        /// "ReceiveDamage".
        /// </summary>
        public void Attack()
        {
            Collider[] hitColliders = Physics.OverlapSphere(attackPoint.position, damageZoneRadius);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.transform.parent.TryGetComponent(out IDamageable hit))
                {
                    if(hit.GetType().Name == "PlayerStats")
                        hit.ReceiveDamage(damage);
                }
            }
        }

        #endregion

        #if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, damageZoneRadius);
        }
        
        #endif
    }
}
