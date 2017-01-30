using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mattatz.ProceduralFlower.Demo {

    [RequireComponent (typeof(MeshFilter))]
    public class PFCombineDemo : MonoBehaviour {

        [SerializeField] PFPartType type = PFPartType.None;
        [SerializeField] ProceduralFlower flower;

        void Awake () {
            var mesh = PFCombine.Combine(flower, type);
            GetComponent<MeshFilter>().sharedMesh = mesh;
        }

    }

}


