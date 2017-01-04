using UnityEngine;
using UnityEditor;

using System.Collections;

namespace mattatz.MeshBuilderSystem {

    public class SaveMeshWindow : EditorWindow {

        string folder = "Assets/";
        string assetName = "GeneratedMesh";
        MeshFilter filter;

        [MenuItem("mattatz/SaveMesh")]
        static void Save () {
            SaveMeshWindow window = EditorWindow.GetWindow(typeof(SaveMeshWindow)) as SaveMeshWindow;
            window.Show();
        }

        void OnGUI () {
            GUILayout.Label("Save Mesh Window", EditorStyles.boldLabel);

            using (new GUILayout.HorizontalScope()) {
                GUILayout.Label("File name");
                assetName = GUILayout.TextField(assetName, GUILayout.Width(200f));
            }

            using (new GUILayout.HorizontalScope()) {
                GUILayout.Label("Target MeshFilter in Scene");
                filter = UnityEditor.EditorGUILayout.ObjectField(filter, typeof(MeshFilter), true, GUILayout.Width(200f)) as MeshFilter;
            }

            if (assetName.Length > 0 && filter != null && GUILayout.Button("Save")) {
                AssetDatabase.CreateAsset(filter.mesh, folder + assetName + ".asset");
                AssetDatabase.SaveAssets();
            }
        }

    }

}


