using Enemies.Following.State_Machine;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies.Following
{
    public enum StateType
    {
        Patrol, Chase, Attack
    }
    
    [RequireComponent(typeof(EnemyAttack))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyMovement : MonoBehaviour
    {
        //Parameters needed for movement AI
        [Header("Move parameters")]
        [SerializeField] private Transform player;
        [SerializeField] [Range(1f, 5f)] private float destinationRange;
        [SerializeField] [Range(0.5f, 5f)] private float walkSpeed = 0.5f;
        [SerializeField] [Range(3f, 10)] private float runSpeed = 3f;

        //Animation components
        [Header("Animations")] 
        [SerializeField] private Animator _animator;
        
        //States
        private BaseState _currenState;
        private PatrolState _patrolState;
        private ChaseState _chaseState;
        private AttackState _attackState;
        
        //Internally needed parameters
        private EnemyAttack _attack;
        private NavMeshAgent _agent;

        private void Awake()
        {
            //Attack module reference
            _attack = GetComponent<EnemyAttack>();
            
            //NavMesh agent reference
            _agent = GetComponent<NavMeshAgent>();
            
            //Initialize states references
            _patrolState = new PatrolState(destinationRange, walkSpeed);
            _chaseState = new ChaseState(player, runSpeed);
            _attackState = _attack.AttackWithAnimationEvents ? new AttackState(_animator) : new AttackState(_attack, _attack.TimeBetweenAttacks);
            
            //First state to execute
            _currenState = _patrolState;
        }

        private void Start()
        {
            //Set enter state behavior on current state
            _currenState.EnterState(gameObject);
        }

        private void Update()
        {
            //Update state behavior
            _currenState.UpdateState();
            
            //Update animator speed parameter
            if(_animator) AnimatorParameters();
        }

        #region Methods

        /// <summary>
        /// Changes state to "PatrolState" 
        /// </summary>
        public void Patrol()
        {
            ChangeState(StateType.Patrol);
        }
        
        /// <summary>
        /// Changes state to "ChaseState" 
        /// </summary>
        public void Chase()
        {
            ChangeState(StateType.Chase);
        }

        /// <summary>
        /// Changes state to "AttackState" 
        /// </summary>
        public void Attack()
        {
            ChangeState(StateType.Attack);
        }

        #endregion
        
        #region Utility

        /// <summary>
        /// This method has the labour to exit previous state, select adequate state to set depending on type sent and
        /// enter new state assigned.
        /// </summary>
        /// <param name="type"></param>
        private void ChangeState(StateType type)
        {
            //Exit from previous state
            _currenState.ExitState();
            
            //Verify type
            switch (type)
            {
                case StateType.Patrol:
                    _currenState = _patrolState;
                    break;
                case StateType.Chase:
                    _currenState = _chaseState;
                    break;
                case StateType.Attack:
                    _currenState = _attackState;
                    break;
            }
            
            //Enter new state
            _currenState.EnterState(gameObject);
        }

        /// <summary>
        /// This method store all parameters from the animator that have to be updated every frame. 
        /// </summary>
        private void AnimatorParameters()
        {
            //Checks if the agent has stopped
            if(!_agent.isStopped)
            {
                //If agent is walking
                if(_agent.speed == walkSpeed) _animator.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
                else _animator.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
            }
            else _animator.SetFloat("Speed", 0f, 0.1f, Time.deltaTime);
        }

        #endregion
        
        #if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, destinationRange);
        }
        
        #endif
    }
}
