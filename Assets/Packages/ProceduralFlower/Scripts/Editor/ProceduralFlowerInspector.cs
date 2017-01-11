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

			flower.c = EditorGUILayout.Slider("C", flower.c, 0.01f, 0.1f);
			flower.n = EditorGUILayout.IntSlider("# of petals", flower.n, 10, 200);
			flower.power = EditorGUILayout.Slider("Power", flower.power, 0.5f, 2f);
			flower.scale = EditorGUILayout.Slider("Scale", flower.scale, 0.1f, 10f);
			flower.min = EditorGUILayout.Slider("Min", flower.min, 0.0f, 1f);
			flower.angle = EditorGUILayout.Slider("Angle", flower.angle, 0f, 90f);
			flower.angleScale = EditorGUILayout.Slider("Angle Scale", flower.angleScale, 0.1f, 1.5f);
			flower.offset = EditorGUILayout.Slider("Offset", flower.offset, 0f, 1f);
			flower.height = EditorGUILayout.Slider("Height", flower.height, 1f, 10f);
			flower.leafCount = EditorGUILayout.IntSlider("# of leafs", flower.leafCount, 0, 10);

			if(EditorGUI.EndChangeCheck()) {
				flower.Build();
			}

		}

	}
		
}

