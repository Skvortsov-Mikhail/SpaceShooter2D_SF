using UnityEngine;

namespace SpaceShooter
{
    public class PowerUpStats : PowerUp
    {
        public enum EffectType
        {
            AddAmmo,
            AddEnergy,
            AddSpeed,
            AddShield
        }

        [SerializeField] private EffectType m_EffectType;

        [SerializeField] private float m_Value;
        [SerializeField] private float m_PowerUpTimer;

        protected override void OnPickedUp(SpaceShip ship)
        {
            if(m_EffectType == EffectType.AddEnergy)
            {
                ship.AddEnergy((int)m_Value);
            }

            if (m_EffectType == EffectType.AddAmmo)
            {
                ship.AddAmmo((int)m_Value);
            }

            if (m_EffectType == EffectType.AddSpeed)
            {
                ship.AddSpeed(m_Value, m_PowerUpTimer);
            }

            if (m_EffectType == EffectType.AddShield)
            {
                ship.SetIndestructable(m_PowerUpTimer);
            }
        }
    }
}
