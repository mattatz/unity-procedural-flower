using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Random = UnityEngine.Random;

namespace mattatz.ProceduralFlower {

	public class Point {
		public Vector3 position;
		public Quaternion rotation;
		public Point (Vector3 p, Quaternion r) {
			position = p;
			rotation = r;
		}
	}

	public class PFStem {

		public Point Tip { get { return tip; } }
		public List<Point> Segments { get { return segments; } }

		public int wresolution = 10;
		public int hresolution = 4;
		public float radius = 0.05f;

		Point tip;
		List<Point> segments;

		public PFStem () {}

		public PFStem (int wresol, int hresol, float r) {
			wresolution = wresol;
			hresolution = hresol;
			radius = r;
		}

		public Mesh Build (List<Vector3> controls) {
			return Build(controls, wresolution, hresolution, radius, (float r) => 1f);
		}

		public Mesh Build (List<Vector3> controls, Func<float, float> f) {
			return Build(controls, wresolution, hresolution, radius, f);
		}

		Mesh Build (List<Vector3> controls, int wresolution, int hresolution, float radius, Func<float, float> f) {

			controls = controls.ToList();
			if(controls.Count < 4) {
				throw new UnityException("control size is not enough");
			}

			Vector3 first = controls[0], second = controls[1];
			Vector3 blast = controls[controls.Count - 2], last = controls[controls.Count - 1];

			controls.Insert(0, first + (first - second).normalized * 0.25f);
			controls.Add(last + (last - blast).normalized * 0.25f);

			var cores = new List<Vector3>();
			for(int i = 1, n = controls.Count - 2; i < n; i++) {
				var tmp = PFCatmullRomSpline.GetCatmullRomPositions(hresolution, controls[i - 1], controls[i], controls[i + 1], controls[i + 2]);
				if(i != 1) {
					tmp.RemoveAt(0);
				}
				cores.AddRange(tmp);
			}

			var vertices = new List<Vector3>();
			var uv = new List<Vector2>();
			var triangles = new List<int>();

			var mesh = new Mesh();

			var circle = new List<Vector3>();
			for(int i = 0; i < wresolution; i++) {
				var r = ((float)i / wresolution) * (Mathf.PI * 2f);
				var cos = Mathf.Cos(r) * radius;
				var sin = Mathf.Sin(r) * radius;
				circle.Add(new Vector3(cos, sin, 0f));
			}

			vertices.Add(cores.First());
			Vector3 right = Vector3.right;
			bool inverse = false;

			segments = new List<Point>();

			for(int i = 0, n = cores.Count; i < n; i++) {
				var core = cores[i];
				var v = (float)i / (n - 1);

				if(i == 0) {
					// first segment
					for(int j = 0; j < wresolution; j++) {
						triangles.Add(0); triangles.Add((j + 1) % wresolution + 1); triangles.Add(j + 1);
					}

					var next = cores[i + 1];
					var dir = (next - core).normalized;
					right = (Quaternion.LookRotation(dir) * Vector3.right).normalized;

					uv.Add(new Vector2(0.5f, 0f));
				}

				var offset = i * wresolution + 1;

				if(i < n - 1) {
					var next = cores[i + 1];
					var dir = (next - core).normalized;
					var cr = (Quaternion.LookRotation(dir) * Vector3.right).normalized;
					if(Vector3.Dot(right, cr) < 0f) inverse = !inverse;

					var rotation = AddCircle(vertices, core, circle, f(v), dir, inverse);
					segments.Add(new Point(core, rotation));

					right = cr;

					for(int j = 0; j < wresolution; j++) {
						var a = offset + j;
						var b = offset + (j + 1) % wresolution;
						var c = a + wresolution;
						var d = b + wresolution;
						triangles.Add(a); triangles.Add(b); triangles.Add(c);
						triangles.Add(c); triangles.Add(b); triangles.Add(d);

						var u = (float)j / (wresolution - 1);
						uv.Add(new Vector2(u, v));
					}

				} else {
					// last segment
					var prev = cores[i - 1];
					var dir = (core - prev).normalized;
					var cr = (Quaternion.LookRotation(dir) * Vector3.right).normalized;
					if(Vector3.Dot(right, cr) < 0f) inverse = !inverse;

					var rotation = AddCircle(vertices, core, circle, f(v), dir, inverse);
					tip = new Point(core, rotation);

					vertices.Add(cores.Last());
					uv.Add(new Vector2(0.5f, 1f));
					int m = vertices.Count - 1;

					for(int j = 0; j < wresolution; j++) {
						triangles.Add(m); 
						triangles.Add(offset + j);
						triangles.Add(offset + ((j + 1) % wresolution)); 

						var u = (float)j / (wresolution - 1);
						uv.Add(new Vector2(u, v));
					}
				}

			}

			mesh.vertices = vertices.ToArray();
			mesh.uv = uv.ToArray();
			mesh.triangles = triangles.ToArray();

			mesh.RecalculateBounds();
			mesh.RecalculateNormals();

			return mesh;
		}

		static Quaternion AddCircle (List<Vector3> vertices, Vector3 core, List<Vector3> circle, float radius, Vector3 dir, bool inverse) {
			Quaternion q;
			if(inverse) {
				q = Quaternion.AngleAxis(180f, dir) * Quaternion.LookRotation(dir);
				circle.ForEach(c => {
					vertices.Add(core + q * (c * radius));
				});
			} else {
				q = Quaternion.LookRotation(dir);
				circle.ForEach(c => {
					vertices.Add(core + q * (c * radius));
				});
			}
			return q;
		}

	}
		
}

