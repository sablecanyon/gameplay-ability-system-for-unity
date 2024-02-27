using System.Collections.Generic;
using GAS.Editor.Ability.AbilityTimelineEditor.Track.AnimationTrack;
using GAS.Runtime.Ability.AbilityTimeline;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
namespace GAS.Editor.Ability.AbilityTimelineEditor.Track
{
    public abstract class TrackBase
    {
        protected TrackDataBase _trackData;
        protected List<AnimationTrackClip> _trackItems = new List<AnimationTrackClip>();

        protected float _frameWidth;
        protected VisualElement Menu;
        protected VisualElement MenuParent;
        protected VisualElement Track;
        protected VisualElement TrackParent;
        private static string TrackAssetGuid => "67e1b3c42dcc09a4dbb9e9b107500dfd";
        private static string MenuAssetGuid => "afb618c74510baa41a7d3928c0e57641";

        public abstract void TickView(int frameIndex, params object[] param);

        public virtual void Init(VisualElement trackParent, VisualElement menuParent, float frameWidth,TrackDataBase trackData)
        {
            _trackData = trackData;
            TrackParent = trackParent;
            MenuParent = menuParent;
            var trackAssetPath = AssetDatabase.GUIDToAssetPath(TrackAssetGuid);
            var menuAssetPath = AssetDatabase.GUIDToAssetPath(MenuAssetGuid);
            Track = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(trackAssetPath).Instantiate().Query().ToList()[1];
            Menu = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(menuAssetPath).Instantiate().Query().ToList()[1];
            TrackParent.Add(Track);
            MenuParent.Add(Menu);

            _frameWidth = frameWidth;
            
            Track.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            Track.RegisterCallback<PointerOutEvent>(OnPointerOut);
            Track.AddManipulator(new ContextualMenuManipulator(OnContextMenu));
        }

        private void OnPointerOut(PointerOutEvent evt)
        {
            foreach (var clipViewPair in _trackItems) clipViewPair.Ve.OnHover(false);
        }

        private void OnPointerMove(PointerMoveEvent evt)
        {
            var mousePos = evt.position;
            foreach (var clipViewPair in _trackItems)
            {
                clipViewPair.Ve.OnHover(false);
                if (!clipViewPair.Ve.InClipRect(mousePos)) continue;
                clipViewPair.Ve.OnHover(true);
                evt.StopImmediatePropagation();
                return;
            }
        }

        public virtual void RefreshShow(float newFrameWidth)
        {
            _frameWidth = newFrameWidth;
        }

        public virtual void RefreshShow()
        {
            RefreshShow(_frameWidth);
        }

        #region Select

        public void Select()
        {
        }

        public void OnSelect()
        {
        }

        public void OnUnSelect()
        {
        }

        #endregion

        
        #region Operation of Track

        private void OnContextMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Add Clip", OnAddClip, DropdownMenuAction.AlwaysEnabled);
            evt.menu.AppendAction("Remove Track", OnRemoveTrack, DropdownMenuAction.AlwaysEnabled);
        }
        
        protected virtual void OnAddClip(DropdownMenuAction action)
        {
            // TODO 添加Clip
            Debug.Log($"[EX]Menu Item Clicked: {action.name}");
        }
        
        protected virtual void OnRemoveTrack(DropdownMenuAction action)
        {
            // TODO 删除Track
            Debug.Log($"[EX]Menu Item Clicked: {action.name}");
        }

        #endregion
    }
}
#endif