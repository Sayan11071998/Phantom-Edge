using StatePattern.Enemy;
using UnityEngine;

namespace StatePattern.Player
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private ParticleSystem attackVFX;

        public PlayerController Controller { get; private set; }
        public Rigidbody Rigidbody { get; private set; }

        private void Start() => Rigidbody = GetComponent<Rigidbody>();

        public void SetController(PlayerController controllerToSet) => Controller = controllerToSet;

        private void Update() => Controller?.UpdatePlayer();

        private void FixedUpdate() => Controller?.FixedUpdatePlayer();

        public void TakeDamage(int damage) => Controller.TakeDamage(damage);

        public void PlayAttackVFX() => attackVFX.Play();

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<EnemyView>() != null && !other.isTrigger)
            {
                var enemyController = other.GetComponent<EnemyView>().Controller;
                Controller.AddEnemy(enemyController);
                enemyController.ToggleEnemyColor(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<EnemyView>() != null && !other.isTrigger)
            {
                var enemyController = other.GetComponent<EnemyView>().Controller;
                Controller.RemoveEnemy(enemyController);
                enemyController.ToggleEnemyColor(false);
            }
        }
    }
}