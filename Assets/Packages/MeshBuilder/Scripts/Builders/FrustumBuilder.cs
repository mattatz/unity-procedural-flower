using UnityEngine;

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace mattatz.MeshBuilderSystem {

    public class FrustumBuilder : MeshBuilder {

		public static Mesh Build(Vector3 forward, Vector3 up, Matrix4x4 projectionMatrix) {
			var m00 = projectionMatrix.m00;
			var m11 = projectionMatrix.m11;
			var m22 = - projectionMatrix.m22;
			var m23 = - projectionMatrix.m23;

			var nearClip = (2f * m23) / (2f * m22 - 2f);
			var farClip = ((m22 - 1f) * nearClip) / (m22 + 1f);
			var fov = Mathf.Atan(1f / m11) * 2f * Mathf.Rad2Deg;
			var aspectRatio = (1f / m00) / (1f / m11);

			return Build(forward, up, nearClip, farClip, fov, aspectRatio);
		}

        public static Mesh Build(Vector3 forward, Vector3 up, float nearClip, float farClip, float fieldOfView = 60f, float aspectRatio = 1f) {
            var mesh = new Mesh();

            forward = forward.normalized;
            up = up.normalized;
            var left = Vector3.Cross(forward, up);

            var hfov = fieldOfView * 0.5f * Mathf.Deg2Rad;
            var near = forward * nearClip;
            var far = forward * farClip;

            var nearUp = up * Mathf.Tan(hfov) * nearClip;
            var nearLeft = left * Mathf.Tan(hfov) * nearClip * aspectRatio;

            var farUp = up * Mathf.Tan(hfov) * farClip;
            var farLeft = left * Mathf.Tan(hfov) * farClip * aspectRatio;

            var vertices = new Vector3[] {
                near + nearUp + nearLeft, // near top left
                near + nearUp - nearLeft, // near top right
                near - nearUp - nearLeft, // near bottom right
                near - nearUp + nearLeft, // near bototm left

                far + farUp + farLeft, // far top left
                far + farUp - farLeft, // far top right
                far - farUp - farLeft, // far bottom right
                far - farUp + farLeft // far bottom left
            };
            mesh.vertices = vertices;
            mesh.triangles = new int[] {
                // front
                0, 1, 2,
                2, 3, 0,

                // back
                4, 6, 5,
                6, 4, 7,

                // top
                0, 5, 1,
                4, 5, 0,

                // bottom
                3, 2, 7,
                6, 7, 2,

                // left
                0, 3, 4,
                3, 7, 4,

                // right
                1, 5, 6,
                6, 2, 1,
            };


            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }

    }

}

