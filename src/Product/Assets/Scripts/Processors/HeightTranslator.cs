using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets {
    class HeightTranslator {
        private const float VRNODE_TO_REAL_HEIGHT_RATIO = 1.13f;
        private const float SHOE_SOUL_HEIGHT = 0.02f;
        public const float KYLE_HEIGHT = 1.8f;


        public static float CalculateHeight(Vector3 head) {
            float userHeight = (head.y - SHOE_SOUL_HEIGHT) * VRNODE_TO_REAL_HEIGHT_RATIO;
            return userHeight;
        }

        public static float CalculateSizeRatio(float userHeight) {
            float sizeRatio = userHeight / KYLE_HEIGHT;
            return sizeRatio;
        }

    }
}
