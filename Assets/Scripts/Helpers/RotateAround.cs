using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [SerializeField] private Vector3 speed;

    private void Update()
    {
        transform.Rotate(speed * Time.deltaTime);
    }
}
