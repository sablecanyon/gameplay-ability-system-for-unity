using Sirenix.OdinInspector;
using UnityEngine;

namespace GAS.Runtime
{
    /// <summary>
    ///  MMC based on attribute hybrid GE stack
    /// </summary>
    [CreateAssetMenu(fileName = "AttrBasedWithStackModCalculation", menuName = "GAS/MMC/AttrBasedWithStackModCalculation")]
    public class AttrBasedWithStackModCalculation:AttributeBasedModCalculation
    {
        public enum StackMagnitudeOperation
        {
            Add,
            Multiply
        }
        
        [InfoBox(" Formula: StackCount * sK + sB")]
        [TabGroup("Default", "AttributeBasedModCalculation")]
        [Title("Stack Magnitude Calculation")]
        [LabelText("Coefficient (sK)")]
        public float sK = 1;

        [TabGroup("Default", "AttributeBasedModCalculation")]
        [LabelText("Constant (sB)")]
        public float sB = 0;

        [TabGroup("Default", "AttributeBasedModCalculation")]
        [Title("Final result")]
        [InfoBox(" Final formula: \n" +
                 "Add:(AttributeValue * k + b)+(StackCount * sK + sB); \n" +
                 "Multiply:(AttributeValue * k + b)*(StackCount * sK + sB)")]
        [LabelText("Stack Magnitude and Attr Magnitude calculation method")]
        public StackMagnitudeOperation stackMagnitudeOperation;

        [TabGroup("Default", "AttributeBasedModCalculation")]
        [LabelText("Final formula")]
        [ShowInInspector]
        [DisplayAsString(TextAlignment.Left, true)]
        public string FinalFormulae
        {
            get
            {
                var formulae = stackMagnitudeOperation switch
                {
                    StackMagnitudeOperation.Add => $"({attributeName} * {k} + {b}) + (StackCount * {sK} + {sB})",
                    StackMagnitudeOperation.Multiply => $"({attributeName} * {k} + {b}) * (StackCount * {sK} + {sB})",
                    _ => ""
                };

                return $"<size=15><b><color=green>{formulae}</color></b></size>";
            }
        }
        
        public override float CalculateMagnitude(GameplayEffectSpec spec, float modifierMagnitude)
        {
            var attrMagnitude = base.CalculateMagnitude(spec, modifierMagnitude);
            
            if (spec.Stacking.stackingType == StackingType.None) return attrMagnitude;
            
            var stackMagnitude = spec.StackCount * sK + sB;

            return stackMagnitudeOperation switch
            {
                StackMagnitudeOperation.Add => attrMagnitude + stackMagnitude,
                StackMagnitudeOperation.Multiply => attrMagnitude * stackMagnitude,
                _ => attrMagnitude + stackMagnitude
            };
        }
        
    }
}