using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace mattatz.ProceduralFlower {

	[ExecuteInEditMode]
    public class Leaf : MonoBehaviour {

		[SerializeField] float size = 1f;
		[SerializeField] Shape shape;
		[SerializeField] int numberOfVerticesOnOneSide = 30;
		[SerializeField] int resolution = 4;
		[SerializeField] float depth = 0.25f;
		[SerializeField] Vector2 noiseScale = new Vector2(3f, 2f);

        void Start() {
			GetComponent<MeshFilter>().mesh = Petal.Build(size, shape.controls, numberOfVerticesOnOneSide, resolution, depth, noiseScale);
        }

    }

}


