using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace com.Klazapp.Editor
{
    //Checks for object's data such as:
    //Is user created MonoBehaviour/ScriptableObject?
    //Is inheriting from user defined class?
    public partial class Inspector 
    {
        //Checks to see if script is user created MonoBehaviour or ScriptableObject
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsValidObject()
        {
            var targetObjects = serializedObject.targetObjects;

            //Get last target object
            var lastTargetObject = targetObjects[^1];

            //Check if lastTargetObject is MonoBehaviour or ScriptableObject
            if (lastTargetObject is MonoBehaviour or ScriptableObject)
            {
                return !IsBuiltInAssembly(serializedObject);
            }

            return false;
            
            #region Local Functions
            bool IsBuiltInAssembly(SerializedObject serializedObj)
            {
                var targetType = serializedObj.targetObject.GetType();
                var assembly = targetType.Assembly;
            
                //Check if assembly is built-in .NET or Unity assembly
                return assembly.FullName.StartsWith("System")
                       || assembly.FullName.StartsWith("mscorlib")
                       || assembly.FullName.StartsWith("UnityEngine")
                       || assembly.FullName.StartsWith("UnityEditor");
            }
            #endregion
        }
    }
}
