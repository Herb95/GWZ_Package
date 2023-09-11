using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Custom_Toolbar.ToolbarElements
{
	[Serializable]
	internal class ToolbarRecompile : BaseToolbarElement
	{
		private static GUIContent recompileBtn;

		public override string NameInList => "[Button] Recompile";

		public override void Init()
		{
			recompileBtn = EditorGUIUtility.IconContent("WaitSpin05");
			recompileBtn.tooltip = "Recompile";
		}

		protected override void OnDrawInList(Rect position)
		{

		}

		protected override void OnDrawInToolbar()
		{
			if (GUILayout.Button(recompileBtn, ToolbarStyles.CommandButtonStyle))
			{
				UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
				Debug.Log("Recompile");
			}
		}
	}
}