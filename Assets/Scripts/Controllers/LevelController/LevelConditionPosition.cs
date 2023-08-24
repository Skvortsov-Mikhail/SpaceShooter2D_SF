using UnityEngine;

namespace SpaceShooter
{
    public class LevelConditionPosition : MonoBehaviour, ILevelCondition
    {
        [SerializeField] private float m_TargetPointRadius;

        private bool m_Reached;

        bool ILevelCondition.isCompleted
        {
            get
            {
                if (Player.Instance != null && Player.Instance.ActiveShip != null)
                {
                    if ((transform.position - Player.Instance.ActiveShip.transform.position).sqrMagnitude < m_TargetPointRadius * m_TargetPointRadius)
                    {
                        m_Reached = true;
                    }

                    else
                    {
                        m_Reached = false;
                    }
                }

                return m_Reached;
            }
        }

#if UNITY_EDITOR
        private static readonly Color GizmosColor = new Color(1, 0, 0, 0.3f);

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = GizmosColor;
            Gizmos.DrawSphere(transform.position, m_TargetPointRadius);
        }
#endif
    }
}
