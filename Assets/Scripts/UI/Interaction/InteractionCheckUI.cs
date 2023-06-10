using System;
using Player.ObjectGrabbing;
using Props.Interactables;
using Props.Pickables;
using UI.Inventory;
using UI.Utils;
using UnityEngine;

namespace UI
{
    public class InteractionCheckUI : MonoBehaviour
    {
        //Follow UI elements in use
        [Header("Throwable objects UI")] 
        [SerializeField] private FollowAtWorld grabIcon;
        [SerializeField] private FollowAtWorld throwIcon;

        [Header("Interactable objects UI")] 
        [SerializeField] private FollowAtWorld interactableIcon;
        
        //Needed references
        [Header("References")] 
        [SerializeField] private GrabDrop _grabDrop;
        
        //Internally needed parameters
        private RaycastHit hit;

        private void Update()
        {
            //Check if player is holding something
            if (_grabDrop.HandsBusy)
            {
                //Check if throw icon has already been activated
                CheckHold();
                return;
            }
            
            //Check pickable objects on range
            CheckPickableOnRange();
        }

        #region Methods

        /// <summary>
        /// Check if throw icon is active. If not, deactivate any follow icon turned on, activate throw icon and
        /// sent pickable object info to throw icon in order to follow it.
        /// </summary>
        private void CheckHold()
        {
            //Check throw icon status
            if (!throwIcon.gameObject.activeSelf)
            {
                //Turn off any other follow UI
                TurnOffUI();
                
                //Activate throw icon
                throwIcon.gameObject.SetActive(true);
                
                //Send pickable info to follow
                throwIcon.AssignLookAt(_grabDrop.ItemInhold);
            }
        }

        /// <summary>
        /// This method has the behavior to throw a raycast with the same properties has the "GrabDrop" one. This one will
        /// detect for pickable objects on range and will activate the proper UI follow icon.
        /// </summary>
        private void CheckPickableOnRange()
        {
            //Check if ray has hit info
            if(Physics.Raycast(_grabDrop.transform.position, _grabDrop.transform.forward.normalized, out hit, _grabDrop.DetectionRange))
            {
                //Check if hit object has implemented "IPickable"
                if (hit.collider.TryGetComponent(out IPickable pickable))
                {
                    //Check if grab icon is already active
                    if(!grabIcon.gameObject.activeSelf) grabIcon.gameObject.SetActive(true);
                    
                    //Send hit info in order to accomplish follow behavior
                    grabIcon.AssignLookAt(hit.transform);
                    return;
                }
                
                //Check if hit object has implemented "IInteractable"
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    //Check if interactable icon is already active
                    if(!interactableIcon.gameObject.activeSelf) interactableIcon.gameObject.SetActive(true);
                    
                    //Send hit info in order to accomplish follow behavior
                    interactableIcon.AssignLookAt(hit.transform);
                    
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        interactable.Interact();
                    }
                    
                    return;
                }
            }
            
            //Turn off any residual follow UI active
            TurnOffUI();
        }
        
        #endregion

        #region Utility

        /// <summary>
        /// This method will turn off all follow UI implemented and reset it's follow target
        /// </summary>
        private void TurnOffUI()
        {
            grabIcon.gameObject.SetActive(false);
            throwIcon.gameObject.SetActive(false);
            interactableIcon.gameObject.SetActive(false);
            
            grabIcon.UnAssignLookAt();
            throwIcon.UnAssignLookAt();
            interactableIcon.UnAssignLookAt();
        }

        #endregion
    }
}
