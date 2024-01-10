using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace com.Klazapp.Editor
{
    public partial class Inspector
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DrawNotes(int width = 0, int height = 0, bool hasNotes = false, SerializedProperty serializedProperty = null, bool hasReadOnly = false)
        {
            if (!hasNotes)
                return;

            if (hasReadOnly)
            {
                GUI.enabled = false;
            }
            
            var (noteTitle, noteDescription) = GetNoteAttributeDetails(serializedProperty, true);

            if (!string.IsNullOrEmpty(noteTitle))
            {
                var titleStyle = new GUIStyle()
                {
                    wordWrap = true,
                    fontStyle = FontStyle.Bold,
                    fontSize = 13,
                    normal = new GUIStyleState
                    {
                        textColor = Color.white,
                    },
                    alignment = TextAnchor.MiddleLeft,
                };
                    
                CustomEditorHelper.DrawBoxWithBackground(width, height, 5, new Color32(55, 215, 212, 15), new Color32(64, 86, 112, 255), noteTitle, titleStyle);
            }

            if (string.IsNullOrEmpty(noteDescription)) 
                return;
            
            var descriptionStyle = new GUIStyle()
            {
                wordWrap = true,
                fontStyle = FontStyle.Italic,
                fontSize = 10,
                normal = new GUIStyleState
                {
                    textColor = Color.white,
                },
                alignment = TextAnchor.MiddleLeft,
            };
                    
            CustomEditorHelper.DrawBoxWithBackground((int)EditorGUIUtility.currentViewWidth - 200, 10, 5, new Color32(55, 215, 212, 15), new Color32(64, 66, 62, 255), noteDescription, descriptionStyle);
            
            if (hasReadOnly)
            {
                GUI.enabled = true;
            }
        }
    }
}