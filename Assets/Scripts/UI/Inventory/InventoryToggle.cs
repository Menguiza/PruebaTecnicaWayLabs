using Cinemachine;
using UnityEngine;

namespace UI.Inventory
{
    public class InventoryToggle : MonoBehaviour
    {
        //References required
        [Header("Inventory References")]
        [SerializeField] private CanvasGroup inventory;
        
        //Needed references
        [Header("Additional References")] 
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
    
        //Internally needed variables
        private Vector2 _previousInputGain;
        private CinemachinePOV _cameraBehavior;

        private void Awake()
        {
            _cameraBehavior = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        }

        private void Start()
        {
            if (!_cameraBehavior) return;
            
            _previousInputGain.x = _cameraBehavior.m_HorizontalAxis.m_MaxSpeed;
            _previousInputGain.y = _cameraBehavior.m_VerticalAxis.m_MaxSpeed;
        }

        void Update()
        {
            InputDetection();
        }

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        private void ToggleCondition()
        {
            if(inventory.alpha == 1) HideInventory();
            else ShowInventory();
        }

        #endregion

        #region Utility

        /// <summary>
        /// 
        /// </summary>
        private void InputDetection()
        {
            if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.I))
            {
                ToggleCondition();
            }

            if (Input.GetKeyDown(KeyCode.Escape) && inventory.alpha == 1)
            {
                ToggleCondition();
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void ShowInventory()
        {
            inventory.alpha = 1;
            inventory.blocksRaycasts = true;
            inventory.interactable = true;

            if (_cameraBehavior)
            {
                _previousInputGain.x = _cameraBehavior.m_HorizontalAxis.m_MaxSpeed;
                _previousInputGain.y = _cameraBehavior.m_VerticalAxis.m_MaxSpeed;
                
                _cameraBehavior.m_HorizontalAxis.m_MaxSpeed = 0;
                _cameraBehavior.m_VerticalAxis.m_MaxSpeed = 0;
            }
            
            SetCursor(true);
        }
    
        /// <summary>
        /// 
        /// </summary>
        private void HideInventory()
        {
            inventory.alpha = 0;
            inventory.blocksRaycasts = false;
            inventory.interactable = false;

            if (_cameraBehavior)
            {
                _cameraBehavior.m_HorizontalAxis.m_MaxSpeed = _previousInputGain.x;
                _cameraBehavior.m_VerticalAxis.m_MaxSpeed = _previousInputGain.y;
            }
            
            SetCursor(false);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        private void SetCursor(bool state)
        {
            if (state)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                return;
            }
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        #endregion
    }
}
