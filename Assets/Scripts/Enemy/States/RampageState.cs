using StatePattern.Main;
using StatePattern.Player;
using StatePattern.StateMachine;
using UnityEngine;

namespace StatePattern.Enemy
{
    public class RampageState<T> : IState where T : EnemyController
    {
        public EnemyController Owner { get; set; }

        private GenericStateMachine<T> stateMachine;
        private PlayerController target;
        private float rampageTimer;
        private float attackTimer;
        private enum SubState { Moving, Attacking }
        private SubState subState;

        public RampageState(GenericStateMachine<T> stateMachine) => this.stateMachine = stateMachine;

        public void OnStateEnter()
        {
            target = GameService.Instance.PlayerService.GetPlayer();
            Owner.Agent.speed *= 1.5f;
            Owner.damageMultiplier = 1.5f;
            rampageTimer = Owner.Data.RampageDuration;
            subState = SubState.Moving;
        }

        public void Update()
        {
            rampageTimer -= Time.deltaTime;
            if (rampageTimer <= 0)
            {
                stateMachine.ChangeState(States.IDLE);
                return;
            }

            switch (subState)
            {
                case SubState.Moving:
                    if (target != null)
                    {
                        Owner.Agent.SetDestination(target.Position);
                        if (Vector3.Distance(Owner.Position, target.Position) <= Owner.Data.PlayerAtackingDistance)
                        {
                            subState = SubState.Attacking;
                            StartAttack();
                        }
                    }
                    break;
                case SubState.Attacking:
                    attackTimer -= Time.deltaTime;
                    if (attackTimer <= 0)
                    {
                        subState = SubState.Moving;
                    }
                    break;
            }
        }

        public void OnStateExit()
        {
            Owner.Agent.speed /= 1.5f;
            Owner.damageMultiplier = 1f;
            target = null;
        }

        private void StartAttack()
        {
            if (Random.value > 0.5f)
            {
                Owner.ChargeAttack();
            }
            else
            {
                Owner.RampageAttack();
            }
            attackTimer = Owner.Data.AttackDuration;
        }
    }
}