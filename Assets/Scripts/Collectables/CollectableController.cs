using StatePattern.Player;
using UnityEngine;

namespace StatePattern.Collectable
{
    public class CollectableController
    {
        private CollectableScriptableObject collectableScriptableObject;
        private CollectableView collectableView;

        public CollectableController(Transform parentTransform, CollectableScriptableObject collectableScriptableObjectToSet)
        {
            collectableScriptableObject = collectableScriptableObjectToSet;
            InitializeView(parentTransform);
        }

        private void InitializeView(Transform parentTransform)
        {
            collectableView = Object.Instantiate(collectableScriptableObject.collectableView);
            Vector2 randomCircle = Random.insideUnitCircle * collectableScriptableObject.collectableRadius;
            Vector3 randomPosition = new(randomCircle.x, 0, randomCircle.y);
            Vector3 spawnPosition = parentTransform.position + randomPosition;
            collectableView.transform.SetPositionAndRotation(spawnPosition, parentTransform.rotation);
            collectableView.SetController(this);
            collectableView.SetCollectableSprite(collectableScriptableObject.collectableType);
        }

        public void PlayerHit(PlayerView playerHit)
        {
            switch (collectableScriptableObject.collectableType)
            {
                case CollectableType.Coin:
                    playerHit.CollectCoin(collectableScriptableObject.coinValue);
                    break;
                case CollectableType.FreezeBombs:
                    playerHit.FreezeEnemies(collectableScriptableObject.freezeTime, collectableScriptableObject.freezeFactor);
                    break;
                case CollectableType.TeleportationPads:
                    playerHit.CollectTeleportationPad(collectableScriptableObject.teleportationPosition);
                    break;
                case CollectableType.HealthPickups:
                    playerHit.CollectHealthPickup(collectableScriptableObject.healthValue);
                    break;
            }
        }
    }
}