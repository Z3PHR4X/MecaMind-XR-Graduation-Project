using UnityEngine;
using Valve.VR;

public class VRController : MonoBehaviour
{
    [Header("Settings")]
    public float gravity = 981.0f;
    public float sensitivity = 0.1f;
    public float maxSpeed = 3.0f;
    public float rotationIncrement = 90;

    [Header("Movement")]
    public SteamVR_Input_Sources handMoveType;
    public SteamVR_Action_Vector2 moveDirectionAction;

    [Header("Snap Turn")]
    public SteamVR_Input_Sources handTurnType;
    public SteamVR_Action_Boolean turnRightAction;
    public SteamVR_Action_Boolean turnLeftAction;

    [Header("Embodiment")] 
    public bool isEmbodied = false;
    public KyleIKScript avatar;
    public Animator animator;

    [Header("Fallback")] 
    public GameObject cameraPrefab;

    private float speed = 0.0f;
    
    private CharacterController characterController;
    private Transform cameraRig;
    private Transform head;

    private void Awake()
    {
        if (!SteamVR.active)
        {
            Debug.Log(gameObject.name + " was disabled due to SteamVR being inactive!" );
            Instantiate(cameraPrefab, gameObject.transform.position, new Quaternion(gameObject.transform.rotation.x, gameObject.transform.rotation.y, gameObject.transform.rotation.z, 1));
            gameObject.SetActive(false); 
        }
        
        characterController = GetComponent<CharacterController>();

        if (isEmbodied)
        {
            avatar.gameObject.SetActive(true);
        }
        else
        {
            avatar.gameObject.SetActive(false);
        }
        
    }

    private void Start()
    {
        cameraRig = SteamVR_Render.Top().origin;
        head = SteamVR_Render.Top().head;
    }

    // Update is called once per frame
    void Update()
    {
        HandleHeight();
        CalculateMovement();
        SnapRotation();
    }
    
    private void HandleHeight()
    {
        //Get the head in local space
        float headHeight = Mathf.Clamp(head.localPosition.y, 1, 2);
        characterController.height = headHeight;

        //Cut in half
        Vector3 newCenter = Vector3.zero;
        newCenter.y = characterController.height / 2;
        newCenter.y += characterController.skinWidth;
        
        //Move capsule in local space
        newCenter.x = head.localPosition.x;
        newCenter.z = head.localPosition.z;
        
        //Apply
        characterController.center = newCenter;
    }

    private void CalculateMovement()
    {
        //Figure out movement orientation
        Quaternion orientation = CalculateOrientation();
        Vector3 movement = Vector3.zero;

        //If not moving
        if (moveDirectionAction.GetAxis(handMoveType).magnitude == 0)
        {
            speed = 0;
        }
        
        //Add, clamp
        speed += moveDirectionAction.GetAxis(handMoveType).magnitude * sensitivity;
        speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);
           
        //Orientation and Grativy
        movement += orientation * (speed * Vector3.forward);
        movement.y -= gravity * Time.deltaTime;
        
        //Apply
        characterController.Move(movement * Time.deltaTime);
        if (isEmbodied)
        {
            animator.SetFloat("VelocityX", movement.x);
            animator.SetFloat("VelocityY", movement.z);
            animator.transform.position.Set(characterController.transform.position.x, characterController.transform.position.y, characterController.transform.position.z);
            animator.transform.rotation.Set(0, head.eulerAngles.y, 0, 0); 
        }
    }

    private Quaternion CalculateOrientation()
    {
        float rotation = Mathf.Atan2(moveDirectionAction.axis.x, moveDirectionAction.axis.y);
        rotation *= Mathf.Rad2Deg;
        
        Vector3 orientationEuler = new Vector3(0, head.eulerAngles.y + rotation, 0);
        return Quaternion.Euler(orientationEuler);
    }

    private void SnapRotation()
    {
        float snapValue = 0.0f;

        if (turnLeftAction.GetStateDown(handTurnType))
        {
            snapValue = -Mathf.Abs(rotationIncrement);
        }

        if (turnRightAction.GetStateDown(handTurnType))
        {
            snapValue = Mathf.Abs(rotationIncrement);
        }
        
        transform.RotateAround(head.position, Vector3.up, snapValue);
    }
}
