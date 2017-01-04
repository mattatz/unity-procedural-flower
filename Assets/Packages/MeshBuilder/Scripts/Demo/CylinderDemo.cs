using UnityEngine;
using System.Collections;

namespace mattatz.MeshBuilderSystem {

    [ExecuteInEditMode]
    [RequireComponent (typeof(MeshFilter), typeof(MeshRenderer))]
    public class CylinderDemo : Demo {

        [SerializeField] float radius = 1f;
        [SerializeField] float height = 4f;
        [SerializeField] int segments = 8;
        [SerializeField] bool openEnded = false;

        protected override void Start () {
            base.Start();
            filter.mesh = CylinderBuilder.Build(radius, height, segments, openEnded);
        }

    }

}


