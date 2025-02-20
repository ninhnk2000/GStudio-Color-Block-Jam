using System;
using UnityEngine;
using UnityEngine.UI;
using static GameEnum;

public class TopBarGameplayUI : MonoBehaviour
{
    [SerializeField] private Image background;

    [SerializeField] private Sprite[] backgroundSprites;

    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private IntVariable currentLevel;

    public static event Action<ScreenRoute> showPopupEvent;

    void Awake()
    {
        LevelLoader.startLevelEvent += OnLevelStarted;
    }

    void OnDestroy()
    {
        LevelLoader.startLevelEvent -= OnLevelStarted;
    }

    private void OnLevelStarted()
    {
        LevelDifficulty levelDifficulty = CommonUtil.GetLevelDifficulty(currentLevel.Value);

        background.sprite = backgroundSprites[(int)levelDifficulty];
    }
}
