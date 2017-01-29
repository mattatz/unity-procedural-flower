using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace mattatz.ProceduralFlower {

    public class PFCatmullRomSpline {

        public static int Loop(int index, int length) {
            if (index < 0) {
                index = length - 1;
            }

            if (index > length) {
                index = 1;
            } else if (index > length - 1) {
                index = 0;
            }

            return index;
        }

        public static List<Vector3> GetCatmullRomPositions(int count, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
            var points = new List<Vector3>();

            float inv = 1f / (count - 1);
            for (int i = 0; i < count; i++) {
                var p = GetCatmullRomPosition(i * inv, p0, p1, p2, p3);
                points.Add(p);
            }

            return points;
        }

        // Returns a position between 4 Vector3 with Catmull-Rom spline algorithm
        //http://www.iquilezles.org/www/articles/minispline/minispline.htm
        public static Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
            //The coefficients of the cubic polynomial (except the 0.5f * which I added later for performance)
            Vector3 a = 2f * p1;
            Vector3 b = p2 - p0;
            Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
            Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

            //The cubic polynomial: a + b * t + c * t^2 + d * t^3
            return 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));
        }
    }

}

