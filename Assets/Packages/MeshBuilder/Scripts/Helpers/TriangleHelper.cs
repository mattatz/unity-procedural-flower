using UnityEngine;
using System.Collections;

namespace mattatz.MeshBuilderSystem {

    [ExecuteInEditMode]
    [RequireComponent (typeof(MeshFilter))]
    public class TriangleHelper : MonoBehaviour {

        Mesh mesh;

        void Start () {
            mesh = GetComponent<MeshFilter>().sharedMesh;
        }

        void OnDrawGizmos () {
            if (mesh == null) return;

            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.white;

            var vertices = mesh.vertices;
            var triangles = mesh.triangles;

            for(int i = 0, n = triangles.Length; i < n; i += 3) {
                var a = vertices[triangles[i]];
                var b = vertices[triangles[i + 1]];
                var c = vertices[triangles[i + 2]];
                Gizmos.DrawLine(a, b);
                Gizmos.DrawLine(b, c);
                Gizmos.DrawLine(c, a);
            }
        }

    }

}

