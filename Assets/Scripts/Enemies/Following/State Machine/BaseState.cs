using UnityEngine;

namespace Enemies.Following.State_Machine
{
    public abstract class BaseState
    {
        private protected GameObject AffectedObject;

        /// <summary>
        /// Method called when state is started
        /// </summary>
        /// <param name="affectedGameObject">Object that implements an state machine behavior</param>
        public abstract void EnterState(GameObject affectedGameObject);

        /// <summary>
        /// Method called every frame to update state's behavior
        /// </summary>
        public abstract void UpdateState();

        /// <summary>
        /// When current state is changed previous has to exit first (optional if not implemented)
        /// </summary>
        public abstract void ExitState();
    }
}