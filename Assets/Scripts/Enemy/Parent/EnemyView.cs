using StatePattern.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace StatePattern.Enemy
{
    public class EnemyView : MonoBehaviour
    {
        public EnemyController Controller { get; private set; }

        private SphereCollider rangeTriggerCollider;

        [SerializeField] public NavMeshAgent Agent;
        [SerializeField] private SpriteRenderer enemyGraphic;
        [SerializeField] private SpriteRenderer detectableRange;
        [SerializeField] private ParticleSystem muzzleFlash;
        [SerializeField] private List<EnemyColor> enemyColors;
        [SerializeField] private GameObject bloodStain;
        [SerializeField] private Animator animator;

        private void Start()
        {
            rangeTriggerCollider = GetComponent<SphereCollider>();
            Controller?.InitializeAgent();

            if (animator != null)
                animator.SetBool("Idle", true);
        }

        public void SetController(EnemyController controllerToSet) => Controller = controllerToSet;

        public void SetDetectableZone(float radiusToSet, float angleToSet)
        {
            SetRangeColliderRadius(radiusToSet);
            SetRangeImageRadius(radiusToSet);
        }

        private void SetRangeColliderRadius(float radiusToSet)
        {
            if (rangeTriggerCollider != null)
                rangeTriggerCollider.radius = radiusToSet;
        }

        private void SetRangeImageRadius(float radiusToSet) => detectableRange.transform.localScale = new Vector3(radiusToSet, radiusToSet, 1);

        public void PlayShootingEffect() => muzzleFlash.Play();

        private void Update()
        {
            var otherColliders = Physics.OverlapSphere(transform.position, Controller.Data.RangeRadius)?.ToList();
            var playerCollider = otherColliders?.Find(item => item.GetComponent<PlayerView>() != null && !item.isTrigger);

            if (playerCollider != null)
            {
                Vector3 playerVector = playerCollider.transform.position - transform.position;
                bool isInsideCone = Vector3.Angle(transform.forward, playerVector.normalized) <= Controller.Data.RangeAngle;

                if (isInsideCone && !IsObstructed(playerCollider.transform))
                {
                    detectableRange.color = Color.red;
                    Controller.PlayerEnteredRange(playerCollider.GetComponent<PlayerView>().Controller);
                }
                else
                {
                    detectableRange.color = Color.green;
                    Controller.PlayerExitedRange();
                }
            }
            else
            {
                detectableRange.color = Color.green;
                Controller.PlayerExitedRange();
            }

            Controller?.UpdateEnemy();
        }

        private bool IsObstructed(Transform target)
        {
            Vector3 direction = target.position - transform.position;

            if (Physics.Raycast(transform.position, direction, out RaycastHit hit))
                return hit.transform != target;

            return false;
        }

        public void Destroy() => StartCoroutine(EnemyDeathSequence());

        private IEnumerator EnemyDeathSequence()
        {
            Controller.ToggleKillOverlay(true);
            Controller.ShakeCamera();

            yield return new WaitForSeconds(0.1f);

            var blood = Instantiate(bloodStain);
            blood.transform.position = transform.position;
            Controller.ToggleKillOverlay(false);

            Destroy(gameObject);
        }

        public void ChangeColor(EnemyColorType colorType) => enemyGraphic.color = enemyColors.Find(item => item.Type == colorType).Color;

        public void SetDefaultColor(EnemyColorType colorType)
        {
            EnemyColor coloToSetAsDefault = new EnemyColor();
            coloToSetAsDefault.Type = EnemyColorType.Default;
            coloToSetAsDefault.Color = enemyColors.Find(item => item.Type == colorType).Color;

            enemyColors.Remove(enemyColors.Find(item => item.Type == EnemyColorType.Default));
            enemyColors.Add(coloToSetAsDefault);
        }
    }

    [System.Serializable]
    public struct EnemyColor
    {
        public EnemyColorType Type;
        public Color Color;
    }

    public enum EnemyColorType
    {
        Default,
        Vulnerable,
        Clone
    }
}