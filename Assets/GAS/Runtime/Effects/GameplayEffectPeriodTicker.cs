using UnityEngine;

namespace GAS.Runtime
{
    public class GameplayEffectPeriodTicker
    {
        private float _periodRemaining;
        private readonly GameplayEffectSpec _spec;

        public GameplayEffectPeriodTicker(GameplayEffectSpec spec)
        {
            _spec = spec;
            _periodRemaining = Period;
        }

        private float Period => _spec.GameplayEffect.Period;

        public void Tick()
        {
            _spec.TriggerOnTick();
            
            if (_periodRemaining <= 0)
            {
                ResetPeriod();
                _spec.PeriodExecution?.TriggerOnExecute();
            }
            else
            {
                _periodRemaining -= Time.deltaTime;
            }

            if (_spec.DurationPolicy== EffectsDurationPolicy.Duration && _spec.DurationRemaining() <= 0)
            {
                // Handle STACKING
                if (_spec.GameplayEffect.Stacking.stackingType == StackingType.None)
                {
                    _spec.RemoveSelf();
                }
                else
                {
                    if (_spec.GameplayEffect.Stacking.expirationPolicy == ExpirationPolicy.ClearEntireStack)
                    {
                        _spec.RemoveSelf();
                    }
                    else if (_spec.GameplayEffect.Stacking.expirationPolicy ==
                              ExpirationPolicy.RemoveSingleStackAndRefreshDuration)
                    {
                        if (_spec.StackCount > 1)
                        {
                            _spec.RefreshStack(_spec.StackCount - 1);
                            _spec.RefreshDuration();
                        }
                        else
                        {
                            _spec.RemoveSelf();
                        }
                    }
                    else if (_spec.GameplayEffect.Stacking.expirationPolicy == ExpirationPolicy.RefreshDuration)
                    {
                        //When the duration ends, refresh the Duration again, which is equivalent to infinite Duration.
                        //TODO :You can handle the number of layers by calling OnStackCountChange(GameplayEffect ActiveEffect, int OldStackCount, int NewStackCount) of GameplayEffectsContainer.
                        //TODO :This can achieve complex effects such as reducing two layers and refreshing Duration at the end of Duration.
                        _spec.RefreshDuration();
                    }
                }
            }
        }
        
        public void ResetPeriod()
        {
            _periodRemaining = Period;
        }
    }
}