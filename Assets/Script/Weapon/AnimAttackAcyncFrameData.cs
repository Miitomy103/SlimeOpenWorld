using UnityEngine;

[System.Serializable]
public class AnimAttackAcyncFrameData : IAnimAttackAcyncFrameData
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
