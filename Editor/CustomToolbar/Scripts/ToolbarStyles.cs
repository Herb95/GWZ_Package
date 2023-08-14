using UnityEngine;

namespace Custom_Toolbar
{
    static class ToolbarStyles
    {
        public static GUIStyle CommandButtonStyle { get; }

        static ToolbarStyles()
        {
            CommandButtonStyle = new GUIStyle("Command")
            {
                fontSize = 16,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove,
                fontStyle = FontStyle.Bold
            };
        }
    }
}