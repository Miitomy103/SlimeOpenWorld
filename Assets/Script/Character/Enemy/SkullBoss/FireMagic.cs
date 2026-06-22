using System.Collections;
using UnityEngine;

/// <summary>
/// 指定位置でエフェクトを順次有効化する炎魔法コンポーネント
/// </summary>
public class FireMagic : MonoBehaviour
{
    [SerializeField]ObjectFloatData<GameObject>[] fireMagicDatas;

    private void Start()
    {
        Fire(transform.position, 0);
    }
    public void Fire(Vector3 pos,float attack)
    {
        transform.position = pos;
        for (int i = 0; i < fireMagicDatas.Length; i++)
        {
            StartCoroutine(EnterEffects(fireMagicDatas[i]));
        }
    }

    IEnumerator EnterEffects(ObjectFloatData<GameObject> data)
    {
        data.Obj.gameObject.SetActive(false);
        yield return new WaitForSeconds(data.Value);
        data.Obj.gameObject.SetActive(true);
    }
}
