using UnityEngine;

public class AIRoutePoint : MonoBehaviour
{
    [SerializeField] private float m_Radius;

    [SerializeField] private int m_ID;
    public int ID => m_ID;
    public float Radius => m_Radius;

#if UNITY_EDITOR
    private static readonly Color GizmosColor = new Color(0, 0, 1, 0.2f);

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = GizmosColor;
        Gizmos.DrawSphere(transform.position, m_Radius);
    }

#endif
}
