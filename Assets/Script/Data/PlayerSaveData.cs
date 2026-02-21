using UnityEngine;

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
