using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        //Needed references
        [Header("UI References")] 
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text quantity;

        //Internally needed parameters
        private Transform _dragParent;

        //Accessible parameters 
        public Item ItemInfo { get; private set; }
        public int Amount { get; private set; } = 1;
        public TMP_Text Quantity => quantity;
        
        //Events
        public UnityEvent dragging, dropped;

        private void Update()
        {
            if(Amount == 0) Destroy(gameObject);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            image.raycastTarget = false;

            _dragParent = transform.parent;
            transform.SetParent(_dragParent.parent.parent.parent);
            
            dragging.Invoke();
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            image.raycastTarget = true;
            
            transform.SetParent(_dragParent);
            
            dropped.Invoke();
        }

        #region Utility

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dragParent"></param>
        public void SetDragParent(Transform dragParent)
        {
            _dragParent = dragParent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void InitializeItem(Item item)
        {
            if (item != null)
            {
                ItemInfo = item;
                image.sprite = item.icon;
            }
            
            Color color = Color.white;
            color.a = 255;
            image.color = color;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetInfo()
        {
            ItemInfo = null;
            image.sprite = null;
            quantity.text = "";
            
            Color color = Color.white;
            color.a = 0;
            image.color = color;
        }

        public void IncrementAmount()
        {
            Amount++;
            
            quantity.text = Amount == 1 ? "" : Amount.ToString();
        }

        public void DecreaseAmount()
        {
            Amount--;
            
            quantity.text = Amount == 1 ? "" : Amount.ToString();
        }

        #endregion
    }
}
