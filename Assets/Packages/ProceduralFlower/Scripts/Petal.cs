using UnityEngine;

using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace mattatz.ProceduralFlower {

	public class ControlPoint {
		public float width;
		public float height;

		public ControlPoint (float width, float height) {
			this.width = Mathf.Clamp01(width);
			this.height = Mathf.Clamp01(height);
		}
	}

    public class Petal {

		public float height = 1f;
		public float width = 0.5f;
		public List<ControlPoint> controls;
		public int numberOfVerticesOnOneSide = 20;
		public int resolution = 2;
		public float depth = 0.25f;
		public Vector2 scale = Vector2.zero;

		public Mesh Build () {
			return Build(height, width, controls, numberOfVerticesOnOneSide, resolution, depth, scale);
		}

		public static Mesh Build (float height, float width, List<ControlPoint> controls, int numberOfVerticesOnOneSide = 20, int resolution = 2, float depth = 0.25f, Vector2 scale = default(Vector2)) {
            var mesh = new Mesh();

			resolution = Mathf.Max(1, resolution);

			/*
			var controls = new List<ControlPoint>();
			controls.Add(new ControlPoint(0.35f, 0.35f));
			controls.Add(new ControlPoint(0.75f, 0.80f));
			*/

			if(controls.Count < 1) {
				new UnityException("control points size is not enough");
			}

			int controlSize = controls.Count;

			var counts = GetCounts(controls, numberOfVerticesOnOneSide);
			int countOnEdge = counts.Aggregate((acc, next) => {
				return acc + next;
			});

            var bottom = new Vector3(0f, 0f, 0f);
            var top = new Vector3(0f, height, 0f);

			var points = new List<Vector3>();
			points.Add(bottom);
			points.AddRange(controls.Select(p => new Vector3(p.width * width, p.height * height, 0f)));
			points.Add(top);

			controls.Reverse();
			points.AddRange(controls.Select(p => new Vector3(-p.width * width, p.height * height, 0f)));

			var vertices = new List<Vector3>();
			var uv = new List<Vector2>();

			// left side
			for(int i = 0; i < resolution; i++) {
				var r = (float)(resolution - i) / resolution;
				for(int c = 0; c <= controlSize; c++) {
	            	var edge = CatmullRomSpline.GetCatmullRomPositions(
						counts[c], 
						GetEdge(GetLoopPoint(points, c - 1), r),
						GetEdge(GetLoopPoint(points, c), r),
						GetEdge(GetLoopPoint(points, c + 1), r),
						GetEdge(GetLoopPoint(points, c + 2), r)
					);
					vertices.AddRange(edge);
					uv.AddRange(edge.Select(v => {
						return new Vector2(v.x / width, v.y / height);
					}));
				}
			}

			// midrib 
			for(int c = 0; c <= controlSize; c++) {
				var edge = CatmullRomSpline.GetCatmullRomPositions(
					counts[c], 
					GetEdge(GetLoopPoint(points, c - 1), 0f),
					GetEdge(GetLoopPoint(points, c), 0f),
					GetEdge(GetLoopPoint(points, c + 1), 0f),
					GetEdge(GetLoopPoint(points, c + 2), 0f)
				);
				vertices.AddRange(edge);
				uv.AddRange(edge.Select(v => {
					return new Vector2(0f, v.y / height);
				}));
			}

			// right side
			for(int i = 0; i < resolution; i++) {
				var r = -(float)(i + 1) / resolution;
				for(int c = 0; c <= controlSize; c++) {
	            	var edge = CatmullRomSpline.GetCatmullRomPositions(
						counts[c], 
						GetEdge(GetLoopPoint(points, c - 1), r),
						GetEdge(GetLoopPoint(points, c), r),
						GetEdge(GetLoopPoint(points, c + 1), r),
						GetEdge(GetLoopPoint(points, c + 2), r)
					);
					vertices.AddRange(edge);
					uv.AddRange(edge.Select(v => {
						return new Vector2(v.x / width, v.y / height);
					}));
				}
			}

			var triangles = new List<int>();

			// left side faces
			for(int i = 0, n = resolution; i < n; i++) {
				var offset = i * countOnEdge;
				for(int j = 0; j < countOnEdge - 1; j++) {
					var a = j + offset;
					var b = a + 1;
					var c = a + countOnEdge;
					var d = b + countOnEdge;
					triangles.Add(a); triangles.Add(b); triangles.Add(c);
					triangles.Add(b); triangles.Add(d); triangles.Add(c);
				}
			}

			// right side faces
			for(int i = 0, n = resolution; i < n; i++) {
				var offset = (i + resolution) * countOnEdge;
				for(int j = 0; j < countOnEdge - 1; j++) {
					var a = j + offset;
					var b = a + 1;
					var c = a + countOnEdge;
					var d = b + countOnEdge;
					triangles.Add(a); triangles.Add(d); triangles.Add(c);
					triangles.Add(b); triangles.Add(d); triangles.Add(a);
				}
			}

			var noiseOffset = new Vector2(width, height);
			mesh.vertices = vertices.Select(v => {
				return new Vector3(v.x, v.y, depth * Depth(v, noiseOffset, scale));
			}).ToArray();
			mesh.uv = uv.ToArray();
			mesh.triangles = triangles.ToArray();
			mesh.RecalculateBounds();
			mesh.RecalculateNormals();

			return mesh;
		}

		static int[] GetCounts (List<ControlPoint> controls, int count) {
			var controlSize = controls.Count;

			var points = controls.Select(c => new Vector2(c.width, c.height)).ToList();
			var counts = new float[controlSize + 1];

			var acc = 0f;
			var lengthes = new float[controlSize + 1];
			for(int i = 0, n = points.Count; i <= n; i++) {
				var l = 0f;
				if(i == 0) {
					l = points[0].magnitude;
				} else if(i < n) {
					l = (points[i] - points[i - 1]).magnitude;
				} else {
					l = (new Vector2(0f, 1f) - points[i - 1]).magnitude;
				}
				acc += l;
				lengthes[i] = l;
			}

			return lengthes.Select(l => Mathf.FloorToInt((l / acc) * count)).ToArray();
		}

		static Vector2 GetEdge (Vector2 p, float ratio) {
			return new Vector2(p.x * ratio, p.y);
		}

		static Vector2 GetLoopPoint (List<Vector3> points, int index) {
			if(index < 0) {
				return GetLoopPoint(points, points.Count + index);
			} else if(index >= points.Count) {
				return GetLoopPoint(points, index - points.Count);
			}
			return points[index];
		}

		static float Depth (Vector3 v, Vector2 offset, Vector2 scale) {
			return Mathf.PerlinNoise((v.x + offset.x) * scale.x, (v.y + offset.y) * scale.y);
		}

    }

}


