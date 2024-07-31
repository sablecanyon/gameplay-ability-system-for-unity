using System;
using GAS.General;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GAS.Runtime
{
    /// <summary>
    /// Activation Policy for granting abilities
    /// </summary>
    public enum GrantedAbilityActivationPolicy
    {
        /// <summary>
        /// Not activated, waiting for the user to call ASC to activate
        /// </summary>
        [LabelText("None - Not activated, waiting for the user to call ASC to activate", SdfIconType.Joystick)]
        None,

        /// <summary>
        /// Activated when ability is added (Activated when GE is added)
        /// </summary>
        [LabelText("WhenAdded - Activated when the ability is added (activates when GE is added) ", SdfIconType.LightningChargeFill)]
        WhenAdded,

        /// <summary>
        /// Activated when Synchronous GE is activated
        /// </summary>
        [LabelText("SyncWithEffect - Activates when sync GE is activated", SdfIconType.Robot)]
        SyncWithEffect,
    }

    /// <summary>
    /// Deactivation policy for granted abilities
    /// </summary>
    public enum GrantedAbilityDeactivationPolicy
    {
        /// <summary>
        /// There is no relevant deactivation logic, and the user needs to call ASC to deactivate
        /// </summary>
        [LabelText("None - No deactivation logic, user needs to call ASC to deactivate", SdfIconType.Joystick)]
        None,

        /// <summary>
        /// Synchronize GE, deactivate when GE is inactive
        /// </summary>
        [LabelText("SyncWithEffect - Syncs with GE and deactivates when GE is deactivated", SdfIconType.Robot)]
        SyncWithEffect,
    }

    /// <summary>
    /// Removal policy for granted abilities
    /// </summary>
    public enum GrantedAbilityRemovePolicy
    {
        /// <summary>
        /// Do not remove
        /// </summary>
        [LabelText("None - Do not remove", SdfIconType.Joystick)]
        None,

        /// <summary>
        /// Synchronize GE, remove when GE is removed
        /// </summary>
        [LabelText("SyncWithEffect - Sync with GE, remove when GE is removed", SdfIconType.Robot)]
        SyncWithEffect,

        /// <summary>
        /// Removes itself when the ability ends
        /// </summary>
        [LabelText("WhenEnd - Removes itself when the ability ends", SdfIconType.LightningChargeFill)]
        WhenEnd,

        /// <summary>
        /// Removes itself when ability is cancelled.
        /// </summary>
        [LabelText("WhenCancel - Removes itself when the ability is canceled", SdfIconType.LightningChargeFill)]
        WhenCancel,

        /// <summary>
        /// Removes itself when the ability ends or is cancelled.
        /// </summary>
        [LabelText("WhenCancelOrEnd - removes itself when the ability ends or is canceled", SdfIconType.LightningChargeFill)]
        WhenCancelOrEnd,
    }

    [Serializable]
    public struct GrantedAbilityConfig
    {
        private const int LABEL_WIDTH = 50;

        [LabelWidth(LABEL_WIDTH)]
        [LabelText(GASTextDefine.LABEL_GRANT_ABILITY)]
        [AssetSelector]
        public AbilityAsset AbilityAsset;

        [LabelWidth(LABEL_WIDTH)]
        [LabelText(GASTextDefine.LABEL_GRANT_ABILITY_LEVEL)]
        public int AbilityLevel;

        [LabelWidth(LABEL_WIDTH)]
        [LabelText(GASTextDefine.LABEL_GRANT_ABILITY_ACTIVATION_POLICY)]
        [Tooltip(GASTextDefine.TIP_GRANT_ABILITY_ACTIVATION_POLICY)]
        public GrantedAbilityActivationPolicy ActivationPolicy;

        [LabelWidth(LABEL_WIDTH)]
        [LabelText(GASTextDefine.LABEL_GRANT_ABILITY_DEACTIVATION_POLICY)]
        [Tooltip(GASTextDefine.TIP_GRANT_ABILITY_DEACTIVATION_POLICY)]
        public GrantedAbilityDeactivationPolicy DeactivationPolicy;

        [LabelWidth(LABEL_WIDTH)]
        [LabelText(GASTextDefine.LABEL_GRANT_ABILITY_REMOVE_POLICY)]
        [Tooltip(GASTextDefine.TIP_GRANT_ABILITY_REMOVE_POLICY)]
        public GrantedAbilityRemovePolicy RemovePolicy;
    }

    public class GrantedAbilityFromEffect
    {
        public readonly AbstractAbility Ability;
        public readonly int AbilityLevel;
        public readonly GrantedAbilityActivationPolicy ActivationPolicy;
        public readonly GrantedAbilityDeactivationPolicy DeactivationPolicy;
        public readonly GrantedAbilityRemovePolicy RemovePolicy;

        public GrantedAbilityFromEffect(GrantedAbilityConfig config)
        {
            Ability =
                Activator.CreateInstance(config.AbilityAsset.AbilityType(), args: config.AbilityAsset) as
                    AbstractAbility;
            AbilityLevel = config.AbilityLevel;
            ActivationPolicy = config.ActivationPolicy;
            DeactivationPolicy = config.DeactivationPolicy;
            RemovePolicy = config.RemovePolicy;
        }

        public GrantedAbilityFromEffect(
            AbstractAbility ability,
            int abilityLevel,
            GrantedAbilityActivationPolicy activationPolicy,
            GrantedAbilityDeactivationPolicy deactivationPolicy,
            GrantedAbilityRemovePolicy removePolicy)
        {
            Ability = ability;
            AbilityLevel = abilityLevel;
            ActivationPolicy = activationPolicy;
            DeactivationPolicy = deactivationPolicy;
            RemovePolicy = removePolicy;
        }

        public GrantedAbilitySpecFromEffect CreateSpec(GameplayEffectSpec sourceEffectSpec)
        {
            var grantedAbility = new GrantedAbilitySpecFromEffect(this, sourceEffectSpec);
            return grantedAbility;
        }
    }

    public class GrantedAbilitySpecFromEffect
    {
        public readonly GrantedAbilityFromEffect GrantedAbility;
        public readonly GameplayEffectSpec SourceEffectSpec;
        public readonly AbilitySystemComponent Owner;

        public readonly string AbilityName;
        public int AbilityLevel => GrantedAbility.AbilityLevel;
        public GrantedAbilityActivationPolicy ActivationPolicy => GrantedAbility.ActivationPolicy;
        public GrantedAbilityDeactivationPolicy DeactivationPolicy => GrantedAbility.DeactivationPolicy;
        public GrantedAbilityRemovePolicy RemovePolicy => GrantedAbility.RemovePolicy;
        public AbilitySpec AbilitySpec => Owner.AbilityContainer.AbilitySpecs()[AbilityName];

        public GrantedAbilitySpecFromEffect(GrantedAbilityFromEffect grantedAbility,
            GameplayEffectSpec sourceEffectSpec)
        {
            GrantedAbility = grantedAbility;
            SourceEffectSpec = sourceEffectSpec;
            AbilityName = GrantedAbility.Ability.Name;
            Owner = SourceEffectSpec.Owner;
            if (Owner.AbilityContainer.HasAbility(AbilityName))
            {
                Debug.LogError($"GrantedAbilitySpecFromEffect: {Owner.name} already has ability {AbilityName}");
            }

            Owner.GrantAbility(GrantedAbility.Ability);
            AbilitySpec.SetLevel(AbilityLevel);

            // Whether to activate when adding
            if (ActivationPolicy == GrantedAbilityActivationPolicy.WhenAdded)
            {
                Owner.TryActivateAbility(AbilityName);
            }

            switch (RemovePolicy)
            {
                case GrantedAbilityRemovePolicy.WhenEnd:
                    AbilitySpec.RegisterEndAbility(RemoveSelf);
                    break;
                case GrantedAbilityRemovePolicy.WhenCancel:
                    AbilitySpec.RegisterCancelAbility(RemoveSelf);
                    break;
                case GrantedAbilityRemovePolicy.WhenCancelOrEnd:
                    AbilitySpec.RegisterEndAbility(RemoveSelf);
                    AbilitySpec.RegisterCancelAbility(RemoveSelf);
                    break;
            }
        }

        private void RemoveSelf()
        {
            Owner.RemoveAbility(AbilityName);
        }
    }
}