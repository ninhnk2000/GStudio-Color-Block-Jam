using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Saferio/Screw Away/IAPDataContainer")]
public class IAPDataContainer : ScriptableObject
{
    [SerializeField] private IAPPackageData[] productsData;

    public IAPPackageData[] ProductsData
    {
        get => productsData;
    }
}
