using System.Runtime.CompilerServices;
using com.Klazapp.Utility;
using UnityEditor;

namespace com.Klazapp.Editor
{
    public partial class Inspector
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnDisplayProperties()
        {
            serializedObject.Update();

            ResetClass();

            var serializedProperty = serializedObject.GetIterator();
            var hasChildren = serializedProperty.NextVisible(enterChildren: true);
         
            if (!hasChildren)
                return;

            StartClassGroup(GetClassName(true), scriptIcon, isInheritingFromCustomClass ? "Parent Class" : "Current Class");
            
            while (serializedProperty.NextVisible(enterChildren: false))
            {
                var hasReadOnly = CheckIfHasAttribute<ReadOnlyAttribute>(serializedProperty, true);
                var hasNotes = CheckIfHasAttribute<NoteAttribute>(serializedProperty, true);

                CheckIfInheritingFromParentClass(serializedProperty);
              
                
                if (!hasReadOnly)
                {
                    DrawNotes(hasNotes, serializedProperty);
                    CustomEditorHelper.DrawProperty(serializedProperty, (int)EditorGUIUtility.currentViewWidth - 200, 20, false);
                }
                else
                {
                    DrawReadOnly(serializedProperty, hasNotes);
                }
                
                CustomEditorHelper.DrawSpace(20);
            }
            
            EndClassGroup();
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}