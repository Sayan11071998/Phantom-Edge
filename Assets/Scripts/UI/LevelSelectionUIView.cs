using UnityEngine;

namespace StatePattern.UI
{
    public class LevelSelectionUIView : MonoBehaviour, IUIView
    {
        [SerializeField] private Transform levelButtonContainer;

        private LevelSelectionUIController controller;

        public void SetController(IUIController controllerToSet) => controller = controllerToSet as LevelSelectionUIController;

        public void DisableView() => gameObject.SetActive(false);

        public void EnableView() => gameObject.SetActive(true);

        public LevelButtonView AddButton(LevelButtonView levelButtonPrefab) => Instantiate(levelButtonPrefab, levelButtonContainer);
    }
}