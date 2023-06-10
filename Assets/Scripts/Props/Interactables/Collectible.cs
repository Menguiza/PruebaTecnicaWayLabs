using UI.Inventory;
using UnityEngine;

namespace Props.Interactables
{
    public class Collectible : MonoBehaviour, IInteractable
    {
        [Header("Item info Reference")] 
        [SerializeField] private Item item;
        
        public void Interact()
        {
            InventoryManager.instance.AddItem(item);
            Destroy(gameObject);
        }
    }
}
