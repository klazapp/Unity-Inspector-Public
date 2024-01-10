using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace com.Klazapp.Editor
{
    public partial class Inspector
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void DrawClassHeaderNameBox(bool useParentClass)
        {
            //Get class header name by useParentClass
            var classHeaderName = useParentClass ? InspectorClassGroupModule.PARENT_SCRIPT_TITLE : InspectorClassGroupModule.CURRENT_SCRIPT_TITLE;
            classHeaderName = " " + classHeaderName;
            
            //Get length of class header name
            var lengthOfClassHeader = classHeaderName.Length;
            
            //Draw header box
            //Dynamically adjust box width by length of class header
            CustomEditorHelper.DrawBox(lengthOfClassHeader * 7, 10, inspectorClassGroupModule.classHeaderNameColor, classHeaderName, inspectorClassGroupModule.classNameHeaderStyle, null, Alignment.None);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void DrawClassScriptBox(string className)
        { 
            //Find script attached to gameobject
            var scriptProp = serializedObject.FindProperty("m_Script");

            if (scriptProp == null) 
                return;

            EditorGUILayout.BeginHorizontal();

            var targetType = serializedObject.targetObject.GetType();
            var baseType = targetType.BaseType;

            var fullWidth = (int)EditorGUIUtility.currentViewWidth;
          
            var isTargetOrBaseClass = className == targetType.Name || (baseType != null && className == GetReadableTypeName(baseType));

            if (isTargetOrBaseClass) 
            {
                var isBaseClass = className == GetReadableTypeName(baseType);
                var (loadedObject, assetPath) = inspectorClassGroupModule.GetClassObjectAndPath(isBaseClass);

                //Draw button for script location
                var pressedScriptLocation = CustomEditorHelper.DrawButton(fullWidth - 70, 20, new Color32(120, 120, 120, 255), loadedObject.name + ".cs", inspectorClassGroupModule.classScriptStyle, scriptIcon);
                if (pressedScriptLocation)
                {
                    OpenScriptLocation(loadedObject, assetPath);
                }

                //Draw button for editing script
                var pressedScriptEdit = CustomEditorHelper.DrawButton(20, 20, new Color32(120, 120, 120, 255), "", inspectorClassGroupModule.editScriptStyle, scriptIcon2);
                if (pressedScriptEdit)
                {
                    EditScript(loadedObject, assetPath);
                }
            }
            
            EditorGUILayout.EndHorizontal();
            
            CustomEditorHelper.DrawSpace(5);
        }
    }
}
