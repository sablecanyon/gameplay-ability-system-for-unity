using System.Collections.Generic;
using UnityEngine;

namespace GAS.Runtime
{
    public class AbilityContainer
    {
        private readonly AbilitySystemComponent _owner;
        private readonly Dictionary<string, AbilitySpec> _abilities = new Dictionary<string, AbilitySpec>();
        private readonly List<AbilitySpec> _cachedAbilities = new List<AbilitySpec>();

        public AbilityContainer(AbilitySystemComponent owner)
        {
            _owner = owner;
        }

        public void Tick()
        {
            _cachedAbilities.AddRange(_abilities.Values);

            foreach (var abilitySpec in _cachedAbilities)
            {
                abilitySpec.Tick();
            }

            _cachedAbilities.Clear();
        }

        public void GrantAbility(AbstractAbility ability)
        {
            if (_abilities.ContainsKey(ability.Name)) return;
            var abilitySpec = ability.CreateSpec(_owner);
            _abilities.Add(ability.Name, abilitySpec);
        }

        public void RemoveAbility(AbstractAbility ability)
        {
            RemoveAbility(ability.Name);
        }

        public void RemoveAbility(string abilityName)
        {
            if (!_abilities.ContainsKey(abilityName)) return;

            EndAbility(abilityName);
            _abilities[abilityName].Dispose();
            _abilities.Remove(abilityName);
        }

        public bool TryActivateAbility(string abilityName, params object[] args)
        {
            if (!_abilities.ContainsKey(abilityName))
            {
                // Development Guide:
                // If your Preset has an inherent skill configured but there is no such skill (even no skill in _abilities)
                // Maybe you forgot to call ASC::Init(), please check the initialization of AbilitySystemComponent
                // Usually we use ASC::InitWithPreset() to indirectly call ASC::Init() to perform initialization
#if UNITY_EDITOR
                // This output can be deleted. In some cases, it will try to activate a non-existent skill (it doesn't matter if it fails), but it is helpful for debugging during development
                Debug.LogWarning(
                    $"you are trying to activate an ability that does not exist: " +
                    $"abilityName=\"{abilityName}\", GameObject=\"{_owner.name}\", " +
                    $"Preset={(_owner.Preset != null ? _owner.Preset.name : "null")}");
#endif
                return false;
            }

            if (!_abilities[abilityName].TryActivateAbility(args)) return false;

            var tags = _abilities[abilityName].Ability.Tag.CancelAbilitiesWithTags;
            foreach (var kv in _abilities)
            {
                var abilityTag = kv.Value.Ability.Tag;
                if (abilityTag.AssetTag.HasAnyTags(tags))
                {
                    _abilities[kv.Key].TryCancelAbility();
                }
            }

            return true;
        }

        public void EndAbility(string abilityName)
        {
            if (!_abilities.ContainsKey(abilityName)) return;
            _abilities[abilityName].TryEndAbility();
        }

        public void CancelAbility(string abilityName)
        {
            if (!_abilities.ContainsKey(abilityName)) return;
            _abilities[abilityName].TryCancelAbility();
        }

        void CancelAbilitiesByTag(GameplayTagSet tags)
        {
            foreach (var kv in _abilities)
            {
                var abilityTag = kv.Value.Ability.Tag;
                if (abilityTag.AssetTag.HasAnyTags(tags))
                {
                    _abilities[kv.Key].TryCancelAbility();
                }
            }
        }

        public Dictionary<string, AbilitySpec> AbilitySpecs() => _abilities;

        public void CancelAllAbilities()
        {
            foreach (var kv in _abilities)
                _abilities[kv.Key].TryCancelAbility();
        }

        public bool HasAbility(string abilityName) => _abilities.ContainsKey(abilityName);

        public bool HasAbility(AbstractAbility ability) => HasAbility(ability.Name);
    }
}