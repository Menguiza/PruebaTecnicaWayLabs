using UnityEngine;

namespace UI.Utils
{
    public class FollowAtWorld : MonoBehaviour
    {
        //Follow required parameters
        [Header("Follow Parameters")] 
        [SerializeField] private Transform lookAt;
        [SerializeField] private Vector3 offset;
        
        //Internally needed parameters
        private Camera _cam;

        private void Awake()
        {
            //Camera reference
            _cam = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            //Update position to look at
            UIFollow();
        }

        #region Methods

        /// <summary>
        /// This method has the labour to assign follow target from the info sent.
        /// </summary>
        /// <param name="lookAt">Transform info of desired target</param>
        public void AssignLookAt(Transform lookAt)
        {
            //Assign target value
            this.lookAt = lookAt;
        }

        /// <summary>
        /// This function has the task to reset target's value.
        /// </summary>
        public void UnAssignLookAt()
        {
            //Set target to null
            lookAt = null;
        }
        
        /// <summary>
        /// this method has the core task of the script. It checks if there's target to follow. If so, it'll
        /// project target's position as a screen pos. Then it will set this object's position to the screen pos
        /// result. 
        /// </summary>
        private void UIFollow()
        {
            //Check if there's any target
            if (!lookAt) return;
            
            //lookAt pos to screen point
            Vector3 pos = _cam.WorldToScreenPoint(lookAt.position + offset);

            //Set position if is different to previous frame
            if (transform.position != pos)
                transform.position = pos;
        }

        #endregion
    }
}
