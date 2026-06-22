using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// 設定画面のUIから音量を変更し、ConfigManager経由で保存・読込を行うクラス。
/// </summary>
public class ConfigBehaviour : MonoBehaviour
{
    [SerializeField] ConfigData configData;

    [Header("References")]
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] TapSlider[] sliders;

    [Header("Default Value")]
    [SerializeField] ConfigData defaultValue;
    private void Start()
    {
        configData = ConfigManager.Load();
        LoadData(configData);
    }
    private void LoadData(ConfigData configData)
    {
        foreach(AudioType audioType in System.Enum.GetValues(typeof(AudioType)))
        {
            MixerUpdate(configData.GetVolume(audioType), audioType);
        }
        if (sliders != null && sliders.Length != 0)
        {
            for (int i = 0; i < sliders.Length; i++)
            {
                foreach (AudioType audioType in System.Enum.GetValues(typeof(AudioType)))
                {
                    if (sliders[i].Key == audioType.ToString())
                    {
                        sliders[i].SetFillAmount(configData.GetVolume(audioType));
                    }
                }
            }
        }
    }

    public void DefaultReset()
    {
        configData = new ConfigData(defaultValue);
        LoadData(configData);
    }

    public void ChangeMasterVolume(float sliderValue) =>MixerUpdate(sliderValue, AudioType.Master);
    public void ChangeBGMVolume(float sliderValue) => MixerUpdate(sliderValue, AudioType.BGM);
    public void ChangeSEVolume(float sliderValue) => MixerUpdate(sliderValue, AudioType.SE);
    public void ChangeCVVolume(float sliderValue) => MixerUpdate(sliderValue, AudioType.CV);

    void MixerUpdate(float sliderValue,AudioType audioType)
    {
        Debug.Log($"Volume Change : {audioType.ToString()} to {sliderValue}");
        configData.SetVolume(audioType, sliderValue);
        float volume=Mathf.Max(sliderValue, 0.0001f);
        audioMixer.SetFloat(audioType.ToString(), Mathf.Log10(volume) * 20);
        SaveData();
    }
    public void SaveData()
    {
        ConfigManager.Save(configData);
    }

}
