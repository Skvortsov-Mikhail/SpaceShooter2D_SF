using UnityEngine;

namespace SpaceShooter
{
    public class MainMenuController : SingletoneBase<MainMenuController>
    {
        [SerializeField] private SpaceShip m_DefaultSpaceShip;

        [SerializeField] private GameObject m_EpisodeSelection;

        [SerializeField] private GameObject m_ShipSelection;

        [SerializeField] private GameObject m_Statistics;

        private void Start()
        {
            LevelSequenceController.PlayerShip = m_DefaultSpaceShip;
        }

        public void OnButtonStartNew()
        {
            m_EpisodeSelection.SetActive(true);
            gameObject.SetActive(false);
        }

        public void OnButtonSelectShip()
        {
            m_ShipSelection.SetActive(true);
            gameObject.SetActive(false);
        }

        public void OnButtonStatistics()
        {
            m_Statistics.SetActive(true);
            m_Statistics.GetComponent<StatisticsPanel>().ShowGlobalStats();
            gameObject.SetActive(false);
        }

        public void OnButtonExit()
        {
            Application.Quit();
        }
    }
}
