using System;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;

namespace PepperKinematics {
    public class InverseKinematics {
        public float[] calcInvPos(float[] angles, Vector3 targetPos, Matrix4x4 targetOri, float epsilon, bool right) {
            var p = new Vector4(0, 0, 0, 1);
            var sumOld = 100000;

            while (true) {
                var posOriJ = ForwardKinematics.calcFkAndJacob(angles, true, right);
                var pos = posOriJ.Item1;
                var ori = posOriJ.Item2;
                var j = posOriJ.Item3;

                var invJ = j.PseudoInverse();
                var deltaPos = targetPos - pos;
                var v = invJ * deltaPos;

                angles += v; // angles = np.squeeze(np.asarray(v)) + angles

                var sum = Mathf.Abs(deltaPos.x) + Mathf.Abs(deltaPos.y) + Mathf.Abs(deltaPos.z);

                if (sum < epsilon) {
                    break;
                }

                if (sum > sumOld) {
                    throw new Exception("Distance does not converge");
                }

                sumOld = sum;
            }

            return angles;
        }
    }
}
