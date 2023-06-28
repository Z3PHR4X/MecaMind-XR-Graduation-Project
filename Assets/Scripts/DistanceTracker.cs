using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceTracker : MonoBehaviour
{
    public GameObject tracked;
    public float trackedDistance = 0f;
    public float distance;
    public float acceleration;

    private Vector3 oldTransform;
    private Vector3 curTransform;
    private Rigidbody rb;
    private float oldVelocity;
    

    private void Awake()
    {
        rb = tracked.GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        acceleration = 0f;
        trackedDistance = 0f;
        distance = 0f;
        oldTransform = tracked.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        curTransform = tracked.transform.localPosition;

        distance = (curTransform - oldTransform).magnitude;
        trackedDistance += distance;
        //Debug.Log(tracked.name + " distance moved: " + trackedDistance + " (total distance of " + tracked.name + ": " + trackedDistance + ")");

        //float curVelocity = rb.velocity.magnitude;
        //acceleration = (curVelocity - oldVelocity) * Time.deltaTime;
        //Debug.Log(tracked.name + " current acceleration: " + acceleration);
        //oldVelocity = curVelocity;
        oldTransform = curTransform;
        
    }
}
