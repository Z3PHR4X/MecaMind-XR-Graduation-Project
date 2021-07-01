using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WindController : MonoBehaviour
{
    //Contains modified code from: https://answers.unity.com/questions/514656/wind-direction.html
    //Original code by: robertbu

    [Header("Wind Direction")]
    public float minChangeSpeed = 0.1f;
    public float maxChangeSpeed = 0.5f;
    public float maxDirectionChange = 60f;
    public float minTime = 0.2f;
    public float maxTime = 5f;
    [Header("Intensity Settings")] 
    public float intensityChangeSpeed = 0.3f;
    public float minIntensity = 0.1f;
    public float maxIntensity = 3f;
    public float turbulenceChangeSpeed = 0.2f;
    public float minTurbulence = 0.02f;
    public float maxTurbulence = 4f;
    [Header("Debug")]
    public float intensity;
    public float turbulence;

    private WindZone windZone;
    private float changeSpeed;
    private Quaternion qTo = Quaternion.identity;

    private void Awake()
    {
        windZone = GetComponent<WindZone>();
    }

    // Start is called before the first frame update
    void Start()
    {
        intensity = 0.5f;
        turbulence = 0.5f;
        Invoke("ChangeDirection",0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, qTo, changeSpeed * Time.deltaTime);
        windZone.windMain = Mathf.Lerp(windZone.windMain, intensity, intensityChangeSpeed * Time.deltaTime);
        windZone.windTurbulence = Mathf.Lerp(windZone.windTurbulence, turbulence, turbulenceChangeSpeed * Time.deltaTime);
    }

    public void ChangeStrength(float newIntensity)
    {
        intensity = Mathf.Clamp((newIntensity / 2), minIntensity, maxIntensity);
        turbulence = Mathf.Clamp((newIntensity / 1.5f), minTurbulence, maxTurbulence);
    }

    public void ChangeDirection()
    {
        changeSpeed = Random.Range(minChangeSpeed, maxChangeSpeed);
        qTo = Quaternion.AngleAxis(Random.Range(-maxDirectionChange, maxDirectionChange), Vector3.up) * transform.rotation;
        Invoke("ChangeDirection", Random.Range(minTime, maxTime));
    }
}
