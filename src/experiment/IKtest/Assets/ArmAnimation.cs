using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmAnimation : MonoBehaviour {
    public Transform objToPickUp = null;
    private Animator animator;

	void Start() {
        animator = GetComponent<Animator>();
	}

	void OnAnimatorIK(int layerIndex) {
        if (objToPickUp != null) {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);

            animator.SetIKPosition(AvatarIKGoal.RightHand, objToPickUp.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, objToPickUp.rotation);
        }
	}
}
