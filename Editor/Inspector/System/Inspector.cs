using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using com.Klazapp.Utility;

namespace com.Klazapp.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UnityEngine.Object), editorForChildClasses: true)]
    public partial class Inspector : InspectorBase
    {
        #region Base Flow
        private void OnEnable()
        {
            OnCreated();
        }

        public override void OnInspectorGUI()
        {
            OnDisplayed();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override void OnCreated()
        {
            OnCreateHeader();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override void OnDisplayed()
        {
            OnDisplayHeader();

            CustomEditorHelper.DrawHorizontalLine(10);
            
            OnDisplayEditScript();
            
            CustomEditorHelper.DrawHorizontalLine(10);
            
            OnDisplayProperties();
        }
        #endregion
        
        #region Attribute Check only works in main partial class. figure out why
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool CheckIfHasAttribute<TAttribute>(SerializedProperty prop, bool inherit) where TAttribute : PropertyAttribute
        {
            if (prop == null)
            {
                return false;
            }
        
            var t = prop.serializedObject.targetObject.GetType();
        
            FieldInfo f = null;
            PropertyInfo p = null;
            foreach (var name in prop.propertyPath.Split('.'))
            {
                f = t.GetField(name, (BindingFlags)(-1));
        
                if (f == null)
                {
                    p = t.GetProperty(name, (BindingFlags)(-1));
                    if (p == null)
                    {
                        return false;
                    }
        
                    t = p.PropertyType;
                }
                else
                {
                    t = f.FieldType;
                }
            }
        
            TAttribute[] attributes;

            if (f != null)
            {
                attributes = f.GetCustomAttributes(typeof(TAttribute), inherit) as TAttribute[];
            }
            else if (p != null)
            {
                attributes = p.GetCustomAttributes(typeof(TAttribute), inherit) as TAttribute[];
            }
            else
            {
                return false;
            }

            //return attributes.Length > 0 ? attributes[0] : null;
            return attributes is { Length: > 0 };
        }
        #endregion

        #region ReadOnlyAttribute Check only works in main partial class. figure out why
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool CheckIfHasReadOnlyAttribute(SerializedProperty prop, bool inherit)
        {
            if (prop == null)
            {
                return false;
            }
        
            var t = prop.serializedObject.targetObject.GetType();
        
            FieldInfo f = null;
            PropertyInfo p = null;
            foreach (var name in prop.propertyPath.Split('.'))
            {
                f = t.GetField(name, (BindingFlags)(-1));
        
                if (f == null)
                {
                    p = t.GetProperty(name, (BindingFlags)(-1));
                    if (p == null)
                    {
                        return false;
                    }
        
                    t = p.PropertyType;
                }
                else
                {
                    t = f.FieldType;
                }
            }
        
            ReadOnlyAttribute[] attributes;

            if (f != null)
            {
                attributes = f.GetCustomAttributes(typeof(ReadOnlyAttribute), inherit) as ReadOnlyAttribute[];
            }
            else if (p != null)
            {
                attributes = p.GetCustomAttributes(typeof(ReadOnlyAttribute), inherit) as ReadOnlyAttribute[];
            }
            else
            {
                return false;
            }

            //return attributes.Length > 0 ? attributes[0] : null;
            return attributes is { Length: > 0 };
        }
        #endregion
        
        #region GroupAttribute Check only works in main partial class. figure out why
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool CheckIfHasGroupAttribute(SerializedProperty prop, bool inherit)
        {
            if (prop == null)
            {
                return false;
            }
        
            var t = prop.serializedObject.targetObject.GetType();
        
            FieldInfo f = null;
            PropertyInfo p = null;
            foreach (var name in prop.propertyPath.Split('.'))
            {
                f = t.GetField(name, (BindingFlags)(-1));
        
                if (f == null)
                {
                    p = t.GetProperty(name, (BindingFlags)(-1));
                    if (p == null)
                    {
                        return false;
                    }
        
                    t = p.PropertyType;
                }
                else
                {
                    t = f.FieldType;
                }
            }
        
            ReadOnlyAttribute[] attributes;

            if (f != null)
            {
                attributes = f.GetCustomAttributes(typeof(ReadOnlyAttribute), inherit) as ReadOnlyAttribute[];
            }
            else if (p != null)
            {
                attributes = p.GetCustomAttributes(typeof(ReadOnlyAttribute), inherit) as ReadOnlyAttribute[];
            }
            else
            {
                return false;
            }

            //return attributes.Length > 0 ? attributes[0] : null;
            return attributes is { Length: > 0 };
        }
        #endregion
    }
}