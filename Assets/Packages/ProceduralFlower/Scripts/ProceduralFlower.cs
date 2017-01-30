using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace mattatz.ProceduralFlower {

	[CreateAssetMenu(menuName = "ProceduralFlower/Flower")]
    public class ProceduralFlower : ScriptableObject {

		[SerializeField] ShapeData budData;
		[SerializeField] ShapeData petalData;
		[SerializeField] ShapeData leafData;
		[SerializeField] StemData stemData;

        #region Flower Settings

        // [SerializeField, Range(137.4f, 137.6f)] float alpha = 137.5f;
        [HideInInspector] public float c = 0.01f;
        [HideInInspector] public int n = 70;
        [HideInInspector] public int m = 8;
        [HideInInspector] public float scale = 0.328f;
        [HideInInspector] public float min = 0.1f;
        [HideInInspector] public float angle = 87f;
        [HideInInspector] public float angleScale = 0.92f;
        [HideInInspector] public float offset = 0f;

        #endregion

        [HideInInspector] public float height = 2f;
		[HideInInspector] public int leafCount = 6;
        [HideInInspector] public Vector2 leafScaleRange = new Vector2(0.2f, 0.825f);
        [HideInInspector] public Vector2 leafSegmentRange = new Vector2(0.2f, 0.92f);

        #region Random

        [SerializeField] int seed = 0;
        PFRandom rand;

        #endregion

		public GameObject Build (bool visible = true) {
            rand = new PFRandom(seed);

			budData.Init();
			petalData.Init();
			leafData.Init();
			stemData.Init();

			var root = CreateStem("Root", stemData.stem, stemData.shadowCastingMode, stemData.receiveShadows, (float r) => 1f, height, stemData.bend, visible);
			var stemPart = root.GetComponent<PFPart>();

			var segments = stemData.stem.Segments;
			var offset = leafSegmentRange.x * segments.Count;
            var len = (leafSegmentRange.y - leafSegmentRange.x) * segments.Count;
            var size = 1f;

            for (int i = 0; i < leafCount; i++) {
                var r = (float)(i + 1) / (leafCount + 1);
                int index = Mathf.Min(Mathf.FloorToInt(len * r + offset), segments.Count - 2);
				var from = segments[index];
				var to = segments[index + 1];
				var dir = (to.position - from.position).normalized;
				var leaf = CreateLeaf(segments[index], dir, (i % 4) * 90f + rand.SampleRange(-20f, 20f), visible);
				leaf.transform.SetParent(root.transform);

                // lower leaf becomes bigger than upper one.
                size = rand.SampleRange(size, 1f - (r * 0.5f));
				leaf.transform.localScale *= Mathf.Lerp(leafScaleRange.x, leafScaleRange.y, size);

				var leafPart = leaf.GetComponent<PFPart>();
				stemPart.Add(leafPart, r);
			}

			var flower = CreateFlower(visible);
			flower.transform.SetParent(root.transform);
			flower.transform.localPosition = stemData.stem.Tip.position;
			flower.transform.localRotation *= stemData.stem.Tip.rotation * Quaternion.FromToRotation(Vector3.back, Vector3.up);

			stemPart.Add(flower.GetComponent<PFPart>(), 1f);

			return root;
		}

		GameObject CreateFlower (bool visible) {
            var floret = new Florets();

			var bud = CreateShape("Bud", PFPartType.Petal, budData, visible);
			var petal = CreateShape("Petal", PFPartType.Petal, petalData, visible);

			var flower = new GameObject("Flower");
			var root = flower.AddComponent<PFPart>();
            root.SetType(PFPartType.None);

            var inv = 1f / n;
            for(int i = 0; i < n; i++) {
                var r = i * inv;

                var p = floret.Get(i + 1, c);

				GameObject go;

				if(i < m) {
					go = Instantiate(bud);
                    go.transform.localScale = Vector3.one * (1f + Mathf.Max(min, p.magnitude)) * scale * 0.75f;
                } else {
					go = Instantiate(petal);
                    go.transform.localScale = Vector3.one * (1f + Mathf.Max(min, p.magnitude)) * scale;
                }

				var part = go.GetComponent<PFPart>();
				part.Colorize(new Color(rand.SampleRange(0.5f, 1f), rand.SampleRange(0.5f, 1f), rand.SampleRange(0.5f, 1f)));
				part.Bend(1f - r);
				part.Fade(visible ? 1f + part.EPSILON : 0f);

	            go.transform.SetParent(flower.transform, false);

                go.transform.localPosition = p + Vector3.down * r * offset;
                go.transform.localRotation = Quaternion.LookRotation(Vector3.up, p.normalized) * Quaternion.AngleAxis((1f - r * angleScale) * angle, Vector3.right);

				root.Add(go.GetComponent<PFPart>(), r);
            }

			if(Application.isPlaying) {
				Destroy(bud);
				Destroy(petal);
			} else {
				DestroyImmediate(bud);
				DestroyImmediate(petal);
			}

			return flower;
		}

		GameObject CreateBase (string name, PFPartType type, Mesh mesh, Material material, ShadowCastingMode shadowCastingMode, bool receiveShadows, bool visible) {
			var go = new GameObject(name);
			go.AddComponent<MeshFilter>().mesh = mesh;

			var rnd = go.AddComponent<MeshRenderer>();
			rnd.sharedMaterial = material;
			rnd.shadowCastingMode = shadowCastingMode;
			rnd.receiveShadows = receiveShadows;

			var part = go.AddComponent<PFPart>();
            part.SetType(type);
			part.Fade(visible ? 1f + part.EPSILON : 0f);

			return go;
		}

		GameObject CreateShape(string name, PFPartType type, ShapeData data, bool visible) {
			return CreateBase(name, type, data.mesh, data.material, data.shadowCastingMode, data.receiveShadows, visible);
		}

		GameObject CreateStem(string name, PFStem stem, ShadowCastingMode shadowCastingMode, bool receiveShadows, Func<float, float> f, float height, float bend, bool visible) {
			var controls = GetControls(4, height, bend);
			var mesh = stem.Build(controls, f);
			return CreateBase(name, PFPartType.Stover, mesh, stemData.material, stemData.shadowCastingMode, stemData.receiveShadows, visible);
		}

		GameObject CreateLeaf (Point segment, Vector3 dir, float angle, bool visible) {
			var stem = new PFStem(10, 2, 0.01f);
			var root = CreateStem("Stem", stem, leafData.shadowCastingMode, leafData.receiveShadows, (r) => Mathf.Max(1f - r, 0.2f), 0.05f, 0.0f, visible);
			root.transform.localPosition = segment.position;
			root.transform.localRotation *= Quaternion.FromToRotation(Vector3.forward, dir) * Quaternion.AngleAxis(angle, Vector3.forward);

			var leaf = CreateShape("Leaf", PFPartType.Stover, leafData, visible);
			leaf.transform.SetParent(root.transform, false);
			leaf.transform.localPosition = stem.Tip.position;
			leaf.transform.localRotation *= Quaternion.AngleAxis(rand.SampleRange(0f, 30f), Vector3.up);

			var part = root.GetComponent<PFPart>();
			part.SetSpeed(5f);
			part.Add(leaf.GetComponent<PFPart>(), 1f);

			return root;
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

    class Florets {
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

	#region Define Data classes

	[System.Serializable]
	class ShapeData {
		[SerializeField] PFShape shape;
		public Material material = null;
		[HideInInspector] public Mesh mesh;
		public ShadowCastingMode shadowCastingMode = ShadowCastingMode.On;
		public bool receiveShadows = true;

		public void Init () {
			mesh = shape.Build();
			if(material == null) {
				Debug.LogWarning("ShapeData material is null");
			}
		}
	}

	[System.Serializable]
	class StemData {
		[HideInInspector] public PFStem stem;

		public Material material = null;
		[SerializeField] int wresolution = 10;
		[SerializeField] int hresolution = 8;
		[SerializeField] float radius = 0.012f;
		public float bend = 0.05f;
		public ShadowCastingMode shadowCastingMode = ShadowCastingMode.On;
		public bool receiveShadows = true;

		public void Init () {
			stem = new PFStem(wresolution, hresolution, radius);
			if(material == null) {
				Debug.LogWarning("StemData material is null");
			}
		}
	}

	#endregion

}


