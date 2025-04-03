using StatePattern.StateMachine;
using UnityEngine;

namespace StatePattern.Enemy
{
    public class AdaptiveDefenseState<T> : IState where T : EnemyController
    {
        public EnemyController Owner { get; set; }

        private GenericStateMachine<T> stateMachine;
        private float defenseTimer;

        public AdaptiveDefenseState(GenericStateMachine<T> stateMachine) => this.stateMachine = stateMachine;

        public void OnStateEnter()
        {
            Owner.isDefensive = true;
            Owner.SetDefensiveMode(true);
            defenseTimer = Owner.Data.DefenseDuration;
        }

        public void Update()
        {
            defenseTimer -= Time.deltaTime;
            if (defenseTimer <= 0)
            {
                stateMachine.ChangeState(States.IDLE);
            }
        }

        public void OnStateExit()
        {
            Owner.isDefensive = false;
            Owner.SetDefensiveMode(false);
        }
    }
}