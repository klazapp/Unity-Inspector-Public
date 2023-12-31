using System;
using UnityEngine;

namespace com.Klazapp.Utility
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Enum | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NoteAttribute : PropertyAttribute
    {
        public readonly string noteTitle;
        public readonly string noteDescription;

        public NoteAttribute(string noteTitle, string noteDescription = "")
        {
            this.noteTitle = noteTitle;
            this.noteDescription = noteDescription;
        }
    }
}
