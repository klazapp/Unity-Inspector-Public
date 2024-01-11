using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using com.Klazapp.Utility;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace com.Klazapp.Editor
{
    //TODO: Draw in drop down of each individual element as unity does for debug mode
    public partial class Inspector
    {
        //All properties of this script
        private List<SerializedProperty> serializedProperties;
        private List<SerializedProperty> parentSerializedProperties;
        private List<SerializedProperty> childSerializedProperties;

        #region Foldout Variables (of script classes)
        private readonly List<bool> classFoldouts = new();
        private int classFoldoutIndex = 0;
        #endregion
        
        #region Foldout Variables (with children)
        private List<bool> childrenPropertyFoldouts = new();
        private int childrenPropertyFoldoutIndex = 0;
        private float2 scrollPos;
        #endregion
        
        #region Foldout Textures
        public Texture2D foldoutDownTex;
        public Texture2D foldoutUpTex;
        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnCreatedProperties()
        {
            ResetClassGroup();
            
            childrenPropertyFoldouts = new();
            for (var i = 0; i < 2; i++)
            {
                classFoldouts.Add(i != 0);
            }
            classFoldoutIndex = 0;
            
            var serializedProperty = serializedObject.GetIterator();
            var hasChildren = serializedProperty.NextVisible(enterChildren: true);
         
            if (!hasChildren)
                return;

            childrenPropertyFoldouts = new();
   
            while (serializedProperty.NextVisible(enterChildren: false))
            {
                if (serializedProperty.hasVisibleChildren)
                {
                    childrenPropertyFoldouts.Add(false);
                }
            }
            childrenPropertyFoldoutIndex = 0;

            serializedProperties = new();
            serializedProperties = GetAllProperties(serializedObject);

            parentSerializedProperties = new();
            childSerializedProperties = new();
            (parentSerializedProperties, childSerializedProperties) = SeparateProperties(serializedObject, serializedProperties);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnDisplayProperties()
        {
            switch (InspectorViewHandlerModule.inspectorViewHandlerMode)
            {
                //Draw according to current inspector mode
                case InspectorViewHandlerMode.Custom:
                {
                    childrenPropertyFoldoutIndex = 0;
                    classFoldoutIndex = 0;

                    if (serializedProperties == null)
                    {
                        OnCreatedProperties();
                    }

                    if (serializedProperties == null)
                        return;
                
                    //Draw parent properties
                    DrawProperties(parentSerializedProperties, true);
                
                    //Draw child properties
                    DrawProperties(childSerializedProperties, false);
                    break;
                }
                case InspectorViewHandlerMode.Classic:
                    base.OnInspectorGUI();
                    break;
                case InspectorViewHandlerMode.Debug:
                {
                    //Get all fields from target object
                    var fields = target.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                    foreach (var field in fields)
                    {
                        //Find corresponding SerializedProperty
                        var property = serializedObject.FindProperty(field.Name);

                        //TODO: Draw in drop down of each individual element as unity does for debug mode
                        if (property != null)
                        {
                            EditorGUILayout.PropertyField(property, true);
                        }
                    }

                    break;
                }
                default:
                {
                    childrenPropertyFoldoutIndex = 0;
                    classFoldoutIndex = 0;

                    if (serializedProperties == null)
                    {
                        OnCreatedProperties();
                    }

                    if (serializedProperties == null)
                        return;
                
                    //Draw parent properties
                    DrawProperties(parentSerializedProperties, true);
                
                    //Draw child properties
                    DrawProperties(childSerializedProperties, false);
                    break;
                }
            }
        }
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void DrawProperties(List<SerializedProperty> properties, bool isParent)
        {
            if (isParent && !inspectorClassGroupModule.isInheritingFromCustomClass)
            {
                classFoldoutIndex++;
                return;
            }

            StartClassGroup(isParent);
            
            //Negative padding to account for additional spaces added for foldout properties
            CustomEditorHelper.DrawSpace(-25);
            
            classFoldouts[classFoldoutIndex] = CustomEditorHelper.DrawColoredFoldout(0, 0, "",
                inspectorClassGroupModule.foldoutStyle, new Color32(15, 255, 255, 0),
                classFoldouts[classFoldoutIndex], foldoutDownTex, foldoutUpTex, false);

            if (classFoldouts[classFoldoutIndex])
            {
                foreach (var prop in properties)
                {
                    var hasReadOnly = CheckIfHasAttribute<ReadOnlyAttribute>(prop, true);
                    var hasNotes = CheckIfHasAttribute<NoteAttribute>(prop, true);

                    var childrenProperties = GetChildren(prop);

                    DrawPropertyWithChildren(prop, childrenProperties, hasReadOnly, hasNotes);

                    CustomEditorHelper.DrawSpace(20);
                }
            }
            
            EndClassGroup();
            classFoldoutIndex++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void DrawPropertyWithChildren(SerializedProperty prop, List<SerializedProperty> childrenProperties, bool hasReadOnly, bool hasNotes)
        {
            if (childrenProperties.Count > 0)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                EditorGUILayout.BeginVertical(GUILayout.Width((int)EditorGUIUtility.currentViewWidth - 200));

                if (!hasReadOnly)
                {
                    DrawNotes((int)EditorGUIUtility.currentViewWidth - 200, 20, hasNotes, prop);
                }
                else
                {
                    DrawNotes((int)EditorGUIUtility.currentViewWidth - 200, 20, hasNotes, prop);
                    DrawReadOnly((int)EditorGUIUtility.currentViewWidth - 200, 20, prop, hasNotes);
                }

                childrenPropertyFoldouts[childrenPropertyFoldoutIndex] = CustomEditorHelper.DrawColoredFoldout(0, 0, prop.name,
                    inspectorClassGroupModule.foldoutStyle, new Color32(15, 255, 255, 135),
                    childrenPropertyFoldouts[childrenPropertyFoldoutIndex], foldoutDownTex, foldoutUpTex,
                    hasReadOnly);

                if (childrenPropertyFoldouts[childrenPropertyFoldoutIndex])
                {
                    EditorGUILayout.BeginVertical(GUILayout.Width((int)EditorGUIUtility.currentViewWidth - 150));

                    foreach (var childProperty in childrenProperties)
                    {
                        EditorGUILayout.BeginVertical();
                        hasReadOnly = CheckIfHasAttribute<ReadOnlyAttribute>(childProperty, false);
                        hasNotes = CheckIfHasAttribute<NoteAttribute>(childProperty, false);

                        //If parent has no read only, render each children's read only
                        if (!CheckIfHasAttribute<ReadOnlyAttribute>(prop, true))
                        {
                            DrawNotes((int)EditorGUIUtility.currentViewWidth - 200, 20, hasNotes,
                                childProperty);
                            DrawReadOnly((int)EditorGUIUtility.currentViewWidth - 200, 20, childProperty,
                                hasReadOnly);

                            CustomEditorHelper.DrawProperty(childProperty,
                                (int)EditorGUIUtility.currentViewWidth - 200, 0, hasReadOnly);
                        }
                        else
                        {
                            DrawNotes((int)EditorGUIUtility.currentViewWidth - 200, 20, hasNotes, childProperty,
                                true);
                            CustomEditorHelper.DrawProperty(childProperty, (int)EditorGUIUtility.currentViewWidth - 200, 0, true);
                        }
                        EditorGUILayout.EndVertical();
                    }
                  
                    EditorGUILayout.EndVertical();
                }

                childrenPropertyFoldoutIndex++;

                EditorGUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                hasReadOnly = CheckIfHasAttribute<ReadOnlyAttribute>(prop, true);
                hasNotes = CheckIfHasAttribute<NoteAttribute>(prop, true);

                DrawNotes((int)EditorGUIUtility.currentViewWidth - 200, 20, hasNotes, prop);
                DrawReadOnly((int)EditorGUIUtility.currentViewWidth - 200, 20, prop, hasReadOnly);

                CustomEditorHelper.DrawProperty(prop, (int)EditorGUIUtility.currentViewWidth - 200, 0,
                    hasReadOnly);
            }

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static List<SerializedProperty> GetAllProperties(SerializedObject serializedObject)
        {
            var properties = new List<SerializedProperty>();

            //Get first property of the serialized object
            var iterProperty = serializedObject.GetIterator();

            //Enter first property
            if (!iterProperty.NextVisible(true))
                return properties;
            
            while (iterProperty.NextVisible(false))
            {
                //Add copy of current property to list
                var propCopy = iterProperty.Copy();
                properties.Add(propCopy);
            }

            return properties;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static (List<SerializedProperty> parentProperties, List<SerializedProperty> childProperties) SeparateProperties(SerializedObject serializedObject, IEnumerable<SerializedProperty> properties)
        {
            var childProperties = new List<SerializedProperty>();
            var parentProperties = new List<SerializedProperty>();

            // Get the type of the target object
            var targetType = serializedObject.targetObject.GetType();

            foreach (var property in properties)
            {
                // Get the FieldInfo for this property, searching the entire hierarchy
                var fieldInfo = targetType.GetField(property.propertyPath, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

                if (fieldInfo == null) 
                    continue;
                
                if (fieldInfo.DeclaringType == targetType)
                {
                    // Property belongs to the child class
                    childProperties.Add(property);
                }
                else
                {
                    // Property belongs to a parent class
                    parentProperties.Add(property);
                }
            }

            return (parentProperties, childProperties);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static List<SerializedProperty> GetChildren(SerializedProperty serializedProperty)
        {
            var children = new List<SerializedProperty>();

            if (serializedProperty == null)
                return children;
            
            //Check if property is struct
            var propertyIsStruct = IsStruct(serializedProperty);
            
            if (!serializedProperty.hasChildren)
                return children;

            if (propertyIsStruct)
                return children;
            
            var child = serializedProperty.Copy();
            var parentDepth = serializedProperty.depth;
            var parentPath = serializedProperty.propertyPath;

            //Move to first child
            if (!child.NextVisible(true))
                return children;
            
            do
            {
                //Check if child property is direct child of the parent
                if (child.depth == parentDepth + 1 && IsDirectChildOfPath(parentPath, child.propertyPath))
                {
                    children.Add(child.Copy());
                }
            }
            while (child.NextVisible(false));

            return children;
        }
        
        private static bool IsDirectChildOfPath(string parentPath, string childPath)
        {
            //Construct the expected start of the direct child's path
            var expectedStart = parentPath + ".";
            if (!childPath.StartsWith(expectedStart)) return false;

            //Check if the remainder of the child's path contains no further dots
            var remainder = childPath[expectedStart.Length..];
            return !remainder.Contains(".");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsStruct(SerializedProperty property)
        {
            var obj = GetTargetObjectOfProperty(property);

            if (obj == null)
            {
                return false;
            }
            
            var type = obj.GetType();

            return type.IsValueType && !type.IsPrimitive && !type.IsEnum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static object GetTargetObjectOfProperty(SerializedProperty property)
        {
            var path = property.propertyPath.Replace(".Array.data[", "[");
            object obj = property.serializedObject.targetObject;
            var elements = path.Split('.');
            foreach (var element in elements)
            {
                if (element.Contains("["))
                {
                    var elementName = element[..element.IndexOf("[", StringComparison.Ordinal)];
                    var index = Convert.ToInt32(element[element.IndexOf("[", StringComparison.Ordinal)..].Replace("[", "").Replace("]", ""));
                    obj = GetValue(obj, elementName, index);
                }
                else
                {
                    obj = GetValue(obj, element);
                }
            }
            return obj;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static object GetValue(object source, string name)
        {
            if (source == null)
                return null;
            
            var type = source.GetType();
            
            var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            if (f != null) 
                return f.GetValue(source);
            
            var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
         
            return p != null ? p.GetValue(source, null) : null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static object GetValue(object source, string name, int index)
        {
            var enumerable = GetValue(source, name) as IEnumerable;
            
            var enm = enumerable.GetEnumerator();

            for (var i = 0; i <= index; i++)
            {
                if (!enm.MoveNext()) return null;
            }
            
            return enm.Current;
        }
    }
}
