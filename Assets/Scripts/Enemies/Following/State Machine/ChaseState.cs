using UnityEngine;
using UnityEngine.AI;

namespace Enemies.Following.State_Machine
{
    public class ChaseState : BaseState
    {
        private Vector3 _destination;
        private NavMeshAgent _agent;
        private readonly float _runSpeed;
        private readonly Transform _player;

        public ChaseState(Transform player, float runSpeed)
        {
            _player = player;
            _runSpeed = runSpeed;
        }
        
        public override void EnterState(GameObject affectedGameObject)
        {
            AffectedObject = affectedGameObject;
            
            _agent = AffectedObject.GetComponent<NavMeshAgent>();
            _agent.isStopped = false;
            _agent.speed = _runSpeed;
        }

        public override void UpdateState()
        {
            if (_destination == _player.position) return;
            
            _destination = _player.position;
            _agent.SetDestination(_destination);
        }

        public override void ExitState()
        {
            _agent.isStopped = true;
        }
    }
}
