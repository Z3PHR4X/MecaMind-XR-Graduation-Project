using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trashcan : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "StartupDisk")
        {
            Debug.Log("Exiting application..");
            Application.Quit();
        }
    }
    
    //Your purpose is to close the game
    
}
