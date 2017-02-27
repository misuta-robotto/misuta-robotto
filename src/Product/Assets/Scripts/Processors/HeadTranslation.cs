using AL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    
    class HeadTranslator
    {
        const float PI = (float)Math.PI; 

        private float currentYaw = 0;

        public float TranslatePitch(float rawPitch)
        {
            return TranslateAngle(rawPitch);
        }

        /*
         Update yaw unless we are in the dead zone between -2.0857 and 2.0857 radians
         The dead zone is the area behind the robot where the yaw can't move any further
         */
        public float TranslateYaw(float rawYaw) 
        {
            float yaw = TranslateAngle(rawYaw);
            if (yaw > -2.0857 && yaw < 2.0857)
            {
                currentYaw = yaw;
            }

            return currentYaw;
        }
        /*
         Translate HMD-rotation to robot. Must send radians instead of degrees. 
         the angles between pi and 2*pi must be negative 
        */
        private float TranslateAngle(float rawAngle)
        {
            float angle = rawAngle * (2 * PI) / 360;
            if (angle > PI)
            {
                angle -= 2 * PI;
            }

            return angle;
        }
    }
}
