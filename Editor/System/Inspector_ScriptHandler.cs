using System;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using com.Klazapp.Utility;

namespace com.Klazapp.Editor
{
    public partial class Inspector
    {
        private InspectorEditScriptComponent inspectorEditScriptComponent = new();
        private InspectorOpenScriptLocationComponent inspectorOpenScriptLocationComponent = new();
        
        public Texture2D editScriptIcon;
        public Texture2D openScriptLocationIcon;

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
        private void OpenScriptLocation()
        {
            var targetObjects = serializedObject.targetObjects;

            MonoScript obj;
            var assetPath = "";
            UnityEngine.Object loadedObj = new(); 

            switch (targetObjects[^1])
            {
                case MonoBehaviour monoBehaviour:
                    obj = MonoScript.FromMonoBehaviour(monoBehaviour);
                    assetPath = AssetDatabase.GetAssetPath(obj);
                    loadedObj = EditorGUIUtility.Load(assetPath);
                    break;
                case ScriptableObject scriptableObject:
                    obj = MonoScript.FromScriptableObject(scriptableObject);
                    assetPath = AssetDatabase.GetAssetPath(obj);
                    loadedObj = EditorGUIUtility.Load(assetPath);
                    break;
                default:
                    LogMessage.DebugError("This is neither a monobehaviour nor a scriptable object");
                    break;
            }

            if (loadedObj == null)
            {
                LogMessage.DebugError("Could not find monobehaviour or scriptable object");
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
                        
                EditorGUIUtility.PingObject(loadedObj);
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
                    
                    OpenScriptLocation();
                }
                
                Repaint();
            }
            
            if (!CustomEditorHelper.OnPointerUp()) 
                return;
            
            if (!CustomEditorHelper.OnPointerLeftRect())
                return;
                
            inspectorOpenScriptLocationComponent.pointerDown = false;
            inspectorOpenScriptLocationComponent.pointerUp = false;
                    
            Repaint();
        }
    }
}