using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Player.Stats;
using UnityEditor.Rendering;
using UnityEngine;

namespace UI.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        //Singleton
        public static InventoryManager instance;
        
        [Header("Slots")]
        [SerializeField] private List<InventorySlot> slots = new List<InventorySlot>();

        [Header("Needed References")] 
        [SerializeField] private GameObject inventoryItemPrefab;
        [SerializeField] private PlayerStats playerStats;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            instance = this;
        }

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(Item item)
        {
            foreach (InventorySlot slot in slots)
            {
                InventoryItem itemInSlot = null;
                
                if(slot.transform.childCount > 0) itemInSlot = slot.transform.GetChild(0).GetComponent<InventoryItem>();

                if (itemInSlot != null)
                {
                    if (itemInSlot.ItemInfo.isStackable && itemInSlot.ItemInfo.itemName == item.itemName)
                    {
                        if (itemInSlot.Amount < itemInSlot.ItemInfo.maxStack)
                        {
                            itemInSlot.IncrementAmount();
                            slot.SyncQuantity();
                            return;
                        }
                    }
                }
                else
                {
                    SpawnItem(item, slot);
                    return;
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="slot"></param>
        public  void UseItem(int index)
        {
            if (slots.ElementAt(index).transform.childCount == 0) return;
            
            InventoryItem item = slots.ElementAt(index).transform.GetChild(0).GetComponent<InventoryItem>();

            playerStats.AddHealth(item.ItemInfo.health);
            playerStats.SpeedBoost(item.ItemInfo.speed, item.ItemInfo.boostDuration);
            
            if(item.Amount > 1) item.DecreaseAmount();
            else Destroy(item.gameObject);

            StartCoroutine(SyncFix(index));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="slot"></param>
        private void SpawnItem(Item item, InventorySlot slot)
        {
            InventoryItem newItem = Instantiate(inventoryItemPrefab, slot.transform).GetComponent<InventoryItem>();
            
            newItem.InitializeItem(item);
            
            slot.CheckFirstSync();
        }

        private IEnumerator SyncFix(int index)
        {
            yield return new WaitForSeconds(0.1f);
            slots.ElementAt(index).CheckFirstSync();
        }
        
        #endregion
    }
}
