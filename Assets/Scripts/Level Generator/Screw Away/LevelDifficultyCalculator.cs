
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using static GameEnum;
#if UNITY_EDITOR
using UnityEditor;

public class LevelDifficultyCalculator : EditorWindow
{
    private int level;
    private LevelDataContainer levelDataContainer;

    private LevelDifficultyConfiguration _currentLevelDifficultyConfiguration;
    private float expectedDifficultyNormalized;

    private string prefabPath = "Assets/Prefabs/MyPrefab.prefab";


    [MenuItem("Saferio/Tools/Level Difficulty Calculator")]
    public static void ShowWindow()
    {
        GetWindow<LevelDifficultyCalculator>("Level Difficulty Calculator");
    }

    private void OnGUI()
    {
        float padding = 20f;

        Rect areaRect = new Rect(padding, padding, position.width - 2 * padding, position.height - 2 * padding);

        GUILayout.BeginArea(areaRect);

        // prefabPath = EditorGUILayout.TextField("Prefab Path", prefabPath);

        level = EditorGUILayout.IntField("Level", level);
        levelDataContainer = (LevelDataContainer)EditorGUILayout.ObjectField("Level Data Container", levelDataContainer, typeof(LevelDataContainer), true);
        expectedDifficultyNormalized = EditorGUILayout.FloatField("Expected Difficulty", expectedDifficultyNormalized);

        if (GUILayout.Button("Calculate"))
        {
            Calculate();
        }

        if (GUILayout.Button("Generate Screw Box Difficulty"))
        {
            GenerateScrewBoxDifficulty();
        }

        if (GUILayout.Button("Generate Screw Box Difficulty Template"))
        {
            GenerateScrewBoxDifficultyTemplate(level);
        }

        GUILayout.EndArea();
    }

