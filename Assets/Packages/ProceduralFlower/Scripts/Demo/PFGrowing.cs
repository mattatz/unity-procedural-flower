using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mattatz.ProceduralFlower.Demo {

	public class PFGrowing : MonoBehaviour {

		[SerializeField] ProceduralFlower flower;

		void Start () {
			var root = flower.Build(false);
			root.transform.SetParent(transform, false);
			root.GetComponent<PFPart>().Animate();
		}

	}

}

