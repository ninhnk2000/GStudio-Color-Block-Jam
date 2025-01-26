using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public static class DataUtility
{
    // use to save interface or abstract class
    private static JsonSerializerSettings settings = new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.Auto
    };

    public static void SaveWithReferenceLoopHandling<T>(string fileName, string key, T data)
    {
        string filePath = Application.persistentDataPath + $"/{fileName}.json";

        string fileText = "{}";

        if (File.Exists(filePath))
        {
            fileText = File.ReadAllText(filePath);
        }

        try
        {
            JObject json = JObject.Parse(fileText);

            if (json.ContainsKey(key))
            {
                json[key] = JsonConvert.SerializeObject(data, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                // json[key] = JsonUtility.ToJson(data);
            }
            else
            {
                json.Add(key, JsonConvert.SerializeObject(data, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }));
                // json.Add(key, JsonUtility.ToJson(data));
            }

            File.WriteAllText(filePath, json.ToString());
        }
        catch (Exception e)
        {
            // Debug.Log("SAFERIO " + e.Message);
        }
    }

    public static void SaveWithReferenceLoopHandling<T>(string key, T data)
    {
        SaveWithReferenceLoopHandling(GameConstants.SAVE_FILE_NAME, key, data);
    }

    public static void Save<T>(string fileName, string key, T data)
    {
        string filePath = Application.persistentDataPath + $"/{fileName}.json";

        string fileText = "{}";


        // try
        // {
        //     using (var file = File.Open(filePath, FileMode.OpenOrCreate))
        //     {
        //         byte[] fileBytes = new byte[file.Length];
        //         file.Read(fileBytes, 0, (int)file.Length);
        //         fileText = Encoding.UTF8.GetString(fileBytes);

        //         JObject json = JObject.Parse(fileText);

        //         if (json.ContainsKey(key))
        //         {
        //             json[key] = JsonConvert.SerializeObject(data);
        //             // json[key] = JsonUtility.ToJson(data);
        //         }
        //         else
        //         {
        //             json.Add(key, JsonConvert.SerializeObject(data));
        //             // json.Add(key, JsonUtility.ToJson(data));
        //         }

        //         Debug.Log("SAFERIO " + json.ToString());

        //         File.WriteAllText(filePath, json.ToString());
        //     }
        // }
        // catch (IOException ioEx)
        // {
        //     Debug.LogError($"SAFERIO File I/O Error: {ioEx.Message}");
        // }
        // catch (JsonException jsonEx)
        // {
        //     Debug.LogError($"SAFERIO  JSON Error: {jsonEx.Message}");
        // }
        // catch (Exception ex)
        // {
        //     Debug.LogError($"SAFERIO Unexpected Error: {ex.Message}");
        // }


        int attempt = 0;
        int maxRetries = 3;

        while (attempt < maxRetries)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    fileText = File.ReadAllText(filePath);
                }

                JObject json = JObject.Parse(fileText);

                if (json.ContainsKey(key))
                {
                    json[key] = JsonConvert.SerializeObject(data);
                    // json[key] = JsonUtility.ToJson(data);
                }
                else
                {
                    json.Add(key, JsonConvert.SerializeObject(data));
                    // json.Add(key, JsonUtility.ToJson(data));
                }

                File.WriteAllText(filePath, json.ToString());

                break;
            }
            catch (Exception e)
            {
                Debug.Log("SAFERIO " + e.Message);

                attempt++;
            }
        }





        // if (File.Exists(filePath))
        // {
        //     fileText = File.ReadAllText(filePath);
        // }

        // try
        // {
        //     JObject json = JObject.Parse(fileText);

        //     if (json.ContainsKey(key))
        //     {
        //         json[key] = JsonConvert.SerializeObject(data);
        //         // json[key] = JsonUtility.ToJson(data);
        //     }
        //     else
        //     {
        //         json.Add(key, JsonConvert.SerializeObject(data));
        //         // json.Add(key, JsonUtility.ToJson(data));
        //     }

        //     Debug.Log("SAFERIO " + json.ToString());

        //     if (json.ToString() == "")
        //     {
        //         return;
        //     }

        //     File.WriteAllText(filePath, json.ToString());
        // }
        // catch (Exception e)
        // {
        //     Debug.Log("SAFERIO " + e.Message);
        // }
    }

    public static void Save<T>(string key, T data)
    {
        Save(GameConstants.SAVE_FILE_NAME, key, data);
    }

    public async static Task SaveAsync<T>(string fileName, string key, T data)
    {
        Save(fileName, key, data);

        // string filePath = Application.persistentDataPath + $"/{fileName}.json";

        // string fileText = "{}";

        // if (File.Exists(filePath))
        // {
        //     fileText = await File.ReadAllTextAsync(filePath);
        // }

        // JObject json = JObject.Parse(fileText);

        // if (json.ContainsKey(key))
        // {
        //     json[key] = JsonConvert.SerializeObject(data, settings);
        // }
        // else
        // {
        //     json.Add(key, JsonConvert.SerializeObject(data, settings));
        // }

        // await File.WriteAllTextAsync(filePath, json.ToString());
    }

    public static async Task SaveAsync<T>(string key, T data)
    {
        Save(key, data);

        // await SaveAsync(GameConstants.SAVE_FILE_NAME, key, data);
    }

    public static T Load<T>(string fileName, string key, T defaultValue)
    {
        string filePath = Application.persistentDataPath + $"/{fileName}.json";

        try
        {
            if (File.Exists(filePath))
            {
                string data = File.ReadAllText(filePath);

                if (data != "")
                {
                    JObject json = JObject.Parse(data);

                    if (json.ContainsKey(key))
                    {
                        return JsonConvert.DeserializeObject<T>(json[key].ToString());
                    }
                    else
                    {
                        return defaultValue;
                    }
                }
                else
                {
                    return defaultValue;
                }
            }
            else
            {
                return defaultValue;
            }
        }
        catch
        {

        }

        return defaultValue;
    }

    public static T Load<T>(string key, T defaultValue)
    {
        return Load(GameConstants.SAVE_FILE_NAME, key, defaultValue);
    }

    public static async Task<T> LoadAsync<T>(string fileName, string key, T defaultValue)
    {
        string filePath = Application.persistentDataPath + $"/{fileName}.json";

        if (File.Exists(filePath))
        {
            string data = await File.ReadAllTextAsync(filePath);

            if (data != "")
            {
                JObject json = JObject.Parse(data);

                if (json.ContainsKey(key))
                {
                    return JsonConvert.DeserializeObject<T>(json[key].ToString());
                }
                else
                {
                    return defaultValue;
                }
            }
            else
            {
                return defaultValue;
            }
        }
        else
        {
            return defaultValue;
        }
    }

    public static T LoadAsync<T>(string key, T defaultValue)
    {
        return Load(GameConstants.SAVE_FILE_NAME, key, defaultValue);
    }

    // public static async Task<T> LoadAsync<T>(string key, T defaultValue)
    // {
    //     return await LoadAsync(GameConstants.SAVE_FILE_NAME, key, defaultValue);
    // }
}