    private void Calculate()
    {
        const int MAX_SCREW = 300;

        // const float WEIGHT_NUM_SCREW = 0.2f;
        const float WEIGHT_PHASES_DIFFICULTY = 580f;
        // const float WEIGHT_MODEL_DIFFICULTY = 1.8f;
        const float WEIGHT_BLOCKED_RATIO_DIFFICULTY = 20f;
        const float WEIGHT_SCREW_PORT_FACTION_CHANCE_MODIFIER = 200f;
        const float WEIGHT_START_GAME_TRIPLE_SAME_FACTION_GENERATION = 200f;

        prefabPath = $"Assets/Prefabs/Screw Away/Levels/Level {level}.prefab";

        GameObject levelPrefab = PrefabUtility.LoadPrefabContents(prefabPath);

        _currentLevelDifficultyConfiguration = levelDataContainer.GetLevelDifficultyConfiguration(level);



        // Number Screw difficulty
        BasicScrew[] screws = GetComponentsFromAllChildren<BasicScrew>(levelPrefab.transform).ToArray();

        int numScrew = screws.Length;

        float numScrewDifficultyNormalized = (float)numScrew / MAX_SCREW;



        // Initial difficulty
        float MAX_SCREW_BLOCKED = (numScrew - 1);

        int totalObstacles = 0;
        int numScrewBlocked = 0;

        for (int i = 0; i < numScrew; i++)
        {
            int numBlockingObjects = screws[i].CountBlockingObjects(isIncludeHiddenScrew: false);

            totalObstacles += numBlockingObjects;

            if (numBlockingObjects > 0)
            {
                numScrewBlocked++;
            }

            // if (numBlockingObjects > 2)
            // {
            //     totalObstacles++;
            // }
        }

        float blockedRatioDifficultyNormalized = (float)numScrewBlocked / MAX_SCREW_BLOCKED;
        float finalDifficultyMultiplier = (1 + 0.1f * ((float)totalObstacles / numScrew) * totalObstacles / 100f);



        // Screw boxes generation difficulty
        float MAX_SCREW_BOXES_GENERATION_DIFFICULTY = Mathf.Pow(2, GameConstants.SCREW_FACTION.Length - 1);

        float screwBoxesGenerationDifficulty = 0;

        float currentProgress = 0;

        for (int i = 0; i < _currentLevelDifficultyConfiguration.LevelPhases.Length; i++)
        {
            LevelPhase phase = _currentLevelDifficultyConfiguration.LevelPhases[i];

            screwBoxesGenerationDifficulty += (phase.EndProgress - currentProgress) * Mathf.Pow(2, phase.PhaseIndex);

            currentProgress = phase.EndProgress;
        }

        float screwBoxesGenerationDifficultyNormalized = screwBoxesGenerationDifficulty / MAX_SCREW_BOXES_GENERATION_DIFFICULTY;



        // Chance to get screw ports' faction difficulty
        float MAX_SCREW_PORT_FACTION_DIFFICULTY_MODIFIER = 99;

        float screwPortFactionChanceModifierDifficultyNormalized =
            _currentLevelDifficultyConfiguration.ScrewPortFactionDifficultyModifier / MAX_SCREW_PORT_FACTION_DIFFICULTY_MODIFIER;



        // Number of triple matched color no blocked at game start
        float startGameTripleSameFactionGenerationDifficulty =
            1 - _currentLevelDifficultyConfiguration.RatioGeneratingSameFactionForTripleNoBlockedScrewStartGame;


        // Final
        float finalDifficulty =
            // WEIGHT_NUM_SCREW * numScrewDifficultyNormalized +
            WEIGHT_PHASES_DIFFICULTY * screwBoxesGenerationDifficultyNormalized +
            // WEIGHT_MODEL_DIFFICULTY * modelDifficultyNormalized +
            WEIGHT_BLOCKED_RATIO_DIFFICULTY * blockedRatioDifficultyNormalized +
            WEIGHT_SCREW_PORT_FACTION_CHANCE_MODIFIER * screwPortFactionChanceModifierDifficultyNormalized +
            WEIGHT_START_GAME_TRIPLE_SAME_FACTION_GENERATION * startGameTripleSameFactionGenerationDifficulty;

        finalDifficulty *= finalDifficultyMultiplier;

        // string difficultyText = $"Num Screw Difficulty: <color=#FF7A75>{WEIGHT_NUM_SCREW * numScrewDifficultyNormalized}";
        // difficultyText += $" <color=#9DFF8D>({numScrewDifficultyNormalized * 100}%)";
        // difficultyText += $" <color=#fff>- Model Difficulty: <color=#FF7A75>{WEIGHT_MODEL_DIFFICULTY * modelDifficultyNormalized}";
        // difficultyText += $" <color=#9DFF8D>({modelDifficultyNormalized * 100}%)";
        string difficultyText = $"Screw Boxes Generation Difficulty: <color=#FF7A75>{WEIGHT_PHASES_DIFFICULTY * screwBoxesGenerationDifficultyNormalized}";
        difficultyText += $" <color=#9DFF8D>({screwBoxesGenerationDifficultyNormalized * 100}%)";
        difficultyText += $" <color=#fff>- Blocked Ratio Difficulty: <color=#FF7A75>{WEIGHT_BLOCKED_RATIO_DIFFICULTY * blockedRatioDifficultyNormalized}";
        difficultyText += $" <color=#9DFF8D>({blockedRatioDifficultyNormalized * 100}%)";
        difficultyText += $" <color=#fff>- Screw Port Faction Chance Modifier Difficulty: <color=#FF7A75>{WEIGHT_SCREW_PORT_FACTION_CHANCE_MODIFIER * screwPortFactionChanceModifierDifficultyNormalized}";
        difficultyText += $" <color=#9DFF8D>({screwPortFactionChanceModifierDifficultyNormalized * 100}%)";
        difficultyText += $" <color=#fff>- Start Game Triple Match Difficulty: <color=#FF7A75>{WEIGHT_START_GAME_TRIPLE_SAME_FACTION_GENERATION * startGameTripleSameFactionGenerationDifficulty}";
        difficultyText += $" <color=#9DFF8D>({startGameTripleSameFactionGenerationDifficulty * 100}%)";
        difficultyText += $" <color=#fff>- Total Screws: <color=#FF7A75>({numScrew})";
        difficultyText += $" <color=#fff>- Total Obstacles: <color=#FF7A75>({totalObstacles})";
        difficultyText += $" <color=#fff>- Final Difficulty Multiplier: <color=#FF7A75>({finalDifficultyMultiplier})";
        difficultyText += $" <color=#fff>- Final Difficulty: <color=#FF7A75>{finalDifficulty}</color>";

        Debug.Log(difficultyText);

        EditorGUIUtility.systemCopyBuffer = $"{finalDifficulty}";
    }

