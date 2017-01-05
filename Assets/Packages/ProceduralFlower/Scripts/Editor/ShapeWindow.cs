using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace mattatz.ProceduralFlower {

	public class ShapeWindow : EditorWindow {

		enum Mode {
			None,
			Select
		};

		static Texture grid;
		static Vector2 size = new Vector2(512f, 512f);
		const float unit = 20f;

		Mode mode = Mode.None;
		int selected = -1;

		List<Vector2> points;
		Shape shape;

		[MenuItem("ProceduralFlower/Window")]
		static void Open () {
			var window = GetWindow<ShapeWindow>(typeof(SceneView));
			var icon = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Packages/ProceduralFlower/Gizmos/Shape Icon.png");
			window.titleContent = new GUIContent("PF", icon);
		}

		void Update () {
			Repaint();
		}

		void CheckInit () {
			var obj = Selection.activeObject;
			if(obj && obj as Shape) {
				var current = obj as Shape;
				if(current != shape) {
					Load(current);
				} 
				shape = current;
			}

			if(grid == null) {
				grid = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Packages/ProceduralFlower/Textures/Grid.png");
			}

			if(points == null) {
				points = new List<Vector2>();
			}
		}

		Vector2 Translate (Vector2 p) {
			return new Vector2(p.x + size.x * 0.5f, p.y);
		}

		void Load (Shape shp) {
			var controls = shp.controls;
			points = controls.Select(c => Convert(c)).ToList();
		}

		void Apply () {
			shape.controls = GetControls(points);
		}

		void DrawHeader() {
			var style = new GUIStyle();
			style.fontSize = 15;
			if(shape) {
				GUI.Label(new Rect(5f, 5f, 80f, 25f), shape.name + ".asset", style);
			} else {
				GUI.Label(new Rect(5f, 5f, 80f, 25f), "Select Shape asset", style);
			}

			if(shape && GUI.Button(new Rect(5f, 30f, 80f, 25f), "Apply")) {
				Apply();
			}

			if(GUI.Button(new Rect(5f, 60f, 80f, 25f), "Clear All")) {
				points.Clear();
			}
		}

		void DrawShape (List<Vector3> mirror, int count, int resolution = 5) {

			// left side
			var points = new List<Vector3>();
			for(int i = 0; i <= count; i++) {
				var edge = CatmullRomSpline.GetCatmullRomPositions(
					resolution, 
					Petal.GetLoopPoint(mirror, i - 1),
					Petal.GetLoopPoint(mirror, i),
					Petal.GetLoopPoint(mirror, i + 1),
					Petal.GetLoopPoint(mirror, i + 2)
				);
				if(i != 0) edge.RemoveAt(0);
				points.AddRange(edge);
			}

			for(int i = 0, n = points.Count - 1; i < n; i++) {
				var from = points[i];
				var to = points[i + 1];
				Handles.DrawLine(Translate(from), Translate(to));
			}

			for(int i = 0, n = points.Count - 1; i < n; i++) {
				var from = points[i];
				var to = points[i + 1];
				Handles.DrawLine(Translate(new Vector2(-from.x, from.y)), Translate(new Vector2(-to.x, to.y)));
			}

		}

		Vector2 Convert (ControlPoint c) {
			return new Vector2(0.5f - c.width, c.height);
		}

		ControlPoint Convert (Vector2 p) {
			return new ControlPoint(0.5f - p.x, p.y);
		}

		List<ControlPoint> GetControls (List<Vector2> points) {
			return points.Select(p => {
				return Convert(p);
			}).ToList();
		}

		int Pick (Vector2 position) {
			float distance = unit / size.x;
			return points.FindIndex(p => {
				return Vector2.Distance(p, position) <= distance;
			});
		}

		bool Contains (Vector2 position) {
			return 0.05f < position.x && position.x < 0.5f && 0.05f < position.y && position.y < 0.95f;
		}

		void Sort () {
			points.Sort((p0, p1) => {
				return p0.y.CompareTo(p1.y);
			});
		}

		void CatchEvent () {
			var mp = Event.current.mousePosition;
			var position = new Vector2(mp.x / size.x, mp.y / size.y); 

			switch(mode) {

				case Mode.None:
					switch(Event.current.type) {

						case EventType.mouseDown:
							selected = Pick(position);
							if(selected >= 0) {
								mode = Mode.Select;
							}
							break;

						case EventType.mouseUp:
							if(Contains(position)) {
								points.Add(position);
								Sort();
							}
							break;

						case EventType.keyUp:
							if(selected >= 0 && Input.GetKeyUp(KeyCode.Delete)) {
								points.RemoveAt(selected);
							}
							break;
					}
					break;

				case Mode.Select:
					switch(Event.current.type) {
						case EventType.mouseDrag:
							if(selected >= 0 && Contains(position)) {
								points[selected] = position;
							}
							break;
						case EventType.mouseUp:
							Sort();
							mode = Mode.None;
							break;
						}

					break;
			}
		}

		void OnGUI () {
			CheckInit();
			GUI.DrawTexture(new Rect(0f, 0f, size.x, size.y), grid);

			for(int i = 0, n = points.Count; i < n; i++) {
				if(i == selected) {
					GUI.color = new Color(0.6f, 0.75f, 1f);
				} else {
					GUI.color = Color.white;
				}
				var p = points[i];
				GUI.Box(new Rect(Vector2.Scale(p, size) - new Vector2(unit, unit) * 0.5f, new Vector2(unit, unit)), "");
			}
			GUI.color = Color.white;

			// GUI.Box(new Rect(new Vector2((size.x - unit) * 0.5f, 0f), new Vector2(unit, unit)), "top");
			// GUI.Box(new Rect(new Vector2((size.x - unit) * 0.5f, size.y - unit), new Vector2(unit, unit)), "bottom");

			if(points.Count >= 2) {
				var mirror = Petal.Mirror(GetControls(points), size.y, size.x);
				Handles.color = Color.black;
				DrawShape(mirror, points.Count);
			}

			DrawHeader();
			CatchEvent();
		}

	}
		
}

