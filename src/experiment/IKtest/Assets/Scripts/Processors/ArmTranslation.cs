using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MishutaRoboto;

namespace Assets { 
    public class ArmTranslation {
        public const float L_SHOULDER_PITCH_MIN = -119.5f * Mathf.Deg2Rad;
        public const float L_SHOULDER_PITCH_MAX = 119.5f * Mathf.Deg2Rad;
        public const float L_SHOULDER_ROLL_MIN = 0.5f * Mathf.Deg2Rad;
        public const float L_SHOULDER_ROLL_MAX = 89.5f * Mathf.Deg2Rad;
        public const float L_ELBOW_YAW_MIN = -119.5f * Mathf.Deg2Rad;
        public const float L_ELBOW_YAW_MAX = 119.5f * Mathf.Deg2Rad;
        public const float L_ELBOW_ROLL_MIN = -89.5f * Mathf.Deg2Rad;
        public const float L_ELBOW_ROLL_MAX = -0.5f * Mathf.Deg2Rad;

        public static float[] TranslateShoulderPitchAndRoll(Transform shoulder, Vector3 upperArmPos, Vector3 forearmPos) {
            Vector3 localLeftUpperArm = shoulder.InverseTransformPoint(upperArmPos);
            Vector3 localLeftForearm = shoulder.InverseTransformPoint(forearmPos);
            Vector3 leftUpperArm = localLeftForearm - localLeftUpperArm;

            float leftShoulderRoll = TranslateShoulderRoll(leftUpperArm).Clamp(L_SHOULDER_ROLL_MIN, L_SHOULDER_ROLL_MAX);
            float leftShoulderPitch = TranslateShoulderPitch(leftUpperArm).Clamp(L_SHOULDER_PITCH_MIN, L_SHOULDER_PITCH_MAX);
            return new float[] { leftShoulderPitch, leftShoulderRoll };
        }

        public static float TranslateShoulderPitch(Vector3 upperArm) {
            return -(float)Math.Atan2(upperArm.y, upperArm.z);
        }

        public static float TranslateShoulderRoll(Vector3 upperArm) {
            float y = upperArm.y;
            float z = upperArm.z;
            return (float)Math.Acos(Math.Sqrt(y * y + z * z) / upperArm.magnitude);
        }

        public static float[] TranslateElbowYawAndRoll(Transform upperArm, Vector3 forearmPos, Vector3 wristPos) {
            Vector3 localLeftForearm = upperArm.InverseTransformPoint(forearmPos);
            Vector3 localLeftWrist = upperArm.InverseTransformPoint(wristPos);
            Vector3 leftForearm = localLeftWrist - localLeftForearm;

            float leftElbowYaw = TranslateElbowYaw(leftForearm).Clamp(L_ELBOW_YAW_MIN, L_ELBOW_YAW_MAX);
            float leftElbowRoll = TranslateElbowRoll(leftForearm).Clamp(L_ELBOW_ROLL_MIN, L_ELBOW_ROLL_MAX);
            return new float[] { leftElbowYaw, leftElbowRoll };
        }

        public static float TranslateElbowYaw(Vector3 forearm) {
            return -(float)Math.Atan2(forearm.y, forearm.z);
        }

        public static float TranslateElbowRoll(Vector3 forearm) {
            float y = forearm.y;
            float x = forearm.x;
            return (float)(-Math.Acos(Math.Sqrt(y * y + x * x) / forearm.magnitude));
        }
    }
}