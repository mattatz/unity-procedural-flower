using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mattatz.ProceduralFlower.Demo {

	public class PFTester : MonoBehaviour {

		public ProceduralFlower flower;

		GameObject child;

        void Start () {
			Build();
        }

		public void Build () {
			Clear();
			child = flower.Build();
			child.transform.SetParent(transform, false);
		}

		public void Clear () {
			if(child != null) {
				if(Application.isPlaying) {
					Destroy(child);
				} else {
					DestroyImmediate(child);
				}
			}
		}

	}
		
}

