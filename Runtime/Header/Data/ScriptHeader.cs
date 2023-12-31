using System;
using UnityEngine;

namespace com.Klazapp.Utility
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ScriptHeader : PropertyAttribute
    {
        public readonly string description;

        public ScriptHeader(string description)
        {
            this.description = description;
        }
    }
}