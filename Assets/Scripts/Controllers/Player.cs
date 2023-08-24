using UnityEngine;

namespace SpaceShooter
{
    public class Player : SingletoneBase<Player>
    {
        [SerializeField] private int m_NumLives;
        [SerializeField] private SpaceShip m_Ship;
        [SerializeField] private GameObject m_PlayerShipPrefab;
        [SerializeField] private ImpactEffect m_DestroyEffectPrefab;

        [SerializeField] public GameObject m_IndestructibleMessage;
        [SerializeField] public GameObject m_DamageEffect;

        public SpaceShip ActiveShip => m_Ship;

        [SerializeField] private CameraController m_CameraController;
        [SerializeField] private MovementController m_MovementController;

        protected override void Awake()
        {
            base.Awake();

            if (m_Ship != null)
            {
                Destroy(m_Ship.gameObject);
            }
        }

        private void Start()
        {
            Respawn();
        }

        private void OnShipDeath(GameObject gameObject)
        {
            m_NumLives--;
            
            if(m_DestroyEffectPrefab != null)
            {
                var effect = Instantiate(m_DestroyEffectPrefab);
                effect.transform.position = m_Ship.transform.position;
            }
            
            if (m_NumLives > 0)
            {
                Respawn();
            }

            else
            {
                LevelSequenceController.Instance.FinishCurrentLevel(false);
            }
        }

        private void Respawn()
        {
            if(LevelSequenceController.PlayerShip != null)
            {
                var newPlayerShip = Instantiate(LevelSequenceController.PlayerShip);
                newPlayerShip.GetComponent<CollisionDamageApplicator>().DamageEffect = m_DamageEffect.GetComponent<Animator>();

                m_Ship = newPlayerShip.GetComponent<SpaceShip>();
                m_Ship.IsPlayerShip = true;

                m_IndestructibleMessage.GetComponent<IndestructibleMessage>().SetPlayerShip();

                m_CameraController.SetTarget(m_Ship.transform);
                m_MovementController.SetTagetShip(m_Ship);

                m_Ship.EventOnDeath.AddListener(OnShipDeath);
            }
        }

        #region Score

        public int Score { get; private set; }

        public int NumKills { get; private set; }

        [SerializeField] private float m_ExtraBonusMultiplierPerSecond;
        public float ExtraBonusMultiplierPerSecond => m_ExtraBonusMultiplierPerSecond;

        public void AddKill()
        {
            NumKills++;
        }

        public void AddScore(int num)
        {
            Score += num;
        }

        #endregion

        #region IndestructibleMessage
        public void EnableMessage()
        {
            m_IndestructibleMessage.SetActive(true);
        }

        public void DisableMessage()
        {
            m_IndestructibleMessage.SetActive(false);
        }
        #endregion

        #region DamageEffect
        public void EnableDamageEffect()
        {
            m_DamageEffect.SetActive(true);
        }

        public void DisableDamageEffect()
        {
            m_DamageEffect.SetActive(false);
        }
        #endregion
    }
}
