using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class HPVar 
{
    [SerializeField] Slider hpSlider;
    public void SetHPVar(float hp,float maxHp)
    {
        float value = Mathf.InverseLerp(0, maxHp, hp);
        hpSlider.value = value;
    }
}
