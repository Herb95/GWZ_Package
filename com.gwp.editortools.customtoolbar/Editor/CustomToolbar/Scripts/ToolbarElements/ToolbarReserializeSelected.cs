using System;
using UnityEngine;
using UnityEditor;

namespace Gwp.EditorTools.CustomToolbar
{
	[Serializable]
	internal class ToolbarReserializeSelected : BaseToolbarElement
	{
		private static GUIContent reserializeSelectedBtn;

		public override string NameInList => "[Button] Reserialize selected";

		public override void Init()
		{
			reserializeSelectedBtn = EditorGUIUtility.IconContent("Refresh");
			reserializeSelectedBtn.tooltip = "Reserialize Selected Assets";
		}

		protected override void OnDrawInList(Rect position)
		{

		}

		protected override void OnDrawInToolbar()
		{
			if (GUILayout.Button(reserializeSelectedBtn, ToolbarStyles.CommandButtonStyle))
			{
				ForceReserializeAssetsUtils.ForceReserializeSelectedAssets();
			}
		}
	}
}