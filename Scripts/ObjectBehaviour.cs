using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectBehaviour : MonoBehaviour
{
    public bool enableBreaking = true;
    public float breakFactor = 0.5f;
    public ObjectList objectList;
    public GameObject particles;
    
    private Rigidbody rb;
    private float breakForce;
    private int maxSpawns = 4;
    private bool canSpawn = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        maxSpawns = Random.Range(1, maxSpawns);
        breakForce = rb.mass * breakFactor;
    }

    private void Start()
    {
        Invoke("DisableSafety", 5);
    }

    private void DisableSafety()
    {
        canSpawn = true;
    }

    private void EnableSafety()
    {
        canSpawn = false;
        Invoke("DisableSafety", 2);
    }

    private void BreakObject()
    {
        SendNewMessage();
        for (int i = 0; i < maxSpawns; i++)
        {
            Instantiate(particles, transform.position, Quaternion.identity);
            int objectToSpawn = (int)Random.Range(0f, objectList.items.Count);
            GameObject obj = Instantiate(objectList.items[objectToSpawn], transform.position, Quaternion.identity);
            obj.transform.rotation = transform.rotation;
            obj.GetComponent<Rigidbody>().velocity = rb.velocity;
        }
        Destroy(this.gameObject);
    }
    
    private void SendNewMessage(){
        List<GameObject> gos = new List<GameObject>();
        foreach(var obj in GameObject.FindGameObjectsWithTag("Objects"))
        {
            gos.Add(obj);
        }
        for(var i = 0; i < gos.Count; i++){
            gos[i].SendMessage("EnableSafety");
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (enableBreaking)
        {
            //print(this.name + "Force: " + rb.velocity.magnitude + " Required: " + breakForce + " Safety off: " + canSpawn);
            if (rb.velocity.magnitude > breakForce && canSpawn)
            {
                BreakObject();
            }
        }
    }
}
