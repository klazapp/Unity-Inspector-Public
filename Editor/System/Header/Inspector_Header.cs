using System.Collections.Generic;
using System.Runtime.CompilerServices;
using com.Klazapp.Utility;
using UnityEditor;
using UnityEngine;

namespace com.Klazapp.Editor
{
    public partial class Inspector
    {
        internal InspectorHeaderModule inspectorHeaderModule = new();
        
        internal readonly List<InspectorHeaderComponent> noteComponents = new();

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnCreatedHeader()
        {
            inspectorHeaderModule = new();
            inspectorHeaderModule.OnCreated();
            
            var targetObjects = serializedObject.targetObjects;

            //Get last target object
            var lastTargetObject = targetObjects[^1];

            //Get all attributes associated with said object
            var attributes = lastTargetObject.GetType().GetCustomAttributes(inherit: false);
            
            switch (lastTargetObject)
            {
                case MonoBehaviour monoBehaviour:
                    foreach (var attr in attributes)
                    {
                        switch (attr)
                        {
                            case ScriptHeader scriptHeader:
                                this.noteComponents.Add(
                                    GetHeaderComponentByType(InspectorHeaderType.MonoBehaviour, scriptHeader.description));
                                break;
                            case TodoHeader todoHeader:
                                this.noteComponents.Add(
                                    GetHeaderComponentByType(InspectorHeaderType.Todo, todoHeader.description));
                                break;
                        }
                    }
                    break;
                case ScriptableObject scriptableObject:
                    foreach (var attr in attributes)
                    {
                        switch (attr)
                        {
                            case ScriptHeader scriptHeader:
                                this.noteComponents.Add(
                                    GetHeaderComponentByType(InspectorHeaderType.ScriptableObject, scriptHeader.description));
                                break;
                            case TodoHeader todoHeader:
                                this.noteComponents.Add(
                                    GetHeaderComponentByType(InspectorHeaderType.Todo, todoHeader.description));
                                break;
                        }
                    }
                    break;
                default:
                    //Enable log message if required
                    //LogMessage.DebugWarning("The property is part of a type that is neither a component nor a scriptable object.");
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnDisplayHeader()
        {
            EditorGUILayout.Space(20);

            //Retrieve original background color
            var defaultColor = GUI.backgroundColor;

            foreach (var noteComponent in noteComponents)
            {
                //Set new background color for class notes
                GUI.backgroundColor = noteComponent.headerColor;
            
                EditorGUILayout.BeginVertical(GUI.skin.window);
                
                CustomEditorHelper.DrawBox(0, GetHeaderHeightByType(noteComponent.headerType), new Color32(255, 255, 255, 0), noteComponent.header, noteComponent.headerStyle);
           
                //Set back original background color
                GUI.backgroundColor = defaultColor;
                
                EditorGUILayout.Space(20);
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(20);
            }

            //Set back original background color
            GUI.backgroundColor = defaultColor;
        }
    }
}