    public static LevelDifficultyConfiguration GenerateScrewBoxDifficultyTemplate(int level)
    {
        float WEIGHT_REPEAT_TEMPLATE = 0.8f;
        float WEIGHT_LEVEL_INDEX = 1 - WEIGHT_REPEAT_TEMPLATE;
        float DIFFICULTY_MULTIPLIER = 1.6f;

        float MAX_SCREW_BOXES_GENERATION_DIFFICULTY = Mathf.Pow(2, GameConstants.SCREW_FACTION.Length - 1);

        List<LevelPhase> generatedLevelPhases = new List<LevelPhase>();

        // // RANDOM
        // List<int> validMaxPhaseIndexToRandom = new List<int>();

        // for (int i = 0; i < GameConstants.SCREW_FACTION.Length; i++)
        // {
        //     if (0.9f * (Mathf.Pow(2, i) / MAX_SCREW_BOXES_GENERATION_DIFFICULTY) > expectedDifficultyNormalized)
        //     {
        //         validMaxPhaseIndexToRandom.Add(i);
        //     }
        // }

        // int randomIndex = UnityEngine.Random.Range(0, validMaxPhaseIndexToRandom.Count);
        // int maxPhase = validMaxPhaseIndexToRandom[randomIndex];

        int PREDICTED_MAX_LEVEL = 100;

        float[] weightForModules = new float[5] { 0.08f, 0.12f, 0.18f, 0.4f, 0.22f };

        float expectedDifficultyNormalized =
            DIFFICULTY_MULTIPLIER * WEIGHT_REPEAT_TEMPLATE * weightForModules[(level - 1) % 5] + WEIGHT_LEVEL_INDEX * ((float)level / PREDICTED_MAX_LEVEL);

        int maxPhaseIndex = GameConstants.SCREW_FACTION.Length - 1;

        for (int i = maxPhaseIndex; i >= 0; i--)
        {
            int phaseIndex = i;

            float progress = expectedDifficultyNormalized / Mathf.Pow(2, phaseIndex) * MAX_SCREW_BOXES_GENERATION_DIFFICULTY;

            if (progress > 0.1f && progress < 0.9f)
            {
                LevelPhase levelPhase = new LevelPhase(0, 1 - progress);

                generatedLevelPhases.Add(levelPhase);

                levelPhase = new LevelPhase(phaseIndex, 1);

                generatedLevelPhases.Add(levelPhase);

                break;
            }
            else
            {
                continue;
            }
        }

        // levelDataContainer.SetLevelDifficultyConfiguration(30, generatedLevelPhases.ToArray());

        LevelDifficultyConfiguration levelDifficultyConfiguration = new LevelDifficultyConfiguration();

        levelDifficultyConfiguration.LevelPhases = generatedLevelPhases.ToArray();
        levelDifficultyConfiguration.AdsScrewBoxDifficultyLevelModifier = -99;
        levelDifficultyConfiguration.RatioGeneratingSameFactionForTripleNoBlockedScrewStartGame = 1;

        return levelDifficultyConfiguration;
    }

