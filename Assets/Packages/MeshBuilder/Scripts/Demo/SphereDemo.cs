using UnityEngine;
using System.Collections;

namespace mattatz.MeshBuilderSystem {

    [ExecuteInEditMode]
    [RequireComponent (typeof(MeshFilter), typeof(MeshRenderer))]
    public class SphereDemo : Demo {

        [SerializeField, Range(0.5f, 10f)] float radius = 1f;
        [SerializeField, Range(8, 20)] int lonSegments = 10;
        [SerializeField, Range(8, 20)] int latSegments = 10;

        protected override void Start () {
            base.Start();
            filter.mesh = SphereBuilder.Build(radius, lonSegments, latSegments);
        }

    }

}


