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
using System.Runtime.InteropServices;
using UnityEngine;

namespace AL
{
    // ALMotionProxy is used to communicate with a robot and perform
    // various motion actions.
    //
    // For more information see
    // http://doc.aldebaran.com/2-1/naoqi/motion/control-cartesian-api.html
    public class ALMotionProxy
    {
        [DllImport("bridge_d")]
        private static extern IntPtr ALMotionProxyNew(string server, int port);

        [DllImport("bridge_d")]
        private static extern void ALMotionProxyAngleInterpolation(IntPtr self, IntPtr names, IntPtr angleList, IntPtr timeLists, bool isAbsolute);

        [DllImport("bridge_d")]
        private static extern void ALMotionProxyAngleInterpolationWithSpeed(IntPtr self, IntPtr names, IntPtr targetAngles, float maxSpeedFraction);

        [DllImport("bridge_d")]
        private static extern void ALMotionProxyAngleInterpolationBezier(IntPtr self, IntPtr jointNames, IntPtr times, IntPtr controlPoints);

        [DllImport("bridge_d")]
        private static extern void ALMotionProxySetAngles(IntPtr self, IntPtr names, IntPtr angles, float fractionMaxSpeed);

        [DllImport("bridge_d")]
        private static extern void ALMotionProxyChangeAngles(IntPtr self, IntPtr names, IntPtr changes, float fractionMaxSpeed);

        [DllImport("bridge_d")]
        private static extern IntPtr ALMotionProxyGetAngles(IntPtr self, IntPtr names, bool useSensors);

        [DllImport("bridge_d")]
        private static extern IntPtr ALMotionProxyCloseHand(IntPtr self, string handName);

        [DllImport("bridge_d")]
        private static extern IntPtr ALMotionProxyOpenHand(IntPtr self, string handName);

        [DllImport("bridge_d")]
        private static extern IntPtr ALMotionProxyFree(IntPtr self);

        [DllImport("bridge_d")]
        private static extern void ALMotionProxyMoveInit(IntPtr self);

        [DllImport("bridge_d")]
        private static extern void ALMotionProxyMove(IntPtr self, float x, float y, float theta);

        [DllImport("bridge_d")]
        private static extern void ALMotionProxyMoveTo(IntPtr self, float x, float y, float theta);

        [DllImport("bridge_d")]
        private static extern void ALMotionProxyMoveToAsync(IntPtr self, float x, float y, float theta);

        [DllImport("bridge_d")]
        private static extern void ALMotionProxyGetRobotPosition(IntPtr self, bool useSensors, [In, Out] IntPtr buffer);

        [DllImport("bridge_d")]
        private static extern void ALMotionProxyStopMove(IntPtr self);

        [DllImport("bridge_d")]
        private static extern void ALMotionProxyKillMove(IntPtr self);

        private IntPtr unmanagedMem;

        public ALMotionProxy(string server, int port)
        {
            this.unmanagedMem = ALMotionProxyNew(server, port);
        }

        ~ALMotionProxy()
        {
            ALMotionProxyFree(unmanagedMem);
        }
        
        public bool IsConnected()
        {
            return unmanagedMem != IntPtr.Zero;
        }

        public void Disconnect()
        {
            ALMotionProxyFree(unmanagedMem);
            unmanagedMem = IntPtr.Zero;
        }

        public void AngleInterpolation(string[] names, float[] angles, float[] timeLists, bool isAbsolute)
        {
            ALMotionProxyAngleInterpolation(
                unmanagedMem,
                new ALValue(names).Pointer,
                new ALValue(angles).Pointer,
                new ALValue(timeLists).Pointer,
                isAbsolute
            );
        }

        public void AngleInterpolationWithSpeed(string[] names, float[] targetAngles, float maxSpeedFraction)
        {
            ALMotionProxyAngleInterpolationWithSpeed(
                unmanagedMem,
                new ALValue(names).Pointer,
                new ALValue(targetAngles).Pointer,
                maxSpeedFraction
            );
        }

        public void AngleInterpolationBezier(string[] jointNames, float[] times, float[] controlPoints)
        {
            ALMotionProxyAngleInterpolationBezier(
                unmanagedMem,
                new ALValue(jointNames).Pointer,
                new ALValue(times).Pointer,
                new ALValue(controlPoints).Pointer
            );
        }

        public void SetAngles(string[] names, float[] angles, float fractionMaxSpeed)
        {
            ALMotionProxySetAngles(
                unmanagedMem,
                new ALValue(names).Pointer,
                new ALValue(angles).Pointer,
                fractionMaxSpeed
            );
        }

        public void ChangeAngles(string[] names, float[] changes, float fractionMaxSpeed)
        {
            ALMotionProxyChangeAngles(
                unmanagedMem,
                new ALValue(names).Pointer,
                new ALValue(changes).Pointer,
                fractionMaxSpeed
            );
        }

        public float[] GetAngles(string[] names, bool useSensors)
        {
            IntPtr returned = ALMotionProxyGetAngles(
                unmanagedMem,
                new ALValue(names).Pointer,
                useSensors
            );

            float[] arr = new float[names.Length];
            Marshal.Copy(returned, arr, 0, names.Length);

            // TODO: Release memory
            return arr;
        }

        public void CloseHand(string handName)
        {
            ALMotionProxyCloseHand(unmanagedMem, handName);
        }

        public void OpenHand(string handName)
        {
            ALMotionProxyOpenHand(unmanagedMem, handName);
        }

        public void MoveInit()
        {
            ALMotionProxyMoveInit(
                unmanagedMem
            );
        }

        public void Move(float x, float y, float theta)
        {
            ALMotionProxyMove(
                unmanagedMem,
                x,
                y,
                theta
            );
        }

        public void MoveTo(float x, float y, float theta)
        {
            ALMotionProxyMoveTo(
                unmanagedMem,
                x,
                y,
                theta
            );
        }

        public void MoveToAsync(float x, float y, float theta)
        {
            ALMotionProxyMoveToAsync(
                unmanagedMem,
                x,
                y,
                theta
            );
        }

        public float[] GetRobotPosition(bool useSensors)
        {
            float[] arr = new float[3];
            IntPtr buffer = Marshal.AllocHGlobal(arr.Length * sizeof(float));
            ALMotionProxyGetRobotPosition(unmanagedMem, useSensors, buffer);

            Marshal.Copy(buffer, arr, 0, 3);
            Marshal.FreeHGlobal(buffer);
            
            return arr;
        }

        public void StopMove()
        {
            ALMotionProxyStopMove(
                unmanagedMem
            );
        }

        public void KillMove()
        {
            ALMotionProxyKillMove(
                unmanagedMem
            );
        }
    }
}
