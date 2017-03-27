using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets { 
    public class ArmTranslation {
        public static float[] TranslateShoulderPitchAndRoll(Transform shoulder, Vector3 upperArmPos, Vector3 forearmPos) {
            Vector3 localLeftUpperArm = shoulder.InverseTransformPoint(upperArmPos);
            Vector3 localLeftForearm = shoulder.InverseTransformPoint(forearmPos);
            Vector3 leftUpperArm = localLeftForearm - localLeftUpperArm;

            float leftShoulderRoll = TranslateShoulderRoll(leftUpperArm);
            float leftShoulderPitch = TranslateShoulderPitch(leftUpperArm);
            return new float[] { (float)leftShoulderPitch, (float)leftShoulderRoll };
        }

        public static float TranslateShoulderPitch(Vector3 upperArm) {
            return -(float)Math.Atan2(upperArm.y, upperArm.z);
        }

        public static float TranslateShoulderRoll(Vector3 upperArm) {
            float y = upperArm.y;
            float z = upperArm.z;
            return (float)Math.Acos(Math.Sqrt(y * y + z * z) / upperArm.magnitude);
        }
    }
}