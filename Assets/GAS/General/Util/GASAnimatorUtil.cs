namespace GAS.General
{
    using System.Collections.Generic;
#if UNITY_EDITOR
    using UnityEditor;
    using UnityEditor.Animations;
#endif
    using UnityEngine;
    
    public static class GASAnimatorUtil
    {
        /// <summary>
        ///     Only For Editor
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="layerIndex"></param>
        /// <returns></returns>
        public static Dictionary<string, AnimationClip> GetAllAnimationState(this Animator animator, int layerIndex = 0)
        {
#if UNITY_EDITOR
            var result = new Dictionary<string, AnimationClip>();

            var runtimeController = animator.runtimeAnimatorController;
            if (runtimeController == null)
            {
                Debug.LogError("RuntimeAnimatorController must not be null.");
                return null;
            }

            if (animator.runtimeAnimatorController is AnimatorOverrideController)
            {
                var overrideController =
                    AssetDatabase.LoadAssetAtPath<AnimatorOverrideController>(
                        AssetDatabase.GetAssetPath(runtimeController));
                if (overrideController == null)
                {
                    Debug.LogErrorFormat("AnimatorOverrideController must not be null.");
                    return null;
                }

                var controller =
                    AssetDatabase.LoadAssetAtPath<AnimatorController>(
                        AssetDatabase.GetAssetPath(overrideController.runtimeAnimatorController));
                var overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
                overrideController.GetOverrides(overrides);
                // Get the state machine of the Layer
                var stateMachine = controller.layers[layerIndex].stateMachine;
                // Iterate through all states and print the name
                foreach (var state in stateMachine.states)
                {
                    if (state.state.motion is AnimationClip clip)
                    {
                        foreach (var pair in overrides)
                        {
                            if (pair.Key.name == clip.name)
                            {
                                result.Add(state.state.name, pair.Value);
                                break;
                            }
                        }
                        
                        if(!result.ContainsKey(state.state.name)) result.Add(state.state.name, clip);
                    }
                }
            }
            else
            {
                var controller =
                    AssetDatabase.LoadAssetAtPath<AnimatorController>(AssetDatabase.GetAssetPath(runtimeController));
                if (controller == null)
                {
                    Debug.LogErrorFormat("AnimatorController must not be null.");
                    return null;
                }

                // Get the state machine of the first Layer
                var stateMachine = controller.layers[layerIndex].stateMachine;
                // Iterate through all states and print the name
                foreach (var state in stateMachine.states)
                    result.Add(state.state.name, state.state.motion as AnimationClip);
            }

            return result;
#endif
            return null;
        }
    }
}
