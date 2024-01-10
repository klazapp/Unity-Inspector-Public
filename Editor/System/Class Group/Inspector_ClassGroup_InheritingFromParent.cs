using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEditor;

namespace com.Klazapp.Editor
{
    public partial class Inspector 
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsInheritingFromParentClass(SerializedObject serializedObj)
        {
            if (serializedObj == null)
            {
                return false;
            }

            var targetType = serializedObj.targetObject.GetType();
            var baseType = targetType.BaseType;
           
            //Check if base type is not null and not built-in system or Unity type
            while (baseType != null && baseType != typeof(object))
            {
                //Check if assembly of base type is not built-in assembly
                if (!IsBuiltInAssembly(baseType.Assembly))
                {
                    return true;
                }

                baseType = baseType.BaseType;
            }

            return false;
            
            #region Local Functions
            bool IsBuiltInAssembly(Assembly assembly)
            {
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
