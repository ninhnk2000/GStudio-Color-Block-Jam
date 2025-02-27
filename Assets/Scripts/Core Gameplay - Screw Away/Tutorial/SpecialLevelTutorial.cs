using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

public class SpecialLevelTutorial : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button continueButton;

    void Awake()
    {
        continueButton.onClick.AddListener(Continue);
    }

    private void Continue()
    {
        Tween.Custom(1, 0, duration: 0.3f, onValueChange: newVal =>
        {
            canvasGroup.alpha = newVal;
        })
        .OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
