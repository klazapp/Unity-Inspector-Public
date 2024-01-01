using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using com.Klazapp.Utility;
using UnityEditor;
using UnityEngine;

namespace com.Klazapp.Editor
{
    public partial class Inspector 
    {
        private readonly List<InspectorHeaderComponent> noteComponents = new();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnCreateHeader()
        {
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
                                    GetNoteComponentByType(InspectorHeaderType.MonoBehaviour, scriptHeader.description));
                                break;
                            case TodoHeader todoHeader:
                                this.noteComponents.Add(
                                    GetNoteComponentByType(InspectorHeaderType.Todo, todoHeader.description));
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
                                    GetNoteComponentByType(InspectorHeaderType.ScriptableObject, scriptHeader.description));
                                break;
                            case TodoHeader todoHeader:
                                this.noteComponents.Add(
                                    GetNoteComponentByType(InspectorHeaderType.Todo, todoHeader.description));
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

            for (var i = 0; i < noteComponents.Count; i++)
            {
                //Set new background color for class notes
                GUI.backgroundColor = noteComponents[i].headerColor;
            
                EditorGUILayout.BeginVertical(GUI.skin.window);
                
                EditorGUILayout.LabelField(noteComponents[i].header, noteComponents[i].headerStyle, 
                    GUILayout.ExpandWidth(true), 
                    GUILayout.Height(GetNoteHeightByType(noteComponents[i].headerType)));
            
                //Set back original background color
                GUI.backgroundColor = defaultColor;
                
                EditorGUILayout.Space(20);
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(20);
            }

            //Set back original background color
            GUI.backgroundColor = defaultColor;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetNoteHeightByType(InspectorHeaderType headerType) => headerType switch
        {
            InspectorHeaderType.MonoBehaviour => 40,
            InspectorHeaderType.ScriptableObject => 40,
            InspectorHeaderType.Todo => 20,
            _ => 40
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static InspectorHeaderComponent GetNoteComponentByType(InspectorHeaderType headerType, string description) => headerType switch
        {
            InspectorHeaderType.MonoBehaviour => new InspectorHeaderComponent
            {
                header = description,
                headerColor = new(135, 215, 209, 255),
                headerStyle = new()
                {
                    fontStyle = FontStyle.Bold,
                    fontSize = 15,
                    alignment = TextAnchor.MiddleCenter,
                    normal = new GUIStyleState
                    {
                        textColor = Color.white,
                    },
                    wordWrap = true,
                },
                headerType = InspectorHeaderType.MonoBehaviour,   
            },
            InspectorHeaderType.ScriptableObject => new InspectorHeaderComponent
            {
                header = description,
                headerColor = new(115, 135, 255, 255),
                headerStyle = new()
                {
                    fontStyle = FontStyle.Bold,
                    fontSize = 15,
                    alignment = TextAnchor.MiddleCenter,
                    normal = new GUIStyleState
                    {
                        textColor = Color.white,
                    },
                    wordWrap = true,
                },
                headerType = InspectorHeaderType.ScriptableObject,
            },
            InspectorHeaderType.Todo => new InspectorHeaderComponent
            {
                header = "TODO: " + description,
                headerColor = new(235, 115, 109, 255),
                headerStyle = new()
                {
                    fontStyle = FontStyle.Italic,
                    fontSize = 13,
                    alignment = TextAnchor.MiddleLeft,
                    normal = new GUIStyleState
                    {
                        textColor = Color.white,
                    },
                    wordWrap = true,
                },
                headerType = InspectorHeaderType.Todo,
                
            },
            _ => throw new ArgumentOutOfRangeException(nameof(headerType), headerType, null)
        };
    }
}
