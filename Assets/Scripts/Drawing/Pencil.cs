using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pencil : MonoBehaviour
{
    public DrawingVR drawingVR;

    public void AssignHand()
    {
        string handName = transform.parent.name;
        //drawingVR.controllerRb = transform.parent.GetComponent<Rigidbody>();
        drawingVR.AssignHand(handName);
    }
}
