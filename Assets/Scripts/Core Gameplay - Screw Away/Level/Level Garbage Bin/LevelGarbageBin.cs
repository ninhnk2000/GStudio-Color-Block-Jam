using UnityEngine;

public class LevelGarbageBin : MonoBehaviour
{
    public static LevelGarbageBin Instance;

    public static Transform GarbageBin
    {
        get => Instance.transform;
    }

    void Awake()
    {
        LevelLoader.startLevelEvent += EmptyGarbageBin;

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnDestroy()
    {
        LevelLoader.startLevelEvent -= EmptyGarbageBin;
    }

    private void EmptyGarbageBin()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
