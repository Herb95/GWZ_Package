using UnityEditorInternal;

namespace Gwp.EditorTools.HierarchyDecorator
{
    public class ReorderableDrawer : GUIDrawer<ReorderableList>
    {
        public ReorderableDrawer(ReorderableList target) : base (target) { }

        protected override void OnGUI()
        {
            HierarchyGUI.Space();
            Target.DoLayoutList ();
        }

        protected override float GetHeight()
        {
            return Target.GetHeight();
        }
    }
}