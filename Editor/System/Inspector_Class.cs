using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

namespace com.Klazapp.Editor
{
    public partial class Inspector
    {
        public Texture2D scriptIcon;
        
        private bool isInheritingFromCustomClass;
        private bool iteratedToChildClass;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ResetClass()
        {
            isInheritingFromCustomClass = IsInheritingFromUserDefinedClass(serializedObject);
            iteratedToChildClass = false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckIfInheritingFromParentClass(SerializedProperty serializedProperty)
        {
            if (!IsInheritingFromUserDefinedClass(serializedObject)) 
                return;
            
            var parentPropertyPaths = GetParentPropertyPath();
                    
            //Check if the property is not in the parent's property paths
            //Single class properties will always be in child class
            if (!parentPropertyPaths.Contains(serializedProperty.name))
            {
                if (iteratedToChildClass) 
                    return;
                
                EndClassGroup();
                StartClassGroup(GetClassName(false), scriptIcon, "Current Class");
                            
                iteratedToChildClass = true;
            }
            else
            {
                //Property likely from the parent class
                iteratedToChildClass = false;
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private List<string> GetParentPropertyPath()
        {
            var targetType = serializedObject.targetObject.GetType();
            var parentType = targetType.BaseType;
            return parentType != null ? GetPropertyPathsOfType(parentType) : new List<string>();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string GetClassName(bool getParentClass)
        {
            //Get property paths of the parent class
            var targetType = serializedObject.targetObject.GetType();
            var parentType = targetType.BaseType;
            var parentPropertyPaths = parentType != null ? GetPropertyPathsOfType(parentType) : new List<string>();
            var className = AddSpacesToSentence(targetType.ToString());

            if (parentType != null)
            {
                className = AddSpacesToSentence(isInheritingFromCustomClass ? parentType.ToString() : targetType.ToString());
            }

            if (!getParentClass)
            {
                className = AddSpacesToSentence(targetType.ToString());
            }

            return className;
        }
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsInheritingFromUserDefinedClass(SerializedObject serializedObj)
        {
            if (serializedObj == null)
            {
                return false;
            }

            var targetType = serializedObj.targetObject.GetType();
            var baseType = targetType.BaseType;

            //Check if the base type is not null and not a built-in system or Unity type
            while (baseType != null && baseType != typeof(object))
            {
                //Check if the assembly of the base type is not a built-in assembly
                if (!IsBuiltInAssembly(baseType.Assembly))
                {
                    return true;
                }

                baseType = baseType.BaseType;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsBuiltInAssembly(Assembly assembly)
        {
            //Check if the assembly is a built-in .NET or Unity assembly
            return assembly.FullName.StartsWith("System")
                   || assembly.FullName.StartsWith("mscorlib")
                   || assembly.FullName.StartsWith("UnityEngine")
                   || assembly.FullName.StartsWith("UnityEditor");
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static List<string> GetPropertyPathsOfType(IReflect type)
        {
            var paths = new List<string>();
            const BindingFlags BINDING_FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var fields = type.GetFields(BINDING_FLAGS);

            foreach (var field in fields)
            {
                var path = field.Name;
                paths.Add(path);
            }

            return paths;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string AddSpacesToSentence(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            var newText = new System.Text.StringBuilder(text.Length * 2);
            newText.Append(text[0]);

            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                    newText.Append(' ');

                newText.Append(text[i]);
            }

            return newText.ToString();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void StartClassGroup(string className, Texture2D scriptTex, string classNameHeaderName)
        {
            var classNameHeaderStyle = new GUIStyle()
            {
                fontSize = 10,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft,
                wordWrap = true,
                normal =
                {
                    textColor = Color.white
                },
                padding = new RectOffset(5, 0, 0, 0)
            };
            
            var classNameStyle = new GUIStyle()
            {
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft,
                wordWrap = true,
                normal =
                {
                    textColor = Color.white
                },
                padding = new RectOffset(5, 0, 0, 0)
            };

            
            CustomEditorHelper.DrawBox(75, 10, new Color32(81, 84, 84, 255), classNameHeaderName, classNameHeaderStyle, null, Alignment.None);
            CustomEditorHelper.DrawBox((int)EditorGUIUtility.currentViewWidth - 20, 20, new Color32(81, 84, 84, 255), className, classNameStyle, scriptTex);
            EditorGUILayout.BeginVertical(GUI.skin.window);
            CustomEditorHelper.DrawSpace(20);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void EndClassGroup()
        {
            EditorGUILayout.EndVertical();
            CustomEditorHelper.DrawSpace(30);
        }
    }
}