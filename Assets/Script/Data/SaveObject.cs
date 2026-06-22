using System.IO;
using UnityEngine;

/// <summary>
/// 任意のデータをJSON形式でpersistentDataPathに保存・読込する汎用クラス。
/// </summary>
public class SaveObject<T> where T : class
{
    string fullPath;
    const string extension = ".json";
    public SaveObject(string path)
    {
        fullPath = Application.persistentDataPath + "/" + path;
    }

    public void Save(T data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(fullPath + extension, json);
    }
    public T Get()
    {
        if(!File.Exists(fullPath + extension))
        {
            Debug.LogWarning("Save file not found: " + fullPath + extension);
            return null;
        }
        string json = File.ReadAllText(fullPath + extension);
        return JsonUtility.FromJson<T>(json);
    }
}
