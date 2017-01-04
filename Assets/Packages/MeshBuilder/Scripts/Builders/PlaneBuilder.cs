using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace mattatz.MeshBuilderSystem {

    public class PlaneBuilder : MeshBuilder {

        public static Mesh Build (float width = 1f, float height = 1f, int wSegments = 1, int hSegments = 1) {
            return Build(new ParametricPlaneDefault(), width, height, wSegments, hSegments);
        }

        public static Mesh Build(ParametricPlane param, float width = 1f, float height = 1f, int wSegments = 1, int hSegments = 1) {
            wSegments = Mathf.Max(1, wSegments);
            hSegments = Mathf.Max(1, hSegments);

            var mesh = new Mesh();

            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            var uvs = new List<Vector2>();

            float hinv = 1f / (hSegments - 1);
            float winv = 1f / (wSegments - 1);
            for(int y = 0; y < hSegments; y++) {
                float ry = y * hinv;
                for(int x = 0; x < wSegments; x++) {
                    float rx = x * winv;
                    vertices.Add(new Vector3((rx - 0.5f) * width, (ry - 0.5f) * height, param.Height(rx, ry)));
                    uvs.Add(new Vector2(rx, ry));
                }

                if(y < hSegments - 1) {
                    var offset = y * wSegments;
                    for(int x = 0, n = wSegments - 1; x < n; x++) {
                        triangles.Add(offset + x);
                        triangles.Add(offset + x + wSegments);
                        triangles.Add(offset + x + 1);

                        triangles.Add(offset + x + 1);
                        triangles.Add(offset + x + wSegments);
                        triangles.Add(offset + x + 1 + wSegments);
                    }
                }
            }

            mesh.vertices = vertices.ToArray();
            mesh.SetTriangles(triangles.ToArray(), 0);
            mesh.uv = uvs.ToArray();

            return mesh;
        }

    }

}


