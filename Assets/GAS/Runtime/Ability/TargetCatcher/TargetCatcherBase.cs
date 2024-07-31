using System;
using System.Collections.Generic;
using UnityEngine;

namespace GAS.Runtime
{
    public abstract class TargetCatcherBase
    {
        public AbilitySystemComponent Owner;

        protected TargetCatcherBase()
        {
        }

        public virtual void Init(AbilitySystemComponent owner)
        {
            Owner = owner;
        }

        [Obsolete("Please use the CatchTargetsNonAlloc method to avoid garbage collection (GC).")]
        public List<AbilitySystemComponent> CatchTargets(AbilitySystemComponent mainTarget)
        {
            var result = new List<AbilitySystemComponent>();

            CatchTargetsNonAlloc(mainTarget, result);

            return result;
        }

        public void CatchTargetsNonAllocSafe(AbilitySystemComponent mainTarget, List<AbilitySystemComponent> results)
        {
            results.Clear();

            CatchTargetsNonAlloc(mainTarget, results);
        }

        protected abstract void CatchTargetsNonAlloc(AbilitySystemComponent mainTarget, List<AbilitySystemComponent> results);

#if UNITY_EDITOR
        public virtual void OnEditorPreview(GameObject obj)
        {
        }
#endif
    }
}