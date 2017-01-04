using UnityEngine;
using System.Collections;

namespace mattatz.MeshBuilderSystem {

    [ExecuteInEditMode]
    [RequireComponent (typeof(MeshFilter), typeof(MeshRenderer))]
    public class CubeDemo : Demo {

        [SerializeField, Range(0.5f, 10f)] float width = 1f;
        [SerializeField, Range(0.5f, 10f)] float height = 1f;
        [SerializeField, Range(0.5f, 10f)] float depth = 1f;
        [SerializeField, Range(2, 20)] int widthSegments = 2;
        [SerializeField, Range(2, 20)] int heightSegments = 2;
        [SerializeField, Range(2, 20)] int depthSegments = 2;

        protected override void Start () {
            base.Start();

            filter.mesh = CubeBuilder.Build(
                width, height, depth,
                widthSegments, heightSegments, depthSegments
            );
        }

    }

}


