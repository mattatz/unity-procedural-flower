using UnityEngine;
using System.Collections;

namespace mattatz.MeshBuilderSystem {

    [ExecuteInEditMode]
    [RequireComponent (typeof(MeshFilter))]
    public class VertexHelper : MonoBehaviour {

        [SerializeField] float size = 0.025f;
        Mesh mesh;

        void Start () {
            mesh = GetComponent<MeshFilter>().sharedMesh;
        }

        void OnDrawGizmos () {
            if (mesh == null) return;

            Gizmos.matrix = transform.localToWorldMatrix;

            var vertices = mesh.vertices;
            var uvs = mesh.uv;

			if(uvs.Length == vertices.Length) {
				for(int i = 0, n = vertices.Length; i < n; i++) {
	                var v = vertices[i];
	                var uv = uvs[i];
	                Gizmos.color = new Color(uv.x, uv.y, 0f);
	                Gizmos.DrawSphere(v, size);
	            }
			} else {
				for(int i = 0, n = vertices.Length; i < n; i++) {
	                var v = vertices[i];
					var r = (float)i / (n - 1);
	                Gizmos.color = new Color(r, 0f, 0f);
	                Gizmos.DrawSphere(v, size);
	            }
			}
        }

    }

}

