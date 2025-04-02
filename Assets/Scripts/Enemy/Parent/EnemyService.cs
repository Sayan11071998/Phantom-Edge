using StatePattern.Collectable;
using StatePattern.Level;
using StatePattern.Main;
using StatePattern.Player;
using StatePattern.Sound;
using StatePattern.UI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StatePattern.Enemy
{
    public class EnemyService
    {
        private SoundService SoundService => GameService.Instance.SoundService;
        private UIService UIService => GameService.Instance.UIService;
        private LevelService LevelService => GameService.Instance.LevelService;
        private PlayerService PlayerService => GameService.Instance.PlayerService;

        private List<EnemyController> activeEnemies;
        private int spawnedEnemies;

        public EnemyService()
        {
            InitializeVariables();
            SubscribeToEvents();
        }

        private void InitializeVariables() => activeEnemies = new List<EnemyController>();

        private void SubscribeToEvents() => GameService.Instance.EventService.OnLevelSelected.AddListener(SpawnEnemies);
        private void UnsubscribeToEvents() => GameService.Instance.EventService.OnLevelSelected.RemoveListener(SpawnEnemies);

        public void SpawnEnemies(int levelId)
        {
            List<EnemyScriptableObject> enemyDataForLevel = LevelService.GetEnemyDataForLevel(levelId);

            foreach (EnemyScriptableObject enemySO in enemyDataForLevel)
            {
                EnemyController enemy = CreateEnemy(enemySO);
                AddEnemy(enemy);
            }

            SetEnemyCount();
            UnsubscribeToEvents();
        }

        private void SetEnemyCount()
        {
            spawnedEnemies = activeEnemies.Count;
            UIService.UpdateEnemyCount(activeEnemies.Count, spawnedEnemies);
        }

        public EnemyController CreateEnemy(EnemyScriptableObject enemyScriptableObject)
        {
            EnemyController enemy;

            switch (enemyScriptableObject.Type)
            {
                case EnemyType.OnePunchMan:
                    enemy = new OnePunchManController(enemyScriptableObject);
                    break;
                case EnemyType.PatrolMan:
                    enemy = new PatrolManController(enemyScriptableObject);
                    break;
                case EnemyType.HitMan:
                    enemy = new HitManController(enemyScriptableObject);
                    break;
                case EnemyType.CloneMan:
                    enemy = new CloneManController(enemyScriptableObject);
                    break;
                case EnemyType.Infernoth:
                    enemy = new InfernothController(enemyScriptableObject);
                    break;
                case EnemyType.Titanis:
                    enemy = new TitanisController(enemyScriptableObject);
                    break;
                default:
                    enemy = new EnemyController(enemyScriptableObject);
                    break;
            }

            return enemy;
        }

        public void AddEnemy(EnemyController enemy) => activeEnemies.Add(enemy);

        public async void EnemyDied(EnemyController deadEnemy)
        {
            activeEnemies.Remove(deadEnemy);
            PlayerService.GetPlayer().RemoveEnemy(deadEnemy);
            SoundService.PlaySoundEffects(SoundType.ENEMY_DEATH);

            foreach (var collectableData in deadEnemy.Data.collectableData)
                _ = new CollectableController(deadEnemy.EnemyView.transform, collectableData);

            UIService.UpdateEnemyCount(activeEnemies.Count, spawnedEnemies);

            if (PlayerWon())
            {
                SoundService.PlaySoundEffects(SoundType.GAME_WON);
                await Task.Delay(deadEnemy.Data.DelayAfterGameEnd * 1000);
                UIService.GameWon();
            }
        }

        public void PlayerDied()
        {
            foreach (EnemyController enemy in activeEnemies)
                enemy.SetState(EnemyState.DEACTIVE);
        }

        private bool PlayerWon() => activeEnemies.Count == 0;

        public void FreezeEnemies(int freezeTime, float freezeFactor)
        {
            if (activeEnemies != null && activeEnemies.Count > 0)
            {
                foreach (var enemy in activeEnemies)
                    enemy.FreezeEnemy(freezeTime, freezeFactor);
            }
        }
    }
}