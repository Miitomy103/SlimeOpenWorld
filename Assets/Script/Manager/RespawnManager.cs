using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [SerializeField] SlimeHost slimeHost;
    [SerializeField] Transform defaultPosition;

    public void Respawn()
    {
        PlayerController.Instance.SetHost(slimeHost,null);
        slimeHost.transform.position = defaultPosition.position;
    }
}
