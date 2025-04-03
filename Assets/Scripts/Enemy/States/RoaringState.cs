using StatePattern.Main;
using StatePattern.StateMachine;
using UnityEngine;

namespace StatePattern.Enemy
{
    public class RoaringState<T> : IState where T : EnemyController
    {
        public EnemyController Owner { get; set; }

        private GenericStateMachine<T> stateMachine;
        private float roarTimer;

        public RoaringState(GenericStateMachine<T> stateMachine) => this.stateMachine = stateMachine;

        public void OnStateEnter()
        {
            GameService.Instance.SoundService.PlaySoundEffects(Sound.SoundType.ENEMY_BOSS_ROAR);
            GameService.Instance.PlayerService.SlowPlayerDown(Owner.Data.SlowPlayerDownDuration);
            Owner.ShakeNearbyObjects();
            roarTimer = Owner.Data.RoarDuration;
        }

        public void Update()
        {
            roarTimer -= Time.deltaTime;
            if (roarTimer <= 0)
            {
                stateMachine.ChangeState(States.CHASING);
            }
        }

        public void OnStateExit() { }
    }
}