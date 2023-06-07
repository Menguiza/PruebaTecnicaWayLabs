using UnityEngine;

namespace Player.Movement
{
    public class BodyLookAt : MonoBehaviour
    {
        //References
        [Header("Reference Camera")]
        [SerializeField] private Camera cam;
        
        private void Update()
        {
            //Rotate player body
            RotateBody();
        }

        #region Methods

        /// <summary>
        /// This method rotates player body depending on cams forward vector.
        /// </summary>
        private void RotateBody()
        {
            Vector3 camDir = cam.transform.forward;

            transform.forward = new Vector3(camDir.x, 0, camDir.z).normalized;
        }

        #endregion
    }
}
