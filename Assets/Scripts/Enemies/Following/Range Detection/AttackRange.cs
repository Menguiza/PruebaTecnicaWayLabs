using UnityEngine;
using UnityEngine.Events;

namespace Enemies.Following
{
    public class AttackRange : MonoBehaviour
    {
        //Events
        public UnityEvent attackRangeReached, attackRangeLost;
        
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player")) attackRangeReached.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Player")) attackRangeLost.Invoke();
        }
    }
}
