using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace mattatz.ProceduralFlower {

	[CustomEditor (typeof(PFShape))]
	public class PFShapeInspector : Editor {

		public override void OnInspectorGUI () {
			base.OnInspectorGUI();

			if(GUILayout.Button("Open Editor")) {
				PFShapeWindow.Open();
			}
		}

	}
		
}

