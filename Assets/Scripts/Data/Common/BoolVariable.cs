using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Saferio/BoolVariable")]
public class BoolVariable : ScriptableObject
{
    [SerializeField] private bool value;

    public bool Value
    {
        get => value;
        set => this.value = value;
    }
}
