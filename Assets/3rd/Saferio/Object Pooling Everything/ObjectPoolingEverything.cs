using System;
using System.Collections.Generic;
using UnityEngine;

public enum SaferioPrefabIdentifier
{
    Explosion
}

public class ObjectPoolingEverything : MonoBehaviour
{
    public static ObjectPoolingEverything Instance;
    private Dictionary<string, ObjectPool> _poolGroup;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }

        _poolGroup = new Dictionary<string, ObjectPool>();
    }

    public static void AddToPool(string key, ObjectPool pool)
    {
        Instance._poolGroup.Add(key, pool);
    }

    public static void ReturnToPool(string key, GameObject pooledGameObject)
    {
        Instance._poolGroup[key].ReturnPool(pooledGameObject);
    }

    public static GameObject SpawnGameObject(GameObject prefab)
    {
        return Instantiate(prefab, Instance.transform);
    }

    public static T GetFromPool<T>(string identifier)
    {
        foreach (var group in Instance._poolGroup)
        {
            if (identifier == group.Key)
            {
                return group.Value.GetFromPool<T>();
            }
        }

        return default;
    }
}

[Serializable]
public struct PrefabWithIdentifier
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private string identifier;
    [SerializeField] private int poolSize;

    public GameObject Prefab => prefab;
    public string Identifier => identifier;
}

[Serializable]
public class ObjectPool
{
    private GameObjectWithComponent[] _pool;

    private int _currentIndex;
    private GameObject _cachedPrefab;

    public void Init<T>(GameObject prefab, int poolSize)
    {
        _pool = new GameObjectWithComponent[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            GameObject gameObject = ObjectPoolingEverything.SpawnGameObject(prefab);

            GameObjectWithComponent gameObjectWithComponent = new GameObjectWithComponent();

            gameObjectWithComponent.gameObject = gameObject;
            gameObjectWithComponent.component = gameObject.GetComponent<T>();

            _pool[i] = gameObjectWithComponent;

            gameObject.SetActive(false);
        }

        _cachedPrefab = prefab;
    }

    public T GetFromPool<T>()
    {
        for (int i = 0; i < _pool.Length; i++)
        {
            if (_pool[i].gameObject == null)
            {
                GameObject gameObject = ObjectPoolingEverything.SpawnGameObject(_cachedPrefab);

                GameObjectWithComponent gameObjectWithComponent = new GameObjectWithComponent();

                gameObjectWithComponent.gameObject = gameObject;
                gameObjectWithComponent.component = gameObject.GetComponent<T>();

                _pool[i] = gameObjectWithComponent;

                gameObject.SetActive(false);

                continue;
            }

            if (!_pool[i].gameObject.activeSelf)
            {
                _currentIndex = i;

                break;
            }
        }

        _pool[_currentIndex].gameObject.SetActive(true);

        T componentToGet = (T)_pool[_currentIndex].component;

        _currentIndex++;

        if (_currentIndex >= _pool.Length)
        {
            _currentIndex = 0;
        }

        return componentToGet;
    }

    public void ReturnPool(GameObject gameObject)
    {
        gameObject.transform.SetParent(ObjectPoolingEverything.Instance.transform);

        gameObject.SetActive(false);
    }
}

public class GameObjectWithComponent
{
    public GameObject gameObject;
    public object component;
}
