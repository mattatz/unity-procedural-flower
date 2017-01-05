using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mattatz.ProceduralFlower {

	[CreateAssetMenu(menuName = "ProceduralFlower/Shape")]
	public class Shape : ScriptableObject {

		public List<ControlPoint> controls;

	}
		
}

