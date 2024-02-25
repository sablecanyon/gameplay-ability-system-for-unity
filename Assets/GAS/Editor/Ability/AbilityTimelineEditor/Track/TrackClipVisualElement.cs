using GAS.Runtime.Ability.AbilityTimeline;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
namespace GAS.Editor.Ability.AbilityTimelineEditor.Track
{
    public class TrackClipVisualElement:VisualElement
    {
        public new class UxmlFactory : UxmlFactory<TrackClipVisualElement, UxmlTraits> { }
        protected string ItemAssetGUID => "3197d239f4ce79b41b2278ecea5aaab8";

        private VisualElement _outsideBox;
        private Label _itemLabel;
        private VisualElement _overLine;
        private VisualElement _selectedBottomLine;
        private VisualElement _mainArea;
        private VisualElement _leftResizeArea;
        private VisualElement _rightResizeArea;

        public Label ItemLabel => _itemLabel;

        private TrackClipBase _clip;
        private float FrameUnitWidth=>_clip.FrameUnitWidth;
        private int StartFrameIndex=>_clip.StartFrameIndex;
        private int EndFrameIndex => _clip.EndFrameIndex;
        private int DurationFrame => _clip.DurationFrame;
        
        private int _startDragFrameIndex;
        private float _startDragX;
        private bool _dragging;
        
        
        public TrackClipVisualElement()
        {
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(AssetDatabase.GUIDToAssetPath(ItemAssetGUID));
            visualTree.CloneTree(this);
            AddToClassList("trackItem");
            
            _outsideBox = this.Q<VisualElement>("OutsideBox");
            _itemLabel = this.Q<Label>("ItemLabel");
            _overLine = this.Q<VisualElement>("OverLine");
            _selectedBottomLine = this.Q<VisualElement>("SelectedBottomLine");
            _mainArea = this.Q<VisualElement>("MainArea");
            _leftResizeArea = this.Q<VisualElement>("LeftResizeArea");
            _rightResizeArea = this.Q<VisualElement>("RightResizeArea");
            
            
            _mainArea.RegisterCallback<MouseDownEvent>(OnMainMouseDown);
            _mainArea.RegisterCallback<MouseUpEvent>(OnMainMouseUp);
            _mainArea.RegisterCallback<MouseMoveEvent>(OnMainMouseMove);
            _mainArea.RegisterCallback<MouseOutEvent>(OnMouseOut);
            _mainArea.generateVisualContent += OnDrawBoxGenerateVisualContent;
        }

        public void InitClipInfo(TrackClipBase trackClipBase)
        {
            _clip = trackClipBase;
        }
        
        private void OnDrawBoxGenerateVisualContent(MeshGenerationContext mgc)
        {
            bool Hovered = true;
            if (Hovered)
            {
                var paint2D = mgc.painter2D;
                paint2D.strokeColor = new Color(68, 192, 255, 255);
                paint2D.BeginPath();
                paint2D.MoveTo(new Vector2(0, 0));
                paint2D.LineTo(new Vector2(_mainArea.worldBound.width, 0));
                paint2D.LineTo(new Vector2(_mainArea.worldBound.width, _mainArea.worldBound.height));
                paint2D.LineTo(new Vector2(0, _mainArea.worldBound.height));
                paint2D.LineTo(new Vector2(0, 0));
                paint2D.Stroke();
            }
        }
        
        #region Mouse Event

        protected void OnMainMouseDown(MouseDownEvent evt)
        {
            _dragging = true;
            _startDragFrameIndex = StartFrameIndex;
            _startDragX = evt.mousePosition.x;
            Select();
        }

        protected void OnMainMouseUp(MouseUpEvent evt)
        {
            _dragging = false;
            ApplyDrag();
        }

        protected void OnMainMouseMove(MouseMoveEvent evt)
        {
            if (_dragging)
            {
                var offset = evt.mousePosition.x - _startDragX;
                var offsetFrame = Mathf.RoundToInt(offset / FrameUnitWidth);
                var targetFrame = _startDragFrameIndex + offsetFrame;
                if (offsetFrame == 0 || targetFrame < 0) return;

                var checkDrag = offsetFrame > 0
                    ? _clip.trackBase.CheckFrameIndexOnDrag(targetFrame + AnimationClipEvent.durationFrame)
                    : _clip.trackBase.CheckFrameIndexOnDrag(targetFrame);

                if (checkDrag)
                {
                    _clip.StartFrameIndex = targetFrame;
                    if (EndFrameIndex > AbilityTimelineEditorWindow.Instance.AbilityAsset.MaxFrameCount)
                        AbilityTimelineEditorWindow.Instance.CurrentSelectFrameIndex = EndFrameIndex;
                    RefreshShow(FrameUnitWidth);
                    AbilityTimelineEditorWindow.Instance.SetInspector(this);
                }
            }
        }

        protected void OnMouseOut(MouseOutEvent evt)
        {
            if (_dragging) ApplyDrag();
            _dragging = false;
        }

        protected void ApplyDrag()
        {
            if (StartFrameIndex != _startDragFrameIndex) _clip.trackBase.SetFrameIndex(_startDragFrameIndex, StartFrameIndex);
        }

        #endregion
        
        #region Select

        protected static readonly Color NormalColor = new(0, 0.8f, 0.1f, 0.5f);
        protected static readonly Color SelectColor = new(1, 0.8f, 0.1f, 0.5f);

        public virtual void Select()
        {
            // 更新小面板
            AbilityTimelineEditorWindow.Instance.SetInspector(this);
        }

        public virtual void OnSelect()
        {
            style.backgroundColor = SelectColor;
        }

        public virtual void OnUnSelect()
        {
            style.backgroundColor = NormalColor;
        }

        #endregion
    }
}
#endif