using StatePattern.StateMachine;

namespace StatePattern.Enemy
{
    public class AdaptiveDefenseState<T> : IState where T : EnemyController
    {
        public EnemyController Owner { get; set; }

        private GenericStateMachine<T> stateMachine;

        public AdaptiveDefenseState(GenericStateMachine<T> stateMachine) => this.stateMachine = stateMachine;

        public void OnStateEnter()
        {
            Owner.Data.IsDefensive = true;
            Owner.SetDefensiveMode(true);
        }

        public void Update() { }

        public void OnStateExit()
        {
            Owner.Data.IsDefensive = false;
            Owner.SetDefensiveMode(false);
        }
    }
}
