using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Code from wolfs cry games on Youtube
//source: https://www.youtube.com/watch?v=UDxyV-8Xil8

public class KyleIKScript : MonoBehaviour
{
     public Transform rightVRController;
     public Transform leftVRController;
     public Transform vrCamera;
     public Transform kyleHead;
     private Vector3 offset;
     Animator animator;

     public float crouchHeight;
     public float standHeight;

     void Start()
        {
            animator = GetComponent<Animator>();
            offset = vrCamera.position - kyleHead.position;
        }
    
        void OnAnimatorIK(int layerIndex)
        {
            float reachR = animator.GetFloat("KyleHandRight");
            float reachL = animator.GetFloat("KyleHandLeft");
            
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, reachR);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, reachL);
            
            animator.SetIKPosition(AvatarIKGoal.RightHand, rightVRController.position);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftVRController.position);
            
            animator.SetLookAtWeight(1f);
            animator.SetLookAtPosition(vrCamera.position+vrCamera.forward * 10f);
            
            animator.SetFloat("KyleHead", Mathf.Lerp(0,1,(vrCamera.position.y-crouchHeight)/(standHeight-crouchHeight)));
            
            Quaternion angleOffset = Quaternion.Euler(0,0,0);
            animator.SetIKRotation(AvatarIKGoal.RightHand, rightVRController.rotation * angleOffset);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftVRController.rotation * angleOffset);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand,1);
        }
    }