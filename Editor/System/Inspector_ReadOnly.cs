using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace com.Klazapp.Editor
{
    public partial class Inspector
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DrawReadOnly(SerializedProperty serializedProperty, bool hasNotes)
        {
            GUIStyle readOnlyTitleStyle = new()
            {
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                wordWrap = true,
                normal =
                {
                    textColor = Color.white
                },
                padding = new RectOffset(5, 0, 0, 0)
            };

            GUI.enabled = false;
            CustomEditorHelper.DrawBox((int)EditorGUIUtility.currentViewWidth - 200, 20, new Color32(34, 45, 54, 255), "READ ONLY", readOnlyTitleStyle);
                    
            DrawNotes(hasNotes, serializedProperty);
                    
            CustomEditorHelper.DrawProperty(serializedProperty, (int)EditorGUIUtility.currentViewWidth - 200, 0, true);

            GUI.enabled = true;
        }
    }
}