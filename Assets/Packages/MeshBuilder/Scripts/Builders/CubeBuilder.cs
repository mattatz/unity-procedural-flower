using UnityEngine;

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace mattatz.MeshBuilderSystem {

    public class CubeBuilder : MeshBuilder {

        public static Mesh Build (
            float width = 1f, float height = 1f, float depth = 1f,
            int widthSegments = 2, int heightSegments = 2, int depthSegments = 2
        ) {
            widthSegments = Mathf.Max(2, widthSegments);
            heightSegments = Mathf.Max(2, heightSegments);
            depthSegments = Mathf.Max(2, depthSegments);

            var mesh = new Mesh();

            var vertices = new List<Vector3>();
            var uvs = new List<Vector2>();
            var triangles = new List<int>();

            var hw = width * 0.5f;
            var hh = height * 0.5f;
            var hd = depth * 0.5f;

            // front
            CalculatePlane(
                vertices, uvs, triangles,
                Vector3.forward * -hd, Vector3.right * width, Vector3.up * height, widthSegments, heightSegments
            );

            // right
            CalculatePlane(
                vertices, uvs, triangles,
                Vector3.right * hw, Vector3.forward * depth, Vector3.up * height, depthSegments, heightSegments
            );

            // back
            CalculatePlane(
                vertices, uvs, triangles,
                Vector3.forward * hd, Vector3.left * width, Vector3.up * height, widthSegments, heightSegments
            );

            // left
            CalculatePlane(
                vertices, uvs, triangles,
                Vector3.right * -hw, Vector3.back * depth, Vector3.up * height, depthSegments, heightSegments
            );

            // top
            CalculatePlane(
                vertices, uvs, triangles,
                Vector3.up * hh, Vector3.right * width, Vector3.forward * depth, widthSegments, depthSegments
            );

            // bottom
            CalculatePlane(
                vertices, uvs, triangles,
                Vector3.up * -hh, Vector3.right * width, Vector3.back * depth, widthSegments, depthSegments
            );

            mesh.vertices = vertices.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.SetTriangles(triangles.ToArray(), 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }

        public static void CalculatePlane (
            List<Vector3> vertices, List<Vector2> uvs, List<int> triangles,
            Vector3 origin, Vector3 right, Vector3 up, int rSegments = 2, int uSegments = 2
        ) {
            float rInv = 1f / (rSegments - 1);
            float uInv = 1f / (uSegments - 1);

            int triangleOffset = vertices.Count;

            for(int y = 0; y < uSegments; y++) {
                float ru = y * uInv;
                for(int x = 0; x < rSegments; x++) {
                    float rr = x * rInv;
                    vertices.Add(origin + right * (rr - 0.5f) + up * (ru - 0.5f));
                    uvs.Add(new Vector2(rr, ru));
                }

                if(y < uSegments - 1) {
                    var offset = y * rSegments + triangleOffset;
                    for(int x = 0, n = rSegments - 1; x < n; x++) {
                        triangles.Add(offset + x);
                        triangles.Add(offset + x + rSegments);
                        triangles.Add(offset + x + 1);

                        triangles.Add(offset + x + 1);
                        triangles.Add(offset + x + rSegments);
                        triangles.Add(offset + x + 1 + rSegments);
                    }
                }
            }

        }

    }
}


