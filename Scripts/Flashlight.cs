using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Flashlight : MonoBehaviour
{
    public Light lamp;
    
    [Header("SteamVR")] 
    public SteamVR_Input_Sources controlHandType;
    public SteamVR_Action_Boolean triggerAction;
    public SteamVR_Action_Vibration hapticAction;

    private bool isOn = false;
    private float cooldown;

    // Start is called before the first frame update
    void Start()
    {
        cooldown = 1.0f;
        lamp.gameObject.SetActive(isOn);
    }

    private void toggleLight()
    {
        lamp.gameObject.SetActive(isOn);
    }

    public void Use()
    {
        if (triggerAction.GetStateDown(controlHandType) && cooldown < 0)
        {
            isOn = !isOn;
            toggleLight();
            cooldown = 1.0f;
            
            if (SteamVR.active)
            {
                Pulse(0.05f, 150, 20, controlHandType);
            }
        }
        else
        {
            cooldown -= Time.deltaTime;
        }
    }
    
    public void AssignHand()
    {
        string handName = this.transform.parent.name;
        if (handName.Contains("(right)"))
        {
            controlHandType = SteamVR_Input_Sources.RightHand;
        }
        else if(handName.Contains("(left)"))
        {
            controlHandType = SteamVR_Input_Sources.LeftHand;
        }
    }

    private void Pulse(float duration, float frequency, float amplitude, SteamVR_Input_Sources source)
    {
        hapticAction.Execute(0, duration, frequency, amplitude, source);
    }
}
