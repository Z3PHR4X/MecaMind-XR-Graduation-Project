using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eraser : MonoBehaviour
{
    private bool isActive;

    public void SetActive()
    {
        isActive = true;
    }

    public void SetInactive()
    {
        isActive = false;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (isActive)
        {
            if (other.transform.CompareTag("Paint"))
            {
                Destroy(other.gameObject);
            }
        }
    }
}
