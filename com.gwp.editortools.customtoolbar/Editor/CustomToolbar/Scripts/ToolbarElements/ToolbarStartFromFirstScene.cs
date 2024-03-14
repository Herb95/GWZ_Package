using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEditor.SceneManagement;

namespace Gwp.EditorTools.CustomToolbar
{
    [Serializable]
    internal class ToolbarStartFromFirstScene : BaseToolbarElement
    {
        private static GUIContent startFromFirstSceneBtn;

        public override string NameInList => "[Button] Start from first scene";

        public override void Init()
        {
            EditorApplication.playModeStateChanged += LogPlayModeState;
            var lookDevSingle =(Texture2D)AssetDatabase.LoadAssetAtPath(
                $"{GetPackageRootPath}/Editor Resources/Icons/LookDevSingle1@2x.png", typeof(Texture2D));
            startFromFirstSceneBtn = new GUIContent(lookDevSingle, "Start from 1 scene");
        }

        protected override void OnDrawInList(Rect position)
        {
        }

        protected override void OnDrawInToolbar()
        {
            if (GUILayout.Button(startFromFirstSceneBtn, ToolbarStyles.CommandButtonStyle))
            {
                if (!EditorApplication.isPlaying)
                {
                    EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                    EditorPrefs.SetInt("LastActiveSceneToolbar", SceneManager.GetActiveScene().buildIndex);
                    EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(0));
                }

                EditorApplication.isPlaying = !EditorApplication.isPlaying;
            }
        }

        private static void LogPlayModeState(PlayModeStateChange state)
        {
            if (state != PlayModeStateChange.EnteredEditMode || !EditorPrefs.HasKey("LastActiveSceneToolbar")) return;
            int lastActiveSceneToolbar = EditorPrefs.GetInt("LastActiveSceneToolbar");
            if (lastActiveSceneToolbar == -1)
            {
                return;
            }

            EditorSceneManager.OpenScene(
                SceneUtility.GetScenePathByBuildIndex(EditorPrefs.GetInt("LastActiveSceneToolbar")));
            EditorPrefs.DeleteKey("LastActiveSceneToolbar");
        }
    }
}