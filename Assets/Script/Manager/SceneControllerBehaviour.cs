using UnityEngine;

public class SceneControllerBehaviour : MonoBehaviour
{
    [SerializeField] string sceneName;
    [SerializeField] Vector3 pos;
    public void LoadScene()
    {
        SceneController.Instance.LoadScene(sceneName, pos);
    }
}
