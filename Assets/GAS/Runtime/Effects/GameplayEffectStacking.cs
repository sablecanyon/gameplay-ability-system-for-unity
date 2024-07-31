using System;
using GAS.General;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GAS.Runtime
{
    public enum StackingType
    {
        [LabelText("Independent", SdfIconType.XCircleFill)]
        None, //It will not be superimposed. If it is released multiple times, each Effect is equivalent to a single Effect.

        [LabelText("Source", SdfIconType.Magic)]
        AggregateBySource, //Each Source ASC on the Target has a separate stack instance, and each Source can apply X GameplayEffects in the stack.

        [LabelText("Target", SdfIconType.Person)]
        AggregateByTarget //There is only one stack instance on the target regardless of the source, and each source can apply the stack within the shared stack limit.
    }

    public enum DurationRefreshPolicy
    {
        [LabelText("NeverRefresh - Do not refresh the duration of the Effect", SdfIconType.XCircleFill)]
        NeverRefresh, //Do not refresh the duration of the Effect

        [LabelText(
            "RefreshOnSuccessfulApplication - refresh duration after each successful apply",
            SdfIconType.HourglassTop)]
        RefreshOnSuccessfulApplication //After each successful apply, the duration of the Effect is refreshed. If denyOverflowApplication is True, the redundant Apply will not refresh the Duration.
    }

    public enum PeriodResetPolicy
    {
        [LabelText("NeverReset - Do not reset the Effect's cycle timer", SdfIconType.XCircleFill)]
        NeverRefresh, //Do not reset the Effect's cycle timer

        [LabelText("ResetOnSuccessfulApplication - resets the Effect's cycle timer after each successful apply", SdfIconType.HourglassTop)]
        ResetOnSuccessfulApplication //Reset the Effect cycle timer after each successful apply
    }

    public enum ExpirationPolicy
    {
        [LabelText("ClearEntireStack - When the duration ends, clear all stacks", SdfIconType.TrashFill)]
        ClearEntireStack, //When the duration ends, all stacks are cleared

        [LabelText("RemoveSingleStackAndRefreshDuration - Removes one stack when the duration ends, then starts over again for the duration", SdfIconType.EraserFill)]
        RemoveSingleStackAndRefreshDuration, //At the end of the duration, one stack is reduced, and then a new Duration is continued until the number of layers is reduced to 0.

        [LabelText("RefreshDuration - When the duration ends, refresh the Duration again", SdfIconType.HourglassTop)]
        RefreshDuration //When the duration ends, refresh the Duration again, which is equivalent to infinite Duration.
        //TODO :You can handle the number of layers by calling OnStackCountChange(GameplayEffect ActiveEffect, int OldStackCount, int NewStackCount) of GameplayEffectsContainer.
        //TODO :This can achieve complex effects such as reducing two layers and refreshing Duration at the end of Duration.
    }

    // GE stack data structure
    public struct GameplayEffectStacking
    {
        public string stackingCodeName; // In fact, it is not allowed to use it, but the hash value of stackingCodeName, that is, stackingHashCode
        public int stackingHashCode;
        public StackingType stackingType;
        public int limitCount;
        public DurationRefreshPolicy durationRefreshPolicy;
        public PeriodResetPolicy periodResetPolicy;
        public ExpirationPolicy expirationPolicy;

        // Overflow logic processing
        public bool denyOverflowApplication; //Corresponding to StackDurationRefreshPolicy, if it is True, the redundant Apply will not refresh the Duration
        public bool clearStackOnOverflow; //This is only effective when DenyOverflowApplication is True. When Overflow occurs, all layers are deleted directly.
        public GameplayEffect[] overflowEffects; // When more than the StackLimitCount number of Effects are applied, the OverflowEffects will be called.

        public void SetStackingCodeName(string stackingCodeName)
        {
            this.stackingCodeName = stackingCodeName;
            this.stackingHashCode = stackingCodeName?.GetHashCode() ?? 0; // Compatible with old SO data
        }

        public void SetStackingHashCode(int stackingHashCode)
        {
            this.stackingHashCode = stackingHashCode;
        }

        public void SetStackingType(StackingType stackingType)
        {
            this.stackingType = stackingType;
        }

        public void SetLimitCount(int limitCount)
        {
            this.limitCount = limitCount;
        }

        public void SetDurationRefreshPolicy(DurationRefreshPolicy durationRefreshPolicy)
        {
            this.durationRefreshPolicy = durationRefreshPolicy;
        }

        public void SetPeriodResetPolicy(PeriodResetPolicy periodResetPolicy)
        {
            this.periodResetPolicy = periodResetPolicy;
        }

        public void SetExpirationPolicy(ExpirationPolicy expirationPolicy)
        {
            this.expirationPolicy = expirationPolicy;
        }

        public void SetOverflowEffects(GameplayEffect[] overflowEffects)
        {
            this.overflowEffects = overflowEffects;
        }

        public void SetOverflowEffects(GameplayEffectAsset[] overflowEffectAssets)
        {
            overflowEffects = new GameplayEffect[overflowEffectAssets.Length];
            for (var i = 0; i < overflowEffectAssets.Length; ++i)
            {
                overflowEffects[i] = new GameplayEffect(overflowEffectAssets[i]);
            }
        }

        public void SetDenyOverflowApplication(bool denyOverflowApplication)
        {
            this.denyOverflowApplication = denyOverflowApplication;
        }

        public void SetClearStackOnOverflow(bool clearStackOnOverflow)
        {
            this.clearStackOnOverflow = clearStackOnOverflow;
        }

        public static GameplayEffectStacking None
        {
            get
            {
                var stack = new GameplayEffectStacking();
                stack.SetStackingType(StackingType.None);
                return stack;
            }
        }
    }

    [Serializable]
    public sealed class GameplayEffectStackingConfig
    {
        private const int LABEL_WIDTH = 100;

        [LabelWidth(LABEL_WIDTH)]
        [VerticalGroup]
        [LabelText(GASTextDefine.LABEL_GE_STACKING_TYPE)]
        [EnumToggleButtons]
        public StackingType stackingType;

        [LabelWidth(LABEL_WIDTH)]
        [VerticalGroup]
        [HideIf("IsNoStacking")]
        [LabelText(GASTextDefine.LABEL_GE_STACKING_CODENAME)]
        [InlineButton(@"@stackingCodeName = """"", SdfIconType.EraserFill, "")]
        public string stackingCodeName;

        [LabelWidth(LABEL_WIDTH)]
        [VerticalGroup]
        [LabelText(GASTextDefine.LABEL_GE_STACKING_COUNT)]
        [HideIf("IsNoStacking")]
        [InlineButton(@"@limitCount = int.MaxValue", SdfIconType.Hammer, "max")]
        [InlineButton(@"@limitCount = 0", SdfIconType.Hammer, "min")]
        [ValidateInput("@limitCount >= 0", "must>=0")]
        public int limitCount;

        [LabelWidth(LABEL_WIDTH)]
        [VerticalGroup]
        [LabelText(GASTextDefine.LABEL_GE_STACKING_DURATION_REFRESH_POLICY)]
        [HideIf("IsNoStacking")]
        [InfoBox(GASTextDefine.LABEL_GE_STACKING_DENY_OVERFLOW_APPLICATION+"When True, redundant Apply will not refresh Duration", InfoMessageType.None,
            VisibleIf =
                "@durationRefreshPolicy == DurationRefreshPolicy.RefreshOnSuccessfulApplication && denyOverflowApplication")]
        public DurationRefreshPolicy durationRefreshPolicy;

        [LabelWidth(LABEL_WIDTH)]
        [VerticalGroup]
        [LabelText(GASTextDefine.LABEL_GE_STACKING_PERIOD_RESET_POLICY)]
        [HideIf("IsNoStacking")]
        public PeriodResetPolicy periodResetPolicy;

        [LabelWidth(LABEL_WIDTH)]
        [VerticalGroup]
        [LabelText(GASTextDefine.LABEL_GE_STACKING_EXPIRATION_POLICY)]
        [HideIf("IsNoStacking")]
        public ExpirationPolicy expirationPolicy;

        // Overflow logic processing
        [LabelWidth(LABEL_WIDTH)]
        [VerticalGroup]
        [LabelText(GASTextDefine.LABEL_GE_STACKING_DENY_OVERFLOW_APPLICATION)]
        [HideIf("@IsNoStacking() || IsNeverRefreshDuration()")]
        public bool denyOverflowApplication;

        [VerticalGroup]
        [LabelWidth(LABEL_WIDTH)]
        [LabelText(GASTextDefine.LABEL_GE_STACKING_CLEAR_STACK_ON_OVERFLOW)]
        [ShowIf("IsDenyOverflowApplication")]
        public bool clearStackOnOverflow;

        [VerticalGroup]
        [LabelWidth(LABEL_WIDTH)]
        [LabelText(GASTextDefine.LABEL_GE_STACKING_CLEAR_OVERFLOW_EFFECTS)]
        [HideIf("IsNoStacking")]
        public GameplayEffectAsset[] overflowEffects;

        /// <summary>
        /// Convert to runtime data
        /// </summary>
        /// <returns></returns>
        public GameplayEffectStacking ToRuntimeData()
        {
            var stack = new GameplayEffectStacking();
            stack.SetStackingCodeName(stackingCodeName);
            stack.SetStackingType(stackingType);
            stack.SetLimitCount(limitCount);
            stack.SetDurationRefreshPolicy(durationRefreshPolicy);
            stack.SetPeriodResetPolicy(periodResetPolicy);
            stack.SetExpirationPolicy(expirationPolicy);
            stack.SetOverflowEffects(overflowEffects);
            stack.SetDenyOverflowApplication(denyOverflowApplication);
            stack.SetClearStackOnOverflow(clearStackOnOverflow);
            return stack;
        }

        #region UTIL FUNCTION FOR ODIN INSPECTOR

        public bool IsNoStacking() => stackingType == StackingType.None;

        public bool IsNeverRefreshDuration() =>
            IsNoStacking() || durationRefreshPolicy == DurationRefreshPolicy.NeverRefresh;

        public bool IsDenyOverflowApplication() => !IsNoStacking() && denyOverflowApplication;

        #endregion
    }
}