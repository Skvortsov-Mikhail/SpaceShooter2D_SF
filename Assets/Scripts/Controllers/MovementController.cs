using UnityEngine;


namespace SpaceShooter
{
    public class MovementController : MonoBehaviour
    {
        public enum ControlMode
        {
            Keyboard,
            Mobile
        }

        [SerializeField] private SpaceShip m_TargetShip;

        [SerializeField] private VirtualJoystick m_MobileJoystick;

        [SerializeField] private ControlMode m_ControlMode;

        [SerializeField] private PointerClickHold m_MobileFirePrimary;
        [SerializeField] private PointerClickHold m_MobileFireSecondary;

        private void Start()
        {
            if (m_ControlMode == ControlMode.Keyboard)
            {
                m_MobileJoystick.gameObject.SetActive(false);

                m_MobileFirePrimary.gameObject.SetActive(false);
                m_MobileFireSecondary.gameObject.SetActive(false);
            }

            else
            {
                m_MobileJoystick.gameObject.SetActive(true);

                m_MobileFirePrimary.gameObject.SetActive(true);
                m_MobileFireSecondary.gameObject.SetActive(true);

            }
        }

        private void Update()
        {
            if (m_TargetShip == null) return;

            if (m_ControlMode == ControlMode.Keyboard)
                ControlKeyboard();

            if (m_ControlMode == ControlMode.Mobile)
                ControlMobile();
        }

        /*
        
        /// <summary>
        /// Метод разворачивает и перемещает корабль в ту сторону, куда направлен Джойстик
        /// </summary>
        private void ControlMobile()
        {
            Vector3 dir = m_MobileJoystick.Value;

            var dot = Vector2.Dot(dir, m_TargetShip.transform.up);
            var dot2 = Vector2.Dot(dir, m_TargetShip.transform.right);

            m_TargetShip.ThrustControl = Mathf.Max(0, dot);
            m_TargetShip.TorqueControl = -dot2;
        }

        */

        /// <summary>
        /// Метод дает тягу в ту сторону, в какую подает Джойстик
        /// </summary>
        private void ControlMobile()
        {
            var dir = m_MobileJoystick.Value;
            m_TargetShip.ThrustControl = dir.y;
            m_TargetShip.TorqueControl = -dir.x;

            if (m_MobileFirePrimary.IsHold == true)
            {
                m_TargetShip.Fire(TurretMode.Primary);
            }

            if (m_MobileFireSecondary.IsHold == true)
            {
                m_TargetShip.Fire(TurretMode.Secondary);
            }
        }

        private void ControlKeyboard()
        {
            float thrust = 0;
            float torque = 0;

            if (Input.GetKey(KeyCode.UpArrow) == true)
                thrust = 1.0f;

            if (Input.GetKey(KeyCode.DownArrow) == true)
                thrust = -1.0f;

            if (Input.GetKey(KeyCode.LeftArrow) == true)
                torque = 1.0f;

            if (Input.GetKey(KeyCode.RightArrow) == true)
                torque = -1.0f;

            if(Input.GetKey(KeyCode.Space) == true)
            {
                m_TargetShip.Fire(TurretMode.Primary);
            }

            if(Input.GetKey(KeyCode.X) == true)
            {
                m_TargetShip.Fire(TurretMode.Secondary);
            }

            m_TargetShip.ThrustControl = thrust;
            m_TargetShip.TorqueControl = torque;
        }

        public void SetTagetShip(SpaceShip ship)
        {
            m_TargetShip = ship;
        }
    }
}
