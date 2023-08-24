using UnityEngine;

/*

namespace SpaceShooter
{
    public class ProjectileRocket : Projectile
    {
        [SerializeField] private float m_ActiveRadius;

        [SerializeField] private float m_ForwardFlightTimer;

        private Destructable currentTarget;


        protected override void FixedUpdate()
        {
            float stepLength = Time.fixedDeltaTime * m_Velocity;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, stepLength);

            if (hit)
            {
                Destructable dest = hit.collider.transform.root.GetComponent<Destructable>();
                if (dest != null && dest != m_Parent)
                {
                    dest.ApplyDamage(m_Damage);
                }

                OnProjectileLifeEnd(hit.collider, hit.point);
            }

            Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, m_ActiveRadius);

            if (currentTarget == null)
            {
                currentTarget = SearchNearestTarget(targets);
            }

            if (currentTarget != null && m_ForwardFlightTimer < m_Timer)
            {
                transform.up = (currentTarget.transform.position - transform.position).normalized;
            }

            Vector2 step = transform.up * stepLength;
            transform.position += new Vector3(step.x, step.y, 0);

            m_Timer += Time.fixedDeltaTime;

            if (m_Timer > m_Lifetime)
            {
                Destroy(gameObject);
            }

        }

        private Destructable SearchNearestTarget(Collider2D[] list)
        {
            if (list.Length == 0) return null;

            Destructable nearest = list[0].GetComponent<Destructable>();
            float distance = Vector2.Distance(list[0].transform.position, transform.position);

            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].GetComponent<Destructable>() == null || list[i].GetComponent<Destructable>() == m_Parent) continue;

                if (distance > Vector2.Distance(list[i].transform.position, transform.position))
                {
                    distance = Vector2.Distance(list[i].transform.position, transform.position);
                    nearest = list[i].GetComponent<Destructable>();
                }
            }

            return nearest;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = new Color(0, 1, 0, 0.3f);
            UnityEditor.Handles.DrawSolidDisc(transform.position, transform.forward * m_Velocity * 0.02f, m_ActiveRadius);
        }
#endif
    }
}

 */