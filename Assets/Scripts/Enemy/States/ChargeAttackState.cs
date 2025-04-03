using StatePattern.Main;
using StatePattern.Player;
using StatePattern.StateMachine;
using UnityEngine;

namespace StatePattern.Enemy
{
    public class ChargeAttackState<T> : IState where T : EnemyController
    {
        public EnemyController Owner { get; set; }

        private GenericStateMachine<T> stateMachine;
        private PlayerController target;

        public ChargeAttackState(GenericStateMachine<T> stateMachine) => this.stateMachine = stateMachine;

        public void OnStateEnter()
        {
            target = GameService.Instance.PlayerService.GetPlayer();
            if (Owner is TitanisController titan)
            {
                titan.Agent.speed = titan.Data.ChargeSpeed;
            }
        }

        public void Update()
        {
            if (target != null)
            {
                Owner.Agent.SetDestination(target.Position);
                if (Vector3.Distance(Owner.Position, target.Position) <= Owner.Data.PlayerAtackingDistance)
                {
                    Owner.Agent.ResetPath();
                    Owner.ChargeAttack();
                    stateMachine.ChangeState(States.IDLE);
                }
            }
        }

        public void OnStateExit()
        {
            if (Owner is TitanisController titan)
            {
                titan.Agent.speed = titan.Data.MovementSpeed;
            }

            target = null;
        }
    }
}