using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace FamilyTree.Encyclopedia
{
    /* Reference EncyclopediaUnitTreeNodeItemBrushWidget */
    public class EncyclopediaFamilyTreeNodeItemBrushWidget : BrushWidget
    {
        private readonly Action<Widget, Widget> _listItemAddedHandler;

        private bool _isLinesDirty;

        private ListPanel _childContainer;

        private Widget _lineContainer;

        private Brush _lineBrush;

        public EncyclopediaFamilyTreeNodeItemBrushWidget(UIContext context) : base(context)
        {
            _listItemAddedHandler = new Action<Widget, Widget>(OnListItemAdded);
        }

        protected override void OnLateUpdate(float dt)
        {
            base.OnLateUpdate(dt);
            if (_isLinesDirty)
            {
                float numDiff2 = 0f;
                for (int i = 0; i < ChildContainer.ChildCount; i++)
                {
                    Widget child = ChildContainer.GetChild(i);
                    Widget lineContainer = LineContainer.GetChild(i);
                    float numBase = GlobalPosition.X + Size.X * 0.5f;
                    float numChild = child.GlobalPosition.X + child.Size.X * 0.5f;
                    float numDiff = numBase - numChild;
                    if (ChildContainer.ChildCount % 2 == 1)
                    {
                        int middle = ChildContainer.ChildCount / 2;
                        Widget middleChild = ChildContainer.GetChild(middle);
                        float numMiddleChild = middleChild.GlobalPosition.X + middleChild.Size.X * 0.5f;
                        numDiff2 = numBase - numMiddleChild;
                        if (i == 0)
                        {
                            child.MarginLeft += numDiff2 * 1.5f;
                        }
                        if (i == middle)
                        {
                            continue;
                        }
                    }
                    if (numBase > numChild)
                    {
                        lineContainer.SetState("Left");
                        float numDiff3 = numDiff - numDiff2;
                        lineContainer.ScaledSuggestedWidth = numDiff3;
                        lineContainer.ScaledPositionXOffset = -numDiff3 * 0.5f - 5f * _scaleToUse;
                    }
                    else if (numBase < numChild)
                    {
                        lineContainer.SetState("Right");
                        float numDiff4 = -numDiff + numDiff2;
                        lineContainer.ScaledSuggestedWidth = numDiff4;
                        lineContainer.ScaledPositionXOffset = numDiff4 * 0.5f + 5f * _scaleToUse;
                    }
                }
                _isLinesDirty = false;
            }
        }

        public void OnListItemAdded(Widget parentWidget, Widget addedWidget)
        {
            Widget widget = CreateLineWidget();
            if (ChildContainer.ChildCount == 1)
            {
                widget.SetState("Straight");
                return;
            }
            if (ChildContainer.ChildCount == 2)
            {
                _isLinesDirty = true;
            }
        }

        private Widget CreateLineWidget()
        {
            BrushWidget brushWidget = new(Context)
            {
                WidthSizePolicy = SizePolicy.Fixed,
                HeightSizePolicy = SizePolicy.StretchToParent,
                Brush = LineBrush
            };
            brushWidget.SuggestedWidth = brushWidget.Brush.Sprite.Width;
            brushWidget.SuggestedHeight = brushWidget.Brush.Sprite.Height;
            brushWidget.HorizontalAlignment = HorizontalAlignment.Center;
            brushWidget.AddState("Left");
            brushWidget.AddState("Right");
            brushWidget.AddState("Straight");
            LineContainer.AddChild(brushWidget);
            return brushWidget;
        }

        [Editor(false)]
        public ListPanel ChildContainer
        {
            get
            {
                return _childContainer;
            }
            set
            {
                if (_childContainer != value)
                {
                    ListPanel childContainer = _childContainer;
                    if (childContainer is not null)
                    {
                        childContainer.ItemAddEventHandlers.Remove(_listItemAddedHandler);
                    }
                    _childContainer = value;
                    OnPropertyChanged(value, "ChildContainer");
                    ListPanel childContainer2 = _childContainer;
                    if (childContainer2 is null)
                    {
                        return;
                    }
                    childContainer2.ItemAddEventHandlers.Add(_listItemAddedHandler);
                }
            }
        }

        [Editor(false)]
        public Widget LineContainer
        {
            get
            {
                return _lineContainer;
            }
            set
            {
                if (_lineContainer != value)
                {
                    _lineContainer = value;
                    OnPropertyChanged(value, "LineContainer");
                }
            }
        }

        [Editor(false)]
        public Brush LineBrush
        {
            get
            {
                return _lineBrush;
            }
            set
            {
                if (_lineBrush != value)
                {
                    _lineBrush = value;
                    OnPropertyChanged(value, "LineBrush");
                }
            }
        }
    }
}
