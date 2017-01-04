using UnityEngine;

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace mattatz.MeshBuilderSystem {

    [RequireComponent (typeof(MeshFilter))]
    public class Demo : MonoBehaviour {

        protected MeshFilter filter;
        protected Material lineMaterial;

        protected virtual void Start () {
            filter = GetComponent<MeshFilter>();
        }

        void OnRenderObject () {
            if (filter == null || filter.sharedMesh == null) return;

            var mesh = filter.sharedMesh;
            var vertices = mesh.vertices;
            var triangles = mesh.triangles;

            CheckInit();
            if(lineMaterial != null) {
                lineMaterial.SetPass(0);
            }

            GL.PushMatrix();
            GL.MultMatrix(transform.localToWorldMatrix);

            GL.Begin(GL.LINES);

            for(int i = 0, n = triangles.Length; i < n; i += 3) {
                var a = vertices[triangles[i]];
                var b = vertices[triangles[i + 1]];
                var c = vertices[triangles[i + 2]];
                GL.Vertex(a); GL.Vertex(b);
                GL.Vertex(b); GL.Vertex(c);
                GL.Vertex(c); GL.Vertex(a);
            }

            GL.End();

            GL.PopMatrix();
        }

        void CheckInit () {
            if(lineMaterial == null) {
                Shader shader = Shader.Find("mattatz/MeshBuilder/DebugLine");
                if (shader == null) return;
                lineMaterial = new Material(shader);
                lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
        }

    }

}

