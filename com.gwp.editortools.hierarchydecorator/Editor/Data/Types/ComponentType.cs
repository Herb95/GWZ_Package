using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Gwp.EditorTools.HierarchyDecorator
{
    [Serializable]
    public class ComponentType : IComparable<ComponentType>
    {
        // Fields

        // --- Component information

        [SerializeField] protected string displayName;
        [SerializeField] protected string name;
        [SerializeField] protected GUIContent content;

        // --- Settings

        [SerializeField] protected bool shown = false;

        //  --- Type Data

        [SerializeField] private bool isBuiltIn;
        [SerializeField] private MonoScript script;
        // [SerializeField] private int hash = -1;

        // Properties

        /// <summary>
        /// The full name of the component
        /// </summary>
        public string Name
        {
            get => name;
            set => name = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public string DisplayName => displayName;

        /// <summary>
        /// The type of component.
        /// </summary>
        public Type Type { get; private set; } = typeof(DefaultAsset);

        /// <summary>
        /// 
        /// </summary>
        public MonoScript Script
        {
            get => script;

            set => script = value;
        }

        /// <summary>
        /// Is the component activated or not
        /// </summary>
        public bool Shown
        {
            get => shown;

            set => shown = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsBuiltIn => isBuiltIn;

        /// <summary>
        /// The GUIContent displayed for this component.
        /// </summary>
        public GUIContent Content => content;

        // Constructor 

        /// <summary>
        /// ComponentType constructor.
        /// </summary>
        /// <param name="type">The type of component.</param>
        /// <param name="isBuiltIn"></param>
        public ComponentType(Type type, bool isBuiltIn)
        {
            Type = type;
            name = type.AssemblyQualifiedName;
            displayName = type.Name;
            this.isBuiltIn = isBuiltIn;
        }

        // Methods

        public bool IsValid()
        {
            // Need to check type and content to validate GUI
            if (!IsBuiltIn && script == null)
            {
                return false;
            }
            if (content == null)
            {
                return false;
            }
            return Type != null && content.image != null;
        }

        /// <summary>
        /// Update the component
        /// </summary>
        /// <param name="type"></param>
        /// <param name="updateContent"></param>
        public void UpdateType(Type type, bool updateContent = false)
        {
            if (type == null)
            {
                Type = null;
                name = "Undefined";
                UpdateContent();
                return;
            }
            if (!isBuiltIn && script == null)
            {
                return;
            }
            Type = type;
            name = type.AssemblyQualifiedName;
            displayName = type.Name;
            if (updateContent)
            {
                UpdateContent();
            }
        }

        public void UpdateType(MonoScript monoScript)
        {
            script = monoScript;
            if (monoScript == null)
            {
                UpdateType(null, true);
                return;
            }
            UpdateType(monoScript.GetClass(), true);
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateContent()
        {
            content = GetTypeContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private GUIContent GetTypeContent()
        {
            if (Type == null)
            {
                return new GUIContent(GUIContent.none);
            }
            GUIContent guiContent = new GUIContent(displayName, displayName);
            Texture texture;
            if (isBuiltIn)
            {
                texture = EditorGUIUtility.ObjectContent(null, Type).image;
            }
            else
            {
                string path = AssetDatabase.GetAssetPath(script);
                texture = AssetDatabase.GetCachedIcon(path);
            }
            guiContent.image = texture;
            return guiContent;
        }

        // --- Overrides

        /// <summary>
        /// Compare this component type to another.
        /// </summary>
        /// <param name="other">The other component type.</param>
        /// <returns>Returns an integer based on their sort position.</returns>
        public int CompareTo(ComponentType other)
        {
            if (other == null)
            {
                return 1;
            }
            return name.CompareTo(other.name);
        }

        public int CompareTo(Type type)
        {
            return name.CompareTo(type.AssemblyQualifiedName);
        }

        public override string ToString()
        {
            return $"Component Type: {displayName}, {Type}";
        }

        public override bool Equals(object obj)
        {
            if (obj is ComponentType component)
            {
                return Type == component.Type;
            }
            if (obj is Type type)
            {
                return Type == type;
            }
            return false;
        }


        public override int GetHashCode()
        {
            int hashCode = 1280150957;
            hashCode *= -1521134295 + name.GetHashCode();
            hashCode *= -1521134295 + displayName.GetHashCode();
            hashCode *= -1521134295 + shown.GetHashCode();
            hashCode *= -1521134295 + isBuiltIn.GetHashCode();
            hashCode *= -1521134295 + script.GetHashCode();
            return hashCode;
        }
    }
}