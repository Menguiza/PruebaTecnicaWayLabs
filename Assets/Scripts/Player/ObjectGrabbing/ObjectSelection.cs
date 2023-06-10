using System.Collections.Generic;
using System.Linq;
using UI.Inventory;
using UnityEngine;

namespace Player.ObjectGrabbing
{
    public class ObjectSelection : MonoBehaviour
    {
        //Needed references
        [Header("Selection Positions")]
        [SerializeField] private List<InventorySlot> posiblePositions;
        
        //Internally needed parameters
        private int currentIndex = 0;

        void Update()
        {
            //Locate border of selection by scroll wheel input
            ScrollPosition();

            if (Input.GetKeyDown(KeyCode.Return))
            {
                InventoryManager.instance.UseItem(currentIndex);
            }
        }

        #region Utility

        /// <summary>
        /// 
        /// </summary>
        private void ScrollPosition()
        {
            float scrollDelta = Input.mouseScrollDelta.y;
    
            if (scrollDelta != 0f)
            {
                currentIndex += Mathf.RoundToInt(scrollDelta);
                
                if (currentIndex < 0)
                {
                    currentIndex = posiblePositions.Count - 1;
                }
                else if (currentIndex >= posiblePositions.Count)
                {
                    currentIndex = 0;
                }

                transform.position = posiblePositions.ElementAt(currentIndex).transform.position;
            }
        }

        private void UseItem()
        {
            
        }

        #endregion
    }
}
