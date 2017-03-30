namespace AL {
    // ALMotionProxy is used to communicate with a robot and perform
    // various motion actions.
    // 
    // For more information see
    // http://doc.aldebaran.com/2-1/naoqi/motion/control-cartesian-api.html
    public class ALMotionProxy {
        [DllImport("bridge_d.dll")]
        private static extern IntPtr ALMotionProxyNew(string server, int port);

        [DllImport("bridge_d.dll")]
        private static extern void ALMotionProxyAngleInterpolation(IntPtr self, IntPtr names, IntPtr angleList, IntPtr timeLists, bool isAbsolute);

        [DllImport("bridge_d.dll")]
        private static extern void ALMotionProxyAngleInterpolationWithSpeed(IntPtr self, IntPtr names, IntPtr targetAngles, float maxSpeedFraction);

        [DllImport("bridge_d.dll")]
        private static extern void ALMotionProxyAngleInterpolationBezier(IntPtr self, IntPtr jointNames, IntPtr times, IntPtr controlPoints);

        [DllImport("bridge_d.dll")]
        private static extern void ALMotionProxySetAngles(IntPtr self, IntPtr names, IntPtr angles, float fractionMaxSpeed);

        [DllImport("bridge_d.dll")]
        private static extern void ALMotionProxyChangeAngles(IntPtr self, IntPtr names, IntPtr changes, float fractionMaxSpeed);

        [DllImport("bridge_d.dll")]
        private static extern IntPtr ALMotionProxyGetAngles(IntPtr self, IntPtr names, bool useSensors);

        [DllImport("bridge_d.dll")]
        private static extern IntPtr ALMotionProxyCloseHand(IntPtr self, string handName);

        [DllImport("bridge_d.dll")]
        private static extern IntPtr ALMotionProxyOpenHand(IntPtr self, string handName);

        [DllImport("bridge_d.dll")]
        private static extern IntPtr AlMotionProxyFree(IntPtr self);

        private IntPtr unmanagedMem;

        public ALMotionProxy(string server, int port) {
            this.unmanagedMem = ALMotionProxyNew(server, port);
        }

        public void AngleInterpolation(string[] names, float[] angles, float[] timeLists, bool isAbsolute)
        {
            ALMotionProxyAngleInterpolation(
                unmanagedMem,
                new ALValue(names),
                new ALValue(angles),
                new ALValue(timeLists),
                isAbsolute
            );
        }

        public void AngleInterpolationWithSpeed(string[] names, float[] targetAngles, float maxSpeedFraction)
        {
            ALMotionProxyAngleInterpolationWithSpeed(
                unmanagedMem,
                new ALValue(names),
                new ALValue(targetAngles),
                maxSpeedFraction
            );
        }

        public void AngleInterpolationBezier(string[] jointNames, float[] times, float[] controlPoints)
        {
            ALMotionProxyAngleInterpolationBezier(
                unmanagedMem,
                new ALValue(jointNames),
                new ALValue(times),
                new ALValue(controlPoints)
            );
        }

        public void SetAngles(string[] names, float[] angles, float fractionMaxSpeed)
        {
            ALMotionProxySetAngles(
                unmanagedMem,
                new ALValue(names),
                new ALValue(angles),
                fractionMaxSpeed
            );
        }

        public void ChangeAngles(string[] names, float[] changes, float fractionMaxSpeed)
        {
            ALMotionProxyChangeAngles(
                unmanagedMem,
                new ALValue(names),
                new ALValue(changes),
                fractionMaxSpeed
            );
        }

        public ALValue GetAngles(string[] names, bool useSensors)
        {
            throw new NotImplementedError();
        }

        public void CloseHand(string handName)
        {
            ALMotionProxyCloseHand(handName);
        }

        public void OpenHand(string handName)
        {
            ALMotionProxyOpenHand(handName);
        }

        protected override void Finalize()
        {
            AlMotionProxyFree(unmanagedMem);
        }
    }
}
