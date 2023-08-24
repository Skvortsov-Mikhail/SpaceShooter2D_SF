using UnityEngine;

namespace SpaceShooter
{
    public class AIRandomPatrolSpawner : EntitySpawner
    {
        [SerializeField] AIController m_AIRandomPatrolPrefab;
        protected override void SpawnEntities()
        {
            for (int i = 0; i < m_NumSpawns; i++)
            {
                AIController ai = Instantiate(m_AIRandomPatrolPrefab);

                ai.transform.position = m_Area.GetRandomInsideZone();
                ai.SetRandomPatrolBehaviour();
            }
        }
    }
}
