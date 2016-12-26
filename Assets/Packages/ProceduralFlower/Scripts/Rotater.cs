using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour {

    [SerializeField] float speed = 1f;
    [SerializeField] Vector3 axis = Vector3.up;

	void FixedUpdate () {
        transform.Rotate(axis, speed * Time.fixedDeltaTime);
	}

}
