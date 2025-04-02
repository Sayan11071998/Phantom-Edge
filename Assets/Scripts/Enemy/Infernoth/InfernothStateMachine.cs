using StatePattern.StateMachine;

namespace StatePattern.Enemy
{
    public class InfernothStateMachine : GenericStateMachine<InfernothController>
    {
        public InfernothStateMachine(InfernothController Owner) : base(Owner)
        {
            this.Owner = Owner;
            CreateStates();
            SetOwner();
        }

        private void CreateStates()
        {
            States.Add(StateMachine.States.IDLE, new IdleState<InfernothController>(this));
            States.Add(StateMachine.States.CHASING, new ChasingState<InfernothController>(this));
            States.Add(StateMachine.States.ROARING_INTIMIDATION, new RoaringState<InfernothController>(this));
            States.Add(StateMachine.States.QUADRUPLE_ATTACK, new QuadrupleAttackState<InfernothController>(this));
            States.Add(StateMachine.States.FIRE_BREATH, new FireBreathState<InfernothController>(this));
            States.Add(StateMachine.States.TELEPORTING, new TeleportingState<InfernothController>(this));
            States.Add(StateMachine.States.SUMMONING, new SummoningState<InfernothController>(this));
            States.Add(StateMachine.States.ULTIMATE, new UltimateState<InfernothController>(this));
        }
    }
}