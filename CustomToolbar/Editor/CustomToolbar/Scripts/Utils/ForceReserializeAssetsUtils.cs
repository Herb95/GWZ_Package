using System;
using UnityEditor;
using UnityEngine;

namespace Custom_Toolbar.Utils
{
    public static class ForceReserializeAssetsUtils
    {
        public static void ForceReserializeAllAssets()
        {
            if (!EditorUtility.DisplayDialog("Attention",
                    "Do you want to force reserialize all assets? This can be time heavy operation and result in massive list of changes.",
                    "Ok", "Cancel"))
            {
                return;
            }

            AssetDatabase.ForceReserializeAssets();
        }

        public static void ForceReserializeSelectedAssets()
        {
            if (Application.isPlaying)
            {
                EditorUtility.DisplayDialog("Attention", "This scene is Playing.", "Ok");
                return;
            }

            var assetGUIDs = Selection.assetGUIDs;
            if (assetGUIDs.Length == 0)
            {
                EditorUtility.DisplayDialog("Attention", "No assets are selected.", "Ok");
                return;
            }

            var assetPaths = Array.ConvertAll<string, string>(assetGUIDs, AssetDatabase.GUIDToAssetPath);
            AssetDatabase.ForceReserializeAssets(assetPaths);
        }
    }
}