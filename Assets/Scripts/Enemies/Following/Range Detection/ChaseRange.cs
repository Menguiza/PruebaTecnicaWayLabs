using UnityEngine;
using UnityEngine.Events;

namespace Enemies.Following
{
    public class ChaseRange : MonoBehaviour
    {
        //Events
        public UnityEvent chaseRangeReached, chaseRangeLost;
    
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player")) chaseRangeReached.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Player")) chaseRangeLost.Invoke();
        }
    }
}
