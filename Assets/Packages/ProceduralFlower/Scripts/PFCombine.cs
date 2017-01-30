using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace mattatz.ProceduralFlower {

    public class PFCombine {

        public static Mesh Combine (ProceduralFlower flower) {
            var root = flower.Build();
            var filters = new List<MeshFilter>();
            Traverse(root.GetComponent<PFPart>(), filters);
            var combine = new CombineInstance[filters.Count];
            for(int i = 0, n = filters.Count; i < n; i++) {
                combine[i].mesh = filters[i].sharedMesh;
                combine[i].transform = filters[i].transform.localToWorldMatrix;
            }

            var mesh = new Mesh();
            mesh.CombineMeshes(combine);
            return mesh;
        }

        public static Mesh CombinePetals (ProceduralFlower flower) {
            var root = flower.Build();
            var filters = new List<MeshFilter>();
            Traverse(root.GetComponent<PFPart>(), filters);
            filters = filters.FindAll(filter => {
                return filter.GetComponent<PFPart>().Type == PFPartType.Petal;
            }).ToList();

            var combine = new CombineInstance[filters.Count];
            for(int i = 0, n = filters.Count; i < n; i++) {
                combine[i].mesh = filters[i].sharedMesh;
                combine[i].transform = filters[i].transform.localToWorldMatrix;
            }

            var mesh = new Mesh();
            mesh.CombineMeshes(combine);
            return mesh;
        }

        static void Traverse (PFPart root, List<MeshFilter> filters) {
            var filter = root.GetComponent<MeshFilter>();
            if (filter != null) filters.Add(filter);

            root.children.ForEach(child => {
                Traverse(child.part, filters);
            });
        }

    }

}


