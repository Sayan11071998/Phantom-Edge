using StatePattern.Player;
using UnityEngine;

namespace StatePattern.Collectable
{
    public class CollectableView : MonoBehaviour
    {
        private CollectableController collectableController;

        [SerializeField] private SpriteRenderer displayCollectableSprite;
        [SerializeField] private Sprite[] collectableSprites;

        public void SetController(CollectableController controllerToSet) => collectableController = controllerToSet;

        public void SetCollectableSprite(CollectableType collectableType) => displayCollectableSprite.sprite = collectableSprites[(int)collectableType];

        private void OnTriggerEnter(Collider other)
        {
            if (HasHitPlayer(other))
            {
                if (other.isTrigger)
                    return;
                else
                    collectableController.PlayerHit(other.GetComponent<PlayerView>());
            }

            Destroy(gameObject);
        }

        private bool HasHitPlayer(Collider other) => other.GetComponent<PlayerView>() != null;
    }
}