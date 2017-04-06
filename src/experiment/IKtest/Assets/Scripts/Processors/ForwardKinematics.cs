using System;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;

namespace PepperKinematics {
    public class ForwardKinematics {
        private static float L1 = 0.14974;
        private static float L2 = 0.15;
        private static float L3 = 0.1812;
        private static float L4 = 0;
        private static float L5 = 0.150;
        private static float L6 = 0.0695;
        private static float L7 = 0.0303;

        private static Vector4 p = new Vector4(0, 0, 0, 1);
        private static Vector4 v0 = new Vector4(1, 0, 0, 0);
        private static Vector4 v1 = new Vector4(0, 1, 0, 0);
        private static Vector4 v2 = new Vector4(0, 0, 1, 0);

        // Converts an array to a unity matrix
        private static Matrix4x4 arrayToMatrix(float[4,4] array) {
            Matrix4x4 m;
            for (int col = 0; col < 4; col++) {
                for (int row = 0; row < 4; row++) {
                    m[row, col] = array[row, col];
                }
            }
            return m;
        }

        private static Matrix4x4 topLeft3x3(Matrix4x4 mat) {
            var newMat = Matrix4x4.Identity;
            for (int col = 0; col < 3; col++) {
                for (int row = 0; row < 3; row++) {
                    newMat[row, col] = mat[row, col]
                }
            }
        }

        private static Matrix4x4 transX(th, x, y, z) {
            float s = Mathf.Sin(th);
            float c = Mathf.Cos(th);
            return arrayToMatrix(new[,] {
                {1, 0, 0, x}, {0, c, -s, y}, {0, s, c, z}, {0, 0, 0, 1}
            });
        }

        private static Matrix4x4 transY(th, x, y, z) {
            float s = Mathf.Sin(th);
            float c = Mathf.Cos(th);
            return arrayToMatrix(new[,] {
                {c, 0, -s, x}, {0, 1, 0, y}, {s, 0, c, z}, {0, 0, 0, 1}
            });
        }

        private static Matrix4x4 transZ(th, x, y, z) {
            float s = Mathf.Sin(th);
            float c = Mathf.Cos(th);
            return arrayToMatrix(new[,] {
                {c, -s, 0, x}, {s, c, 0, y}, {0, 0, 1, z}, {0, 0, 0, 1}
            });
        }

        public static Tuple<Vector3, Matrix4x4, Matrix<Float>> calcFkAndJacob(float[] angles, bool jacob, bool right) {
            var _L1_ = right ? -L1 : L1;
            var _L2_ = right ? -L2 : L2;

            var T1 = transY(-angles[0], 0, _L1_, 0);
            var T2 = transZ(angles[1], 0, 0, 0);
            var Td = transY(9.0f / 180.0f * Mathf.PI, L3, _L2_, 0);
            var T3 = transX(angles[2], 0, 0, 0);
            var T4 = transZ(angles[3], 0, 0, 0);
            var T5 = transX(angles[4], L5, 0, 0);
            var T6 = transZ(0, L6, 0, -L7);

            var T1Abs = T1;
            var T2Abs = T1Abs * T2;
            var TdAbs = T2Abs * Td;
            var T3Abs = TdAbs * T3;
            var T4Abs = T3Abs * T4;
            var T5Abs = T4Abs * T5;
            var T6Abs = T5Abs * T6;

            var pos = T6Abs * p;
            var ori = topLeft3x3(T6Abs);

            // TODO: find some way to only calculate jacobian when needed without
            // duplicating code
            // if (!jacob) {
            //     return pos, ori;
            // }

            var OfstT1 = L1 * T1Abs * v1;
            var OfstTd = TdAbs * new Vector4(L3, L2, 0, 0);
            var OfstT5 = L5 * T5Abs * v0;
            var OfstT6 = T5Abs * new Vector4(L6, 0, -L7, 0);

            var vec6 = OfstT6;
            var vec5 = vec6 + OfstT5;
            var vec4 = vec5;
            var vec3 = vec4;
            var vecd = vec3 + OfstTd;
            var vec2 = vecd;
            var vec1 = vec2 + OfstT1;

            var j1 = T1Abs * v1;
            var j2 = T2Abs * v2;
            var jd = TdAbs * v1;
            var j3 = T3Abs * v0;
            var j4 = T4Abs * v2;
            var j5 = T5Abs * v0;

            var J1 = cross(j1, vec1);
            var J2 = cross(j2, vec2);
            var J3 = cross(j3, vec3);
            var J4 = cross(j4, vec4);
            var J5 = cross(j5, vec5);

            var J = DenseMatrix.OfColumnArrays(new []{J1, J2, J3, J4, J5});
            return Tuple.Create(pos, ori, J);
        }

        private Vector3 cross(Vector4 j, Vector4 v) {
            var t0 = j[1] * v[2] - j[2] * v[1];
            var t1 = j[2] * v[0] - j[0] * v[2];
            var t2 = j[0] * v[1] - j[1] * v[0];
            return new Vector3(t0, t1, t2);
        }
    }
}
