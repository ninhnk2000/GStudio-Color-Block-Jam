using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Saferio/Screw Away/GameSetting")]
public class GameSetting : ScriptableObject
{
    [SerializeField] private bool isDebug;
    [SerializeField] private bool isTurnOnBackgroundMusic = true;
    [SerializeField] private bool isTurnOnSound = true;
    [SerializeField] private bool isVibrate = true;
    [SerializeField] private string currentLanguage;

    public bool IsDebug
    {
        get => isDebug;
    }

    public bool IsTurnOnBackgroundMusic
    {
        get => isTurnOnBackgroundMusic;
        set
        {
            isTurnOnBackgroundMusic = value;
            Save();
        }
    }

    public bool IsTurnOnSound
    {
        get => isTurnOnSound;
        set
        {
            isTurnOnSound = value;
            Save();
        }
    }

    public bool IsVibrate
    {
        get => isVibrate;
        set
        {
            isVibrate = value;
            Save();
        }
    }

    public string CurrentLanguage
    {
        get => currentLanguage;
        set
        {
            currentLanguage = value;
            Save();
        }
    }

    public GameSetting(string defaultLanguage)
    {
        currentLanguage = defaultLanguage;
    }

    public void Save()
    {
        DataUtility.SaveAsync(GameConstants.GAME_SETTING, this);
    }

    public async Task LoadAsync()
    {
        GameSetting defaultGameSetting = new GameSetting(GameConstants.DEFAULT_LANGUAGE);

        GameSetting savedGameSetting = DataUtility.LoadAsync(GameConstants.GAME_SETTING, defaultGameSetting);

        isTurnOnBackgroundMusic = savedGameSetting.isTurnOnBackgroundMusic;
        isTurnOnSound = savedGameSetting.IsTurnOnSound;
        isVibrate = savedGameSetting.IsVibrate;
        currentLanguage = savedGameSetting.CurrentLanguage;
    }
}
