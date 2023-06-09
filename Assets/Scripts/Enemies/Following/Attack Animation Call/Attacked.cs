using UnityEngine;
using UnityEngine.Events;

namespace Enemies.Following.Attack_Animation_Call
{
    public class Attacked : MonoBehaviour
    {
        //Events
        public UnityEvent AttackExcuted;

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public void AttackEvent()
        {
            AttackExcuted.Invoke();
        }

        #endregion
    }
}
