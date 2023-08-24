using UnityEngine;

namespace SpaceShooter
{
    public class ProjectileRocket : Projectile
    {
        [SerializeField] private float m_EnemySearchRadius;
        [SerializeField] private float m_DamageRadius;
        //[SerializeField] private ImpactEffect m_DestroyArea;

        [SerializeField] private float m_ForwardFlightTimer;

        private Destructible currentTarget;

        protected override void FixedUpdate()
        {
            float stepLength = Time.fixedDeltaTime * m_Velocity;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, stepLength);

            if (hit)
            {
                Collider2D[] objectsForDestroy = Physics2D.OverlapCircleAll(transform.position, m_DamageRadius);

                for (int i = 0; i < objectsForDestroy.Length; i++)
                {
                    Destructible dest = objectsForDestroy[i].transform.root.GetComponent<Destructible>();

                    if (dest != null && dest != m_Parent)
                    {
                        if (dest.GetComponent<SpaceShip>() != null && (dest.CurrentHitPoints - m_Damage) <= 0 && m_IsPlayerProjectile == true)
                        {
                            Player.Instance.AddKill();
                        }

                        dest.ApplyDamage(m_Damage);

                        if (m_IsPlayerProjectile == true)
                        {
                            Player.Instance.AddScore(dest.ScoreValue);
                        }
                    }
                }

                OnProjectileLifeEnd(hit.collider, hit.point);
            }

            Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, m_EnemySearchRadius);

            if (currentTarget == null)
            {
                currentTarget = SearchNearestTarget(targets);
            }

            if (currentTarget != null && m_ForwardFlightTimer < m_Timer)
            {
                transform.up = Vector3.Slerp(transform.up, (currentTarget.transform.position - transform.position).normalized, m_Velocity * Time.fixedDeltaTime);
            }

            Vector2 step = transform.up * stepLength;
            transform.position += new Vector3(step.x, step.y, 0);

            m_Timer += Time.fixedDeltaTime;

            if (m_Timer > m_Lifetime)
            {
                Destroy(gameObject);
            }
        }

        private Destructible SearchNearestTarget(Collider2D[] list)
        {
            if (list.Length == 0) return null;

            Destructible nearest = null;

            float minDistance = float.MaxValue;

            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].transform.root.GetComponent<Destructible>() == null || list[i].transform.root.GetComponent<Destructible>() == m_Parent) continue;

                if (minDistance > Vector2.Distance(list[i].transform.position, transform.position))
                {
                    minDistance = Vector2.Distance(list[i].transform.position, transform.position);
                    nearest = list[i].transform.root.GetComponent<Destructible>();
                }
            }

            return nearest;
        }
        /*
        protected override void OnProjectileLifeEnd(Collider2D col, Vector2 pos)
        {
            if(m_DestroyArea != null)
            {
                var showRadiusEffect = Instantiate(m_DestroyArea);
                showRadiusEffect.transform.position = pos;
            }

            base.OnProjectileLifeEnd(col, pos);
        }*/

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = new Color(0, 1, 0, 0.3f);
            UnityEditor.Handles.DrawSolidDisc(transform.position, transform.forward * m_Velocity * 0.02f, m_EnemySearchRadius);
        }
#endif
    }
}
