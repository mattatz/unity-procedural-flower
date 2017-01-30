using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace mattatz.ProceduralFlower.Demo {

	[CustomEditor (typeof(PFTester))]
	public class PFTesterInspector : Editor {

        public override void OnInspectorGUI () {
            base.OnInspectorGUI();

			var tester = target as PFTester;
			if(tester.flower == null) {
				return;
			}

			EditorGUI.BeginChangeCheck();

			DisplayFlowerGUI(tester.flower);

			if(EditorGUI.EndChangeCheck()) {
                // Undo.RecordObject(flower, "Flower");
                if(Application.isPlaying) {
					tester.Build();
                }
			}

		}

		void DisplayFlowerGUI (ProceduralFlower flower) {
			flower.c = EditorGUILayout.Slider("Petal distance from center", flower.c, 0.001f, 0.05f);
			flower.n = EditorGUILayout.IntSlider("# of petals", flower.n, 4, 200);
			flower.m = EditorGUILayout.IntSlider("# of buds", flower.m, 0, flower.n);
			flower.scale = EditorGUILayout.Slider("Scale", flower.scale, 0.1f, 0.6f);
			flower.min = EditorGUILayout.Slider("Min", flower.min, 0.0f, 1f);
			flower.angle = EditorGUILayout.Slider("Angle", flower.angle, 30f, 100f);
			flower.angleScale = EditorGUILayout.Slider("Angle Scale", flower.angleScale, 0.1f, 1.5f);
			flower.offset = EditorGUILayout.Slider("Offset", flower.offset, 0f, 1f);
			flower.height = EditorGUILayout.Slider("Height", flower.height, 1f, 10f);
			flower.leafCount = EditorGUILayout.IntSlider("# of leafs", flower.leafCount, 0, 10);
			flower.leafScaleRange.x = EditorGUILayout.Slider("Leaf scale range min", flower.leafScaleRange.x, 0.1f, 0.9f);
			flower.leafScaleRange.y = EditorGUILayout.Slider("Leaf scale range max", flower.leafScaleRange.y, flower.leafScaleRange.x, 0.95f);
			flower.leafSegmentRange.x = EditorGUILayout.Slider("Leaf segment range min", flower.leafSegmentRange.x, 0.1f, 0.9f);
			flower.leafSegmentRange.y = EditorGUILayout.Slider("Leaf segment range max", flower.leafSegmentRange.y, flower.leafSegmentRange.x, 0.95f);
		}

	}
		
}

