using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace mattatz.ProceduralFlower {

    public class Leaf : MonoBehaviour {

		[SerializeField] float height = 1f;
		[SerializeField] float width = 0.25f;
		[SerializeField] List<Vector2> points;

        void Awake () {
			var controls = points.Select(p => {
				return new ControlPoint(p.x, p.y);
			}).ToList();

			// (float height, float width, List<ControlPoint> controls, int numberOfVerticesOnOneSide = 20, int resolution = 2, float depth = 0.25f, Vector2 scale = default(Vector2)) 
			GetComponent<MeshFilter>().mesh = Petal.Build(height, width, controls, 30, 4, 0.25f, new Vector2(3f, 1.5f));
        }

    }

}


