using StatePattern.StateMachine;

namespace StatePattern.Enemy
{
    public class HitManStateMachine : GenericStateMachine<HitManController>
    {
        public HitManStateMachine(HitManController Owner) : base(Owner)
        {
            this.Owner = Owner;
            CreateStates();
            SetOwner();
        }

        private void CreateStates()
        {
            States.Add(StateMachine.States.IDLE, new IdleState<HitManController>(this));
            States.Add(StateMachine.States.PATROLLING, new PatrollingState<HitManController>(this));
            States.Add(StateMachine.States.CHASING, new ChasingState<HitManController>(this));
            States.Add(StateMachine.States.SHOOTING, new ShootingState<HitManController>(this));
            States.Add(StateMachine.States.TELEPORTING, new TeleportingState<HitManController>(this));
        }
    }
}