using System;
using UnityEngine;

namespace com.Klazapp.Editor
{
    internal class InspectorHeaderModule
    {
        internal InspectorHeaderComponent monoBehaviourHeaderComponent;
        internal InspectorHeaderComponent scriptableObjectHeaderComponent;
        internal InspectorHeaderComponent todoHeaderComponent;

        internal void OnCreated()
        {
            monoBehaviourHeaderComponent = new InspectorHeaderComponent
            {
                header = "",
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
            };

            scriptableObjectHeaderComponent = new InspectorHeaderComponent
            {
                header = "",
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
            };

            todoHeaderComponent = new InspectorHeaderComponent
            {
                header = "",
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
            };
        }

        internal InspectorHeaderComponent GetHeaderComponentByType(InspectorHeaderType inspectorHeaderType, string description)
        {
            switch (inspectorHeaderType)
            {
                case InspectorHeaderType.MonoBehaviour:
                    monoBehaviourHeaderComponent.header = description;
                    return monoBehaviourHeaderComponent;
                case InspectorHeaderType.ScriptableObject:
                    scriptableObjectHeaderComponent.header = description;
                    return scriptableObjectHeaderComponent;
                case InspectorHeaderType.Todo:
                    todoHeaderComponent.header = "TODO: " + description;
                    return todoHeaderComponent;
                default:
                    throw new ArgumentOutOfRangeException(nameof(inspectorHeaderType), inspectorHeaderType, null);
            }
        }
    }
}