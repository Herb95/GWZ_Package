﻿using System;
using UnityEngine;
using UnityEditor;
using Custom_Toolbar.Utils;

namespace Custom_Toolbar.ToolbarElements
{
    [Serializable]
    internal class ToolbarReserializeAll : BaseToolbarElement
    {
        private static GUIContent reserializeAllBtn;

        public override string NameInList => "[Button] Reserialize all";

        public override void Init()
        {
            reserializeAllBtn = EditorGUIUtility.IconContent("P4_Updating");
            reserializeAllBtn.tooltip = "Reserialize All Assets";
        }

        protected override void OnDrawInList(Rect position)
        {
        }

        protected override void OnDrawInToolbar()
        {
            if (GUILayout.Button(reserializeAllBtn, ToolbarStyles.CommandButtonStyle))
            {
                ForceReserializeAssetsUtils.ForceReserializeAllAssets();
            }
        }
    }
}