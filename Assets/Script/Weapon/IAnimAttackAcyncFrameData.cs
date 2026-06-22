/// <summary>
/// アニメーションのどのフレーム区間で攻撃判定を有効にするかを表すデータ。
/// Enable/Disableはアニメーションのフレーム番号、SampleFrameはサンプリングフレーム数、
/// AnimatorSpeedは再生速度で、これらから攻撃有効時間(秒)を算出する。
/// </summary>
public interface IAnimAttackAcyncFrameData
{
    float AnimatorSpeed { get; }
    int Disable { get; }
    int Enable { get; }
    int SampleFrame { get; }
}