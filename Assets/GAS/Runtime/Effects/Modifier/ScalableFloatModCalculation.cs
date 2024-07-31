using Sirenix.OdinInspector;
using UnityEngine;

namespace GAS.Runtime
{
    [CreateAssetMenu(fileName = "ScalableFloatModCalculation", menuName = "GAS/MMC/ScalableFloatModCalculation")]
    public class ScalableFloatModCalculation : ModifierMagnitudeCalculation
    {
        private const string Desc = "Calculation formula: ModifierMagnitude * k + b";

        private const string Detail =
            "ScalableFloatModCalculation: Scalable floating point calculation\nThis type calculates the Modifier modulus value based on Magnitude. The calculation formula is: ModifierMagnitude * k + b. It is actually a linear function. k and b are editable parameters that can be set in the editor.";

        [DetailedInfoBox(Desc, Detail, InfoMessageType.Info)] [SerializeField]
        private float k = 1f;

        [SerializeField] private float b = 0f;

        public override float CalculateMagnitude(GameplayEffectSpec spec, float input)
        {
            return input * k + b;
        }
    }
}