    private void GenerateScrewBoxDifficulty()
    {
        float MAX_SCREW_BOXES_GENERATION_DIFFICULTY = Mathf.Pow(2, GameConstants.SCREW_FACTION.Length - 1);

        List<LevelPhase> generatedLevelPhases = new List<LevelPhase>();

        // RANDOM
        List<int> validMaxPhaseIndexToRandom = new List<int>();

        for (int i = 0; i < GameConstants.SCREW_FACTION.Length; i++)
        {
            if (0.9f * (Mathf.Pow(2, i) / MAX_SCREW_BOXES_GENERATION_DIFFICULTY) > expectedDifficultyNormalized)
            {
                validMaxPhaseIndexToRandom.Add(i);
            }
        }

        int randomIndex = UnityEngine.Random.Range(0, validMaxPhaseIndexToRandom.Count);
        int maxPhase = validMaxPhaseIndexToRandom[randomIndex];

        for (int i = maxPhase; i >= 0; i--)
        {
            int phaseIndex = i;

            float progress = expectedDifficultyNormalized / Mathf.Pow(2, phaseIndex) * MAX_SCREW_BOXES_GENERATION_DIFFICULTY;

            if (progress > 0.1f && progress < 0.9f)
            {
                LevelPhase levelPhase = new LevelPhase(0, 1 - progress);

                generatedLevelPhases.Add(levelPhase);

                levelPhase = new LevelPhase(phaseIndex, 1);

                generatedLevelPhases.Add(levelPhase);

                break;
            }
            else
            {
                continue;
            }
        }

        levelDataContainer.SetLevelDifficultyConfiguration(level, generatedLevelPhases.ToArray());
    }

    #region UTIL
    public List<T> GetComponentsFromAllChildren<T>(Transform parent) where T : Component
    {
        List<T> components = new List<T>();
        GetComponentsFromAllChildrenRecursive<T>(parent, components);
        return components;
    }

    private void GetComponentsFromAllChildrenRecursive<T>(Transform parent, List<T> components) where T : Component
    {
        T component = parent.GetComponent<T>();
        if (component != null)
        {
            components.Add(component);
        }

        foreach (Transform child in parent)
        {
            GetComponentsFromAllChildrenRecursive<T>(child, components);
        }
    }
    #endregion
}
#endif


public static class LevelDifficultyCalculatorUltility
{
    public static LevelDifficultyConfiguration GenerateScrewBoxDifficultyTemplate(int level)
    {
        float WEIGHT_REPEAT_TEMPLATE = 0.8f;
        float WEIGHT_LEVEL_INDEX = 1 - WEIGHT_REPEAT_TEMPLATE;
        float DIFFICULTY_MULTIPLIER = RemoteConfigController.GetFloatConfig(FirebaseConfig.DIFFICULTY_MULTIPLIER, 1.25f);

        float MAX_SCREW_BOXES_GENERATION_DIFFICULTY = Mathf.Pow(2, GameConstants.SCREW_FACTION.Length - 1);

        List<LevelPhase> generatedLevelPhases = new List<LevelPhase>();

        int PREDICTED_MAX_LEVEL = 100;

        float[] weightForModules = new float[5] { 0.08f, 0.12f, 0.18f, 0.4f, 0.22f };

        float expectedDifficultyNormalized =
            DIFFICULTY_MULTIPLIER * WEIGHT_REPEAT_TEMPLATE * weightForModules[(level - 1) % 5] + WEIGHT_LEVEL_INDEX * ((float)level / PREDICTED_MAX_LEVEL);

        int maxPhaseIndex = GameConstants.SCREW_FACTION.Length - 1;

        for (int i = maxPhaseIndex; i >= 0; i--)
        {
            int phaseIndex = i;

            float progress = expectedDifficultyNormalized / Mathf.Pow(2, phaseIndex) * MAX_SCREW_BOXES_GENERATION_DIFFICULTY;

            if (progress > 0.1f && progress < 0.9f)
            {
                LevelPhase levelPhase = new LevelPhase(0, 1 - progress);

                generatedLevelPhases.Add(levelPhase);

                levelPhase = new LevelPhase(phaseIndex, 1);

                generatedLevelPhases.Add(levelPhase);

                break;
            }
            else
            {
                continue;
            }
        }

        LevelDifficultyConfiguration levelDifficultyConfiguration = new LevelDifficultyConfiguration();

        levelDifficultyConfiguration.LevelPhases = generatedLevelPhases.ToArray();
        levelDifficultyConfiguration.AdsScrewBoxDifficultyLevelModifier = -99;
        levelDifficultyConfiguration.RatioGeneratingSameFactionForTripleNoBlockedScrewStartGame = 1;

        return levelDifficultyConfiguration;
    }
}