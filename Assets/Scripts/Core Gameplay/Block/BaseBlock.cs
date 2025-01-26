using UnityEngine;

public class BaseBlock : MonoBehaviour
{
    [SerializeField] private BlockProperty blockProperty;

    public BlockProperty BlockProperty
    {
        get => blockProperty;
    }
}
