using System.Runtime.CompilerServices;

namespace com.Klazapp.Editor
{
    public partial class Inspector 
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetHeaderHeightByType(InspectorHeaderType headerType) => headerType switch
        {
            InspectorHeaderType.MonoBehaviour => 40,
            InspectorHeaderType.ScriptableObject => 40,
            InspectorHeaderType.Todo => 20,
            _ => 40
        };


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private InspectorHeaderComponent GetHeaderComponentByType(InspectorHeaderType headerType,
            string description)
        {
            return inspectorHeaderModule.GetHeaderComponentByType(headerType, description);
        }
        
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // private static InspectorHeaderComponent GetNoteComponentByType(InspectorHeaderType headerType, string description) => headerType switch
        // {
        //     InspectorHeaderType.MonoBehaviour => new InspectorHeaderComponent
        //     {
        //         header = description,
        //         headerColor = new(135, 215, 209, 255),
        //         headerStyle = new()
        //         {
        //             fontStyle = FontStyle.Bold,
        //             fontSize = 15,
        //             alignment = TextAnchor.MiddleCenter,
        //             normal = new GUIStyleState
        //             {
        //                 textColor = Color.white,
        //             },
        //             wordWrap = true,
        //         },
        //         headerType = InspectorHeaderType.MonoBehaviour,   
        //     },
        //     InspectorHeaderType.ScriptableObject => new InspectorHeaderComponent
        //     {
        //         header = description,
        //         headerColor = new(115, 135, 255, 255),
        //         headerStyle = new()
        //         {
        //             fontStyle = FontStyle.Bold,
        //             fontSize = 15,
        //             alignment = TextAnchor.MiddleCenter,
        //             normal = new GUIStyleState
        //             {
        //                 textColor = Color.white,
        //             },
        //             wordWrap = true,
        //         },
        //         headerType = InspectorHeaderType.ScriptableObject,
        //     },
        //     InspectorHeaderType.Todo => new InspectorHeaderComponent
        //     {
        //         header = "TODO: " + description,
        //         headerColor = new(235, 115, 109, 255),
        //         headerStyle = new()
        //         {
        //             fontStyle = FontStyle.Italic,
        //             fontSize = 13,
        //             alignment = TextAnchor.MiddleLeft,
        //             normal = new GUIStyleState
        //             {
        //                 textColor = Color.white,
        //             },
        //             wordWrap = true,
        //         },
        //         headerType = InspectorHeaderType.Todo,
        //         
        //     },
        //     _ => throw new ArgumentOutOfRangeException(nameof(headerType), headerType, null)
        // };
    }
}
