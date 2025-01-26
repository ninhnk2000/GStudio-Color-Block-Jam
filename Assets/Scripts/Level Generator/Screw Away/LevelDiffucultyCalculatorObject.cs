#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class LevelDiffucultyCalculatorObject : MonoBehaviour
{
    [SerializeField] private GameObject level;
    [SerializeField] private int levelNumber;
    [SerializeField] private LevelDataContainer levelDataContainer;
    private LevelDifficultyConfiguration _currentLevelDifficultyConfiguration;

    public void Calculate()
    {
        const int MAX_SCREW = 300;

        // const float WEIGHT_NUM_SCREW = 0.2f;
        const float WEIGHT_PHASES_DIFFICULTY = 500f;
        // const float WEIGHT_MODEL_DIFFICULTY = 1.8f;
        const float WEIGHT_BLOCKED_RATIO_DIFFICULTY = 100f;
        const float WEIGHT_SCREW_PORT_FACTION_CHANCE_MODIFIER = 200f;
        const float WEIGHT_START_GAME_TRIPLE_SAME_FACTION_GENERATION = 200f;

        _currentLevelDifficultyConfiguration = levelDataContainer.GetLevelDifficultyConfiguration(levelNumber);



        // Number Screw difficulty
        BasicScrew[] screws = GetComponentsFromAllChildren<BasicScrew>(level.transform).ToArray();

        int numScrew = screws.Length;

        float numScrewDifficultyNormalized = (float)numScrew / MAX_SCREW;



        // Initial difficulty
        float MAX_SCREW_BLOCKED = (numScrew - 1);

        int totalObstacles = 0;
        int numScrewBlocked = 0;

        for (int i = 0; i < numScrew; i++)
        {
            int numBlockingObjects = screws[i].CountBlockingObjects(isIncludeHiddenScrew: false);

            Debug.Log(numBlockingObjects);

            // totalObstacles += numBlockingObjects;

            if (numBlockingObjects > 0)
            {
                numScrewBlocked++;
            }

            if (numBlockingObjects > 2)
            {
                totalObstacles++;
            }
        }

        float blockedRatioDifficultyNormalized = (float)numScrewBlocked / MAX_SCREW_BLOCKED;
        float finalDifficultyMultiplier = ((float)totalObstacles / numScrew) * totalObstacles / 100f;



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

[CustomEditor(typeof(LevelDiffucultyCalculatorObject))]
public class YourComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LevelDiffucultyCalculatorObject levelDiffucultyCalculator = (LevelDiffucultyCalculatorObject)target;

        if (GUILayout.Button("Calculate"))
        {
            levelDiffucultyCalculator.Calculate();
        }
    }
}
#endif
