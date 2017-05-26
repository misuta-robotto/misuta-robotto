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
         The dead zone is the area behind the robot where the yaw can't move any further.
         */
        public float TranslateYaw(float baseYaw, float theta)
        {
            float yaw = -TranslateAngle(baseYaw) - theta;
            if (yaw > -2.0857 && yaw < 2.0857)
            {
                currentYaw = yaw;
            }

            return currentYaw;
        }
        /*
         Translate HMD-rotation to robot. Must send radians instead of degrees. 
         the angles between pi and 2*pi must be negative.
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
