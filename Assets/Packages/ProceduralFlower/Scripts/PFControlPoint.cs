using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mattatz.ProceduralFlower {

	[System.Serializable]
	public class PFControlPoint {
		public float width;
		public float height;

		public PFControlPoint (float width, float height) {
			this.width = Mathf.Clamp01(width);
			this.height = Mathf.Clamp01(height);
		}
	}

}
