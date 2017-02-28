namespace AL {
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
