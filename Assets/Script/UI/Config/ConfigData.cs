using UnityEngine;

/// <summary>
/// 音量設定の保存用データ。JSONとしてシリアライズされる。
/// </summary>
[System.Serializable]
public class ConfigData
{
    public ConfigData() { }
    public ConfigData(ConfigData other)
    {
        masterVolume = other.masterVolume;
        bgmVolume = other.bgmVolume;
        seVolume = other.seVolume;
        cvVolume = other.cvVolume;
    }

    [Header("音量設定")]
    // 音量設定
    [Range(0.0f, 1.0f)]
    public float masterVolume = 1.0f;
    [Range(0.0f, 1.0f)]
    public float bgmVolume = 0.5f;
    [Range(0.0f, 1.0f)]
    public float seVolume = 0.5f;
    [Range(0.0f, 1.0f)]
    public float cvVolume = 0.5f;

    public void SetVolume(AudioType type, float volume)
    {
        switch (type)
        {
            case AudioType.Master:
                masterVolume = volume;
                break;
            case AudioType.BGM:
                bgmVolume = volume;
                break;
            case AudioType.SE:
                seVolume = volume;
                break;
            case AudioType.CV:
                cvVolume = volume;
                break;
        }
    }
    public float GetVolume(AudioType type)
    {
        return type switch
        {
            AudioType.Master => masterVolume,
            AudioType.BGM => bgmVolume,
            AudioType.SE => seVolume,
            AudioType.CV => cvVolume,
            _ => 1.0f,
        };
    }

}
public enum AudioType
{
    Master,
    BGM,
    SE,
    CV
}
