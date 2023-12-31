namespace com.Klazapp.Editor
{
    using Editor = UnityEditor.Editor;

    public abstract class InspectorBase : Editor
    {
        protected abstract void OnCreated();
        protected abstract void OnDisplayed();
    }
}