using TMPro;
using UnityEngine;

namespace SpaceShooter
{
    public class IndestructibleMessage : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        private SpaceShip ship;

        private void Start()
        {
            SetPlayerShip();
        }

        void Update()
        {
            text.text = "     ������������ ������� ��� " + ship.IndestructibleTimer.ToString("F") + " ���.";
        }

        public void SetPlayerShip()
        {
            ship = Player.Instance.ActiveShip;
        }
    }
}
