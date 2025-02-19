using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Saferio/Screw Away/UserResourcesObserver")]
public class UserResourcesObserver : ScriptableObject
{
    [SerializeField] private UserResources userResources;

    public UserResources UserResources
    {
        get => userResources;
        set
        {
            userResources = value;
        }
    }

    public void Save()
    {
        DataUtility.SaveAsync(GameConstants.USER_RESOURCES, userResources);
    }

    public void Load()
    {
        UserResources defaultUserResources = new UserResources();

        defaultUserResources.CoinQuantity = 100;
        defaultUserResources.BoosterQuantities = new int[3];
        
        for (int i = 0; i < defaultUserResources.BoosterQuantities.Length; i++)
        {
            defaultUserResources.BoosterQuantities[i] = 1;
        }

        userResources = DataUtility.Load(GameConstants.USER_RESOURCES, defaultUserResources);
    }

    public void ChangeCoin(float value)
    {
        Load();

        userResources.CoinQuantity += value;

        Save();
    }

    public void ChangeBoosterQuantity(int boosterIndex, int value)
    {
        // UserResources updatedUserResources = userResources;

        // int[] updatedBoosterQuantities = updatedUserResources.BoosterQuantities;

        Load();

        userResources.BoosterQuantities[boosterIndex] += value;
        userResources.BoosterQuantities[boosterIndex] = Mathf.Max(userResources.BoosterQuantities[boosterIndex], 0);

        // updatedUserResources.BoosterQuantities = updatedBoosterQuantities;
        // userResources = updatedUserResources;

        Save();
    }

    public void ConsumeBooster(int boosterIndex)
    {
        Load();

        if (userResources.BoosterQuantities[boosterIndex] > 0)
        {
            userResources.BoosterQuantities[boosterIndex]--;

            Save();
        }
    }
}

[Serializable]
public class UserResources
{
    [SerializeField] private float coinQuantity;
    [SerializeField] private int[] boosterQuantities;
    [SerializeField] private int lives;

    public float CoinQuantity
    {
        get => coinQuantity;
        set
        {
            coinQuantity = value;
        }
    }

    public int[] BoosterQuantities
    {
        get => boosterQuantities;
        set
        {
            boosterQuantities = value;
        }
    }

    public int Lives
    {
        get => lives;
        set
        {
            lives = value;
        }
    }

    public UserResources()
    {
        boosterQuantities = new int[3];

        lives = GameConstants.DEFAULT_LIVES;
    }
}
