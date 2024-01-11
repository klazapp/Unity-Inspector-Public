using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using com.Klazapp.Utility;

namespace com.Klazapp.Editor
{
    public partial class Inspector
    {
        private readonly InspectorEditScriptComponent inspectorEditScriptComponent = new();
        private readonly InspectorOpenScriptLocationComponent inspectorOpenScriptLocationComponent = new();
        
        public Texture2D editScriptIcon;
        public Texture2D openScriptLocationIcon;
        public Texture2D scriptIcon;
        public Texture2D scriptIcon2;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnCreatedScriptHandler()
        {
            
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnDisplayEditScript()
        {
            GUIStyle openScriptContentStyle = new()
            {
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                wordWrap = true,
                normal =
                {
                    textColor = Color.white
                }
            };

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical();
            
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            
            CustomEditorHelper.DrawBox(200, 40, inspectorEditScriptComponent.GetColorByClickState(inspectorEditScriptComponent.pointerDown, inspectorEditScriptComponent.pointerUp), "EDIT SCRIPT", openScriptContentStyle, editScriptIcon);

            CheckEditScriptPointerState();
            
            CustomEditorHelper.DrawSpace(50);
                
            CustomEditorHelper.DrawBox(200, 40, inspectorOpenScriptLocationComponent.GetColorByClickState(inspectorOpenScriptLocationComponent.pointerDown, inspectorOpenScriptLocationComponent.pointerUp), "SCRIPT LOCATION", openScriptContentStyle, openScriptLocationIcon);
    
            CheckOpenScriptLocationPointerState();
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndVertical();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EditScript()
        {
            var targetObjects = serializedObject.targetObjects;

            MonoScript obj;
            var assetPath = "";
            UnityEngine.Object loadedObj;
            
            switch (targetObjects[^1])
            {
                case MonoBehaviour monoBehaviour:
                    obj = MonoScript.FromMonoBehaviour(monoBehaviour);
                    assetPath = AssetDatabase.GetAssetPath(obj);
                    loadedObj = EditorGUIUtility.Load(assetPath);
                    AssetDatabase.OpenAsset(loadedObj);
                    break;
                case ScriptableObject scriptableObject:
                    obj = MonoScript.FromScriptableObject(scriptableObject);
                    assetPath = AssetDatabase.GetAssetPath(obj);
                    loadedObj = EditorGUIUtility.Load(assetPath);
                    AssetDatabase.OpenAsset(loadedObj);
                    break;
                default:
                    LogMessage.DebugError("This is neither a monobehaviour nor a scriptable object");
                    break;
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EditScript(UnityEngine.Object loadedObject, string assetPath)
        {
            AssetDatabase.OpenAsset(loadedObject);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static string GetParentScriptName(SerializedObject serializedObj)
        {
            var targetType = serializedObj.targetObject.GetType();
            var baseType = targetType.BaseType;
            return baseType != null ? GetFriendlyName(baseType) : "";

            #region Local Function
            string GetFriendlyName(Type type)
            {
                if (!type.IsGenericType)
                {
                    return type.Name;
                }

                var name = type.Name;
                var backtickIndex = name.IndexOf('`');
                if (backtickIndex != -1)
                {
                    name = name.Remove(backtickIndex);
                }

                return name;
            }
            #endregion
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static string GetCurrentScriptName(SerializedObject serializedObj)
        {
            var targetType = serializedObj.targetObject.GetType();
            return targetType.Name;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static (UnityEngine.Object loadedObject, string assetPath) GetScriptObjectAndPath(string assetName)
        {
             //Specify looking for script assets via t:script
            var searchFilter = assetName + " t:script";
            string[] searchInFolders = { "Assets", "Packages" };
            var guids = AssetDatabase.FindAssets(searchFilter, searchInFolders);

            List<string> filteredGuids = new List<string>();
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid).ToLower(); // Convert path to lowercase
                string searchKeyword = "klazapp".ToLower(); // Convert search keyword to lowercase

                // Add assets from the 'Assets' folder
                if (path.StartsWith("assets/"))
                {
                    filteredGuids.Add(guid);
                }
                // Add assets from the 'Packages' folder that contain 'klazapp' (case-insensitive) in their path
                else if (path.StartsWith("packages/") && path.Contains(searchKeyword))
                {
                    filteredGuids.Add(guid);
                }
            }
            
            var assetPath = AssetDatabase.GUIDToAssetPath(filteredGuids[^1]);
            var loadedObject = EditorGUIUtility.Load(assetPath);
            
            return (loadedObject, assetPath);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void OpenScriptLocation(UnityEngine.Object loadedObject, string assetPath)
        {
            if (loadedObject == null)
            {
                LogMessage.DebugError("Could not find mono behaviour or scriptable object");
            }
            else
            {
                var assetPathWithoutAsset = System.IO.Path.GetDirectoryName(assetPath);
                var loadedFocusedObj = AssetDatabase.LoadAssetAtPath(assetPathWithoutAsset, typeof(UnityEngine.Object));

                EditorUtility.FocusProjectWindow();

                var pt = Type.GetType("UnityEditor.ProjectBrowser,UnityEditor");

                if (pt == null)
                {
                    LogMessage.DebugError("Unable to find project browser window");
                    return;
                }
                    
                var ins = pt.GetField("s_LastInteractedProjectBrowser", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)?.GetValue(null);

                var showDirMeth = pt.GetMethod("ShowFolderContents", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                if (showDirMeth == null)
                {
                    LogMessage.DebugError("Unable to find directory method info");
                    return;
                }
                    
                showDirMeth.Invoke(ins, new object[]
                {
                    loadedFocusedObj.GetInstanceID(), true
                });
                        
                EditorGUIUtility.PingObject(loadedObject);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]            
        private void CheckEditScriptPointerState()
        {
            if (!inspectorEditScriptComponent.pointerDown)
            {
                inspectorEditScriptComponent.pointerDown = CustomEditorHelper.OnPointerDownInLastRect();
            }

            if (inspectorEditScriptComponent.pointerDown)
            {
                inspectorEditScriptComponent.pointerUp = CustomEditorHelper.OnPointerUpInLastRect();

                if (inspectorEditScriptComponent.pointerUp)
                { 
                    inspectorEditScriptComponent.pointerDown = false;
                    inspectorEditScriptComponent.pointerUp = false;
                    
                    EditScript();
                }
                
                Repaint();
            }

            if (!CustomEditorHelper.OnPointerUp()) 
                return;
            
            if (!CustomEditorHelper.OnPointerLeftRect())
                return;
                
            inspectorEditScriptComponent.pointerDown = false;
            inspectorEditScriptComponent.pointerUp = false;
                    
            Repaint();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckOpenScriptLocationPointerState()
        {
            if (!inspectorOpenScriptLocationComponent.pointerDown)
            {
                inspectorOpenScriptLocationComponent.pointerDown = CustomEditorHelper.OnPointerDownInLastRect();
            }

            if (inspectorOpenScriptLocationComponent.pointerDown)
            {
                inspectorOpenScriptLocationComponent.pointerUp = CustomEditorHelper.OnPointerUpInLastRect();

                if (inspectorOpenScriptLocationComponent.pointerUp)
                {
                    inspectorOpenScriptLocationComponent.pointerDown = false;
                    inspectorOpenScriptLocationComponent.pointerUp = false;

                    var currentScriptName = GetCurrentScriptName(serializedObject);
                    var (loadedObject, assetPath) = GetScriptObjectAndPath(currentScriptName);
                    OpenScriptLocation(loadedObject, assetPath);
                }
            }
            
            if (!CustomEditorHelper.OnPointerUp()) 
                return;
            
            if (!CustomEditorHelper.OnPointerLeftRect())
                return;
                
            inspectorOpenScriptLocationComponent.pointerDown = false;
            inspectorOpenScriptLocationComponent.pointerUp = false;
        }
    }
}
