using UnityEngine;

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace mattatz.MeshBuilderSystem {

    // http://wiki.unity3d.com/index.php/ProceduralPrimitives#C.23_-_Sphere
    public class SphereBuilder : MeshBuilder {

        public static Mesh Build(float radius = 1f, int lonSegments = 24, int latSegments = 16) {
            var mesh = new Mesh();

            var vertices = new Vector3[(lonSegments + 1) * latSegments + 2];

            float pi2 = Mathf.PI * 2f;

            vertices[0] = Vector3.up * radius;
            for (int lat = 0; lat < latSegments; lat++) {
                float a1 = Mathf.PI * (float)(lat + 1) / (latSegments + 1);
                float sin = Mathf.Sin(a1);
                float cos = Mathf.Cos(a1);

                for (int lon = 0; lon <= lonSegments; lon++) {
                    float a2 = pi2 * (float)(lon == lonSegments ? 0 : lon) / lonSegments;
                    float sin2 = Mathf.Sin(a2);
                    float cos2 = Mathf.Cos(a2);
                    vertices[lon + lat * (lonSegments + 1) + 1] = new Vector3(sin * cos2, cos, sin * sin2) * radius;
                }
            }
            vertices[vertices.Length - 1] = Vector3.up * -radius;

            int len = vertices.Length;

            Vector2[] uvs = new Vector2[len];
            uvs[0] = Vector2.up;
            uvs[uvs.Length - 1] = Vector2.zero;
            for (int lat = 0; lat < latSegments; lat++) {
                for (int lon = 0; lon <= lonSegments; lon++) {
                    uvs[lon + lat * (lonSegments + 1) + 1] = new Vector2((float)lon / lonSegments, 1f - (float)(lat + 1) / (latSegments + 1));
                }
            }

            int[] triangles = new int[len * 2 * 3];

            // top cap
            int acc = 0;
            for (int lon = 0; lon < lonSegments; lon++) {
                triangles[acc++] = lon + 2;
                triangles[acc++] = lon + 1;
                triangles[acc++] = 0;
            }

            // middle
            for (int lat = 0; lat < latSegments - 1; lat++) {
                for (int lon = 0; lon < lonSegments; lon++) {
                    int current = lon + lat * (lonSegments + 1) + 1;
                    int next = current + lonSegments + 1;

                    triangles[acc++] = current;
                    triangles[acc++] = current + 1;
                    triangles[acc++] = next + 1;

                    triangles[acc++] = current;
                    triangles[acc++] = next + 1;
                    triangles[acc++] = next;
                }
            }

            // bottom cap
            for (int lon = 0; lon < lonSegments; lon++) {
                triangles[acc++] = len - 1;
                triangles[acc++] = len - (lon + 2) - 1;
                triangles[acc++] = len - (lon + 1) - 1;
            }

            mesh.vertices = vertices.ToArray();
            mesh.uv = uvs;
            mesh.triangles = triangles;

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }

    }

}

