using UnityEngine;

[DisallowMultipleComponent]
public abstract class SingletoneBase<T> : MonoBehaviour where T : MonoBehaviour
{
    [Header("Singletone")]
    [SerializeField] private bool m_DoNotDestroyOnLoad;

    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("MonoSingletone: object of type already exists, instance will be destroed = " + typeof(T).Name);
            Destroy(this);
            return;
        }

        Instance = this as T;

        if (m_DoNotDestroyOnLoad)
            DontDestroyOnLoad(gameObject);
    }
}
