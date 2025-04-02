using StatePattern.Main;
using StatePattern.Player;
using StatePattern.StateMachine;
using UnityEngine;

namespace StatePattern.Enemy
{
    public class InfernothController : EnemyController
    {
        private InfernothStateMachine stateMachine;

        private bool summonedAt25 = false;
        private bool summonedAt50 = false;
        private bool summonedAt75 = false;

        public InfernothController(EnemyScriptableObject enemyScriptableObject) : base(enemyScriptableObject)
        {
            enemyView.SetController(this);
            ChanngeColor(EnemyColorType.Default);
            CreateStateMachine();
            stateMachine.ChangeState(States.IDLE);
        }

        private void CreateStateMachine() => stateMachine = new InfernothStateMachine(this);

        public override void UpdateEnemy()
        {
            if (currentState == EnemyState.DEACTIVE) return;
            stateMachine.Update();
        }

        public override void PlayerEnteredRange(PlayerController targetToSet)
        {
            if (!isEnemyAlerted)
            {
                base.PlayerEnteredRange(targetToSet);
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

            if (currentHealth <= enemyScriptableObject.MaximumHealth * 0.01)
            {
                stateMachine.ChangeState(States.ULTIMATE);
            }
            else if (currentHealth <= enemyScriptableObject.MaximumHealth * 0.75 && !summonedAt75)
            {
                stateMachine.ChangeState(States.SUMMONING);
                summonedAt75 = true;
            }
            else if (currentHealth <= enemyScriptableObject.MaximumHealth * 0.50 && !summonedAt50)
            {
                stateMachine.ChangeState(States.SUMMONING);
                summonedAt50 = true;
            }
            else if (currentHealth <= enemyScriptableObject.MaximumHealth * 0.25 && !summonedAt25)
            {
                stateMachine.ChangeState(States.SUMMONING);
                summonedAt25 = true;
            }
        }

        public override void FireBreathAttack()
        {
            base.FireBreathAttack();
            enemyView.FireBreathAttack();
            GameService.Instance.SoundService.PlaySoundEffects(Sound.SoundType.ENEMY_SHOOT);

            var player = GameService.Instance.PlayerService.GetPlayer();
            if (Vector3.Distance(player.Position, enemyView.transform.position) <= Data.PlayerAtackingDistance)
                player.TakeDamage(Data.FireBreathDamage);
        }

        public override void QuadrupleAttack()
        {
            base.QuadrupleAttack();
            enemyView.QuadrupleAttack();
            GameService.Instance.SoundService.PlaySoundEffects(Sound.SoundType.ENEMY_SHOOT);

            var player = GameService.Instance.PlayerService.GetPlayer();
            if (Vector3.Distance(player.Position, enemyView.transform.position) <= Data.PlayerAtackingDistance)
                player.TakeDamage(Data.QuadrupleAttackDamage);
        }

        protected override void Die()
        {
            stateMachine.ChangeState(States.IDLE);
            base.Die();
        }

        public void Teleport() => stateMachine.ChangeState(States.TELEPORTING);
        public void ChanngeColor(EnemyColorType colorType) => enemyView.ChangeColor(colorType);
    }
}