using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace mattatz.ProceduralFlower {

	[CustomEditor (typeof(ProceduralFlower))]
	public class ProceduralFlowerInspector : Editor {

		public override void OnInspectorGUI () {
			base.OnInspectorGUI();

			var flower = target as ProceduralFlower;

			EditorGUI.BeginChangeCheck();

			flower.c = EditorGUILayout.Slider("C", flower.c, 0f, 0.1f);
			flower.n = EditorGUILayout.IntSlider("# of petals", flower.n, 10, 200);
			flower.height = EditorGUILayout.Slider("Height", flower.height, 1f, 10f);
			flower.leafCount = EditorGUILayout.IntSlider("# of leafs", flower.leafCount, 0, 10);

			if(EditorGUI.EndChangeCheck()) {
				flower.Build();
			}

		}

	}
		
}

