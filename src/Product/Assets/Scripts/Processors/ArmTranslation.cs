using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MishutaRoboto;

namespace Assets {
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

        public static float[] TranslateLeftShoulderPitchAndRoll(Transform shoulder, Vector3 upperArmPos, Vector3 forearmPos) {
            Vector3 localLeftUpperArm = shoulder.InverseTransformPoint(upperArmPos);
            Vector3 localLeftForearm = shoulder.InverseTransformPoint(forearmPos);
            Vector3 leftUpperArm = localLeftForearm - localLeftUpperArm;

            float leftShoulderRoll = L_SHOULDER_ROLL_MIN;

            if (localLeftForearm.x < -0.06) {
                leftShoulderRoll = TranslateLeftShoulderRoll(leftUpperArm).Clamp(L_SHOULDER_ROLL_MIN, L_SHOULDER_ROLL_MAX);
            }
            float leftShoulderPitch = TranslateLeftShoulderPitch(leftUpperArm).Clamp(SHOULDER_PITCH_MIN, SHOULDER_PITCH_MAX);
            return new float[] { leftShoulderPitch, leftShoulderRoll };
        }

        public static float TranslateLeftShoulderPitch(Vector3 upperArm) {
            return -(float)Math.Atan2(upperArm.y, upperArm.z);
        }

        public static float TranslateLeftShoulderRoll(Vector3 upperArm) {
            float y = upperArm.y;
            float z = upperArm.z;
            return (float)Math.Acos(Math.Sqrt(y * y + z * z) / upperArm.magnitude);
        }

        public static float[] TranslateLeftElbowYawAndRoll(Transform upperArm, Vector3 forearmPos, Vector3 wristPos) {
            Vector3 localLeftForearm = upperArm.InverseTransformPoint(forearmPos);
            Vector3 localLeftWrist = upperArm.InverseTransformPoint(wristPos);
            Vector3 leftForearm = localLeftWrist - localLeftForearm;

            float leftElbowYaw = 0;
            Vector2 straightArm = new Vector2(leftForearm.y, leftForearm.z);
            if (straightArm.magnitude > 0.05)
            {
                leftElbowYaw = TranslateLeftElbowYaw(leftForearm).Clamp(ELBOW_YAW_MIN, ELBOW_YAW_MAX);
            }
            float leftElbowRoll = TranslateLeftElbowRoll(leftForearm).Clamp(L_ELBOW_ROLL_MIN, L_ELBOW_ROLL_MAX);
            return new float[] { leftElbowYaw, leftElbowRoll };
        }

        public static float TranslateLeftElbowYaw(Vector3 forearm) {
            return -Mathf.Atan2(forearm.y, forearm.z);
        }

        public static float TranslateLeftElbowRoll(Vector3 forearm) {
            return -Mathf.Atan2(forearm.z, -forearm.x);
        }

        // RIGHT SIDE
        public static float[] TranslateRightShoulderPitchAndRoll(Transform shoulder, Vector3 upperArmPos, Vector3 forearmPos) {
            Vector3 localRightUpperArm = shoulder.InverseTransformPoint(upperArmPos);
            Vector3 localRightForearm = shoulder.InverseTransformPoint(forearmPos);
            Vector3 rightUpperArm = localRightForearm - localRightUpperArm;

            float rightShoulderRoll = R_SHOULDER_ROLL_MAX;

            if (localRightForearm.x > 0.06) {
                rightShoulderRoll = TranslateRightShoulderRoll(rightUpperArm).Clamp(R_SHOULDER_ROLL_MIN, R_SHOULDER_ROLL_MAX);
            }
            float rightShoulderPitch = TranslateRightShoulderPitch(rightUpperArm).Clamp(SHOULDER_PITCH_MIN, SHOULDER_PITCH_MAX);
            return new float[] { rightShoulderPitch, rightShoulderRoll };
        }

        public static float TranslateRightShoulderPitch(Vector3 upperArm) {
            return -Mathf.Atan2(-upperArm.y, -upperArm.z);
        }

        public static float TranslateRightShoulderRoll(Vector3 upperArm) {
            float y = upperArm.y;
            float z = upperArm.z;
            return -Mathf.Acos(Mathf.Sqrt(y * y + z * z) / upperArm.magnitude);
        }

        public static float[] TranslateRightElbowYawAndRoll(Transform upperArm, Vector3 forearmPos, Vector3 wristPos) {
            Vector3 localRightForearm = upperArm.InverseTransformPoint(forearmPos);
            Vector3 localRightWrist = upperArm.InverseTransformPoint(wristPos);
            Vector3 rightForearm = localRightWrist - localRightForearm;

            float rightElbowYaw = 0;
            Vector2 straightArm = new Vector2(rightForearm.y, rightForearm.z);
            if (straightArm.magnitude > 0.05) {
                rightElbowYaw = TranslateRightElbowYaw(rightForearm).Clamp(ELBOW_YAW_MIN, ELBOW_YAW_MAX);
            }
            float rightElbowRoll = TranslateRightElbowRoll(rightForearm).Clamp(R_ELBOW_ROLL_MIN, R_ELBOW_ROLL_MAX);
            //Debug.Log("RightElbowRoll: " + RightElbowRoll);
            return new float[] { rightElbowYaw, rightElbowRoll };
        }
        public static float TranslateRightElbowYaw(Vector3 forearm) {
            return -Mathf.Atan2(-forearm.y, -forearm.z);
        }

        public static float TranslateRightElbowRoll(Vector3 forearm) {
            //Debug.Log(Mathf.Atan2(-forearm.z, forearm.x) * Mathf.Rad2Deg);
            return Mathf.Atan2(-forearm.z, forearm.x);
        }
    }
}
