using UnityEngine;

/// <summary>
/// Inspector上でシーン名と座標を指定して呼べる、SceneController.LoadSceneのラッパー。
/// </summary>
public class SceneControllerBehaviour : MonoBehaviour
{
    [SerializeField] string sceneName;
    [SerializeField] Vector3 pos;
    public void LoadScene()
    {
        SceneController.Instance.LoadScene(sceneName, pos);
    }
}
