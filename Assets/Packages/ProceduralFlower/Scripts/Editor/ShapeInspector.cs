using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace mattatz.ProceduralFlower {

	[CustomEditor (typeof(Shape))]
	public class ShapeInspector : Editor {

		Shape shape = null;

		void OnEnable () {
			shape = target as Shape;
		}

		public override void OnInspectorGUI () {
			base.OnInspectorGUI();
		}

	}
		
}

