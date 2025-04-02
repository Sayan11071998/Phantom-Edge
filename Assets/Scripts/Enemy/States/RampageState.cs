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

        public RampageState(GenericStateMachine<T> stateMachine) => this.stateMachine = stateMachine;

        public void OnStateEnter() => SetTarget();

        public void Update()
        {
            MoveTowardsTarget();

            if (ReachedTarget())
            {
                ResetPath();
                stateMachine.ChangeState(States.RAMPAGE);
            }
        }

        public void OnStateExit() => target = null;

        private void SetTarget() => target = GameService.Instance.PlayerService.GetPlayer();
        private bool MoveTowardsTarget() => Owner.Agent.SetDestination(target.Position);

        private bool ReachedTarget()
        {
            var currentDistanceFromPlayer = Vector3.Distance(Owner.Position, target.Position);
            return currentDistanceFromPlayer <= Owner.Data.PlayerAtackingDistance;
        }

        private void ResetPath()
        {
            Owner.Agent.isStopped = true;
            Owner.Agent.ResetPath();
        }
    }
}
