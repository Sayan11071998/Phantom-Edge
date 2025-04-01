using StatePattern.Enemy;
using UnityEngine;

namespace StatePattern.Player
{
    public class PlayerView : MonoBehaviour
    {
        public PlayerController Controller { get; private set; }
        public Rigidbody Rigidbody { get; private set; }

        [SerializeField] private ParticleSystem attackVFX;
        [SerializeField] private Animator animator;

        private void Start() => Rigidbody = GetComponent<Rigidbody>();

        public void SetController(PlayerController controllerToSet) => Controller = controllerToSet;

        private void Update() => Controller?.UpdatePlayer();

        private void FixedUpdate() => Controller?.FixedUpdatePlayer();

        public void TakeDamage(int damage) => Controller.TakeDamage(damage);

        private void PlayAttackVFX() => attackVFX.Play();
        private void PlayAttackAnimation() => animator.SetTrigger("attack");
        public void PlayMovementAnimation(bool isIdle) => animator.SetBool("isIdle", isIdle);
        public void PlayDeathAnimation() => animator.SetTrigger("death");

        public void Attack()
        {
            PlayAttackVFX();
            PlayAttackAnimation();
        }

        public void Move(Vector3 position)
        {
            Rigidbody.MovePosition(position);
            PlayMovementAnimation(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<EnemyView>() != null && !other.isTrigger)
            {
                var enemyController = other.GetComponent<EnemyView>().Controller;
                Controller.AddEnemy(enemyController);
                enemyController.ToggleEnemyColor(EnemyColorType.Vulnerable);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<EnemyView>() != null && !other.isTrigger)
            {
                var enemyController = other.GetComponent<EnemyView>().Controller;
                Controller.RemoveEnemy(enemyController);
                enemyController.ToggleEnemyColor(EnemyColorType.Default);
            }
        }

        public void CollectCoin(int coinValue) => Controller?.CollectCoin(coinValue);
    }
}