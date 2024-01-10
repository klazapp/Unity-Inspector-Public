using System.Runtime.CompilerServices;
using UnityEngine;

namespace com.Klazapp.Editor
{
    public partial class Inspector
    {
        private GUISkin originalGUISkin;
        private Color32 originalGUIBgColor;
        private Color32 originalGUIColor;

        private bool storedOriginalSkinProperties;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnCreatedSkinProperties()
        {
            if (storedOriginalSkinProperties)
                return;
            
            StoreSkinProperties();
            
            storedOriginalSkinProperties = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void StoreSkinProperties()
        {
            originalGUISkin = GUI.skin; 
            originalGUIBgColor = GUI.backgroundColor;
            originalGUIColor = GUI.color;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RestoreSkinProperties()
        {
            GUI.skin = originalGUISkin; 
            GUI.backgroundColor = originalGUIBgColor;
            GUI.color = originalGUIColor;
            Repaint();
        }
    }
}