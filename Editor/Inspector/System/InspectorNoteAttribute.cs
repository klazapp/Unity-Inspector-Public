using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using com.Klazapp.Utility;

namespace com.Klazapp.Editor
{
    [CustomPropertyDrawer(typeof(NoteAttribute))]
    public class InspectorNoteAttribute : DecoratorDrawer
    {
        private readonly Color32 titleNoteColor = new(64, 86, 112, 255);
        private readonly Color32 descriptionNoteColor = new(63, 72, 75, 255);

        private NoteAttribute NoteAttribute => ((NoteAttribute)attribute);

        public override float GetHeight()
        {
            if (string.IsNullOrEmpty(NoteAttribute.noteDescription))
                return base.GetHeight() + 30f;
            
            return base.GetHeight() + 50f;
        }

        public override void OnGUI(Rect position)
        {
            var oldGuiColor = GUI.color;

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

            var rect = position;
            rect.y += 10;
            
            var titleBoxRect = new Rect(rect.x, rect.y, rect.width, 30f);
       
            DrawUIBox(titleBoxRect, descriptionNoteColor, titleNoteColor, 4);

            var titleRect = new Rect(titleBoxRect.x + 5, titleBoxRect.y, titleBoxRect.width, titleBoxRect.height);
            EditorGUI.LabelField(titleRect, NoteAttribute.noteTitle, titleStyle);
            
            //Draw note description
            if (!string.IsNullOrEmpty(NoteAttribute.noteDescription))
            {
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
                
                var descriptionBoxRect = new Rect(titleRect.x - 5, titleRect.y + 30, titleRect.width, 20f);
                EditorGUI.DrawRect(descriptionBoxRect, descriptionNoteColor);
                
                var descriptionRect = new Rect(titleRect.x, titleRect.y + 25, titleRect.width, titleRect.height);
                EditorGUI.LabelField(descriptionRect, NoteAttribute.noteDescription, descriptionStyle);
            }

            GUI.color = oldGuiColor;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DrawUIBox(Rect rect, Color borderColor, Color backgroundColor, int width = 2)
        {
            var outer = new Rect(rect);
            var inner = new Rect(rect.x + width, rect.y + width, rect.width - width * 2, rect.height - width * 2);
            EditorGUI.DrawRect(outer, borderColor);
            EditorGUI.DrawRect(inner, backgroundColor);
        }
    }
}