using System.Collections;
using UnityEngine;

namespace Enemies.Following.State_Machine
{
    public class AttackState : BaseState
    {
        //Internally needed parameters
        private Animator _attackAnimator;
        private EnemyAttack _attack;
        private float _timeBetweenAttacks, _timer;

        #region Contructor

        //Attack cycle is determined by animation timing
        public AttackState(Animator attackAnimator)
        {
            _attackAnimator = attackAnimator;
        }

        //Attack sequence is determined by a certain time
        public AttackState(EnemyAttack attack, float timeBetweenAttacks)
        {
            _attack = attack;
            _timeBetweenAttacks = timeBetweenAttacks;
        }

        #endregion
        
        public override void EnterState(GameObject affectedGameObject)
        {
            //Get affected object reference
            AffectedObject = affectedGameObject;
        }

        public override void UpdateState()
        {
            //Check if an animator was assigned
            if (_attackAnimator) return;
            
            //Decrease timer by motor's time
            _timer -= Time.deltaTime;

            //If timer reach zero or less
            if (_timer <= 0f)
            {
                //Execute attack
                _attack.Attack();

                //Reset timer
                _timer = _timeBetweenAttacks;
            }
        }
        
        public override void ExitState()
        {
            //Logic when state exit
        }
    }
}
