using System.IO;
using UnityEngine;

/// <summary>
/// コンフィグの設定はConfigData.csで行い、保存と読み込みはここで行う
/// </summary>
public static class ConfigManager
{
    static string SavePath =>
    Path.Combine(Application.persistentDataPath, "save.json");

    /// <summary>
    /// Updateで呼ぶなよ？
    /// </summary>
    public static void Save(ConfigData configData)
    {
        string json = JsonUtility.ToJson(configData, true);
        File.WriteAllText(SavePath, json);
    }
    /// <summary>
    /// Updateで呼ぶなよ？
    /// </summary>
    public static ConfigData Load()
    {
        if (!File.Exists(SavePath))
        {
            return new ConfigData();
        }
        string json = File.ReadAllText(SavePath);
        ConfigData configData = JsonUtility.FromJson<ConfigData>(json);
        return configData;
    }
}
