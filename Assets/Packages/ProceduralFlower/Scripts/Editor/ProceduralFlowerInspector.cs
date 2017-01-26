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
			flower.scale = EditorGUILayout.Slider("Scale", flower.scale, 0.1f, 2f);
			flower.min = EditorGUILayout.Slider("Min", flower.min, 0.0f, 1f);
			flower.angle = EditorGUILayout.Slider("Angle", flower.angle, 0f, 90f);
			flower.angleScale = EditorGUILayout.Slider("Angle Scale", flower.angleScale, 0.1f, 1.5f);
			flower.offset = EditorGUILayout.Slider("Offset", flower.offset, 0f, 1f);
			flower.height = EditorGUILayout.Slider("Height", flower.height, 1f, 10f);
			flower.leafCount = EditorGUILayout.IntSlider("# of leafs", flower.leafCount, 0, 10);
			flower.leafRange.x = EditorGUILayout.Slider("Leaf range min", flower.leafRange.x, 0.1f, 0.9f);
			flower.leafRange.y = EditorGUILayout.Slider("Leaf range max", flower.leafRange.y, flower.leafRange.x, 0.95f);

			if(EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(flower, "Flower");

                if(Application.isPlaying) {
                    flower.Build();
                }
			}

		}

	}
		
}

