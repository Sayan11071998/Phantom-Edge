using StatePattern.Main;
using StatePattern.StateMachine;

namespace StatePattern.Enemy
{
    public class CloningState<T> : IState where T : EnemyController
    {
        public EnemyController Owner { get; set; }

        private GenericStateMachine<T> stateMachine;

        public CloningState(GenericStateMachine<T> stateMachine) => this.stateMachine = stateMachine;

        public void OnStateEnter()
        {
            CreateAClone();
            CreateAClone();
        }

        public void Update() { }

        public void OnStateExit() { }

        private void CreateAClone()
        {
            CloneManController clonedMan = GameService.Instance.EnemyService.CreateEnemy(Owner.Data) as CloneManController;
            clonedMan.SetCloneCount((Owner as CloneManController).CloneCountLeft - 1);
            clonedMan.Teleport();
            clonedMan.SetDefaultColor(EnemyColorType.Clone);
            clonedMan.ChangeColor(EnemyColorType.Clone);
            GameService.Instance.EnemyService.AddEnemy(clonedMan);
        }
    }
}