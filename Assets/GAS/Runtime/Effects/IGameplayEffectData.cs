namespace GAS.Runtime
{
    public interface IGameplayEffectData
    {
        string GetDisplayName();
        EffectsDurationPolicy GetDurationPolicy();
        float GetDuration();
        float GetPeriod();

        /// <summary>
        /// Must be an Instant GameplayEffect
        /// </summary>
        IGameplayEffectData GetPeriodExecution();

        GameplayTag[] GetAssetTags();
        GameplayTag[] GetGrantedTags();
        GameplayTag[] GetRemoveGameplayEffectsWithTags();
        GameplayTag[] GetApplicationRequiredTags();
        GameplayTag[] GetApplicationImmunityTags();
        GameplayTag[] GetOngoingRequiredTags();

        // Cues
        GameplayCueInstant[] GetCueOnExecute();
        GameplayCueInstant[] GetCueOnRemove();
        GameplayCueInstant[] GetCueOnAdd();
        GameplayCueInstant[] GetCueOnActivate();
        GameplayCueInstant[] GetCueOnDeactivate();
        GameplayCueDurational[] GetCueDurational();

        // Modifiers
        GameplayEffectModifier[] GetModifiers();
        ExecutionCalculation[] GetExecutions();

        // Granted Ability
        GrantedAbilityConfig[] GetGrantedAbilities();
        
        //Stacking
        GameplayEffectStacking GetStacking();
    }
}