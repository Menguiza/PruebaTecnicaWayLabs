using UnityEngine;
using UnityEngine.AI;

namespace Enemies.Following.State_Machine
{
    public class PatrolState : BaseState
    {
        //Internally needed parameters
        private Vector3 _destination;
        private readonly float _destinationRange, _walkSpeed;
        private NavMeshAgent _agent;

        #region Contructor

        public PatrolState(float destinationRange, float walkSpeed)
        {
            _destinationRange = destinationRange;
            _walkSpeed = walkSpeed;
        }

        #endregion
        
        public override void EnterState(GameObject affectedGameObject)
        {
            //Get affected object reference
            AffectedObject = affectedGameObject;

            _agent = AffectedObject.GetComponent<NavMeshAgent>();
            _agent.isStopped = false;
            _agent.speed = _walkSpeed;
        }

        public override void UpdateState()
        {
            //If agent doesn't have a path to follow
            if (!_agent.hasPath)
            {
                //Assign a random destination within "_destinationRange" radius
                _destination = RandomNavmeshLocation(_destinationRange);
                
                //Set random result as destination to agent
                _agent.SetDestination(_destination);
            }
        }

        public override void ExitState()
        {
            //Logic when state exit
        }

        #region Utility

        /// <summary>
        /// This method gets a point randomized withing a unit sphere with a determined radius. Then it adds agent's
        /// position to locate this sphere on agent's space. Then it checks if there's any near point to the
        /// random point generated. If so, it will set this position as the final position. If this position
        /// has a minimum distance of the 70% of the sphere radius it is returned. If not, the method will loop.
        /// </summary>
        /// <param name="radius">Use to generated search area of range for the random point</param>
        /// <returns>A random vector 3 used to set agent's destination</returns>
        private Vector3 RandomNavmeshLocation(float radius) 
        {
            //Initialize variable
            Vector3 finalPosition = Vector3.zero;
            
            //Set direction by random point on unit sphere with sent radius
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            
            //Locate sphere on agent's space adding position
            randomDirection += AffectedObject.transform.position;
            
            //Check for any near point of navmesh from selected sphere point
            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, 1)) 
            {
                //Set destination to result position
                finalPosition = hit.position;

                //Check if final position has a minimum distance from current agent's position
                if (Vector3.Distance(AffectedObject.transform.position, finalPosition) >= radius * 0.75f)
                    return finalPosition;
            }
            
            //Loop method
            return RandomNavmeshLocation(radius);
        }

        #endregion
    }
}
