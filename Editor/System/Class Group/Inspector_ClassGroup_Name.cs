using System;
using System.Runtime.CompilerServices;
using System.Linq;

namespace com.Klazapp.Editor
{
    public partial class Inspector
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string GetClassName(bool getParentClass)
        {
            //Get property paths of the parent class
            var targetType = serializedObject.targetObject.GetType();
            var parentType = targetType.BaseType;

            var className = "";
            if (parentType != null)
            {
                className = GetReadableTypeName(inspectorClassGroupModule.isInheritingFromCustomClass ? parentType : targetType);
            }
            
            if (!getParentClass)
            {
                className = GetReadableTypeName(targetType);
            }

            return className;
        }
        
        //Returns a readable string representation of given .NET Type
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string GetReadableTypeName(Type type)
        {
            if (!type.IsGenericType)
            {
                return type.Name;
            }
        
            var typeName = type.Name[..type.Name.IndexOf('`')];
            var genericArgs = string.Join(", ", type.GetGenericArguments().Select(t => t.Name));
            return $"{typeName}<{genericArgs}>";
        }
    }
}
