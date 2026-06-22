using UnityEngine;

[CreateAssetMenu(fileName = "AnimAttackAcyncFrameSO", menuName = "ScriptableObject/Weapon/AnimAttackAcyncFrameSO")]
/// <summary>
/// IAnimAttackAcyncFrameDataのアセット版。武器ごとに攻撃フレーム設定を共有・再利用できる。
/// </summary>
public class AnimAttackAcyncFrameSO : ScriptableObject,IAnimAttackAcyncFrameData
{
    [SerializeField] int enable = 30;
    public int Enable => enable;
    [SerializeField] int disable = 50;
    public int Disable => disable;
    [SerializeField] int sampleFrame = 30;
    public int SampleFrame => sampleFrame;
    [SerializeField] float animationSpeed = 1;
    public float AnimatorSpeed => animationSpeed;
}
