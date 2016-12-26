using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mattatz.ProceduralFlower {

    public class PetalTest : MonoBehaviour {

        void Awake () {
            GetComponent<MeshFilter>().mesh = Petal.Build();
        }

    }

}


