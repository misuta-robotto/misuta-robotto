using System;
using System.Runtime.InteropServices;

namespace AL {
    public class ALValue {
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

        public ALValue(float value) {
            this.Pointer = ALValue_f(value);
        }

        public ALValue(string value) {
            this.Pointer = ALValue_s(value);
        }

        public ALValue(float[] value) {
            this.Pointer = ALValue_fv(value, value.Length);
        }

        public ALValue(string[] value) {
            this.Pointer = ALValue_sv(value, value.Length);
        }

        ~ALValue()
        {
            ALValueFree(Pointer);
        }
    }
}
