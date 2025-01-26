using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Saferio/Screw Away/IAPPackageData")]
public class IAPPackageData : ScriptableObject
{
    [SerializeField] private string productId;
    [SerializeField] private int coinQuantity;
    [SerializeField] private int addHoleBoosterQuantity;
    [SerializeField] private int breakObjectBoosterQuantity;
    [SerializeField] private int clearHolesBoosterQuantity;
    [SerializeField] private bool isRemoveAd;
    [SerializeField] private float price;

    public string ProductId
    {
        get => productId;
    }

    public int CoinQuantity
    {
        get => coinQuantity;
    }

    public int AddHoleBoosterQuantity
    {
        get => addHoleBoosterQuantity;
    }

    public int BreakObjectBoosterQuantity
    {
        get => breakObjectBoosterQuantity;
    }

    public int ClearHolesBoosterQuantity
    {
        get => clearHolesBoosterQuantity;
    }

    public bool IsRemoveAd
    {
        get => isRemoveAd;
    }

    public float Price
    {
        get => price;
    }
}
