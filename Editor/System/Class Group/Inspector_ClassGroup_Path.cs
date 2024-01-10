using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Reflection;

namespace com.Klazapp.Editor
{
    public partial class Inspector
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private List<string> GetParentPropertyPath()
        {
            var targetType = serializedObject.targetObject.GetType();
            var parentType = targetType.BaseType;
            return parentType != null ? GetPropertyPaths(parentType) : new List<string>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static List<string> GetPropertyPaths(IReflect type)
        {
            var paths = new List<string>();
            const BindingFlags BINDING_FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var fields = type.GetFields(BINDING_FLAGS);

            foreach (var field in fields)
            {
                var path = field.Name;
                paths.Add(path);
            }

            return paths;
        }
    }
}
