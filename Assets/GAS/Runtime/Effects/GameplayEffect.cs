﻿using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace GAS.Runtime
{
    public enum EffectsDurationPolicy
    {
        [LabelText("Instant", SdfIconType.LightningCharge)]
        Instant = 1,

        [LabelText("Infinite", SdfIconType.Infinity)]
        Infinite,

        [LabelText("Duration", SdfIconType.HourglassSplit)]
        Duration
    }

    public class GameplayEffect
    {
        public readonly string GameplayEffectName;
        public readonly EffectsDurationPolicy DurationPolicy;
        public readonly float Duration; // -1 represents infinite duration
        public readonly float Period;
        public readonly GameplayEffect PeriodExecution;
        public readonly GameplayEffectTagContainer TagContainer;

        // Cues
        public readonly GameplayCueInstant[] CueOnExecute;
        public readonly GameplayCueInstant[] CueOnRemove;
        public readonly GameplayCueInstant[] CueOnAdd;
        public readonly GameplayCueInstant[] CueOnActivate;
        public readonly GameplayCueInstant[] CueOnDeactivate;
        public readonly GameplayCueDurational[] CueDurational;

        // Modifiers
        public readonly GameplayEffectModifier[] Modifiers;
        public readonly ExecutionCalculation[] Executions; // TODO: this should be a list of execution calculations

        // Granted Ability
        public readonly GrantedAbilityFromEffect[] GrantedAbilities;

        //Stacking
        public readonly GameplayEffectStacking Stacking;

        // TODO: Expiration Effects 
        public readonly GameplayEffect[] PrematureExpirationEffect;
        public readonly GameplayEffect[] RoutineExpirationEffectClasses;

        public GameplayEffectSpec CreateSpec(
            AbilitySystemComponent creator,
            AbilitySystemComponent owner,
            float level = 1)
        {
            var spec = new GameplayEffectSpec(this);
            spec.Init(creator, owner, level);
            return spec;
        }

        /// <summary>
        /// The instantiation process of separating GameplayEffectSpec is: instance + data initialization
        /// </summary>
        /// <returns></returns>
        public GameplayEffectSpec CreateSpec()
        {
            var spec = new GameplayEffectSpec(this);
            return spec;
        }

        public GameplayEffect(IGameplayEffectData data)
        {
            if (data is null)
            {
                throw new System.Exception($"GE data can't be null!");
            }

            GameplayEffectName = data.GetDisplayName();
            DurationPolicy = data.GetDurationPolicy();
            Duration = data.GetDuration();
            Period = data.GetPeriod();
            TagContainer = new GameplayEffectTagContainer(data);
            var periodExecutionGe = data.GetPeriodExecution();
#if UNITY_EDITOR
            if (periodExecutionGe != null && periodExecutionGe.GetDurationPolicy() != EffectsDurationPolicy.Instant)
            {
                UnityEngine.Debug.LogError($"PeriodExecution of {GameplayEffectName} should be Instant type.");
            }
#endif
            PeriodExecution = periodExecutionGe != null ? new GameplayEffect(periodExecutionGe) : null;
            CueOnExecute = data.GetCueOnExecute();
            CueOnRemove = data.GetCueOnRemove();
            CueOnAdd = data.GetCueOnAdd();
            CueOnActivate = data.GetCueOnActivate();
            CueOnDeactivate = data.GetCueOnDeactivate();
            CueDurational = data.GetCueDurational();
            Modifiers = data.GetModifiers();
            Executions = data.GetExecutions();
            GrantedAbilities = GetGrantedAbilities(data.GetGrantedAbilities());
            Stacking = data.GetStacking();
        }

        private static GrantedAbilityFromEffect[] GetGrantedAbilities(IEnumerable<GrantedAbilityConfig> grantedAbilities)
        {
            var grantedAbilityList = new List<GrantedAbilityFromEffect>();
            foreach (var grantedAbilityConfig in grantedAbilities)
            {
                if (grantedAbilityConfig.AbilityAsset == null) continue;
                grantedAbilityList.Add(new GrantedAbilityFromEffect(grantedAbilityConfig));
            }

            return grantedAbilityList.ToArray();
        }

        public bool CanApplyTo(IAbilitySystemComponent target)
        {
            return target.HasAllTags(TagContainer.ApplicationRequiredTags);
        }

        public bool CanRunning(IAbilitySystemComponent target)
        {
            return target.HasAllTags(TagContainer.OngoingRequiredTags);
        }

        public bool IsImmune(IAbilitySystemComponent target)
        {
            return target.HasAnyTags(TagContainer.ApplicationImmunityTags);
        }

        public bool StackEqual(GameplayEffect effect)
        {
            if (Stacking.stackingType == StackingType.None) return false;
            if (effect.Stacking.stackingType == StackingType.None) return false;
            if (string.IsNullOrEmpty(Stacking.stackingCodeName)) return false;
            if (string.IsNullOrEmpty(effect.Stacking.stackingCodeName)) return false;

            return Stacking.stackingHashCode == effect.Stacking.stackingHashCode;
        }
    }
}