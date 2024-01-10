using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace com.Klazapp.Editor
{
    [Serializable]
    public class InspectorClassGroupModule
    {
        #region Strings
        internal const string PARENT_SCRIPT_TITLE = "Parent Script";
        internal const string CURRENT_SCRIPT_TITLE = "Script";
        
        internal const string FOLDER_FILTER = "Assets/Scripts";
        #endregion
        
        #region Colors
        internal Color32 classNameColor = new(81, 84, 84, 255);
        internal Color32 classHeaderNameColor = new(81, 84, 84, 255);
        #endregion
        
        #region GUIStyle
        internal GUIStyle classNameHeaderStyle;
        internal GUIStyle classNameStyle;
        internal GUIStyle classScriptStyle;
        internal GUIStyle editScriptStyle;
        internal GUIStyle foldoutStyle;
        #endregion
        
        #region Parent Child
        internal bool isInheritingFromCustomClass;
        internal bool iteratedToChildClass;
        #endregion
        
        #region Script Object
        internal List<(UnityEngine.Object classObject, string classPath)> scriptObjects;
        #endregion

        internal void OnCreated(SerializedObject serializedObj, bool usingCustomParentClass)
        {
            isInheritingFromCustomClass = usingCustomParentClass;
            iteratedToChildClass = false;
            
            #region Set GUIStyle
            classNameHeaderStyle = new()
            {
                fontSize = 10,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                wordWrap = true,
                normal =
                {
                    textColor = Color.white
                },
                padding = new RectOffset(0, 0, 0, 0)
            };
            
            classNameStyle = new()
            {
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                wordWrap = true,
                normal =
                {
                    textColor = Color.white
                },
                padding = new RectOffset(0, 0, 0, 0)
            };

            classScriptStyle = new()
            {
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                wordWrap = true,
                normal =
                {
                    textColor = Color.white,
                },
                active = 
                {
                    textColor = Color.gray,
                    background = Texture2D.whiteTexture,
                },
                padding = new RectOffset(0, 0, 0, 0),
            };
            
            editScriptStyle = new()
            {
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                wordWrap = true,
                padding = new RectOffset(0, 0, 0, 0),
            };
            
            foldoutStyle = new()
            {
                fontSize = 12,
                fontStyle = FontStyle.Italic,
                alignment = TextAnchor.UpperLeft,
                wordWrap = false,
                normal =
                {
                    textColor = Color.white
                },
                padding = new RectOffset(20, 0, 0, 0),
            };
            #endregion

            scriptObjects = new();
            
            if (isInheritingFromCustomClass)
            {
                var parentScriptName = Inspector.GetParentScriptName(serializedObj);
                var (parentObj, parentPath) = Inspector.GetScriptObjectAndPath(parentScriptName);
                
                scriptObjects.Add((parentObj, parentPath));
            }
            
            var currentScriptName = Inspector.GetCurrentScriptName(serializedObj);
            var (currentObj, currentPath) = Inspector.GetScriptObjectAndPath(currentScriptName);
            
            scriptObjects.Add((currentObj, currentPath));
        }

        internal (UnityEngine.Object obj, string path) GetClassObjectAndPath(bool getParent)
        {
            if (getParent)
            {
                return scriptObjects[0];
            }

            return isInheritingFromCustomClass ? scriptObjects[1] : scriptObjects[0];
        }
    }
}