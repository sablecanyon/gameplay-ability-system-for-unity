using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GAS.Runtime
{
    public enum GEOperation
    {
        [LabelText(SdfIconType.PlusLg, Text = "Add")]
        Add = 0,

        [LabelText(SdfIconType.DashLg, Text = "Minus")]
        Minus = 3,

        [LabelText(SdfIconType.XLg, Text = "Multiply")]
        Multiply = 1,

        [LabelText(SdfIconType.SlashLg, Text = "Divide")]
        Divide = 4,

        [LabelText(SdfIconType.Pencil, Text = "Override")]
        Override = 2,
    }

    [Flags]
    public enum SupportedOperation : byte
    {
        None = 0,

        [LabelText(SdfIconType.PlusLg, Text = "Add")]
        Add = 1 << GEOperation.Add,

        [LabelText(SdfIconType.DashLg, Text = "Minus")]
        Minus = 1 << GEOperation.Minus,

        [LabelText(SdfIconType.XLg, Text = "Multiply")]
        Multiply = 1 << GEOperation.Multiply,

        [LabelText(SdfIconType.SlashLg, Text = "Divide")]
        Divide = 1 << GEOperation.Divide,

        [LabelText(SdfIconType.Pencil, Text = "Override")]
        Override = 1 << GEOperation.Override,

        All = Add | Minus | Multiply | Divide | Override
    }

    [Serializable]
    public struct GameplayEffectModifier
    {
        private const int LABEL_WIDTH = 70;

        [LabelText("Modify properties", SdfIconType.Fingerprint)]
        [LabelWidth(LABEL_WIDTH)]
        [OnValueChanged("OnAttributeChanged")]
        [ValueDropdown("@ValueDropdownHelper.AttributeChoices", IsUniqueList = true)]
        [Tooltip("Refers to the modified properties of the GameplayEffect object.")]
        [InfoBox("No attribute selected", InfoMessageType.Error, VisibleIf = "@string.IsNullOrWhiteSpace($value)")]
        [SuffixLabel("@ReflectionHelper.GetAttribute($value)?.CalculateMode")]
        [PropertyOrder(1)]
        public string AttributeName;

        [HideInInspector]
        public string AttributeSetName;

        [HideInInspector]
        public string AttributeShortName;

        [LabelText("Operation parameters", SdfIconType.Activity)]
        [LabelWidth(LABEL_WIDTH)]
        [Tooltip("Modifier's base value. How this value is used is determined by the MMC's operating logic. \nThis value is used directly when MMC is not specified.")]
        [InfoBox("Divisor cannot be zero", InfoMessageType.Error,
            VisibleIf = "@Operation == GEOperation.Divide && ModiferMagnitude == 0 && MMC == null")]
        [PropertyOrder(3)]
        public float ModiferMagnitude;

        [LabelText("Operation", SdfIconType.PlusSlashMinus)]
        [LabelWidth(LABEL_WIDTH)]
        [EnumToggleButtons]
        [PropertyOrder(2)]
        [ValidateInput("@ReflectionHelper.GetAttribute(AttributeName).IsSupportOperation($value)", "Illegal operation: The operation is not supported by this attribute.")]
        public GEOperation Operation;

        [LabelText("Parameter modification", SdfIconType.CpuFill)]
        [LabelWidth(LABEL_WIDTH)]
        [AssetSelector]
        [Tooltip("ModifierMagnitudeCalculation, modifier, responsible for the numerical calculation logic of Attribute in GAS. \nCan be empty (no modification is made to \"calculation parameters\").")]
        [PropertyOrder(4)]
        public ModifierMagnitudeCalculation MMC;

        // TODO
        // public readonly GameplayTagSet SourceTag;

        // TODO
        // public readonly GameplayTagSet TargetTag;

        public GameplayEffectModifier(
            string attributeName,
            float modiferMagnitude,
            GEOperation operation,
            ModifierMagnitudeCalculation mmc = null)
        {
            AttributeName = attributeName;
            var splits = attributeName.Split('.');
            AttributeSetName = splits[0];
            AttributeShortName = splits[1];
            ModiferMagnitude = modiferMagnitude;
            Operation = operation;
            MMC = mmc;
        }

        public float CalculateMagnitude(GameplayEffectSpec spec, float modifierMagnitude)
        {
            return MMC == null ? ModiferMagnitude : MMC.CalculateMagnitude(spec, modifierMagnitude);
        }

        public void SetModiferMagnitude(float value)
        {
            ModiferMagnitude = value;
        }

        void OnAttributeChanged()
        {
            var split = AttributeName.Split('.');
            AttributeSetName = split[0];
            AttributeShortName = split[1];

            if (ReflectionHelper.GetAttribute(AttributeName)?.CalculateMode !=
                CalculateMode.Stacking)
            {
                Operation = GEOperation.Override;
            }
        }
    }
}