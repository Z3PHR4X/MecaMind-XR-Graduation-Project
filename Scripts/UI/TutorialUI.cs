using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private Image indexScheme;
    [SerializeField] private Image oculusScheme;
    [SerializeField] private Image viveScheme;
    [SerializeField] private TMP_Text unsupportedText;
    private string usedSystem;
    
    // Start is called before the first frame update
    private void Awake()
    {
        if (SteamVR.active)
        {
            usedSystem = SteamVR.instance.hmd_Type;
        }
        else
        {
            usedSystem = "inactive";
            unsupportedText.text = "Steam VR is inactive!";
        }
    }

    private void Start()
    {
        print(usedSystem);
        if (usedSystem.Contains("indexhmd"))
        {
            indexScheme.gameObject.SetActive(true);
            oculusScheme.gameObject.SetActive(false);
            viveScheme.gameObject.SetActive(false);
            unsupportedText.gameObject.SetActive(false);
        }
        else if (usedSystem.Contains("oculus"))
        {
            indexScheme.gameObject.SetActive(false);
            oculusScheme.gameObject.SetActive(true);
            viveScheme.gameObject.SetActive(false);
            unsupportedText.gameObject.SetActive(false);
        }
        else if (usedSystem.Contains("vive"))
        {
            indexScheme.gameObject.SetActive(false);
            oculusScheme.gameObject.SetActive(false);
            viveScheme.gameObject.SetActive(true);
            unsupportedText.gameObject.SetActive(false);
        }
        else
        {
            indexScheme.gameObject.SetActive(false);
            oculusScheme.gameObject.SetActive(false);
            viveScheme.gameObject.SetActive(false);
            unsupportedText.gameObject.SetActive(true);
        }
    }
}
