using System;
using UnityEngine;
using UnityEditor;

namespace Custom_Toolbar.ToolbarElements
{
    [Serializable]
    internal class ToolbarClearPrefs : BaseToolbarElement
    {
        private static GUIContent _clearPlayerPrefsBtn;

        public override string NameInList => "[Button] Clear prefs";

        public override void Init()
        {
            _clearPlayerPrefsBtn = EditorGUIUtility.IconContent("SaveFromPlay");
            _clearPlayerPrefsBtn.tooltip = "Clear player prefs";
        }

        protected override void OnDrawInList(Rect position)
        {
        }

        protected override void OnDrawInToolbar()
        {
            if (GUILayout.Button(_clearPlayerPrefsBtn, ToolbarStyles.CommandButtonStyle))
            {
                PlayerPrefs.DeleteAll();
                Debug.Log("Clear Player Prefs");
            }
        }
    }
}