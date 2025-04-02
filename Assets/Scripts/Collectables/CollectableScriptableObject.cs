using UnityEngine;

namespace StatePattern.Collectable
{
    [CreateAssetMenu(fileName = "CollectableScriptableObject", menuName = "ScriptableObjects/CollectableScriptableObject")]
    public class CollectableScriptableObject : ScriptableObject
    {
        public CollectableType collectableType;
        public CollectableView collectableView;
        public float collectableRadius;

        public int coinValue;
        public int freezeTime;
        public float freezeFactor;
        public Vector3 teleportationPosition;
        public int healthValue;
    }
}