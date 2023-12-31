using System.Runtime.CompilerServices;
using com.Klazapp.Utility;
using UnityEditor;
using UnityEngine;

namespace com.Klazapp.Editor
{
    public partial class Inspector
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnDisplayProperties()
        {
            serializedObject.Update();
            
            var serializedProperty = serializedObject.GetIterator();
            var hasChildren = serializedProperty.NextVisible(enterChildren: true);
         
            if (!hasChildren)
                return;

            var startedGroup = false;
            
            while (serializedProperty.NextVisible(enterChildren: false))
            {
                var hasReadOnly = CheckIfHasAttribute<ReadOnlyAttribute>(serializedProperty, true);
                var hasNotes = CheckIfHasAttribute<NoteAttribute>(serializedProperty, true);

                if (!hasReadOnly)
                {
                    if (hasNotes)
                    {
                        EndGroup(startedGroup);
                        
                        StartGroup();
                        startedGroup = true;
                    }
                    
                    CustomEditorHelper.DrawProperty(serializedProperty, (int)EditorGUIUtility.currentViewWidth - 200, 0, false);
                }
                else
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
                  
                    CustomEditorHelper.DrawBox((int)EditorGUIUtility.currentViewWidth - 200, 20, new Color32(34, 45, 54, 255), "READ ONLY", readOnlyTitleStyle);
                    CustomEditorHelper.DrawProperty(serializedProperty, (int)EditorGUIUtility.currentViewWidth - 200, 0, true);
                }
                
                CustomEditorHelper.DrawSpace(20);
            }

            EndGroup(startedGroup);
            
            serializedObject.ApplyModifiedProperties();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void StartGroup()
        {
            EditorGUILayout.Space(30);
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical(GUILayout.Width(EditorGUIUtility.currentViewWidth - 60));
        
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void EndGroup(bool startedGroup)
        {
            if (!startedGroup) 
                return;
            
            EditorGUILayout.Space(10);
            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
    }
}