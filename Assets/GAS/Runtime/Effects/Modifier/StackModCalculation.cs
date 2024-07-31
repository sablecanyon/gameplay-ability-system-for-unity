using Sirenix.OdinInspector;
using UnityEngine;

namespace GAS.Runtime
{
    [CreateAssetMenu( fileName = "StackModCalculation", menuName = "GAS/MMC/StackModCalculation" )]
    public class StackModCalculation:ModifierMagnitudeCalculation
    {
        [InfoBox("The calculation logic is the same as ScalableFloatModCalculation, formula: (StackCount) * k + b")]
        [TabGroup("Default", "StackModCalculation")]
        [LabelText("Coefficient (k)")]
        public float k = 1;

        [TabGroup("Default", "StackModCalculation")]
        [LabelText("Constant (b)")]
        public float b = 0;
        
        public override float CalculateMagnitude(GameplayEffectSpec spec, float modifierMagnitude)
        {
            if (spec.Stacking.stackingType == StackingType.None) return 0;
            
            var stackCount = spec.StackCount;
            return stackCount * k + b;
        }
    }
}