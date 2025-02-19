using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelBlockMenuScreenUI : MonoBehaviour
{
    [SerializeField] private Image block;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private GameObject lockIcon;

    [SerializeField] private Sprite[] activeSprites;
    [SerializeField] private Sprite disabledSprite;

    [SerializeField] private Material[] textMaterials;

    [Header("CUSTOMIZE")]
    [SerializeField] private int index;


    [SerializeField] private IntVariable currentLevel;


    void Awake()
    {
        currentLevel.Load();

        // COLOR BASED ON DIFFICULTY
        int modulusLevel = currentLevel.Value % 5;

        int spriteIndex;

        if (modulusLevel >= 1 && modulusLevel <= 3)
        {
            spriteIndex = 0;
        }
        else if (modulusLevel == 4)
        {
            spriteIndex = 2;
        }
        else
        {
            spriteIndex = 1;
        }

        if (index == 0)
        {


            block.sprite = activeSprites[spriteIndex];
            levelText.fontMaterial = textMaterials[spriteIndex];

            lockIcon.SetActive(false);
        }
        else
        {
            block.sprite = disabledSprite;
            levelText.fontMaterial = textMaterials[spriteIndex];

            lockIcon.SetActive(true);
        }

        levelText.text = $"{currentLevel.Value + index}";
    }
}
