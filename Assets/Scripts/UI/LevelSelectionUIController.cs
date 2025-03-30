using StatePattern.Main;
using System.Collections.Generic;
using UnityEngine;

namespace StatePattern.UI
{
    public class LevelSelectionUIController : IUIController
    {
        private LevelSelectionUIView levelSelectionView;
        private LevelButtonView levelButtonPrefab;
        private List<LevelButtonView> levelButtons;

        public LevelSelectionUIController(LevelSelectionUIView levelSelectionView, LevelButtonView levelButtonPrefab)
        {
            this.levelSelectionView = levelSelectionView;
            this.levelButtonPrefab = levelButtonPrefab;
            levelSelectionView.SetController(this);
            InitializeController();
        }

        private void InitializeController()
        {
            levelButtons = new List<LevelButtonView>();
            Hide();
        }

        public void Show(int levelCount)
        {
            levelSelectionView.EnableView();
            CreateLevelButtons(levelCount);
        }

        public void Hide()
        {
            ResetLevelButtons();
            levelSelectionView.DisableView();
        }

        private void ResetLevelButtons()
        {
            levelButtons.ForEach(button => Object.Destroy(button.gameObject));
            levelButtons.Clear();
        }

        public void CreateLevelButtons(int levelCount)
        {
            for (int i = 1; i <= levelCount; i++)
            {
                var newButton = levelSelectionView.AddButton(levelButtonPrefab);
                newButton.SetOwner(this);
                newButton.SetLevelID(i);
            }
        }

        public void OnLevelSelected(int levelId)
        {
            GameService.Instance.EventService.OnLevelSelected.InvokeEvent(levelId);
            Hide();
        }
    }
}