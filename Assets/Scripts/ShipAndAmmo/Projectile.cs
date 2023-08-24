using UnityEngine;

namespace SpaceShooter
{
    public class Projectile : Entity
    {
        [SerializeField] protected float m_Velocity;

        [SerializeField] protected float m_Lifetime;

        [SerializeField] protected int m_Damage;

        [SerializeField] protected ImpactEffect m_ImpactEffectPrefab;

        protected float m_Timer;
        protected bool m_IsPlayerProjectile;

        protected void Start()
        {
            m_IsPlayerProjectile = m_Parent.transform.root.GetComponent<SpaceShip>().IsPlayerShip;
        }

        protected virtual void FixedUpdate()
        {
            float stepLength = Time.fixedDeltaTime * m_Velocity;
            Vector2 step = transform.up * stepLength;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, stepLength);

            if (hit)
            {
                Destructible dest = hit.collider.transform.root.GetComponent<Destructible>();

                if(dest != null && dest != m_Parent)
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

                OnProjectileLifeEnd(hit.collider, hit.point);
            }

            m_Timer += Time.fixedDeltaTime;

            if(m_Timer > m_Lifetime)
            {
                Destroy(gameObject);
            }

            transform.position += new Vector3(step.x, step.y, 0);
        }

        protected virtual void OnProjectileLifeEnd(Collider2D col, Vector2 pos)
        {
            if(m_ImpactEffectPrefab != null)
            {
                var destroyEffect = Instantiate(m_ImpactEffectPrefab);
                destroyEffect.transform.position = pos;
            }

            Destroy(gameObject);
        }

        protected Destructible m_Parent;

        public void SetParentShooter(Destructible parent)
        {
            m_Parent = parent;
        }
    }
}