using StatePattern.Player;
using StatePattern.StateMachine;

namespace StatePattern.Enemy
{
    public class HitManController : EnemyController
    {
        private HitManStateMachine stateMachine;

        public HitManController(EnemyScriptableObject enemyScriptableObject) : base(enemyScriptableObject)
        {
            enemyView.SetController(this);
            CreateStateMachine();
            stateMachine.ChangeState(States.IDLE);
        }

        private void CreateStateMachine() => stateMachine = new HitManStateMachine(this);

        public override void UpdateEnemy()
        {
            if (currentState == EnemyState.DEACTIVE) return;
            stateMachine.Update();
        }

        public override void Shoot()
        {
            base.Shoot();
            stateMachine.ChangeState(States.TELEPORTING);
        }

        public override void PlayerEnteredRange(PlayerController targetToSet)
        {
            if (!isEnemyAlerted)
            {
                base.PlayerEnteredRange(targetToSet);
                stateMachine.ChangeState(States.CHASING);
            }
        }

        public override void PlayerExitedRange()
        {
            if (isEnemyAlerted)
            {
                base.PlayerExitedRange();
                stateMachine.ChangeState(States.IDLE);
            }
        }
    }
}