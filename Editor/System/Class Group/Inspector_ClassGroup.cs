using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace com.Klazapp.Editor
{
    public partial class Inspector
    {
        private readonly InspectorClassGroupModule inspectorClassGroupModule = new();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ResetClassGroup()
        {
            var isUsingCustomParentClass = IsInheritingFromParentClass(serializedObject);
            inspectorClassGroupModule.OnCreated(serializedObject, isUsingCustomParentClass);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void StartClassGroup(bool useParentClass)
        {
            var className = GetClassName(useParentClass);
            
            //Draw class header name box
            DrawClassHeaderNameBox(useParentClass);

            //Draw class script box
            DrawClassScriptBox(className);

            EditorGUILayout.BeginVertical(GUI.skin.window);
        }

        //If property has iterated into child class, start new class group
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ContinueClassGroup(SerializedProperty serializedProperty)
        {
            if (!inspectorClassGroupModule.isInheritingFromCustomClass)
                return;
            
            var parentPropertyPaths = GetParentPropertyPath();

            //Check if property is not in parent's property paths
            //Single class properties will always be in child class
            if (!parentPropertyPaths.Contains(serializedProperty.name))
            {
                if (inspectorClassGroupModule.iteratedToChildClass) 
                    return;
            
                //If iterated to child class, end previous class group and start new class group
                EndClassGroup();
            
                inspectorClassGroupModule.iteratedToChildClass = true;
            }
            else
            {
                //Property likely from parent class
                inspectorClassGroupModule.iteratedToChildClass = false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EndClassGroup()
        {
            EditorGUILayout.EndVertical();
            CustomEditorHelper.DrawSpace(30);
            
            //Property likely from parent class
            inspectorClassGroupModule.iteratedToChildClass = false;
        }
    }
}
