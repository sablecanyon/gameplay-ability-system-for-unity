using System;
using System.Collections;
using System.Linq;
using GAS.General;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace GAS.Runtime
{
    public abstract class AbilityAsset : ScriptableObject
    {
        protected const int WIDTH_LABEL = 70;

        private static IEnumerable AbilityClassChoice = new ValueDropdownList<string>();

        public abstract Type AbilityType();

        [TitleGroup("Base")]
        [HorizontalGroup("Base/H1")]
        [TabGroup("Base/H1/V1", "Summary", SdfIconType.InfoSquareFill, TextColor = "#0BFFC5", Order = 1)]
        [HideLabel]
        [MultiLineProperty(10)]
        public string Description;

        [TabGroup("Base/H1/V2", "General", SdfIconType.AwardFill, TextColor = "#FF7F00", Order = 2)]
        [LabelText("Ability", SdfIconType.FileCodeFill)]
        [LabelWidth(WIDTH_LABEL)]
        [ShowInInspector]
        [InfoBox("Ability Class is NULL!!! Please check.", InfoMessageType.Error, VisibleIf = "@AbilityType() == null")]
        [PropertyOrder(-1)]
        public string InstanceAbilityClassFullName => AbilityType() != null ? AbilityType().FullName : null;

#if UNITY_EDITOR
        [TabGroup("Base/H1/V2", "General")]
        [TabGroup("Base/H1/V2", "Detail", SdfIconType.TicketDetailedFill, TextColor = "#BC2FDE")]
        [LabelText("Type Name", SdfIconType.FileCodeFill)]
        [LabelWidth(WIDTH_LABEL)]
        [ShowInInspector]
        [PropertyOrder(-1)]
        public string TypeName => GetType().Name;

        [TabGroup("Base/H1/V2", "Detail")]
        [LabelText("Type Full Name", SdfIconType.FileCodeFill)]
        [LabelWidth(WIDTH_LABEL)]
        [ShowInInspector]
        [PropertyOrder(-1)]
        public string TypeFullName => GetType().FullName;

        [TabGroup("Base/H1/V2", "Detail")]
        [ListDrawerSettings(ShowFoldout = true, ShowItemCount = false, ShowPaging = false)]
        [ShowInInspector]
        [LabelText("Inheritance")]
        [LabelWidth(WIDTH_LABEL)]
        [PropertyOrder(-1)]
        public string[] InheritanceChain => GetType().GetInheritanceChain().Reverse().ToArray();
#endif

        [TabGroup("Base/H1/V2", "General", SdfIconType.AwardFill)]
        [InfoBox(GASTextDefine.TIP_UNAME, InfoMessageType.None)]
        [LabelText("U-Name", SdfIconType.Fingerprint)]
        [LabelWidth(WIDTH_LABEL)]
        [InfoBox("Invalid name - does not conform to C# identifier naming rules", InfoMessageType.Error,
            "@GAS.General.Validation.Validations.IsValidVariableName($value) == false")]
        [InlineButton("@UniqueName = name", "Auto", Icon = SdfIconType.Hammer)]
        public string UniqueName;

        [TabGroup("Base/H1/V2", "General")]
        [Title("Cost & Cooldown", bold: true)]
        [LabelWidth(WIDTH_LABEL)]
        [AssetSelector]
        [LabelText(SdfIconType.HeartHalf, Text = GASTextDefine.ABILITY_EFFECT_COST)]
        public GameplayEffectAsset Cost;

        [TabGroup("Base/H1/V2", "General")]
        [LabelWidth(WIDTH_LABEL)]
        [AssetSelector]
        [LabelText(SdfIconType.StopwatchFill, Text = GASTextDefine.ABILITY_EFFECT_CD)]
        public GameplayEffectAsset Cooldown;

        [TabGroup("Base/H1/V2", "General")]
        [LabelWidth(WIDTH_LABEL)]
        [LabelText(SdfIconType.ClockFill, Text = GASTextDefine.ABILITY_CD_TIME)]
        [Unit(Units.Second)]
        public float CooldownTime;

        // Tags
        [TabGroup("Base/H1/V3", "Tags", SdfIconType.TagsFill, TextColor = "#45B1FF", Order = 3)]
        [ListDrawerSettings(ShowFoldout = true, ShowItemCount = false)]
        [ValueDropdown("@ValueDropdownHelper.GameplayTagChoices", IsUniqueList = true, HideChildProperties = true)]
        [Tooltip("Tags describing properties are used to describe the characteristics of Ability, such as damage, treatment, control, etc.")]
        [FormerlySerializedAs("AssetTag")]
        public GameplayTag[] AssetTags;

        [TabGroup("Base/H1/V3", "Tags")]
        [ListDrawerSettings(ShowFoldout = true, ShowItemCount = false)]
        [ValueDropdown("@ValueDropdownHelper.GameplayTagChoices", IsUniqueList = true, HideChildProperties = true)]
        [LabelText("CancelAbility With Tags ")]
        [Space]
        [Tooltip("When an ability is activated, all abilities currently held by the ability holder that have the [Any] tag will be canceled.")]
        public GameplayTag[] CancelAbilityTags;

        [TabGroup("Base/H1/V3", "Tags")]
        [ListDrawerSettings(ShowFoldout = true, ShowItemCount = false)]
        [ValueDropdown("@ValueDropdownHelper.GameplayTagChoices", IsUniqueList = true, HideChildProperties = true)]
        [LabelText("BlockAbility With Tags ")]
        [Space]
        [Tooltip("When an ability is activated, all abilities currently held by the ability holder, those with the tags [Any] will be blocked from activation.")]
        public GameplayTag[] BlockAbilityTags;

        [TabGroup("Base/H1/V3", "Tags")]
        [ListDrawerSettings(ShowFoldout = true, ShowItemCount = false)]
        [ValueDropdown("@ValueDropdownHelper.GameplayTagChoices", IsUniqueList = true, HideChildProperties = true)]
        [Space]
        [Tooltip("When the ability is activated, the holder will gain these tags, and when the ability is deactivated, these tags will be removed.")]
        [FormerlySerializedAs("ActivationOwnedTag")]
        public GameplayTag[] ActivationOwnedTags;

        [TabGroup("Base/H1/V3", "Tags")]
        [ListDrawerSettings(ShowFoldout = true, ShowItemCount = false)]
        [ValueDropdown("@ValueDropdownHelper.GameplayTagChoices", IsUniqueList = true, HideChildProperties = true)]
        [Space]
        [Tooltip("An Ability can only be activated if its owner has [all] of these tags.")]
        public GameplayTag[] ActivationRequiredTags;

        [TabGroup("Base/H1/V3", "Tags")]
        [ListDrawerSettings(ShowFoldout = true, ShowItemCount = false)]
        [ValueDropdown("@ValueDropdownHelper.GameplayTagChoices", IsUniqueList = true, HideChildProperties = true)]
        [Space]
        [Tooltip("An ability cannot be activated if its owner has [any] of these tags.")]
        public GameplayTag[] ActivationBlockedTags;
        // public GameplayTag[] SourceRequiredTags;
        // public GameplayTag[] SourceBlockedTags;
        // public GameplayTag[] TargetRequiredTags;
        // public GameplayTag[] TargetBlockedTags;
    }


    public abstract class AbilityAssetT<T> : AbilityAsset where T : class
    {
        public sealed override Type AbilityType()
        {
            return typeof(T);
        }
    }
}