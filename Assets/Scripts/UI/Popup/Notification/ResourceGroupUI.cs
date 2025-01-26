using TMPro;
using UnityEngine;

public class ResourceGroupUI : MonoBehaviour
{
    [SerializeField] private TMP_Text quantityText;

    public void SetQuantity(int quantity)
    {
        quantityText.text = $"{quantity}";
    }
}
