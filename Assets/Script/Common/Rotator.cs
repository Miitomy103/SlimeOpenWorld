using UnityEngine;

/// <summary>
/// オブジェクトを回すクラス
/// </summary>
public class Rotator 
{
    Transform target;
    public Rotator(Transform target)
    {
        this.target = target;
    }

    public void RotateTowards(Vector3 direction, float rotateSpeed)
    {
        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            target.transform.rotation = Quaternion.Slerp(
                target.transform.rotation,
                targetRot,
                rotateSpeed * Time.deltaTime
            );
        }
    }
}
