using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameEnum;

public class TopBarGameplayUI : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private TMP_Text[] levelTexts;

    [SerializeField] private Sprite[] backgroundSprites;
    [SerializeField] private Material[] textMaterials;

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

        for (int i = 0; i < levelTexts.Length; i++)
        {
            levelTexts[i].fontMaterial = textMaterials[(int)levelDifficulty];
        }
    }
}
