using Sirenix.OdinInspector;
using UnityEngine;

namespace GAS.Runtime
{
    [CreateAssetMenu(fileName = "AttributeBasedModCalculation", menuName = "GAS/MMC/AttributeBasedModCalculation")]
    public class AttributeBasedModCalculation : ModifierMagnitudeCalculation
    {
        public enum AttributeFrom
        {
            [LabelText("Source", SdfIconType.Magic)]
            Source,

            [LabelText("Target", SdfIconType.Person)]
            Target
        }

        public enum GEAttributeCaptureType
        {
            [LabelText("SnapShot", SdfIconType.Camera)]
            SnapShot,

            [LabelText("Track", SdfIconType.Speedometer2)]
            Track
        }

        [TabGroup("Default", "AttributeBasedModCalculation", SdfIconType.PersonBoundingBox, TextColor = "blue")]
        [InfoBox(" Which attribute value (Attribute Name) is captured from whom (Attribute From) and in what way (Capture Type).")]
        [EnumToggleButtons]
        [LabelText("Capture Type")]
        public GEAttributeCaptureType captureType;

        [TabGroup("Default", "AttributeBasedModCalculation")]
        [EnumToggleButtons]
        [LabelText("Capture Target(Attribute From)")]
        public AttributeFrom attributeFromType;

        [TabGroup("Default", "AttributeBasedModCalculation")]
        [ValueDropdown("@ValueDropdownHelper.AttributeChoices", IsUniqueList = true)]
        [LabelText("Attribute Name")]
        [OnValueChanged("@OnAttributeNameChanged()")]
        [InfoBox("No attribute name specified", InfoMessageType.Error, VisibleIf = "@string.IsNullOrWhiteSpace(attributeName)")]
        public string attributeName;

        [TabGroup("Default", "Details", SdfIconType.Bug, TextColor = "orange")]
        [ReadOnly]
        public string attributeSetName;

        [TabGroup("Default", "Details")]
        [ReadOnly]
        public string attributeShortName;

        [InfoBox("The calculation logic is the same as ScalableFloatModCalculation, formula: AttributeValue * k + b")]
        [TabGroup("Default", "AttributeBasedModCalculation")]
        [LabelText("Coefficient (k)")]
        public float k = 1;

        [TabGroup("Default", "AttributeBasedModCalculation")]
        [LabelText("Constant (b)")]
        public float b = 0;

        public override float CalculateMagnitude(GameplayEffectSpec spec, float modifierMagnitude)
        {
            if (attributeFromType == AttributeFrom.Source)
            {
                if (captureType == GEAttributeCaptureType.SnapShot)
                {
                    var snapShot = spec.SnapshotSourceAttributes;
                    var attribute = snapShot[attributeName];
                    return attribute * k + b;
                }
                else
                {
                    var attribute = spec.Source.GetAttributeCurrentValue(attributeSetName, attributeShortName);
                    return (attribute ?? 1) * k + b;
                }
            }

            if (captureType == GEAttributeCaptureType.SnapShot)
            {
                var snapShot = spec.SnapshotTargetAttributes;
                var attribute = snapShot[attributeName];
                return attribute * k + b;
            }
            else
            {
                var attribute = spec.Owner.GetAttributeCurrentValue(attributeSetName, attributeShortName);
                return (attribute ?? 1) * k + b;
            }
        }

        private void OnAttributeNameChanged()
        {
            if (!string.IsNullOrWhiteSpace(attributeName))
            {
                var split = attributeName.Split('.');
                attributeSetName = split[0];
                attributeShortName = split[1];
            }
            else
            {
                attributeSetName = null;
                attributeShortName = null;
            }
        }
    }
}