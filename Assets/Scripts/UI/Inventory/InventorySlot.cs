using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Inventory
{
    public class InventorySlot : MonoBehaviour, IDropHandler
    {
        //Parameters needed to sync slots
        [Header("Synchronization")]
        [SerializeField] private InventorySlot synchronizedSlot;
        [SerializeField] private InventoryItem itemInfo;
        
        //Internally needed parameters
        private bool _waitingForDrop, _called;
        private InventoryItem _item;
        
        //Accessors
        public InventoryItem InvItem => itemInfo;

        public void OnDrop(PointerEventData eventData)
        {
            if (transform.childCount == 0)
            {
                _item = eventData.pointerDrag.GetComponent<InventoryItem>();
                _item.SetDragParent(transform);
                
                _item.dragging.AddListener(WaitForDrop);
                _item.dropped.AddListener(WaitEnded);
                
                if(synchronizedSlot)
                {
                    synchronizedSlot.InvItem.InitializeItem(_item.ItemInfo);
                    SyncQuantity();
                }
            }
        }

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        private void SyncSlot()
        {
            if(!synchronizedSlot) return;

            if (transform.childCount == 0)
            {
                if (_item != null)
                {
                    _item.dragging.RemoveListener(WaitForDrop);
                    _item.dropped.RemoveListener(WaitEnded);
                }
                
                synchronizedSlot.InvItem.ResetInfo();
            }

            if (transform.childCount > 0)
            {
                _item = transform.GetChild(0).GetComponent<InventoryItem>();
                synchronizedSlot.InvItem.InitializeItem(_item.ItemInfo);
            }
        }

        #endregion

        #region Utility

        /// <summary>
        /// 
        /// </summary>
        public void CheckFirstSync()
        {
            if (transform.childCount != 0)
            {
                _item = transform.GetChild(0).GetComponent<InventoryItem>();
                
                _item.dragging.AddListener(WaitForDrop);
                _item.dropped.AddListener(WaitEnded);
            }
            
            if(synchronizedSlot) SyncSlot();
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void WaitForDrop()
        {
            _waitingForDrop = true;

            StartCoroutine(BehaviorWhenDropped());
        }

        /// <summary>
        /// 
        /// </summary>
        private void WaitEnded()
        {
            _waitingForDrop = false;
        }

        public void SyncQuantity()
        {
            synchronizedSlot.InvItem.Quantity.text = _item.Quantity.text;
        }

        #endregion

        #region Coroutines

        /// <summary>
        /// 
        /// </summary>
        private IEnumerator BehaviorWhenDropped()
        {
            yield return new WaitUntil(() => !_waitingForDrop);
            
            SyncSlot();

            yield return null;
        }

        #endregion
    }
}
