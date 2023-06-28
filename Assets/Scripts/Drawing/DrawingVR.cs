using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class DrawingVR : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private float drawFrequency = 60f;
    [SerializeField] private Color lineColor;
    [SerializeField] private float lineWidth = 0.005f;
    [SerializeField] private bool enableParticles = true;
    [SerializeField] private int particleAmount = 10;
    //[SerializeField] private bool enableHaptics = true;
    
    [Header("Optimization")]
    [SerializeField] private bool isOptimized = false;
    [SerializeField] private int optimizedLineLimit = 100;
    [SerializeField] private float smoothnessTolerance = 0.01f;

    [Header("Setup")] 
    public LineManager lineManager;
    public GameObject linePrefab;
    public LineRenderer line;
    public GameObject lineSource;
    public GameObject pencilTip;
    public GameObject previewSphere;
    public ParticleSystem particles;

    [Header("SteamVR")] 
    [SerializeField] SteamVR_Input_Sources controlHandType;
    [SerializeField] private SteamVR_Action_Single triggerValue;
    [SerializeField] private SteamVR_Action_Vector2 sliderValue;
    [SerializeField] private SteamVR_Action_Boolean undoValue;
    [SerializeField] private SteamVR_Action_Boolean redoValue;
    [SerializeField] private SteamVR_Action_Vibration hapticAction;
    //public Rigidbody controllerRb;

    [Header("Debug")] 
    public bool triggerOverride = false;

    private float drawCooldown;
    private List<LineRenderer> lineList = new List<LineRenderer>();
    private ParticleSystem ps;
    private ParticleSystem.MainModule mm;
    private ParticleSystem.EmissionModule em;
    
    void Start()
    {
        drawCooldown = 1 / drawFrequency;
        var render = pencilTip.GetComponent<Renderer>();
        render.material.SetColor("_Color", lineColor);
        render = previewSphere.GetComponent<Renderer>();
        render.material.SetColor("_Color", lineColor);

        if (enableParticles)
        {
            particles.gameObject.SetActive(true);
            ps = particles.GetComponent<ParticleSystem>();
            mm= particles.main;
            em = particles.emission;
        
            mm.startColor = new ParticleSystem.MinMaxGradient(lineColor);
            em.rateOverDistance = 0;
        }

        CreateNewLine();
        ChangeWidth();

        if (!SteamVR.active)
        {
            triggerOverride = true;
        }
    }
    
    //Create new line from Line prefab and set it as child of DrawingVR object
    //Then set the new line as the currently used line
    private void CreateNewLine()
    {
        GameObject curLine = Instantiate(linePrefab,  new Vector3(0,0,0), Quaternion.identity);
        curLine.transform.parent = this.transform;
        line = curLine.GetComponent<LineRenderer>();
        SetupLine();
    }

    //Apply selected settings to the current line
    private void SetupLine()
    {
        line.loop = false;
        line.numCapVertices = 5;
        line.numCornerVertices = 5;
        line.startColor = lineColor;
        line.endColor = lineColor;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.positionCount = 0;
        
        //Add line to list of lines of pencil
        lineManager.AddLine(line);
    }

    //When there's a line, continue updating that line (add new line positions etc.)
    //When there's no line, create a new line and then update the new line
    public void Draw()
    {
        if (triggerValue.GetAxis(controlHandType) > 0.1f || triggerOverride)
        {
            //DisablePreview();

            if (enableParticles)
            {
                em.rateOverDistance = particleAmount;
            }

            if (line != null)
            {
                UpdateLine();
            }
            else
            {
                CreateNewLine();
                UpdateLine();
            }
        }
        else
        {
            //EnablePreview();
            if (enableParticles)
            {
                em.rateOverDistance = 0; 
            }
            if (line != null)
            {
                DetachLine();
            }

            if (undoValue.GetStateDown(controlHandType))
            {
                UndoLine();
            }

            if (redoValue.GetStateDown(controlHandType))
            {
                RedoLine();
            }
        }
    }
    
    private void UpdateLine()
    {
        //When cooldown is over, add new line position and set its transform
        //If there's more line positions than allowed detach from that line and create a new line
        //This is to prevent program from slowing down due to too many positions
        if (drawCooldown < 0) 
        {
            line.positionCount++;
            line.SetPosition(line.positionCount-1, lineSource.transform.position );
            if (isOptimized)
            {
                if (line.positionCount > optimizedLineLimit)
                {
                    DetachLine();
                    CreateNewLine();
                }
            }
            
            drawCooldown = 1 / drawFrequency;

            if (SteamVR.active)
            {
                Pulse(1 / drawFrequency, drawFrequency / 2, 20f, controlHandType); //Haptics
                
                /*float hapticAmplitude;
                if (enableHaptics)
                {
                    hapticAmplitude = Mathf.Clamp(controllerRb.velocity.magnitude, 20, 60);
                    Pulse(1 / drawFrequency, (drawFrequency / 2) * (controllerRb.velocity.magnitude * 20), hapticAmplitude * 10,
                        controlHandType); //Haptics
                }
                else
                {
                    hapticAmplitude = 20f;
                    Pulse(1 / drawFrequency, drawFrequency / 2, hapticAmplitude, controlHandType); //Haptics
                }*/
            }
            
        }
        else
            //If the cooldown is still going, just update the cooldown
        {
            drawCooldown -= Time.deltaTime;
        }
    }

    //Simplifies the line within the set tolerance to improve performance
    //Then disconnects from the line
    public void DetachLine()
    {
        if (isOptimized)
        {
            line.Simplify(smoothnessTolerance);
        }
        
        line = null;
    }

    public void UndoLine()
    {
        if (line)
        {
            DetachLine();
        }
        lineManager.UndoLine();
        if (SteamVR.active)
        {
            Pulse(0.05f, 150, 20, controlHandType);
        }
    }

    public void RedoLine()
    {
        if (line)
        {
            DetachLine();
        }
        lineManager.RedoLine();
        if (SteamVR.active)
        {
            Pulse(0.05f, 150, 30, controlHandType);
        }
    }

    //Sets the color of the next line
    public void ChangeColor(Color newColor)
    {
        var render = pencilTip.GetComponent<Renderer>();
        render.material.SetColor("_Color", newColor);
        render = previewSphere.GetComponent<Renderer>();
        render.material.SetColor("_Color", newColor);
        
        lineColor = newColor;
        if (enableParticles)
        {
            mm.startColor = new ParticleSystem.MinMaxGradient(lineColor);
        }
        DetachLine();

        if (SteamVR.active)
        {
            Pulse(0.05f, 20, 20, controlHandType);
        }
    }
    
    public void ChangeWidth() //Slider Controls
    {
        Vector3 tipScale = new Vector3(1, 1, 1);
        if (triggerValue.GetAxis(controlHandType) < 0.1f)
        {
            //Increment pencil size
            if (sliderValue.GetAxis(controlHandType).y > 0.9f)
            {
                lineWidth += 0.01f;
                lineWidth = Mathf.Clamp(lineWidth, 0.001f, 0.2f);
            }
    
            if (sliderValue.GetAxis(controlHandType).y < -0.9f)
            {
                lineWidth -= 0.01f;
                lineWidth = Mathf.Clamp(lineWidth, 0.001f, 0.2f);
            }
    
            previewSphere.transform.localScale = tipScale * lineWidth * 500f;
        }
    }

    public void EnablePreview()
    {
        previewSphere.gameObject.SetActive(true);
    }

    public void DisablePreview()
    {
        previewSphere.gameObject.SetActive(false);
    }
    
    public void AssignHand(string handName)
    {
        if (handName.Contains("(right)"))
        {
            controlHandType = SteamVR_Input_Sources.RightHand;
        }
        else if(handName.Contains("(left)"))
        {
            controlHandType = SteamVR_Input_Sources.LeftHand;
        }
        //print(gameObject.name + " is held by: " + handName + " (" + controlHandType + ")");
    }
    
    private void Pulse(float duration, float frequency, float amplitude, SteamVR_Input_Sources source)
    {
        hapticAction.Execute(0, duration, frequency, amplitude, source);
    }
}
