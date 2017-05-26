/*
Copyright (c) 2017, Misuta Robotto Group

The contents of this file are subject to the Common Public Attribution License Version 1.0 (the “License”); 
you may not use this file except in compliance with the License. You may obtain a copy of the License at

    https://github.com/Emiluren/misuta-robotto/blob/master/LICENSE.md
    
The License is based on the Mozilla Public License Version 1.1 but Sections 14 and 15 have been added to cover
use of software over a computer network and provide for limited attribution for the Original Developer. In 
addition, Exhibit A has been modified to be consistent with Exhibit B.

Software distributed under the License is distributed on an “AS IS” basis, WITHOUT WARRANTY OF ANY KIND, 
either express or implied. See the License  for the specific language governing rights and limitations 
under the License.

The Original Code is Misuta Robotto.

The Initial Developer of the Original Code is Misuta Robotto Group. 
All portions of the code written by Misuta Robotto Group are Copyright (c) 2017. All Rights Reserved.

Misuta Robotto Group includes Robin Christensen, Jacob Lundberg, Ylva Lundegård, Emil Segerbäck,
Patrik Sletmo, Teo Tiefenbacher, Jon Vik and David Wajngot.
*/

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
