using UnityEngine;

public class LookAtObject : MonoBehaviour
{
    public Transform objectA; // オブジェクトAをInspectorで指定
    public Transform objectB; // オブジェクトBをInspectorで指定
    public float followSpeed = 5f; // 追従の速さ（大きいほど速く追従）

    private void Update()
    {
        // ターゲットオブジェクトの向きを緩やかに追従
        Vector3 targetDirection = objectA.position - objectB.position;
        targetDirection.y = 0f; // 高さは考慮しない場合、y軸の回転を無効にする

        // 線形補間を使用して緩やかな追従を行う
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection.normalized);
        objectB.rotation = Quaternion.Slerp(objectB.rotation, targetRotation, Time.deltaTime * followSpeed);
    }
}