using UnityEngine;

public class GamePopupInitializer : MonoBehaviour
{
    private void Awake()
    {
        InitPopups();
    }

    private void InitPopups()
    {
        BasePopup[] popups = GetComponentsInChildren<BasePopup>(includeInactive: true);

        foreach (var popup in popups)
        {
            popup.gameObject.SetActive(true);
        }
    }
}
