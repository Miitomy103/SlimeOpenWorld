using UnityEngine;

/// <summary>
/// ミニマップ用カメラをプレイヤーのXZ座標に追従させる(Y座標は固定)。
/// </summary>
public class MapCamera : MonoBehaviour
{
    Transform Player => PlayerController.Instance== null ? null : PlayerController.Instance.HostBase.transform;

    private void OnValidate()
    {
        if (Player != null)
        {
            gameObject.transform.position = new Vector3(Player.position.x, transform.position.y, Player.transform.position.z);
        }
    }
    void Update()
    {
        if (Player == null) return;
        gameObject.transform.position=new Vector3(Player.position.x,transform.position.y,Player.transform.position.z);
    }
}
