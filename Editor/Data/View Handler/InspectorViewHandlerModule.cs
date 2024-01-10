using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace com.Klazapp.Editor
{
    [Serializable]
    public class InspectorViewHandlerModule
    {
        internal static InspectorViewHandlerMode inspectorViewHandlerMode;
        
        internal InspectorViewHandlerComponent customViewComponent;
        internal InspectorViewHandlerComponent classicViewComponent;
        internal InspectorViewHandlerComponent debugViewComponent;

        internal GUIStyle viewContentStyle;

        public void OnCreated()
        {
            #region Set GUIStyle
            viewContentStyle = new()                                               
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
            #endregion

            ResetView();
            
            CreateCustomViewComponent();
            CreateClassViewComponent();
            CreateDebugViewComponent();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ResetView()
        {
            inspectorViewHandlerMode = InspectorViewHandlerMode.Custom;   
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CreateCustomViewComponent()
        {
            customViewComponent = new();
            customViewComponent.pointerHoverColor = new(175, 135, 54, 255);
            customViewComponent.pointerDownColor = new(71, 103, 255, 255);
            customViewComponent.pointerUpColor = new(44, 35, 44, 0);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CreateClassViewComponent()
        {
            classicViewComponent = new();
            classicViewComponent.pointerHoverColor = new(175, 135, 54, 255);  
            classicViewComponent.pointerDownColor = new(25, 25, 25, 255);    
            classicViewComponent.pointerUpColor = new(44, 35, 44, 0);       
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CreateDebugViewComponent()
        {
            debugViewComponent = new();
            debugViewComponent.pointerHoverColor = new(175, 135, 54, 255);  
            debugViewComponent.pointerDownColor = new(255, 71, 123, 255);    
            debugViewComponent.pointerUpColor = new(44, 35, 44, 0);       
        }
    }
}