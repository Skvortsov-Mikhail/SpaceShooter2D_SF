using UnityEngine;


namespace SpaceShooter
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SpaceShip : Destructible
    {
        #region Parameters

        [SerializeField] private GameObject RightEngine;
        [SerializeField] private GameObject LeftEngine;
        [SerializeField] private GameObject ForwardEngines;

        /// <summary>
        /// Масса для автоматической установки у ригида.
        /// </summary>
        [Header("Space Ship")]
        [SerializeField]
        private float m_Mass;

        /// <summary>
        /// Толкающая вперед сила.
        /// </summary>
        [SerializeField]
        private float m_Thrust;
        private float m_DefaultThrust;

        /// <summary>
        /// Вращающая сила.
        /// </summary>
        [SerializeField]
        private float m_Mobility;
        private float m_DefaultMobility;

        /// <summary>
        /// Максимальная линейная скорость.
        /// </summary>
        [SerializeField]
        private float m_MaxLinearVelocity;
        public float MaxLinearVelocity => m_MaxLinearVelocity;

        /// <summary>
        /// Максимальная скорость вращения. В градусах/сек.
        /// </summary>
        [SerializeField]
        private float m_MaxAngularVelocity;
        public float MaxAngularVelocity => m_MaxAngularVelocity;

        /// <summary>
        /// Сохраненная ссылка на ригид.
        /// </summary>
        private Rigidbody2D m_Rigid;

        public bool IsPlayerShip = false;

        [SerializeField] private Sprite m_PreviewImage;
        public Sprite PreviewImage => m_PreviewImage;


        #endregion

        #region Public API

        /// <summary>
        /// Управление линейной тягой. от -1.0 до +1.0
        /// </summary>
        public float ThrustControl { get; set; }

        /// <summary>
        /// Управление вращательной тягой. от -1.0 до +1.0
        /// </summary>
        public float TorqueControl { get; set; }

        #endregion

        #region UnityEvents

        protected override void Start()
        {
            base.Start();

            m_Rigid = GetComponent<Rigidbody2D>();
            m_Rigid.mass = m_Mass;

            m_Rigid.inertia = 1;

            InitArguments();
        }

        private void FixedUpdate()
        {
            UpdateRigidBody();

            UseEngineEffect();

            UpdateEnergyRegen();

            if (m_SpeedTimer > 0)
            {
                m_SpeedTimer -= Time.fixedDeltaTime;
            }

            else if (m_Thrust != m_DefaultThrust || m_Mobility != m_DefaultMobility)
            {
                SetDefaultParameters();
            }

            if (m_IndestructibleTimer > 0)
            {
                m_IndestructibleTimer -= Time.fixedDeltaTime;
            }

            else if (m_Indestructible == true)
            {
                m_Indestructible = false;

                Player.Instance.DisableMessage();
                Player.Instance.EnableDamageEffect();
            }
        }

        #endregion

        /// <summary>
        /// Метод добавления сил кораблю для движения
        /// </summary>
        private void UpdateRigidBody()
        {
            m_Rigid.AddForce(ThrustControl * m_Thrust * transform.up * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddForce(-m_Rigid.velocity * (m_Thrust / m_MaxLinearVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddTorque(TorqueControl * m_Mobility * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddTorque(-m_Rigid.angularVelocity * (m_Mobility / m_MaxAngularVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);
        }

        [SerializeField] private Turret[] m_Turrets;

        public void Fire(TurretMode mode)
        {
            for (int i = 0; i < m_Turrets.Length; i++)
            {
                if (m_Turrets[i].Mode == mode)
                {
                    m_Turrets[i].Fire();
                }
            }
        }

        #region PowerUps

        [SerializeField] private int m_MaxEnergy;
        [SerializeField] private int m_MaxAmmo;
        [SerializeField] private int m_EnergyRegenPerSecond;

        private float m_PrimaryEnergy;
        private int m_SecondaryAmmo;

        private float m_SpeedTimer;
        private float m_IndestructibleTimer;
        public float IndestructibleTimer => m_IndestructibleTimer;

        public void AddEnergy(int energy)
        {
            m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy + energy, 0, m_MaxEnergy);
        }

        public void AddAmmo(int ammo)
        {
            m_SecondaryAmmo = Mathf.Clamp(m_SecondaryAmmo + ammo, 0, m_MaxAmmo);
        }

        public void AddSpeed(float speed, float timer)
        {
            m_SpeedTimer = timer;

            if (m_Thrust != m_DefaultThrust || m_Mobility != m_DefaultMobility) return;

            m_Thrust *= speed;
            m_Mobility *= speed;
        }

        public void SetIndestructable(float timer)
        {
            m_IndestructibleTimer = timer;
            m_Indestructible = true;

            Player.Instance.EnableMessage();
            Player.Instance.DisableDamageEffect();
        }

        private void InitArguments()
        {
            m_PrimaryEnergy = m_MaxEnergy;
            m_SecondaryAmmo = m_MaxAmmo;

            m_DefaultThrust = m_Thrust;
            m_DefaultMobility = m_Mobility;
        }

        private void SetDefaultParameters()
        {
            m_Thrust = m_DefaultThrust;
            m_Mobility = m_DefaultMobility;
        }

        private void UpdateEnergyRegen()
        {
            m_PrimaryEnergy += (float)m_EnergyRegenPerSecond * Time.fixedDeltaTime;
            m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy, 0, m_MaxEnergy);
        }

        public bool DrawEnergy(int count)
        {
            if (count == 0)
                return true;

            if (m_PrimaryEnergy >= count)
            {
                m_PrimaryEnergy -= count;
                return true;
            }

            return false;
        }

        public bool DrawAmmo(int count)
        {
            if (count == 0)
                return true;

            if (m_SecondaryAmmo >= count)
            {
                m_SecondaryAmmo -= count;
                return true;
            }

            return false;
        }

        #endregion

        public void AssignWeapon(TurretProperties props)
        {
            for (int i = 0; i < m_Turrets.Length; i++)
            {
                m_Turrets[i].AssignLoadout(props);
            }
        }

        /// <summary>
        /// Метод для визуального отображения работы двигателей корабля
        /// </summary>
        private void UseEngineEffect()
        {
            if (RightEngine == null || LeftEngine == null || ForwardEngines == null) return;

            RightEngine.SetActive(false);
            LeftEngine.SetActive(false);
            ForwardEngines.SetActive(false);

            if (ThrustControl > 0)
            {
                RightEngine.SetActive(true);
                LeftEngine.SetActive(true);
            }

            if (ThrustControl < 0)
                ForwardEngines.SetActive(true);

            if (TorqueControl > 0)
                RightEngine.SetActive(true);

            if (TorqueControl < 0)
                LeftEngine.SetActive(true);
        }
    }
}
