using UnityEngine;

namespace SpaceShooter
{
    public class LevelBoundaryDestroyer : MonoBehaviour
    {
        private void Update()
        {
            if (LevelBoundary.Instance == null) return;

            if (transform.position.magnitude > LevelBoundary.Instance.Radius)
            {
                Destroy(gameObject);
            }
        }
    }
}
