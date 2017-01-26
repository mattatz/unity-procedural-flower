using UnityEngine;

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace mattatz.ProceduralFlower {

    public class ProceduralFlower : MonoBehaviour {

		[SerializeField] ShapeData petalData;
		[SerializeField] ShapeData leafData;
		[SerializeField] StemData stemData;

        #region Flower Settings

        // [SerializeField, Range(137.4f, 137.6f)] float alpha = 137.5f;
        [HideInInspector] public float c = 0.1f;
        [HideInInspector] public int n = 75;
        [HideInInspector] public float scale = 1f;
        [HideInInspector] public float min = 0.1f;
        [HideInInspector] public float angle = 60f;
        [HideInInspector] public float angleScale = 1f;
        [HideInInspector] public float offset = 0.25f;

        #endregion

        [HideInInspector] public float height = 2f;
		[HideInInspector] public int leafCount = 3;
        [HideInInspector] public Vector2 leafRange = new Vector2(0.2f, 0.92f);

        #region Random

        [SerializeField] int seed = 0;
        Rand rand;

        #endregion

        [SerializeField] List<GameObject> children;

		[System.Serializable]
		class ShapeData {
			[SerializeField] Shape shape;
			public Material material;
			[HideInInspector] public Mesh mesh;

			public void Init () {
				mesh = shape.Build();
			}
		}

		[System.Serializable]
		class StemData {
			[HideInInspector] public Stem stem;

			public Material material;
			[SerializeField] int wresolution = 10;
			[SerializeField] int hresolution = 6;
			[SerializeField] float radius = 0.02f;
			public float bend = 0.05f;

			public void Init () {
				stem = new Stem(wresolution, hresolution, radius);
			}
		}

        void Start () {
			Build();
        }

		public void Build () {
            rand = new Rand(seed);

			if(children == null) {
				children = new List<GameObject>();
			} else {
				children.ForEach(child => {
					if(Application.isPlaying) {
						Destroy(child);
					} else {
						DestroyImmediate(child);
					}
				});
				children.Clear();
			}

			petalData.Init();
			leafData.Init();
			stemData.Init();

			var stem = CreateStem(stemData.stem, (float r) => 1f, height, stemData.bend);
			children.Add(stem);

			var segments = stemData.stem.Segments;
			var offset = leafRange.x * segments.Count;
            var len = (leafRange.y - leafRange.x) * segments.Count;
            var size = 1f;

            for (int i = 0; i < leafCount; i++) {
                var r = (float)(i + 1) / (leafCount + 1);
                int index = Mathf.Min(Mathf.FloorToInt(len * r + offset), segments.Count - 2);
				var from = segments[index];
				var to = segments[index + 1];
				var dir = (to.position - from.position).normalized;
				var leaf = CreateLeaf(segments[index], dir, (i % 4) * 90f + rand.SampleRange(-20f, 20f));

                // lower leaf becomes bigger than upper one.
                size = rand.SampleRange(size, 1f - (r * 0.5f));
                leaf.transform.localScale *= size;

				children.Add(leaf);
			}

			var flower = CreateFlower();
			flower.transform.localPosition = stemData.stem.Tip.position;
			flower.transform.localRotation *= stemData.stem.Tip.rotation * Quaternion.FromToRotation(Vector3.back, Vector3.up);

			children.Add(flower);
		}

		GameObject Create(ShapeData data, string name) {
			var go = new GameObject(name);
			go.AddComponent<MeshFilter>().mesh = data.mesh;
			go.AddComponent<MeshRenderer>().material = data.material;
			return go;
		}

		GameObject CreateFlower () {
            var floret = new Florets();
			var source = Create(petalData, "Petal");

			var flower = new GameObject("Flower");
			flower.transform.SetParent(transform, false);

            var inv = 1f / n;
            for(int i = 0; i < n; i++) {
                var r = i * inv;

                var p = floret.Get(i + 1, c);

				var go = Instantiate(source);
                go.transform.SetParent(flower.transform, false);
                go.transform.localScale = Vector3.one * Mathf.Max(min, p.magnitude) * scale;
                go.transform.localPosition = p + Vector3.up * (1f - r) * offset;
                go.transform.localRotation = Quaternion.LookRotation(Vector3.up, p.normalized) * Quaternion.AngleAxis((1f - r * angleScale) * angle, Vector3.right);
            }

			if(Application.isPlaying) {
				Destroy(source);
			} else {
				DestroyImmediate(source);
			}

			return flower;
		}

		GameObject CreateStem(Stem stem, Func<float, float> f, float height, float bend) {
			var controls = GetControls(4, height, bend);
			var mesh = stem.Build(controls, f);
			var go = new GameObject("Stem");
			go.transform.SetParent(transform, false);
			go.AddComponent<MeshFilter>().sharedMesh = mesh;
			go.AddComponent<MeshRenderer>().sharedMaterial = stemData.material;
			return go;
		}

		GameObject CreateLeaf (Point segment, Vector3 dir, float angle) {
			var stem = new Stem(10, 2, 0.01f);
			var go = CreateStem(stem, (r) => Mathf.Max(1f - r, 0.2f), 0.05f, 0.0f);
			go.transform.localPosition = segment.position;
			go.transform.localRotation *= Quaternion.FromToRotation(Vector3.forward, dir) * Quaternion.AngleAxis(angle, Vector3.forward);

			var leaf = Create(leafData, "Leaf");
			leaf.transform.SetParent(go.transform, false);
			leaf.transform.localPosition = stem.Tip.position;
			leaf.transform.localRotation *= Quaternion.AngleAxis(rand.SampleRange(0f, 30f), Vector3.up);

			return go;
		}

		List<Vector3> GetControls (int count, float height, float radius) {
			var controls = new List<Vector3>();
			count = Mathf.Max(4, count);
			for(int i = 0; i < count; i++) {
				var r = (float)i / (count - 1);
                var circle = rand.SampleUnitCircle() * radius;
				controls.Add(new Vector3(circle.x, r * height, circle.y));
			}
			return controls;
		}

    }

    public class Florets {

        const float ANGLE = 137.5f * Mathf.Deg2Rad;

        // Vogel's formula : Generate patterns of florets 
        // n is the ordering number of a floret, counting outward from the center.
        // phi is the angle between a reference direction and the position vector of the nth floret in a polar coordinate system originating at the center of the capitulum.
        // r is the distance between the center of the capitulum and the center of the nth floret, given a constant scaling parameter c.
        public Vector3 Get (int n, float c, float alpha = 137.5f) {
            // var phi = n * ANGLE;
            var phi = n * alpha * Mathf.Deg2Rad;
            var r = c * Mathf.Sqrt(n);
            return new Vector3(Mathf.Cos(phi) * r, 0f, Mathf.Sin(phi) * r);
        }

    }

}


