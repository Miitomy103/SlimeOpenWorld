using UnityEngine;
using System.Collections;

public class LightIntensityController : MonoBehaviour
{
	public Light _light;
	float _time = 0;
	public float Delay = 0.5f;
	public float Down = 1;

	private float initIntensity;
    private void Awake()
    {
        initIntensity = _light.intensity;
    }
    private void OnEnable()
    {
        _time = 0;
        _light.intensity = initIntensity;
    }
    void Update ()
	{
		_time += Time.deltaTime;

		if(_time > Delay)
		{
			if(_light.intensity > 0)
				_light.intensity -= Time.deltaTime*Down;

			if(_light.intensity <= 0)
				_light.intensity = 0;
		}
	}
}
