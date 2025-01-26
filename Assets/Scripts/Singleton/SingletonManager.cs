using UnityEngine;

public class SingletonManager : MonoBehaviour
{
    public static SingletonManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            gameObject.SetActive(false);

            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
