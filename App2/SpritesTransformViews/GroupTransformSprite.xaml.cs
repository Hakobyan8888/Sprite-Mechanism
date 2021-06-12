using App2.MultiSelectObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace App2.SpritesTransformViews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GroupTransformSprite : Canvas
    {
        public bool IsManipulation;
        private Point _startPoint;

        public double Radian => Math.PI / 180 * ViewModel.Rotation;


        public UiObject ViewModel
        {
            get => (UiObject)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(UiObject), typeof(GroupTransformSprite), new PropertyMetadata(0));



        public GroupTransformSprite()
        {
            this.InitializeComponent();
        }

        #region MiddleManipulations
        private void TopMiddle_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            IsManipulation = false;

            CalculateLastPoint();
            var pos = e.Position;
            var delta = pos.Y - _startPoint.Y;
            if (ViewModel.Height - delta >= ViewModel.MinSpriteSize)
            {
                if (MultiSelectionHub.Instance.CurrentMultiSelection == null)
                {
                    ViewModel.RotateObjects(0, 0);
                    ViewModel.HandleTopScale(ViewModel, ViewModel, 1, (float)delta, 0);
                    ViewModel.RotateObjects(ViewModel.Rotation, 0);
                }
                else
                {
                    MultiSelectionHub.Instance.CurrentMultiSelection.HandleTopScale(ViewModel, ViewModel, 1, (float)delta, 0);
                }
            }
        }
        private void BottomMiddle_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            IsManipulation = false;
            CalculateLastPoint();
            var pos = e.Position;
            var delta = _startPoint.Y - pos.Y;
            if (ViewModel.Height - delta >= ViewModel.MinSpriteSize)
            {
                if (MultiSelectionHub.Instance.CurrentMultiSelection == null)
                {
                    ViewModel.RotateObjects(0, 0);
                    ViewModel.HandleBottomScale(ViewModel, ViewModel, 1, (float)delta);
                    ViewModel.RotateObjects(ViewModel.Rotation, 0);
                }
                else
                {
                    MultiSelectionHub.Instance.CurrentMultiSelection.HandleBottomScale(ViewModel, ViewModel, 1, (float)delta);
                }
            }
        }
        private void LeftMiddle_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            IsManipulation = false;
            CalculateLastPoint();
            var pos = e.Position;
            var delta = pos.X - _startPoint.X;
            if (ViewModel.Width - delta >= ViewModel.MinSpriteSize)
            {
                if (MultiSelectionHub.Instance.CurrentMultiSelection == null)
                {
                    ViewModel.RotateObjects(0, 0);
                    ViewModel.HandleLeftScale(ViewModel, ViewModel, 1, (float)delta, 0);
                    ViewModel.RotateObjects(ViewModel.Rotation, 0);
                }
                else
                {
                    MultiSelectionHub.Instance.CurrentMultiSelection.HandleLeftScale(ViewModel, ViewModel, 1, (float)delta, 0);
                }
            }
        }
        private void RightMiddle_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            IsManipulation = false;
            var pos = e.Position;
            var delta = _startPoint.X - pos.X;
            if (ViewModel.Width - delta >= ViewModel.MinSpriteSize)
            {
                if (MultiSelectionHub.Instance.CurrentMultiSelection == null)
                {
                    ViewModel.RotateObjects(0, 0);
                    ViewModel.HandleRightScale(ViewModel, ViewModel, 1, (float)delta);
                    ViewModel.RotateObjects(ViewModel.Rotation, 0);
                }
                else
                    MultiSelectionHub.Instance.CurrentMultiSelection.HandleRightScale(ViewModel, ViewModel, 1, (float)delta);
            }
        }

        #endregion

        #region CornerManipulations
        private void LeftTop_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            IsManipulation = false;

            CalculateLastPoint();
            var pos = e.Position;
            var deltaX = pos.X - _startPoint.X;

            var scale = Math.Abs(ViewModel.Width / (ViewModel.Width - deltaX));
            var deltaY = ViewModel.Height - ViewModel.Height / scale;
            if (ViewModel.Width - deltaX >= ViewModel.MinSpriteSize)
            {
                if (MultiSelectionHub.Instance.CurrentMultiSelection == null)
                {
                    ViewModel.RotateObjects(0, 0);
                    ViewModel.HandleLeftScale(ViewModel, ViewModel, 1, (float)deltaX, 0);
                    ViewModel.HandleTopScale(ViewModel, ViewModel, 1, (float)deltaY, 0);
                    ViewModel.RotateObjects(ViewModel.Rotation, 0);
                }
                else
                {
                    MultiSelectionHub.Instance.CurrentMultiSelection.HandleLeftScale(ViewModel, ViewModel, 1, (float)deltaX, 0);
                    MultiSelectionHub.Instance.CurrentMultiSelection.HandleTopScale(ViewModel, ViewModel, 1, (float)deltaY, 0);
                }
            }
        }
        private void RightTop_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            IsManipulation = false;

            CalculateLastPoint();
            var pos = e.Position;
            var deltaX = _startPoint.X - pos.X;
            var scale = Math.Abs(ViewModel.Width / (ViewModel.Width - deltaX));
            var deltaY = ViewModel.Height - ViewModel.Height / scale;
            if (ViewModel.Width - deltaX >= ViewModel.MinSpriteSize
                && ViewModel.Height - deltaY >= ViewModel.MinSpriteSize)
            {
                if (MultiSelectionHub.Instance.CurrentMultiSelection == null)
                {
                    ViewModel.RotateObjects(0, 0);
                    ViewModel.HandleRightScale(ViewModel, ViewModel, 1, (float)deltaX);
                    ViewModel.HandleTopScale(ViewModel, ViewModel, 1, (float)deltaY, 0);
                    ViewModel.RotateObjects(ViewModel.Rotation, 0);
                }
                else
                {
                    MultiSelectionHub.Instance.CurrentMultiSelection.HandleRightScale(ViewModel, ViewModel, 1, (float)deltaX);
                    MultiSelectionHub.Instance.CurrentMultiSelection.HandleTopScale(ViewModel, ViewModel, 1, (float)deltaY, 0);
                }
            }
        }
        private void LeftBottom_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            IsManipulation = false;
            CalculateLastPoint();
            var pos = e.Position;
            var deltaX = pos.X - _startPoint.X;

            var scale = Math.Abs(ViewModel.Width / (ViewModel.Width - deltaX));
            var deltaY = ViewModel.Height - ViewModel.Height / scale;
            if (ViewModel.Width - deltaX >= ViewModel.MinSpriteSize
                && ViewModel.Height - deltaY >= ViewModel.MinSpriteSize)
            {
                if (MultiSelectionHub.Instance.CurrentMultiSelection == null)
                {
                    ViewModel.RotateObjects(0, 0);
                    ViewModel.HandleLeftScale(ViewModel, ViewModel, 1, (float)deltaX, 0);
                    ViewModel.HandleBottomScale(ViewModel, ViewModel, 1, (float)deltaY);
                    ViewModel.RotateObjects(ViewModel.Rotation, 0);
                }
                else
                {
                    MultiSelectionHub.Instance.CurrentMultiSelection.HandleLeftScale(ViewModel, ViewModel, 1, (float)deltaX, 0);
                    MultiSelectionHub.Instance.CurrentMultiSelection.HandleBottomScale(ViewModel, ViewModel, 1, (float)deltaY);
                }
            }
        }
        private void RightButtom_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            IsManipulation = false;

            var pos = e.Position;
            var deltaX = _startPoint.X - pos.X;

            var scale = Math.Abs(ViewModel.Width / (ViewModel.Width - deltaX));
            var deltaY = ViewModel.Height - ViewModel.Height / scale;
            if (ViewModel.Width - deltaX >= ViewModel.MinSpriteSize
                && ViewModel.Height - deltaY >= ViewModel.MinSpriteSize)
            {
                if (MultiSelectionHub.Instance.CurrentMultiSelection == null)
                {
                    ViewModel.RotateObjects(0, 0);
                    ViewModel.HandleRightScale(ViewModel, ViewModel, 1, (float)deltaX);
                    ViewModel.HandleBottomScale(ViewModel, ViewModel, 1, (float)deltaY);
                    ViewModel.RotateObjects(ViewModel.Rotation, 0);
                }
                else
                {
                    MultiSelectionHub.Instance.CurrentMultiSelection.HandleRightScale(ViewModel, ViewModel, 1, (float)deltaX);
                    MultiSelectionHub.Instance.CurrentMultiSelection.HandleBottomScale(ViewModel, ViewModel, 1, (float)deltaY);
                }
            }
        }
        #endregion

        private void Move_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (IsManipulation)
            {
                var deltaWidth = ViewModel.Width * (1 - e.Delta.Scale);
                var deltaHeight = ViewModel.Height * (1 - e.Delta.Scale);
                if (ViewModel.Width - deltaWidth < ViewModel.MinSpriteSize
                    || ViewModel.Height < ViewModel.MinSpriteSize)
                    deltaWidth = deltaHeight = 0;
                if (MultiSelectionHub.Instance.CurrentMultiSelection == null)
                    ViewModel.Rotation += e.Delta.Rotation;
                else
                    MultiSelectionHub.Instance.CurrentMultiSelection.Rotation += e.Delta.Rotation;

                ViewModel.RotateObjects(0, 0);
                CalculateLastPoint();
                if (e.Delta.Scale == 1)
                {
                    if (MultiSelectionHub.Instance.CurrentMultiSelection == null)
                        ViewModel.HandleObjectMove(ViewModel, ViewModel, (float)e.Delta.Translation.X, (float)e.Delta.Translation.Y);
                    else
                        MultiSelectionHub.Instance.CurrentMultiSelection.HandleObjectMove(ViewModel, ViewModel, (float)e.Delta.Translation.X, (float)e.Delta.Translation.Y);
                }
                else
                {
                    if (MultiSelectionHub.Instance.CurrentMultiSelection == null)
                    {
                        ViewModel.HandleRightScale(ViewModel, ViewModel, 1, deltaWidth / 2);
                        ViewModel.HandleLeftScale(ViewModel, ViewModel, 1, deltaWidth / 2, 0);
                        ViewModel.HandleBottomScale(ViewModel, ViewModel, 1, deltaHeight / 2);
                        ViewModel.HandleTopScale(ViewModel, ViewModel, 1, deltaHeight / 2, 0);
                    }
                    else
                    {
                        MultiSelectionHub.Instance.CurrentMultiSelection.HandleRightScale(ViewModel, ViewModel, 1, deltaWidth / 2);
                        MultiSelectionHub.Instance.CurrentMultiSelection.HandleLeftScale(ViewModel, ViewModel, 1, deltaWidth / 2, 0);
                        MultiSelectionHub.Instance.CurrentMultiSelection.HandleBottomScale(ViewModel, ViewModel, 1, deltaHeight / 2);
                        MultiSelectionHub.Instance.CurrentMultiSelection.HandleTopScale(ViewModel, ViewModel, 1, deltaHeight / 2, 0);
                    }
                }
                ViewModel.RotateObjects(ViewModel.Rotation, 0);
            }
            else
            {
                IsManipulation = true;
            }
        }

        private void Rotate_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            IsManipulation = false;
            //Window.Current.CoreWindow.PointerCursor = RotationCursor;
            var point = e.Container.TransformToVisual(this).TransformPoint(e.Position);
            var center = new Point(ViewModel.CanvasLeft + (ViewModel.Width / 2), ViewModel.CanvasTop + (ViewModel.Height / 2));
            var start = Rotate.TransformToVisual(this).TransformPoint(new Point(Rotate.ActualWidth / 2, Rotate.ActualHeight / 2));

            var rotationDelta = (float)AngleBetweenTwoLines(center, start, center, point);
            if (MultiSelectionHub.Instance.CurrentMultiSelection == null)
            {
                ViewModel.RotateObjects(ViewModel.Rotation, 0);
                ViewModel.Rotation -= rotationDelta;
            }
            else
                MultiSelectionHub.Instance.CurrentMultiSelection.Rotation -= rotationDelta;
        }

        private double AngleBetweenTwoLines(Point primaryContact1, Point secondaryContact1, Point primaryContact2, Point secondaryContact2)
        {
            double angle1 = Math.Atan2(primaryContact1.Y - secondaryContact1.Y, primaryContact1.X - secondaryContact1.X);
            double angle2 = Math.Atan2(primaryContact2.Y - secondaryContact2.Y, primaryContact2.X - secondaryContact2.X);
            return (angle1 - angle2) * 180 / Math.PI;
        }

        private void CalculateLastPoint()
        {
            //var generalTransformStart = LeftTop.TransformToVisual(this);
            //var posEnd = generalTransformStart.TransformPoint(new Point(LeftTop.ActualWidth / 2, LeftTop.ActualHeight / 2));
            //ViewModel.TopLeftPoint = posEnd;
            //generalTransformStart = RightTop.TransformToVisual(this);
            //posEnd = generalTransformStart.TransformPoint(new Point(RightTop.ActualWidth / 2, RightTop.ActualHeight / 2));
            //ViewModel.TopRightPoint = posEnd;
            //generalTransformStart = LeftBottom.TransformToVisual(this);
            //posEnd = generalTransformStart.TransformPoint(new Point(LeftBottom.ActualWidth / 2, LeftBottom.ActualHeight / 2));
            //ViewModel.LeftBottomPoint = posEnd;
            //generalTransformStart = RightBottom.TransformToVisual(this);
            //posEnd = generalTransformStart.TransformPoint(new Point(RightBottom.ActualWidth / 2, RightBottom.ActualHeight / 2));
            //ViewModel.RightBottomPoint = posEnd;
        }

        #region SizeIcon

        private void Rotate_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            //Window.Current.CoreWindow.PointerCursor = RotationCursor;
        }

        private void Rotate_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            //Window.Current.CoreWindow.PointerCursor = DefaultCursor;
        }

        private void LeftTop_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            //Window.Current.CoreWindow.PointerCursor = null;
            //var Pos = e.GetCurrentPoint(sender as UIElement).Position;
            //PART_Left_Top.Margin = new Thickness(
            //                     Pos.X - PART_Left_Top.Width / 2,
            //                     Pos.Y - PART_Left_Top.Height / 2,
            //                     -PART_Left_Top.Width,
            //                     -PART_Left_Top.Height);
            //PART_Left_Top.Visibility = Visibility.Visible;
        }

        private void LeftTop_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            //Window.Current.CoreWindow.PointerCursor = DefaultCursor;
            //PART_Left_Top.Visibility = Visibility.Collapsed;
        }

        private void TopMiddle_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            //Window.Current.CoreWindow.PointerCursor = null;
            //var pos = e.GetCurrentPoint(sender as UIElement).Position;
            //PART_NS_Top.Margin = new Thickness(
            //                     pos.X - PART_NS_Top.Width / 2,
            //                     pos.Y - PART_NS_Top.Height / 2,
            //                     -PART_NS_Top.Width,
            //                     -PART_NS_Top.Height);
            //PART_NS_Top.Visibility = Visibility.Visible;
        }

        private void TopMiddle_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            //Window.Current.CoreWindow.PointerCursor = DefaultCursor;
            //PART_NS_Top.Visibility = Visibility.Collapsed;
        }

        private void RightTop_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            //Window.Current.CoreWindow.PointerCursor = null;
            //var pos = e.GetCurrentPoint(sender as UIElement).Position;
            //PART_Right_Top.Margin = new Thickness(
            //                     pos.X - PART_Right_Top.Width / 2,
            //                     pos.Y - PART_Right_Top.Height / 2,
            //                     -PART_Right_Top.Width,
            //                     -PART_Right_Top.Height);
            //PART_Right_Top.Visibility = Visibility.Visible;
        }

        private void RightTop_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            //Window.Current.CoreWindow.PointerCursor = DefaultCursor;
            //PART_Right_Top.Visibility = Visibility.Collapsed;
        }

        private void LeftMiddle_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            //Window.Current.CoreWindow.PointerCursor = null;
            //var pos = e.GetCurrentPoint(sender as UIElement).Position;
            //PART_EW_Left.Margin = new Thickness(
            //                     pos.X - PART_EW_Left.Width / 2,
            //                     pos.Y - PART_EW_Left.Height / 2,
            //                     -PART_EW_Left.Width,
            //                     -PART_EW_Left.Height);
            //PART_EW_Left.Visibility = Visibility.Visible;
        }

        private void LeftMiddle_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            //Window.Current.CoreWindow.PointerCursor = DefaultCursor;
            //PART_EW_Left.Visibility = Visibility.Collapsed;
        }

        private void RightMiddle_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            //Window.Current.CoreWindow.PointerCursor = null;
            //var Pos = e.GetCurrentPoint(sender as UIElement).Position;
            //PART_EW_Right.Margin = new Thickness(
            //                     Pos.X - PART_EW_Right.Width / 2,
            //                     Pos.Y - PART_EW_Right.Height / 2,
            //                     -PART_EW_Right.Width,
            //                     -PART_EW_Right.Height);
            //PART_EW_Right.Visibility = Visibility.Visible;
        }

        private void RightMiddle_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            //Window.Current.CoreWindow.PointerCursor = DefaultCursor;
            //PART_EW_Right.Visibility = Visibility.Collapsed;
        }

        private void BottomMiddle_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            //Window.Current.CoreWindow.PointerCursor = null;
            //var Pos = e.GetCurrentPoint(sender as UIElement).Position;
            //PART_NS_Bottom.Margin = new Thickness(
            //                     Pos.X - PART_NS_Bottom.Width / 2,
            //                     Pos.Y - PART_NS_Bottom.Height / 2,
            //                     -PART_NS_Bottom.Width,
            //                     -PART_NS_Bottom.Height);
            //PART_NS_Bottom.Visibility = Visibility.Visible;
        }

        private void BottomMiddle_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            //Window.Current.CoreWindow.PointerCursor = DefaultCursor;
            //PART_NS_Bottom.Visibility = Visibility.Collapsed;
        }

        private void LeftBottom_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            //Window.Current.CoreWindow.PointerCursor = null;
            //var Pos = e.GetCurrentPoint(sender as UIElement).Position;
            //PART_Left_Bottom.Margin = new Thickness(
            //                     Pos.X - PART_Left_Bottom.Width / 2,
            //                     Pos.Y - PART_Left_Bottom.Height / 2,
            //                     -PART_Left_Bottom.Width,
            //                     -PART_Left_Bottom.Height);
            //PART_Left_Bottom.Visibility = Visibility.Visible;
        }

        private void LeftBottom_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            //Window.Current.CoreWindow.PointerCursor = DefaultCursor;
            //PART_Left_Bottom.Visibility = Visibility.Collapsed;
        }

        private void RightBottom_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            //Window.Current.CoreWindow.PointerCursor = null;
            //var Pos = e.GetCurrentPoint(sender as UIElement).Position;
            //PART_Right_Bottom.Margin = new Thickness(
            //                     Pos.X - PART_Right_Bottom.Width / 2,
            //                     Pos.Y - PART_Right_Bottom.Height / 2,
            //                     -PART_Right_Bottom.Width,
            //                     -PART_Right_Bottom.Height);
            //PART_Right_Bottom.Visibility = Visibility.Visible;
        }

        private void RightBottom_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            //Window.Current.CoreWindow.PointerCursor = DefaultCursor;
            //PART_Right_Bottom.Visibility = Visibility.Collapsed;
        }

        #endregion

        private void AllManipulationsCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            //Window.Current.CoreWindow.PointerCursor = DefaultCursor;
            //PART_Right_Bottom.Visibility = Visibility.Collapsed;
            //PART_Left_Top.Visibility = Visibility.Collapsed;
            //PART_NS_Top.Visibility = Visibility.Collapsed;
            //PART_Right_Top.Visibility = Visibility.Collapsed;
            //PART_EW_Left.Visibility = Visibility.Collapsed;
            //PART_EW_Right.Visibility = Visibility.Collapsed;
            //PART_NS_Bottom.Visibility = Visibility.Collapsed;
            //PART_Left_Bottom.Visibility = Visibility.Collapsed;
        }


        private void ResizeManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            _startPoint = e.Position;
        }

    }
}
