using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class StatisticsPanel : SingletoneBase<StatisticsPanel>
    {
        [SerializeField] private Text NumKillsText;
        [SerializeField] private Text TotalScoreText;
        [SerializeField] private Text TotalTimeText;

        public void OnButtonReset()
        {
            LevelSequenceController.Instance?.ResetGlobalStats();

            ShowGlobalStats();
        }

        public void OnButtonBackToMenu()
        {
            gameObject.SetActive(false);

            MainMenuController.Instance.gameObject.SetActive(true);
        }

        public void ShowGlobalStats()
        {
            NumKillsText.text = "Total Numkills: " + LevelSequenceController.Instance?.TotalNumKills.ToString();
            TotalScoreText.text = "Total Score: " + LevelSequenceController.Instance?.TotalScore.ToString();
            TotalTimeText.text = "Total Time: " + LevelSequenceController.Instance?.TotalTime.ToString();
        }
    }
}
