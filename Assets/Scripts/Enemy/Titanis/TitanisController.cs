using StatePattern.Main;
using StatePattern.Player;
using StatePattern.StateMachine;
using UnityEngine;

namespace StatePattern.Enemy
{
    public class TitanisController : EnemyController
    {
        private TitanisStateMachine stateMachine;

        public TitanisController(EnemyScriptableObject enemyScriptableObject) : base(enemyScriptableObject)
        {
            enemyView.SetController(this);
            ChangeColor(EnemyColorType.Monster1);
            CreateStateMachine();
            stateMachine.ChangeState(States.IDLE);
        }

        private void CreateStateMachine() => stateMachine = new TitanisStateMachine(this);

        public override void UpdateEnemy()
        {
            if (currentState == EnemyState.DEACTIVE) return;
            stateMachine.Update();
        }

        public override void PlayerEnteredRange(PlayerController target)
        {
            if (!isEnemyAlerted)
            {
                base.PlayerEnteredRange(target);
                stateMachine.ChangeState(States.ROARING_INTIMIDATION);
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

        public override void TakeDamage(int damageValue)
        {
            base.TakeDamage(damageValue);

            if (currentHealth <= enemyScriptableObject.MaximumHealth * 0.3)
                stateMachine.ChangeState(States.ADAPTIVE_DEFENSE);
        }

        public override void ChargeAttack()
        {
            base.ChargeAttack();
            enemyView.ChargeAttack();
            GameService.Instance.SoundService.PlaySoundEffects(Sound.SoundType.ENEMY_SHOOT);

            var player = GameService.Instance.PlayerService.GetPlayer();
            if (Vector3.Distance(player.Position, enemyView.transform.position) <= Data.PlayerAtackingDistance)
                player.TakeDamage(Data.ChargeAttackDamage);
        }

        public override void RampageAttack()
        {
            base.RampageAttack();
            enemyView.RampageAttack();
            GameService.Instance.SoundService.PlaySoundEffects(Sound.SoundType.ENEMY_SHOOT);

            var player = GameService.Instance.PlayerService.GetPlayer();
            if (Vector3.Distance(player.Position, enemyView.transform.position) <= Data.PlayerAtackingDistance)
                player.TakeDamage(Data.RampageAttackDamage);
        }

        protected override void Die()
        {
            stateMachine.ChangeState(States.IDLE);
            base.Die();
        }

        public void ChangeColor(EnemyColorType colorType) => enemyView.ChangeColor(colorType);
    }
}