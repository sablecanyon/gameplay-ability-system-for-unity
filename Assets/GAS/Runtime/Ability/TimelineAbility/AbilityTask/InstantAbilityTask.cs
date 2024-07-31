namespace GAS.Runtime
{
    public abstract class InstantAbilityTask : AbilityTaskBase
    {
#if UNITY_EDITOR
        /// <summary>
        /// For editor preview
        /// [Note] When overriding, remember to wrap it with the UNITY_EDITOR macro. This is a function for preview performance and should not be compiled.
        /// </summary>
        public virtual void OnEditorPreview()
        {
        }
#endif
        public abstract void OnExecute();
    }

    public abstract class InstantAbilityTaskT<T> : InstantAbilityTask where T : AbilitySpec
    {
        public new T Spec => (T)_spec;
    }
}