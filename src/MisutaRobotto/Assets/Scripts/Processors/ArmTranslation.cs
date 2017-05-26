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

using System;
using UnityEngine;
using MishutaRoboto;

namespace Assets {
    public enum Side
    {
        Left,
        Right
    }

    public enum Joint
    {
        ShoulderRoll,
        ElbowRoll
    }

    public class ArmTranslation {
        public const float SHOULDER_PITCH_MIN = -119.5f * Mathf.Deg2Rad;
        public const float SHOULDER_PITCH_MAX = 119.5f * Mathf.Deg2Rad;
        public const float L_SHOULDER_ROLL_MIN = 0.5f * Mathf.Deg2Rad;
        public const float L_SHOULDER_ROLL_MAX = 89.5f * Mathf.Deg2Rad;
        public const float R_SHOULDER_ROLL_MIN = -89.5f * Mathf.Deg2Rad;
        public const float R_SHOULDER_ROLL_MAX = -0.5f * Mathf.Deg2Rad;
        public const float ELBOW_YAW_MIN = -119.5f * Mathf.Deg2Rad;
        public const float ELBOW_YAW_MAX = 119.5f * Mathf.Deg2Rad;
        public const float L_ELBOW_ROLL_MIN = -89.5f * Mathf.Deg2Rad;
        public const float L_ELBOW_ROLL_MAX = -0.5f * Mathf.Deg2Rad;
        public const float R_ELBOW_ROLL_MIN = 0.5f * Mathf.Deg2Rad;
        public const float R_ELBOW_ROLL_MAX = 89.5f * Mathf.Deg2Rad;

        /// <summary>
        ///   Utility function too clamp values for those joints that are different between sides
        /// </summary>
        private static float ClampJoint(float value, Joint joint, Side side) {
            switch (joint) {
            case Joint.ShoulderRoll:
                if (side == Side.Left) {
                    return value.Clamp(L_SHOULDER_ROLL_MIN, L_SHOULDER_ROLL_MAX);
                }
                else {
                    return value.Clamp(R_SHOULDER_ROLL_MIN, R_SHOULDER_ROLL_MAX);
                }
            case Joint.ElbowRoll:
                if (side == Side.Left) {
                    return value.Clamp(L_ELBOW_ROLL_MIN, L_ELBOW_ROLL_MAX);
                }
                else {
                    return value.Clamp(R_ELBOW_ROLL_MIN, R_ELBOW_ROLL_MAX);
                }
            default:
                // We should never end up here
                return 0;
            }
        }

        /// <summary>
        ///   Calculates the pitch and roll for Pepper's shoulder given the rotation in Unity
        /// </summary>
        public static float[] TranslateShoulderPitchAndRoll(Transform shoulder, Vector3 upperArmPos, Vector3 forearmPos, Side side) {
            // Transform upper arm and forearm position to shoulder coordinate system
            Vector3 localUpperArm = shoulder.InverseTransformPoint(upperArmPos);
            Vector3 localForearm = shoulder.InverseTransformPoint(forearmPos);

            // Vector that points in the direction of the upper arm
            Vector3 upperArm = localForearm - localUpperArm;

            // Only calculate shoulder roll if it's enough to the side to be reachable by Pepper
            float shoulderRoll = side == Side.Left ? L_SHOULDER_ROLL_MIN : R_SHOULDER_ROLL_MAX;
            bool armNotInFront = side == Side.Left ? localForearm.x < -0.06 : localForearm.x > 0.06;
            if (armNotInFront) {
                shoulderRoll = ClampJoint(TranslateShoulderRoll(upperArm, side), Joint.ShoulderRoll, side);
            }

            float shoulderPitch = TranslateShoulderPitch(upperArm, side).Clamp(SHOULDER_PITCH_MIN, SHOULDER_PITCH_MAX);
            return new float[] { shoulderPitch, shoulderRoll };
        }

        public static float TranslateShoulderPitch(Vector3 upperArm, Side side) {
            if (side == Side.Left) {
                return -(float)Math.Atan2(upperArm.y, upperArm.z);
            }
            else {
                return -Mathf.Atan2(-upperArm.y, -upperArm.z);
            }
        }

        public static float TranslateShoulderRoll(Vector3 upperArm, Side side) {
            float y = upperArm.y;
            float z = upperArm.z;
            if (side == Side.Left) {
                return (float)Math.Acos(Math.Sqrt((y * y) + (z * z)) / upperArm.magnitude);
            }
            else {
                return -Mathf.Acos(Mathf.Sqrt((y * y) + (z * z)) / upperArm.magnitude);
            }
        }

        /// <summary>
        ///   Calculates the yaw and roll for Pepper's shoulder given the rotation in Unity
        /// </summary>
        public static float[] TranslateElbowYawAndRoll(Transform upperArm, Vector3 forearmPos, Vector3 wristPos, Side side) {
            // Transform forearm and wrist position to elbow coordinate system
            Vector3 localForearm = upperArm.InverseTransformPoint(forearmPos);
            Vector3 localWrist = upperArm.InverseTransformPoint(wristPos);

            // Vector that points in the direction of the forearm
            Vector3 forearm = localWrist - localForearm;

            // Only calculate elbow yaw if the arm is actually bent
            // Otherwise it's impossible to determine from positions because they are all on a straight line
            float elbowYaw = 0;
            Vector2 armBend = new Vector2(forearm.y, forearm.z);
            if (armBend.magnitude > 0.05) {
                elbowYaw = TranslateElbowYaw(forearm, side).Clamp(ELBOW_YAW_MIN, ELBOW_YAW_MAX);
            }
            float elbowRoll = ClampJoint(TranslateElbowRoll(forearm, side), Joint.ElbowRoll, side);
            return new float[] { elbowYaw, elbowRoll };
        }

        public static float TranslateElbowYaw(Vector3 forearm, Side side) {
            if (side == Side.Left) {
                return -Mathf.Atan2(forearm.y, forearm.z);
            }
            else {
                return -Mathf.Atan2(-forearm.y, -forearm.z);
            }
        }

        public static float TranslateElbowRoll(Vector3 forearm, Side side) {
            if (side == Side.Left) {
                return -Mathf.Atan2(forearm.z, -forearm.x);
            }
            else {
                return Mathf.Atan2(-forearm.z, forearm.x);
            }
        }

        public static float TranslateWristYaw(Transform wristJoint, Side side)
        {
            float angle = wristJoint.localEulerAngles.y;
            if (angle > 180)
            {
                angle -= 360;
            }

            if (side == Side.Left)
            {
                if (angle > 70)
                {
                    angle = -82;
                }
            }
            else
            {
                if (angle < -15)
                {
                    angle = 180;
                }

                angle -= 90;
            }

            return Mathf.Deg2Rad * angle;
        }
    }
}
