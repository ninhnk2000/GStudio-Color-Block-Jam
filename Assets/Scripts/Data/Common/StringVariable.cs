using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Saferio/StringVariable")]
public class StringVariable : ScriptableObject
{
    [SerializeField] private string value;

    public string Value
    {
        get => value;
        set => this.value = value;
    }
}
