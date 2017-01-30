using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mattatz.ProceduralFlower.Demo {

    [RequireComponent (typeof(MeshFilter))]
    public class PFCombineDemo : MonoBehaviour {

        [SerializeField] ProceduralFlower flower;

        void Awake () {
            var mesh = PFCombine.Combine(flower);
            GetComponent<MeshFilter>().sharedMesh = mesh;
        }

    }

}


