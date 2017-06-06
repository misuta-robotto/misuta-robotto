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

namespace AL
{
    // ALValue is a container object used in the Aldebaran SDK.
    // Because of its container nature it does not contain any type
    // information. The C# implementation uses ALValue internally to
    // call the rest of the Aldebaran SDK which means that any consumer
    // of this library must not bother with this limitation.
    //
    // For more information see
    // http://doc.aldebaran.com/2-4/ref/libalvalue/classAL_1_1ALValue.html     
    public class ALValue
    {
        [DllImport("bridge_d")]
        private static extern IntPtr ALValue_f(float value);

        [DllImport("bridge_d")]
        private static extern IntPtr ALValue_s(string value);

        [DllImport("bridge_d")]
        private static extern IntPtr ALValue_fv(float[] values, int numValues);

        [DllImport("bridge_d")]
        private static extern IntPtr ALValue_sv(string[] value, int numValues);

        [DllImport("bridge_d")]
        private static extern void ALValueFree(IntPtr memory);

        public IntPtr Pointer
        {
            get; private set;
        }

        public ALValue(float value)
        {
            this.Pointer = ALValue_f(value);
        }

        public ALValue(string value)
        {
            this.Pointer = ALValue_s(value);
        }

        public ALValue(float[] value)
        {
            this.Pointer = ALValue_fv(value, value.Length);
        }

        public ALValue(string[] value)
        {
            this.Pointer = ALValue_sv(value, value.Length);
        }

        ~ALValue()
        {
            ALValueFree(Pointer);
        }
    }
}
