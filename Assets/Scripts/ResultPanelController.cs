using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class ResultPanelController : SingletoneBase<ResultPanelController>
    {
        [SerializeField] private Text m_Kills;
        [SerializeField] private Text m_Score;
        [SerializeField] private Text m_Time;
        [SerializeField] private Text m_ExtraBonus;

        [SerializeField] private Text m_Result;
        [SerializeField] private Text m_ButtonNextText;

        private bool m_Success;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void ShowResults(PlayerStatistics levelResults, bool success)
        {
            gameObject.SetActive(true);

            m_Success = success;

            m_Result.text = success ? "Win" : "Lose";

            m_Kills.text = "Kills: " + levelResults.numKills.ToString();
            m_Score.text = "Score: " + levelResults.score.ToString();
            m_Time.text = "Time: " + levelResults.time.ToString();
            m_ExtraBonus.text = "Extra time bonus: " + levelResults.extraBonus.ToString();

            m_ButtonNextText.text = success ? "Next" : "Restart";

            Time.timeScale = 0;
        }

        public void OnButtonNextAction()
        {
            gameObject.SetActive(false);

            Time.timeScale = 1;

            if(m_Success == true)
            {
                LevelSequenceController.Instance.AdvanceLevel();
            }

            else
            {
                LevelSequenceController.Instance.RestartLevel();
            }
        }
    }
}
