using UnityEngine.Profiling;

namespace GAS.Runtime
{
    public abstract class TimelineAbilityT<T> : AbstractAbility<T> where T : TimelineAbilityAssetBase
    {
        protected TimelineAbilityT(T abilityAsset) : base(abilityAsset)
        {
        }
    }

    public abstract class TimelineAbilitySpecT<T> : AbilitySpec<T> where T : AbstractAbility
    {
        protected TimelineAbilityPlayer<T> _player;

        /// <summary>
        /// The target of the directional skills
        /// </summary>
        public AbilitySystemComponent Target { get; private set; }

        protected TimelineAbilitySpecT(T ability, AbilitySystemComponent owner) : base(ability, owner)
        {
            _player = new TimelineAbilityPlayer<T>(this);
        }

        public void SetAbilityTarget(AbilitySystemComponent mainTarget)
        {
            Target = mainTarget;
        }

        public override void ActivateAbility(params object[] args)
        {
            _player.Play();
        }

        public override void CancelAbility()
        {
            _player.Stop();
        }

        public override void EndAbility()
        {
            _player.Stop();
        }

        protected override void AbilityTick()
        {
            Profiler.BeginSample("TimelineAbilitySpecT<T>::AbilityTick()");
            _player.Tick();
            Profiler.EndSample();
        }
    }

    /// <summary>
    /// This is the simplest TimelineAbility implementation. If you want to implement a more complex TimelineAbility, please use TimelineAbilityT<T> and TimelineAbilitySpecT<T> as the base class.
    /// </summary>
    public sealed class TimelineAbility : TimelineAbilityT<TimelineAbilityAssetBase>
    {
        public TimelineAbility(TimelineAbilityAssetBase abilityAsset) : base(abilityAsset)
        {
        }

        public override AbilitySpec CreateSpec(AbilitySystemComponent owner)
        {
            return new TimelineAbilitySpec(this, owner);
        }
    }

    /// <summary>
    /// This is a simple TimelineAbilitySpec implementation. If you want to implement a more complex TimelineAbility, please use TimelineAbilityT<T> and TimelineAbilitySpecT<T> as the base class.
    /// </summary>
    public sealed class TimelineAbilitySpec : TimelineAbilitySpecT<TimelineAbility>
    {
        public TimelineAbilitySpec(TimelineAbility ability, AbilitySystemComponent owner) : base(ability, owner)
        {
        }
    }
}