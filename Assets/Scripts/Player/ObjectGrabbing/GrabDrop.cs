using Props.Pickables;
using UnityEngine;
using UnityEngine.UI;

namespace Player.ObjectGrabbing
{
    public class GrabDrop : MonoBehaviour
    {
        //Detection parameters
        [Header("Object Detection")] 
        [SerializeField] private float detectionRange = 1f;
        [SerializeField] private LayerMask whatIsPickable;

        //Where objects will be after picking them
        [Header("Object Positioning")] 
        [SerializeField] private Transform grabObjParent;
        [SerializeField] [Range(12f, 50f)] private float objectRecenterSpeed;

        //Throw Stats
        [Header("Throw Stats")] 
        [SerializeField] [Range(0f, 50f)] private float maxThrowForce;
        [SerializeField] [Range(0f, 10f)] private float incrementPerSec;
        [SerializeField] private bool automaticThrow;
        
        //Visual Feedback
        [Header("Throw UI")] 
        [SerializeField] private Slider throwForceSlider;
        [SerializeField] private Image sliderFill;
        [SerializeField] private Color startColor, endColor;
        
        //Internally needed parameters
        private RaycastHit _hit;
        private bool _handsBusy, _incrementThrow, _firstRelease = true;
        private float _throwForce, _blendTimer;
        
        //Accessors
        public bool HandsBusy => _handsBusy;
        public float DetectionRange => detectionRange;
        public Transform ItemInhold => _hit.transform;

        void Update()
        {
            //Check if player is trying to pick something
            InputDetection();
            
            //Increment throw force if input registered
            ThrowIncrement();
        }

        private void FixedUpdate()
        {
            //Apply velocity to grabbed object
            if(_handsBusy) MoveObject();
        }

        #region Methods

        /// <summary>
        /// This method checks if player is currently holding something. Then it will check if the object that is about
        /// to pick has implemented "IPickable" interface. If at least one conditional is proved, it won't perform anything.
        /// If no conditional is proved it will set the player as "busy", will start a hold by setting "_firstRelease" as true and
        /// will call "Pick" function of the item picked.
        /// </summary>
        private void PickObject()
        {
            //Error control (Don't pick something if already has something in hand or if object doesn't have IPickable implemented)
            if(_handsBusy || !_hit.transform.TryGetComponent(out IPickable picked)) return;
            
            //Object grabbed
            _handsBusy = true;
            _firstRelease = true;
            picked.Pick();
        }

        /// <summary>
        /// This method checks if player is currently holding something. Then it will check if the object that is about
        /// to drop has implemented "IPickable" interface. If at least one conditional is proved, it won't perform anything.
        /// If no conditional is proved it will call "Drop" function of the item grabbed, will apply force as impulse to the object
        /// we are about to throw ("_throwForce" should be greater than 0) and it will reset all variables needed to pick a new object.
        /// </summary>
        private void DropObject()
        {
            //Error control (Don't drop something if there's nothing in hand or if object doesn't have IPickable implemented)
            if (!_handsBusy || !_hit.transform.TryGetComponent(out IPickable picked)) return;
            
            //Set object as dropped
            picked.Drop();
            
            //If there's any force channeled, apply it
            if (_throwForce >= 0)
            {
                _hit.rigidbody.AddForce(transform.forward.normalized * _throwForce, ForceMode.Impulse);
            }
            
            //Prepare to pick a new object
            _handsBusy = false;
            throwForceSlider.value = 0;
            _throwForce = 0;
            _blendTimer = 0;
            sliderFill.color = startColor;
        }

        #endregion

        #region Utility

        /// <summary>
        /// This method will cast a ray from this gameObject position with a forward direction and with a "detectionRange"
        /// distance. It might only detect any hit info within this range and with objects that belong to "whatIsPickable"
        /// layer. If something was hit it may store this info in "_hit" RaycastHit variable.
        /// </summary>
        /// <returns>Returns true if it indeed hit something or false if it doesn't</returns>
        private bool CanPickObject()
        {
            return Physics.Raycast(transform.position, transform.forward.normalized, out _hit, detectionRange, whatIsPickable);
        }

        /// <summary>
        /// This method deals with player inputs.
        /// It might perform different behaviors depending on press and release of "Fire1" button.
        /// These behaviors are related with picking and dropping objects.
        /// </summary>
        private void InputDetection()
        {
            //Button pressed
            if (Input.GetButtonDown("Fire1"))
            {
                if (_handsBusy) //If player is holding something
                {
                    //Show increment visual feedback
                    throwForceSlider.gameObject.SetActive(true);
                    
                    //Start throw force increment
                    _incrementThrow = true;

                    //Allow throw when release 
                    _firstRelease = false;
                }
                else if (CanPickObject()) //If player wants to hold something
                {
                    //Call method to pick object
                    PickObject();
                }
            }

            //Button released
            if (Input.GetButtonUp("Fire1"))
            {
                //Check if an object was just picked or if there's no object in hold
                //(if any conditional is proved it might no perform any behavior).
                if (_firstRelease || !_handsBusy) return;
                
                //Deactivate increment visual feedback
                throwForceSlider.gameObject.SetActive(false);
                    
                //Stop throw force increment
                _incrementThrow = false;
                    
                //Call method to drop object
                DropObject();
            }
        }
        
        /// <summary>
        /// This method has the labour of apply forces to grabbed object in order to it to reach "grabObjParent" position.
        /// </summary>
        private void MoveObject()
        {
            Vector3 dirToPoint = grabObjParent.position - _hit.transform.position;
            float disToPoint = dirToPoint.magnitude;

            _hit.rigidbody.velocity = dirToPoint * (objectRecenterSpeed * disToPoint);
        }

        /// <summary>
        /// This method will determine throw force increment by time. It might also blend visual feedback color between two
        /// colors previously set at inspector. This blend is directly related with "maxThrowForce" and "incrementPerSec".
        /// Finally, this method can also trigger a throw depending on "automaticThrow". It might be checked on inspector.
        /// If it is true grabbed object will be thrown after "maxThrowForce" is reached. 
        /// </summary>
        private void ThrowIncrement()
        {
            if (_throwForce >= maxThrowForce) //Max throw force is reached
            {
                //Stop increment
                _incrementThrow = false;
                
                if (automaticThrow) //If automatic throw is enable (modifiable on inspector)
                {
                    //Turn off throw force visual feedback
                    throwForceSlider.gameObject.SetActive(false);
                    
                    //Call method to drop object
                    DropObject();
                }
            }
            else if (_incrementThrow) //If throw force increment is still active
            {
                //Increment throw force by step (the product of "incrementPerSec" multiplied by time)
                _throwForce += incrementPerSec * Time.deltaTime;

                //If blend color timer reach max duration any further logic should be performed 
                if (_blendTimer >= (maxThrowForce / incrementPerSec)) return;

                //Increment color blend timer determined by engine's time
                _blendTimer += Time.deltaTime;
                
                //Define blend ratio determined by max duration (product of max throw force and it's increment per second)
                //and current timer value
                float blendRatio = Mathf.Clamp01(_blendTimer / (maxThrowForce / incrementPerSec));
                
                //Set value blended by blend ratio value
                sliderFill.color = Color.Lerp(startColor, endColor, blendRatio);
            }
            
            //Update throw force visual feedback value (it is a slider from 0 to 1)
            throwForceSlider.value = _throwForce / maxThrowForce;
        }

        #endregion

        #if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, transform.forward.normalized * detectionRange);
        }
        
        #endif
    }
}
