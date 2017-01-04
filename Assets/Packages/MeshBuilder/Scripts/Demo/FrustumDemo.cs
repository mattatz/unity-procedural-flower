using UnityEngine;
using System.Collections;

namespace mattatz.MeshBuilderSystem {

    [ExecuteInEditMode]
    [RequireComponent (typeof(MeshFilter), typeof(MeshRenderer))]
    public class FrustumDemo : Demo {

        [SerializeField, Range(0.1f, 1f)] float nearClip = 0.1f;
        [SerializeField, Range(1f, 5f)] float farClip = 1f;
        [SerializeField, Range(45f, 90f)] float fieldOfView = 60f;
        [SerializeField, Range(0f, 1f)] float aspectRatio = 1f;

        protected override void Start () {
            base.Start();
            filter.mesh = FrustumBuilder.Build(Vector3.forward, Vector3.up, nearClip, farClip, fieldOfView, aspectRatio);
        }

    }

}


