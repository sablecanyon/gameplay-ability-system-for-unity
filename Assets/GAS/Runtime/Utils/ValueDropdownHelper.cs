#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

namespace GAS.Runtime
{
    public static class ValueDropdownHelper
    {
        /// <summary>
        /// Displays a drop-down list containing all property set names.
        /// <example>
        /// Example of use:
        /// <code>
        /// [ValueDropdown("@ValueDropdownHelper.AttributeSetChoices", IsUniqueList = true)]
        /// public string AttributeSet;
        /// </code>
        /// </example>
        /// </summary>
        public static IEnumerable<string> AttributeSetChoices => ReflectionHelper.AttributeSetNames;

        /// <summary>
        /// Displays a drop-down list containing all property names.
        /// <example>
        /// Example of use:
        /// <code>
        /// [ValueDropdown("@ValueDropdownHelper.AttributeChoices", IsUniqueList = true)]
        /// public string Attribute;
        /// </code>
        /// </example>
        /// </summary>
        public static IEnumerable<string> AttributeChoices => ReflectionHelper.AttributeNames;

        private static ValueDropdownItem[] _gameplayTagChoices;

        /// <summary>
        /// Displays a drop-down list containing all GameplayTags.
        /// <example>
        /// Example of use:
        /// <code>
        /// [ValueDropdown("@ValueDropdownHelper.GameplayTagChoices", IsUniqueList = true, HideChildProperties = true)]
        /// public GameplayTag[] Tags;
        /// </code>
        /// </example>
        /// </summary>
        public static IEnumerable<ValueDropdownItem> GameplayTagChoices
        {
            get
            {
                _gameplayTagChoices ??= ReflectionHelper.GameplayTags
                    .Select(gameplayTag => new ValueDropdownItem(gameplayTag.Name, gameplayTag))
                    .ToArray();
                return _gameplayTagChoices;
            }
        }
    }
}
#endif