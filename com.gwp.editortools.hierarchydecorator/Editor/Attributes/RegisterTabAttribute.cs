using System;

namespace Gwp.EditorTools.HierarchyDecorator
{
    public class RegisterTabAttribute : Attribute
    {
        public int priority = 0;

        public RegisterTabAttribute(int priority = 0)
        {
            this.priority = priority;
        }
    }
}
