using UnityEngine;

namespace SpaceShooter
{
    public class AIRoutePatrolSpawner : EntitySpawner
    {
        [SerializeField] AIController m_AIRoutePatrolPrefab;
        protected override void SpawnEntities()
        {
            for (int i = 0; i < m_NumSpawns; i++)
            {
                AIController ai = Instantiate(m_AIRoutePatrolPrefab);

                ai.transform.position = m_Area.GetRandomInsideZone();
                ai.SetRoutePatrolBehaviour();
            }
        }
    }
}
