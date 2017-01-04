using UnityEngine;
using System.Collections;

namespace mattatz.MeshBuilderSystem {

    [ExecuteInEditMode]
    [RequireComponent (typeof(MeshFilter), typeof(MeshRenderer))]

    public class PlaneDemo : Demo {

        public enum PlaneType {
            Default, Noise
        };

        [SerializeField] PlaneType type = PlaneType.Default;
        [SerializeField, Range(0.5f, 10f)] float width = 1f;
        [SerializeField, Range(0.5f, 10f)] float height = 1f;
        [SerializeField, Range(2, 40)] int wSegments = 2;
        [SerializeField, Range(2, 40)] int hSegments = 2;

        protected override void Start () {
            base.Start();

            switch(type) {
                case PlaneType.Noise:
                    filter.mesh = PlaneBuilder.Build(new ParametricPlanePerlin(Vector2.zero, new Vector2(2f, 2f), 0.5f), width, height, wSegments, hSegments);
                    break;
                default:
                    filter.mesh = PlaneBuilder.Build(width, height, wSegments, hSegments);
                    break;
            }
        }

    }

}


