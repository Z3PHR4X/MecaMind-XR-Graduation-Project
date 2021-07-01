using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCulling : MonoBehaviour
{
    public bool isCulling;
    public Camera mainCamera;

    private void Start()
    {
        isCulling = false;
        mainCamera = Camera.main;
    }

    private void EnableCulling()
    {
        var oldMask = mainCamera.cullingMask;
        var newMask = oldMask & ~(1 << 10);
        mainCamera.cullingMask = newMask;
    }

    private void DisableCulling()
    {
        LayerMask oldMask = mainCamera.cullingMask;
        LayerMask newMask = oldMask | (1 << 10);
        mainCamera.cullingMask = newMask;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("3DGlasses"))
        {
            isCulling = false;
            other.gameObject.GetComponent<Light>().intensity = 2f;
            DisableCulling();
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.transform.CompareTag("3DGlasses"))
        {
            isCulling = true;
            other.gameObject.GetComponent<Light>().intensity = 0f;
            EnableCulling();
        }
    }
}
