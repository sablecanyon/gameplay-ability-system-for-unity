using System;

namespace GAS.Runtime
{
    /// <summary>
    /// https://github.com/BillEliot/GASDocumentation_Chinese?tab=readme-ov-file#457-gameplayeffect%E6%A0%87%E7%AD%BE
    /// </summary>
    public struct GameplayEffectTagContainer
    {
        /// <summary>
        /// Tags owned by a GameplayEffect (GE), they have no functionality themselves and are only used to describe the GameplayEffect.
        /// This tag set is used for the match criteria of RemoveGameplayEffectsWithTags, so it has no meaning for Instant GEs.
        /// Note: GrantedTags is also used for matching with RemoveGameplayEffectsWithTags.
        /// </summary>
        public GameplayTagSet AssetTags;

        /// <summary>
        /// A collection of tags to assign to the target when the GameplayEffect (GE) is active.
        /// Tags stored in a GameplayEffect that are used in the ASC that the GameplayEffect is applied to.
        /// They are removed from the ASC when the GameplayEffect is removed. This tag only applies to Duration and Infinite GameplayEffects, and has no meaning for Instant GEs.
        /// When a GE is inactive, these tags are temporarily removed until the GE is activated again.
        /// These tags are also used for matching with RemoveGameplayEffectsWithTags.
        /// </summary>
        public GameplayTagSet GrantedTags;

        /// <summary>
        /// When a GameplayEffect is successfully applied, if the GameplayEffect on the target has any of these tags in its Asset Tags or Granted Tags, it will be removed from the target.
        /// Matching occurs when:
        /// 1. Instant GE is applied;
        /// 2. Each time a non-Instant GE is activated;
        /// 3. Each period of a Period Execution GE (PeriodExecution in a non-Instant GE) expires.
        /// </summary>
        public GameplayTagSet RemoveGameplayEffectsWithTags;

        /// <summary>
        /// ApplicationRequiredTags and ApplicationImmunityTags are a pair of conditions:
        /// The prerequisite for game effects to be applied to the target is:
        /// 1. The target must have all tags in ApplicationRequiredTags;
        /// 2. The target cannot have any tags in ApplicationImmunityTags.
        /// </summary>
        public GameplayTagSet ApplicationRequiredTags;

        /// <summary>
        /// ApplicationRequiredTags and ApplicationImmunityTags are a pair of conditions:
        /// The prerequisite for game effects to be applied to the target is:
        /// 1. The target must have all tags in ApplicationRequiredTags;
        /// 2. The target cannot have any tags in ApplicationImmunityTags.
        /// </summary>
        public GameplayTagSet ApplicationImmunityTags;

        /// <summary>
        /// The set of tags required for Game Effect (GE) activation.
        /// This tag only applies to Duration and Infinite GameplayEffects, and has no meaning for Instant GEs.
        /// Once a GameplayEffect is applied, these tags will determine whether the GameplayEffect is turned on or off. A GameplayEffect can be off but still applied.
        /// If a GameplayEffect is turned off because it does not meet the Ongoing Tag Requirements, but then meets the requirements, the GameplayEffect will be reopened and its Modifiers will be reapplied.
        /// Use scenarios include:
        /// 1. When a GE is applied, if the conditions are met, the GE is activated, otherwise no action is performed;
        /// 2. When the tag changes, if the conditions are met, the GE is activated, otherwise the GE is disabled.
        /// </summary>
        public GameplayTagSet OngoingRequiredTags;

        public GameplayEffectTagContainer(IGameplayEffectData data) : this(
            data.GetAssetTags(),
            data.GetGrantedTags(),
            data.GetApplicationRequiredTags(),
            data.GetOngoingRequiredTags(),
            data.GetRemoveGameplayEffectsWithTags(),
            data.GetApplicationImmunityTags()
        )
        {
        }

        public GameplayEffectTagContainer(
            GameplayTag[] assetTags,
            GameplayTag[] grantedTags,
            GameplayTag[] applicationRequiredTags,
            GameplayTag[] ongoingRequiredTags,
            GameplayTag[] removeGameplayEffectsWithTags,
            GameplayTag[] applicationImmunityTags)
        {
            AssetTags = new GameplayTagSet(assetTags);
            GrantedTags = new GameplayTagSet(grantedTags);
            ApplicationRequiredTags = new GameplayTagSet(applicationRequiredTags);
            OngoingRequiredTags = new GameplayTagSet(ongoingRequiredTags);
            RemoveGameplayEffectsWithTags = new GameplayTagSet(removeGameplayEffectsWithTags);
            ApplicationImmunityTags = new GameplayTagSet(applicationImmunityTags);
        }

        public static GameplayEffectTagContainer CreateEmpty()
        {
            return new GameplayEffectTagContainer(
                Array.Empty<GameplayTag>(),
                Array.Empty<GameplayTag>(),
                Array.Empty<GameplayTag>(),
                Array.Empty<GameplayTag>(),
                Array.Empty<GameplayTag>(),
                Array.Empty<GameplayTag>()
            );
        }
    }
}