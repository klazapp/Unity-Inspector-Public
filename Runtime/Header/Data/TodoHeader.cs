using System;
using UnityEngine;

namespace com.Klazapp.Utility
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class TodoHeader : PropertyAttribute
    {
        public readonly string description;

        public TodoHeader(string description)
        {
            this.description = description;
        }
    }
}