using UnityEngine;

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace mattatz.MeshBuilderSystem {

    public class CylinderBuilder : MeshBuilder {

        public static Mesh Build (float radius, float height, int segments = 10, bool openEnded = true) {
            segments = Mathf.Max(3, segments);

            var mesh = new Mesh();

            var hh = height * 0.5f;
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            var uvs = new List<Vector2>();

            var len = segments * 2;
            var pi2 = Mathf.PI * 2f;

            for (int i = 0; i < segments; i++) {
                float ratio = (float)i / (segments - 1);
                float rad = ratio * pi2;
                var top = new Vector3(Mathf.Cos(rad) * radius, hh, Mathf.Sin(rad) * radius);
                var bottom = new Vector3(Mathf.Cos(rad) * radius, - hh, Mathf.Sin(rad) * radius);

                vertices.Add(top); uvs.Add(new Vector2(ratio, 1f));
                vertices.Add(bottom); uvs.Add(new Vector2(ratio, 0f));

                int idx = i * 2;
                int a = idx, b = idx + 1, c = (idx + 2) % len, d = (idx + 3) % len;
                triangles.Add(a);
                triangles.Add(c);
                triangles.Add(b);

                triangles.Add(c);
                triangles.Add(d);
                triangles.Add(b);
            }

            if(openEnded) {
                vertices.Add(new Vector3(0f, hh, 0f)); // top
                uvs.Add(new Vector2(0.5f, 1f));

                vertices.Add(new Vector3(0f, -hh, 0f)); // bottom
                uvs.Add(new Vector2(0.5f, 0f));

                var top = vertices.Count - 2;
                var bottom = vertices.Count - 1;

                int n = segments * 2;

                // top side
                for (int i = 0; i < n; i += 2) {
                    triangles.Add(top);
                    triangles.Add((i + 2) % n);
                    triangles.Add(i);
                }

                // bottom side
                for (int i = 1; i < n; i += 2) {
                    triangles.Add(bottom);
                    triangles.Add(i);
                    triangles.Add((i + 2) % n);
                }
            }

            mesh.vertices = vertices.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.SetTriangles(triangles.ToArray(), 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }

    }

}


