using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Random = UnityEngine.Random;

public class ColorPicker : MonoBehaviour
{
    public Color color;
    public bool isRandom = false;
    public bool enableSplat = true;
    public GameObject splatPrefab;

    [Header("SteamVR")] 
    [SerializeField] private SteamVR_Input_Sources controlHandType;
    [SerializeField] private SteamVR_Action_Vibration hapticAction;

    private bool cooldown = false;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        if (isRandom)
        {
            color = Random.ColorHSV(0, 1f, 0, 1f, 0, 1f, 1f, 1f);
        }

        rb = GetComponent<Rigidbody>();
        var render = GetComponent<Renderer>();
        render.enabled = true;
        foreach (Material material in render.materials)
        {
            if (material.name == "Paint Material (Instance)")
            {
                material.SetColor("_Color", color);
            }
        }
    }

    public void PaintCan()
    {
        if (enableSplat)
        {
            if (Vector3.Dot(transform.TransformDirection(Vector3.forward), Vector3.down) > 0 && rb.velocity.magnitude > 1f && !cooldown)
            {
                SpillPaint();
            }
        }
    }

    public void EnableGravity()
    {
        rb.useGravity = true;
    }

    public void DisableGravity()
    {
        rb.useGravity = false;
    }

    private void SpillPaint(){
        RaycastHit hit;
        float maxDistance = 10f;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, maxDistance))
        {
            if (hit.transform.CompareTag("Paintable") && rb.velocity.magnitude > 1f)
            {
                //Need to look at making it orient with normal of object
                GameObject paint = Instantiate(splatPrefab, new Vector3(hit.point.x,hit.point.y+0.0001f,hit.point.z), new Quaternion(hit.transform.up.x , Random.rotation.y, hit.transform.up.z ,1));
                paint.GetComponent<Renderer>().material.color = color;
                
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance,
                        Color.yellow);
                if (SteamVR.active)
                {
                    Pulse(0.05f, 20, 20, controlHandType);
                }
                cooldown = true;
                Invoke("DisableCooldown", 0.8f);
            }
        }
    }

    private void DisableCooldown()
    {
        cooldown = false;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "DrawingPencil")
        {
            Pencil pencil = col.gameObject.GetComponent<Pencil>();
            pencil.drawingVR.ChangeColor(color);
        }
    }
    
    private void Pulse(float duration, float frequency, float amplitude, SteamVR_Input_Sources source)
    {
        hapticAction.Execute(0, duration, frequency, amplitude, source);
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
}
