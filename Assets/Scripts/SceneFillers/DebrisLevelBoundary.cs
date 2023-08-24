using UnityEngine;

namespace SpaceShooter
{
    public class DebrisLevelBoundary : MonoBehaviour
    {
        private void Update()
        {
            if (LevelBoundary.Instance == null) return;

            if (transform.position.magnitude > LevelBoundary.Instance.Radius)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity *= -1;
            }
        }
    }
}
