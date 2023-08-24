using UnityEngine;

namespace SpaceShooter
{
    public class LevelConditionTime : MonoBehaviour, ILevelCondition
    {
        [SerializeField] private float m_LevelTime;

        private bool m_Reached;

        bool ILevelCondition.isCompleted
        {
            get
            {
                if (LevelController.Instance.LevelTime <= m_LevelTime)
                {
                    m_Reached = true;
                }

                else
                {
                    LevelSequenceController.Instance.FinishCurrentLevel(false);
                }

                return m_Reached;
            }
        }
    }
}
