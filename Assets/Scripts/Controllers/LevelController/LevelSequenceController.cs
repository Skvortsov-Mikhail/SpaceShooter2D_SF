using UnityEngine.SceneManagement;
using UnityEngine;

namespace SpaceShooter
{
    public class LevelSequenceController : SingletoneBase<LevelSequenceController>
    {
        public static string MainMenuSceneNickname = "Main_menu";

        public Episode CurrentEpisode { get; private set; }

        public int CurrentLevel { get; private set; }

        public bool LastLevelResult { get; private set; }

        public PlayerStatistics LevelStatistics { get; private set; }

        public static SpaceShip PlayerShip { get; set; }

        public int TotalNumKills { get; private set; }
        public int TotalScore { get; private set; }
        public int TotalTime { get; private set; }


        public void StartEpisode(Episode e)
        {
            CurrentEpisode = e;
            CurrentLevel = 0;

            LevelStatistics = new PlayerStatistics();
            LevelStatistics.Reset();

            SceneManager.LoadScene(e.Levels[CurrentLevel]);
        }

        public void RestartLevel()
        {
            Time.timeScale = 1;

            SceneManager.LoadScene(CurrentEpisode.Levels[CurrentLevel]);
        }

        public void FinishCurrentLevel(bool success)
        {
            LastLevelResult = success;

            CalculateLevelStatistics();

            ResultPanelController.Instance.ShowResults(LevelStatistics, success);

            UpdateGeneralStatistic();
        }

        public void AdvanceLevel()
        {
            LevelStatistics.Reset();

            CurrentLevel++;

            if(CurrentEpisode.Levels.Length <= CurrentLevel)
            {
                SceneManager.LoadScene(MainMenuSceneNickname);
            }

            else 
            {
                RestartLevel();
            }
        }

        private void CalculateLevelStatistics()
        {
            LevelStatistics.score = Player.Instance.Score;
            LevelStatistics.numKills = Player.Instance.NumKills;
            LevelStatistics.time = (int)LevelController.Instance.LevelTime;

            float bonus = 1.0f / LevelController.Instance.LevelTime;
            float extraMultiplier = bonus * Player.Instance.ExtraBonusMultiplierPerSecond;

            if (extraMultiplier > 1.0f)
            {
                extraMultiplier = 1.0f;
            }

            LevelStatistics.extraBonus = (int)(Player.Instance.Score * extraMultiplier);
        }

        private void UpdateGeneralStatistic()
        {
            TotalNumKills += LevelStatistics.numKills;
            TotalScore += LevelStatistics.score + LevelStatistics.extraBonus;
            TotalTime += LevelStatistics.time;
        }

        public void ResetGlobalStats()
        {
            TotalNumKills = 0;
            TotalScore = 0;
            TotalTime = 0;
        }
    }
}
