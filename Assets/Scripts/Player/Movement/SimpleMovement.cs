using UnityEngine;

namespace Player.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class SimpleMovement : MonoBehaviour
    {
        //Changeable Parameters
        [Header("Player Stats")] 
        [SerializeField] [Range(0f, 7f)] private float moveSpeed = 1f;
        [SerializeField] [Range(8f, 20f)] private float runSpeed = 8f;
        [SerializeField] [Range(1f, 40f)] private float jumpForce = 25f;
        [SerializeField] private float groundDrag = 2f;

        [Header("Ground Detection")] 
        [SerializeField] private Transform groundOrigin;
        [SerializeField] [Range(0f, 1f)] private float groundCheckLength = 0.1f;
        [SerializeField] private LayerMask whatIsGround;

        //References
        [Header("References")] 
        [SerializeField] private Transform body;
        
        //Internally needed parameters
        private Rigidbody _rb;
        private float _horizontal, _vertical;
        private float _forceMultiplier = 10f;
        private float _speed;
        private RaycastHit _groundHit; 
        
        //Internally changed parameters
        public bool Grounded { get; private set; }

        private void Awake()
        {
            //Cursor set up
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
            
            //Rigidbody reference (Obey "RequireComponent" restriction)
            _rb = GetComponent<Rigidbody>();
            
            //Initial speed set up
            _speed = runSpeed;
        }

        void Update()
        {
            //Detect player inputs
            InputDetection();
            
            //Check if player is grounded
            Grounded = IsGrounded();
            
            //Apply drag
            ApplyDrag();
            
            //Correct Speed
            SpeedLimit();
        }

        private void FixedUpdate()
        {
            //Flat player movement (Physics environment)
            Move();
        }

        #region Methods

        /// <summary>
        /// This method deals with player's flat movement.
        /// It gets a direction derived by input and adds force to the rigidbody respectively.
        /// Behavior may change if player is grounded or gliding.
        /// </summary>
        private void Move()
        {
            if (Grounded) _rb.AddForce(MoveDirection() * (_speed * _forceMultiplier), ForceMode.Force);
            else if (!Grounded) _rb.AddForce(MoveDirection() * (_speed * _forceMultiplier * 2), ForceMode.Force);
        }

        /// <summary>
        /// This method deals with player's jump behavior.
        /// It resets player's y velocity and add an impulse.
        /// This occurs everytime the method is called.
        /// </summary>
        private void Jump()
        {
            _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);

            _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

        #endregion

        #region Utility

        /// <summary>
        /// This method deals with player's inputs.
        /// It sets 2 float variables ("Horizontal" and "Vertical") depending on input axis x and y.
        /// The use of two parameters reduces the cast of the whole input detection.
        /// It may also set player's speed. Speed should change if "Walk" button is pressed or released.
        /// Finally, if "Jump" button is pressed and player is grounded, it may call "Jump" method. 
        /// </summary>
        private void InputDetection()
        {
            _horizontal = Input.GetAxisRaw("Horizontal");
            _vertical = Input.GetAxisRaw("Vertical");

            if (Input.GetButtonDown("Walk")) _speed = moveSpeed;
            else if (Input.GetButtonUp("Walk")) _speed = runSpeed;
            
            if(Input.GetButtonDown("Jump") && Grounded) Jump();
        }
        
        /// <summary>
        /// This method determine move direction. Direction may point to player's body forward axis.
        /// This function also controls if player is stepping on a slope. It returns direction projected
        /// on the floor's normal the player is standing on.
        /// </summary>
        private Vector3 MoveDirection()
        {
            Vector3 moveDir = body.forward * _vertical + body.right * _horizontal;
            return Vector3.ProjectOnPlane(moveDir, _groundHit.normal).normalized;
        }

        /// <summary>
        /// This returns the result of a RayCast casted downwards from a origin point determined on inspector.
        /// If this raycast has hit info, is implicit something was indeed hit and it belongs to "whatIsGround" layer.
        /// This layer can be modified on inspector and may be assigned to any object we want to detect as ground.
        /// <param name="_groundHit"> This parameter is filled with collision info. Product of the RayCast. This will be useful for floor normal.</param>
        /// </summary>
        private bool IsGrounded()
        {
            return Physics.Raycast(groundOrigin.position, Vector3.down, out _groundHit, groundCheckLength,whatIsGround);
        }

        /// <summary>
        /// This method applies drag depending on player's state.
        /// If player is grounded it may add some drag. If it doesn't
        /// it may set drag at 0.
        /// </summary>
        private void ApplyDrag()
        {
            if (Grounded) _rb.drag = groundDrag;
            else _rb.drag = 0;
        }
        
        /// <summary>
        /// This method will limit player's velocity every frame.
        /// Unity's built-in physics may cause player's Rigidbody to keep some residual
        /// Velocity or acceleration. That's why this method sets Rigidbody's velocity
        /// to a determined magnitude product of the parameters we set as "moveSpeed" or
        /// "runSpeed".
        /// </summary>
        private void SpeedLimit()
        {
            Vector3 flatVel = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);

            if (flatVel.magnitude > _speed)
            {
                Vector3 limitedVel = flatVel.normalized * _speed;
                _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
            }
        }

        #endregion
        
        #if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(groundOrigin.position, -groundOrigin.up.normalized * groundCheckLength);
        }
        
        #endif
    }
}