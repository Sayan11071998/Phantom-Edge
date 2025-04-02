using StatePattern.StateMachine;

namespace StatePattern.Enemy
{
    public class TitanisStateMachine : GenericStateMachine<TitanisController>
    {
        public TitanisStateMachine(TitanisController Owner) : base(Owner)
        {
            this.Owner = Owner;
            CreateStates();
            SetOwner();
        }

        private void CreateStates()
        {
            States.Add(StateMachine.States.IDLE, new IdleState<TitanisController>(this));
            States.Add(StateMachine.States.CHASING, new ChasingState<TitanisController>(this));
            States.Add(StateMachine.States.ROARING_INTIMIDATION, new RotatingState<TitanisController>(this));
            States.Add(StateMachine.States.CHARGE_ATTACK, new ChargeAttackState<TitanisController>(this));
            States.Add(StateMachine.States.ADAPTIVE_DEFENSE, new AdaptiveDefenseState<TitanisController>(this));
            States.Add(StateMachine.States.RAMPAGE, new RampageState<TitanisController>(this));
        }
    }
}