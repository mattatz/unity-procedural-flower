using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mattatz.ProceduralFlower {

	[System.Serializable]
	public class ControlPoint {
		public float width;
		public float height;

		public ControlPoint (float width, float height) {
			this.width = Mathf.Clamp01(width);
			this.height = Mathf.Clamp01(height);
		}
	}

}
