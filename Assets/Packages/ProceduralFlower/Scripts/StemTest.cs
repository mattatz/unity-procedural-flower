using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace mattatz.ProceduralFlower {

	[ExecuteInEditMode]
	public class StemTest : MonoBehaviour {

		[SerializeField] List<Vector3> controls;
		[SerializeField] int wresolution = 10;
		[SerializeField] int hresolution = 4;
		[SerializeField] float radius = 0.02f;

		void Start() {
			var stem = new Stem();
			stem.wresolution = wresolution;
			stem.hresolution = hresolution;
			stem.radius = radius;
			GetComponent<MeshFilter>().sharedMesh = stem.Build(controls);
		}

	}
		
}

