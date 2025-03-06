using UnityEngine;
using UnityEngine.Rendering;

public class GameGraphicSetting : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Volume postProcessingVolumn;

    void Awake()
    {
        // if (SystemInfo.systemMemorySize < 6000)
        // {
        //     postProcessingVolumn.enabled = false;
        // }
    }
}
