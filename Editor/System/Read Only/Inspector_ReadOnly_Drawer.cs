using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace com.Klazapp.Editor
{
    public partial class Inspector
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DrawReadOnly(int width = 0, int height = 0, SerializedProperty serializedProperty = null, bool hasReadOnly = true)
        {
            if (!hasReadOnly)
                return;
            
            GUIStyle readOnlyTitleStyle = new()
            {
                fontSize = 12,
                fontStyle = FontStyle.Normal,
                alignment = TextAnchor.MiddleLeft,
                wordWrap = true,
                normal =
                {
                    textColor = Color.white
                },
                padding = new RectOffset(5, 0, 0, 0)
            };

            GUI.enabled = false;
            
            CustomEditorHelper.DrawBox(width, height, new Color32(34, 45, 54, 255), "Read Only", readOnlyTitleStyle);

            GUI.enabled = true;
        }
    }
}