using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcoSimUtil {
    
    public class Constants
    {
        public float MAX_SPEED = 12;
        public float MAX_RANGE = 15;
        public float MAX_ANGLE = 120; // in degrees

        public float MIN_SPEED = 5;
        public float MIN_RANGE = 5;
        public float MIN_ANGLE = 30;

        public static Vector3 dirFromAngle(float degrees) {
            return new Vector3(Mathf.Sin(degrees * Mathf.Deg2Rad), 0, Mathf.Cos(degrees * Mathf.Deg2Rad));
        }
    }
}
