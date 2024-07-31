namespace GAS.Runtime
{
    public abstract class OngoingAbilityTask : AbilityTaskBase
    {
#if UNITY_EDITOR
        /// <summary>
        /// For editor preview
        /// 【Note】 When overriding, remember to wrap it with the UNITY_EDITOR macro. This is a function for preview performance and should not be compiled.
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="startFrame"></param>
        /// <param name="endFrame"></param>
        public virtual void OnEditorPreview(int frame, int startFrame, int endFrame)
        {
        }
#endif
        public abstract void OnStart(int startFrame);

        public abstract void OnEnd(int endFrame);

        public abstract void OnTick(int frameIndex, int startFrame, int endFrame);
    }

    public abstract class OngoingAbilityTaskT<T> : OngoingAbilityTask where T : AbilitySpec
    {
        public new T Spec => (T)_spec;
    }
}