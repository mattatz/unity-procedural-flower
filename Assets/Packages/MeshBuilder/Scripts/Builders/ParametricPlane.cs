using UnityEngine;
using Random = UnityEngine.Random;

using System;
using System.Collections;

namespace mattatz.MeshBuilderSystem {

    public abstract class ParametricPlane {
        public abstract float Height(float ux, float uy);
    }

    public class ParametricPlaneDefault : ParametricPlane {
        public override float Height(float ux, float uy) {
            return 0f;
        }
    }

    public class ParametricPlaneRandom : ParametricPlane {

        float height;

        public ParametricPlaneRandom (float height = 1f) {
            this.height = height;
        }

        public override float Height(float ux, float uy) {
            return Random.value * height;
        }

    }

    public class ParametricPlanePerlin : ParametricPlane {

        Vector2 offset;
        Vector2 scale;
        float height;

        public ParametricPlanePerlin (Vector2 offset, Vector2 scale, float height = 1f) {
            this.offset = offset;
            this.scale = scale;
            this.height = height;
        }

        public override float Height(float ux, float uy) {
            return Mathf.PerlinNoise(offset.x + ux * scale.x, offset.y + uy * scale.y) * height;
        }
    }

}


