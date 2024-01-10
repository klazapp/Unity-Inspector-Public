using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace com.Klazapp.Editor
{
    [Serializable]
    public class InspectorViewHandlerComponent
    {
        internal Color32 pointerHoverColor = new(175, 135, 54, 255);
        internal Color32 pointerDownColor = new(75, 135, 54, 255);
        internal Color32 pointerUpColor = new(44, 35, 44, 255);

        internal bool pointerDown;
        internal bool pointerUp;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Color32 GetColorByClickState(bool pointerIsDown, bool pointerIsUp)
        {
            return pointerIsDown ? pointerDownColor : pointerUpColor;
        }
    }
}