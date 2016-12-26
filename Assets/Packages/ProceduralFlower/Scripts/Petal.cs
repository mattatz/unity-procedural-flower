using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace mattatz.ProceduralFlower {

    public class Petal {

        public static Mesh Build (float length = 1f, float width = 0.5f, float height = 0.1f, float control = 0.75f, int numberOfVerticesOnOneSide = 20) {
            var mesh = new Mesh();

            var bottom = new Vector3(0f, 0f, 0f);

            var lc = length * Mathf.Clamp01(control);
            var left = new Vector3(-width * 0.5f, lc, 0f);
            var right = new Vector3(width * 0.5f, lc, 0f);

            var top = new Vector3(0f, length, 0f);

            var vertices = new List<Vector3>();
            var uv = new List<Vector2>();

            // construct midrib
            var inv = 1f / (numberOfVerticesOnOneSide - 1);
            for(int i = 0; i < numberOfVerticesOnOneSide; i++) {
                var r = i * inv;
                // vertices.Add(new Vector3(0f, length * i * inv, -height * (1f - Mathf.Abs(0.5f - r) / 0.5f)));
                vertices.Add(new Vector3(0f, length * i * inv, 0f));
                uv.Add(new Vector2(0.5f, r));
            }

            var count = Mathf.FloorToInt(numberOfVerticesOnOneSide * 0.5f);
            var bl = CatmullRomSpline.GetCatmullRomPositions(count, right, bottom, left, top);
            var lt = CatmullRomSpline.GetCatmullRomPositions(count, bottom, left, top, right);
            var tr = CatmullRomSpline.GetCatmullRomPositions(count, left, top, right, bottom);
            var rb = CatmullRomSpline.GetCatmullRomPositions(count, top, right, bottom, left);

            vertices.AddRange(bl);
            vertices.AddRange(lt);
            vertices.AddRange(tr);
            vertices.AddRange(rb);

            // bottom -> left -> top
            for (int i = 0; i < numberOfVerticesOnOneSide; i++) {
                var r = i * inv;
                uv.Add(new Vector2(0f, r));
            }
            
            // top -> right -> bottom
            for (int i = 0; i < numberOfVerticesOnOneSide; i++) {
                var r = i * inv;
                uv.Add(new Vector2(1f, 1f - r));
            }

            var triangles = new List<int>();

            for(int i = 0, n = numberOfVerticesOnOneSide - 1; i < n; i++) {
                var a = i;
                var b = i + 1;
                var c = i + numberOfVerticesOnOneSide;
                var d = i + numberOfVerticesOnOneSide + 1;

                triangles.Add(a); triangles.Add(c); triangles.Add(b);
                triangles.Add(b); triangles.Add(c); triangles.Add(d);
            }

            var offset = numberOfVerticesOnOneSide * 2;
            for(int i = 0, n = numberOfVerticesOnOneSide - 1; i < n; i++) {
                var a = numberOfVerticesOnOneSide - i - 1;
                var b = numberOfVerticesOnOneSide - i - 2;
                var c = i + offset;
                var d = i + offset + 1;

                triangles.Add(a); triangles.Add(c); triangles.Add(b);
                triangles.Add(b); triangles.Add(c); triangles.Add(d);
            }

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.uv = uv.ToArray();
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }

        List<Vector3> GetMidribPositions (float length, int count) {
            var midrib = new List<Vector3>();
            var inv = 1f / (count - 1);
            for(int i = 0; i < count; i++) {
                midrib.Add(new Vector3(0f, length * i * inv, 0f));
            }
            return midrib;
        }
        
    }

}


