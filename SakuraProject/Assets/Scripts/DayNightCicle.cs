using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine;

public class DayNightCicle : MonoBehaviour
{
    [SerializeField] private float _rotationFactor = 0.5f;
    [SerializeField] private float _intentensityFactor = 1000f;
    private HDAdditionalLightData _lightData;

    void Start()
    {
        _lightData = GetComponent<HDAdditionalLightData>();
    }

    private void Update()
    {
        RotateLight();
        DecreaseLightIntensity();

    }

    private void RotateLight()
    {
        if (transform.localRotation.x > 0f)
        {
            transform.Rotate(-_rotationFactor * Time.deltaTime, 0.0f, 0.0f, Space.Self);
        }
    }

    private void DecreaseLightIntensity()
    {
        if (_lightData.intensity > 0f)
        {
            _lightData.intensity -= _intentensityFactor * Time.deltaTime;
        }
    }
}
