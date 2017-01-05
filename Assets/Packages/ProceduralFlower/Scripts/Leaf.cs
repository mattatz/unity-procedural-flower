using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace mattatz.ProceduralFlower {

    public class Leaf : MonoBehaviour {

		[SerializeField] float size = 1f;
		[SerializeField] Shape shape;

        void OnEnable () {
			GetComponent<MeshFilter>().mesh = Petal.Build(size, shape.controls, 30, 4, 0.25f, new Vector2(3f, 1.5f));
        }

    }

}


