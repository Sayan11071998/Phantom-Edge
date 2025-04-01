using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StatePattern.UI
{
    public class LevelEndUIView : MonoBehaviour, IUIView
    {
        private LevelEndUIController controller;

        [SerializeField] private TextMeshProUGUI resultText;
        [SerializeField] private Button homeButton;
        [SerializeField] private Button quitButton;

        private void Start() => SubscribeToButtonClicks();

        private void SubscribeToButtonClicks()
        {
            homeButton.onClick.AddListener(controller.OnHomeButtonClicked);
            quitButton.onClick.AddListener(controller.OnQuitButtonClicked);
        }

        public void SetController(IUIController controllerToSet) => controller = controllerToSet as LevelEndUIController;
        public void SetResultText(string textToSet) => resultText.SetText(textToSet);

        public void DisableView() => gameObject.SetActive(false);
        public void EnableView() => gameObject.SetActive(true);
    }
}