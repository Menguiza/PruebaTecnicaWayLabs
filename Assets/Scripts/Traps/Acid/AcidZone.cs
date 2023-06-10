using Character_Common;
using UnityEngine;

namespace Traps.Acid
{
    public class AcidZone : MonoBehaviour
    {
        [Header("Reset Position")] 
        [SerializeField] private Vector3 resetPlayerPosition;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.transform.parent.TryGetComponent(out IDamageable damageable))
                {
                    damageable.ReceiveDamage(50);
                }

                other.transform.parent.position = resetPlayerPosition;
            }
        }
    }
}
