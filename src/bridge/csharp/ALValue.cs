namespace AL {
    // ALValue is a container object used in the Aldebaran SDK.
    // Because of its container nature it does not contain any type
    // information. The C# implementation uses ALValue internally to
    // call the rest of the Aldebaran SDK which means that any consumer
    // of this library must not bother with this limitation.
    //
    // For more information see
    // http://doc.aldebaran.com/2-4/ref/libalvalue/classAL_1_1ALValue.html     
    public class ALValue {
        [DllImport("bridge_d.dll")]
        private static extern IntPtr ALValue_f(float value);

        [DllImport("bridge_d.dll")]
        private static extern IntPtr ALValue_s(string value);

        [DllImport("bridge_d.dll")]
        private static extern IntPtr ALValue_fv(float[] value);

        [DllImport("bridge_d.dll")]
        private static extern IntPtr ALValue_sv(string[] value);

        [DllImport("bridge_d.dll")]
        private static extern void ALValueFree(IntPtr memory);

        private IntPtr unmanagedMem;

        public ALValue(float value) {
            this.unmanagedMem = ALValue_f(value);
        }

        public ALValue(string value) {
            this.unmanagedMem = ALValue_s(value);
        }

        public ALValue(float[] value) {
            this.unmanagedMem = ALValue_fv(value);
        }

        public ALValue(string[] value) {
            this.unmanagedMem = ALValue_sv(value);
        }

        protected override void Finalize()
        {
            ALValueFree(unmanagedMem);
        }
    }
}
