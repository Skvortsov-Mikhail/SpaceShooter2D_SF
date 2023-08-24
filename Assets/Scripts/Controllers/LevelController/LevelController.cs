using UnityEngine;
using UnityEngine.Events;

namespace SpaceShooter
{
    public interface ILevelCondition
    {
        bool isCompleted { get; }
    }

    public class LevelController : SingletoneBase<LevelController>
    {
        [SerializeField] private int m_ReferenceTime;
        public int ReferenceTime => m_ReferenceTime;

        [SerializeField] private UnityEvent m_EventLevelComleted;

        private ILevelCondition[] m_Conditions;

        private bool m_IsLevelCompleted;

        private float m_LevelTime;
        public float LevelTime => m_LevelTime;

        private void Start()
        {
            m_Conditions = GetComponentsInChildren<ILevelCondition>();
        }

        private void Update()
        {
            if(m_IsLevelCompleted == false)
            {
                m_LevelTime += Time.deltaTime;

                CheckLevelConditions();
            }
        }

        private void CheckLevelConditions()
        {
            if (m_Conditions == null || m_Conditions.Length == 0) return;

            int numCompleted = 0;

            foreach (var v in m_Conditions)
            {
                if(v.isCompleted == true)
                {
                    numCompleted++;
                }

                if(numCompleted == m_Conditions.Length)
                {
                    m_IsLevelCompleted = true;

                    m_EventLevelComleted?.Invoke();

                    LevelSequenceController.Instance?.FinishCurrentLevel(true);
                }
            }
        }
    }
}
