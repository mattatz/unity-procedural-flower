using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace mattatz.ProceduralFlower {

	[ExecuteInEditMode]
    public class Leaf : MonoBehaviour {

		[SerializeField] Shape shape;

        void Start() {
			GetComponent<MeshFilter>().mesh = shape.Build();
        }

    }

}


