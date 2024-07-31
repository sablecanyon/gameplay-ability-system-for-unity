using System;
using System.Collections.Generic;
using System.Reflection;
using GAS.General;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace GAS.Runtime
{
    public abstract class TimelineAbilityAssetBase : AbilityAsset
    {
        [TitleGroup("Data")]
        [HorizontalGroup("Data/H1", 1 / 3f)]
        [TabGroup("Data/H1/V1", "Timeline", SdfIconType.ClockHistory, TextColor = "#00FF00")]
        [Button("View/Edit Ability Timeline", ButtonSizes.Large, Icon = SdfIconType.Hammer)]
        [PropertyOrder(-1)]
        private void EditAbilityTimeline()
        {
            try
            {
                var assembly = Assembly.Load("com.exhard.exgas.editor");
                var type = assembly.GetType("GAS.Editor.AbilityTimelineEditorWindow");
                var methodInfo = type.GetMethod("ShowWindow", BindingFlags.Public | BindingFlags.Static);
                methodInfo!.Invoke(null, new object[] { this });
            }
            catch (Exception e)
            {
                Debug.LogError(
                    $"Calling the static method ShowWindow(TimelineAbilityAsset asset) of the \"GAS.Editor.AbilityTimelineEditorWindow\" class failed. The code may have been refactored: {e}");
            }
        }

        [TabGroup("Data/H1/V1", "Timeline")]
        [LabelText(GASTextDefine.ABILITY_MANUAL_ENDABILITY)]
        [LabelWidth(100)]
        public bool manualEndAbility;

        [HideInInspector]
        public int FrameCount; // Ability end time

        [HideInInspector]
        public List<DurationalCueTrackData> DurationalCues = new List<DurationalCueTrackData>();

        [HideInInspector]
        public List<InstantCueTrackData> InstantCues = new List<InstantCueTrackData>();

        [HideInInspector]
        public List<ReleaseGameplayEffectTrackData> ReleaseGameplayEffect = new List<ReleaseGameplayEffectTrackData>();

        [HideInInspector]
        public List<BuffGameplayEffectTrackData> BuffGameplayEffects = new List<BuffGameplayEffectTrackData>();

        [HideInInspector]
        public List<TaskMarkEventTrackData> InstantTasks = new List<TaskMarkEventTrackData>();

        [HideInInspector]
        public List<TaskClipEventTrackData> OngoingTasks = new List<TaskClipEventTrackData>();

#if UNITY_EDITOR
        public void Save()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
#endif
    }

    public abstract class TimelineAbilityAssetT<T> : TimelineAbilityAssetBase where T : class
    {
        public sealed override Type AbilityType()
        {
            return typeof(T);
        }
    }

    /// <summary>
    /// This is the simplest TimelineAbilityAsset implementation. If you want to implement a more complex TimelineAbilityAsset, please use TimelineAbilityAssetBase or TimelineAbilityAssetT as the base class.
    /// </summary>
    public sealed class TimelineAbilityAsset : TimelineAbilityAssetT<TimelineAbility>
    {
    }
}