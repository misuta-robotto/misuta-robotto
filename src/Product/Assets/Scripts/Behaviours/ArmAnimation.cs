using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
/*
 * This class sets the 3D representation of the user in Unity so that its hands
 * try to reach the VRNodes of the Vive wands through Unitys built in IK system.
 */
public class ArmAnimation : MonoBehaviour {
    public Transform leftElbowHint;
    public Transform rightElbowHint;
    private Animator animator;

	void Start() {
        animator = GetComponent<Animator>();
	}

	void OnAnimatorIK(int layerIndex) {
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);

        var rightHandPos = InputTracking.GetLocalPosition(VRNode.RightHand);
        animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandPos);
        var leftHandPos = InputTracking.GetLocalPosition(VRNode.LeftHand);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandPos);

        var rightHandOrientation = InputTracking.GetLocalRotation(VRNode.RightHand);
        animator.SetIKRotation(AvatarIKGoal.RightHand, applyHandRotation(rightHandOrientation, -90));
        var leftHandOrientation = InputTracking.GetLocalRotation(VRNode.LeftHand);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, applyHandRotation(leftHandOrientation, 90));

        if (rightElbowHint != null)
        {
            animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, 0.5f);
            animator.SetIKHintPosition(AvatarIKHint.RightElbow, rightElbowHint.position);
        }

        if (leftElbowHint != null)
        {
            animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, 0.5f);
            animator.SetIKHintPosition(AvatarIKHint.LeftElbow, leftElbowHint.position);
        }
    }

    private Quaternion applyHandRotation(Quaternion rot, float amount)
    {
        var pointingDir = rot * Vector3.forward;
        var handTurn = Quaternion.AngleAxis(amount, pointingDir);

        return handTurn * rot;
    }
}
