using UnityEngine;

/// <summary>
/// プレイヤーのセーブデータ(位置・シーン名・乗っ取り中のホスト名)。
/// </summary>
[System.Serializable]
public class PlayerSaveData
{
    public Vector3 position;
    public string sceneName;
    public string hostName;

    public PlayerSaveData(HostBase hostBase)
    {
        position = hostBase.transform.position;
        sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        hostName = hostBase.GetType().Name;
    }
}
