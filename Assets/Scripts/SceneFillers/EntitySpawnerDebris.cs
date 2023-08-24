using UnityEngine;


namespace SpaceShooter
{
    public class EntitySpawnerDebris : MonoBehaviour
    {
        [SerializeField] private Destructible[] m_DebrisPrefab;

        [SerializeField] private CircleArea m_Area;

        [SerializeField] private int m_NumDebris;

        [SerializeField] private float m_RandomSpeed;

        [SerializeField] private GameObject m_DestroyEffectPrefab;

        private int m_DebrisCount;

        private void Start()
        {
            for (int i = 0; i < m_NumDebris; i++)
            {
                SpawnDebris();
            }
        }

        private void Update()
        {
            if (m_DebrisCount < m_NumDebris)
                SpawnDebris();
        }

        private void SpawnDebris()
        {
            int index = Random.Range(0, m_DebrisPrefab.Length);

            GameObject debris = Instantiate(m_DebrisPrefab[index].gameObject);

            debris.transform.position = m_Area.GetRandomInsideZone();
            debris.GetComponent<Destructible>().EventOnDeath.AddListener(OnDebrisDead);

            Rigidbody2D rb = debris.GetComponent<Rigidbody2D>();

            if (rb != null && m_RandomSpeed > 0)
            {
                rb.velocity = (Vector2) UnityEngine.Random.insideUnitSphere * m_RandomSpeed;
            }

            m_DebrisCount++;
        }

        private void OnDebrisDead(GameObject debris)
        {
            m_DebrisCount--;

            SpawnSmallerDebris(debris);

            if (m_DestroyEffectPrefab != null)
            {
                var effect = Instantiate(m_DestroyEffectPrefab);
                effect.transform.position = debris.transform.position;
            }
        }

        private void SpawnSmallerDebris(GameObject gameObject)
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject smallerObject = Instantiate(gameObject, gameObject.transform.root.position, Quaternion.identity);
                smallerObject.GetComponent<Destructible>().enabled = true;
                smallerObject.GetComponent<Collider2D>().enabled = true;
                smallerObject.GetComponent<Rigidbody2D>().velocity = (Vector2)UnityEngine.Random.insideUnitSphere * m_RandomSpeed;
                smallerObject.transform.localScale *= 0.5f;
            }
        }
    }
}
