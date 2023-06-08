using Props.Pickables;
using UnityEngine;

namespace Player.ObjectGrabbing
{
    [RequireComponent(typeof(Rigidbody))]
    public class PickableObject : MonoBehaviour, IPickable
    {
        //Accessible parameters
        public bool Picked { get; private set; }
        
        //Internally needed parameters
        private Rigidbody _rb;

        private void Awake()
        {
            //Rigidbody reference (follows script requirement)
            _rb = GetComponent<Rigidbody>();
        }

        #region Methods

        /// <summary>
        /// This method will determine certain values to make object draggable and to declare it as picked.
        /// </summary>
        public void Pick()
        {
            //In order to the object to float around it might not be affected by gravity
            _rb.useGravity = false;
            
            //If "Pick" method is called it should be declared as picked ("Picked" as true)
            Picked = true;
        }

        /// <summary>
        /// This method will determine certain values to return object to it's original state.
        /// </summary>
        public void Drop()
        {
            //In order to the object to fall and stop drag action, it might be affected by gravity
            _rb.useGravity = true;
            
            //If "Drop" method is called it should be declared as dropped ("Picked" as false)
            Picked = false;
        }

        #endregion
    }
}
