using UnityEngine;

namespace SpaceShooter
{
    public class LevelConditionAllEnemies : MonoBehaviour, ILevelCondition
    {
        private bool m_Reached;

        bool ILevelCondition.isCompleted
        {
            get
            {
                if (FindObjectsOfType<SpaceShip>().Length == 1)
                {
                    m_Reached = true;
                }

                return m_Reached;
            }
        }
    }
}