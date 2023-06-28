using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceLogger : MonoBehaviour
{
    public List<DistanceTracker> trackers = new List<DistanceTracker>();
    public float totalDistance = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        if (trackers.Count == 0) 
        {
            print("No trackers added to list! Cannot track movement distance.");
        }
        totalDistance = 0f;
        Invoke("LogDistance", 10);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var tracker in trackers)
        {
            totalDistance += tracker.distance;
            //Debug.Log(tracker.name + " acceleration: " + tracker.acceleration);
        }
    }

    private void LogDistance()
    {
        foreach (var tracker in trackers)
        {
            Debug.Log(tracker.name + " distance moved: " + tracker.trackedDistance);
        }
        Debug.Log("Total Distance Moved: " + totalDistance);
        
        Invoke("LogDistance", 10);
    }
}
