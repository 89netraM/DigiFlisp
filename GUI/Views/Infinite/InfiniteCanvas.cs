using Avalonia;
using Avalonia.Input;
using Avalonia.Controls;
using System;
using Avalonia.Media;

namespace GUI.Views.Infinite
{
	public class InfiniteCanvas : Canvas
	{
		#region Child properties

		public static readonly AttachedProperty<Point> PositionProperty = AvaloniaProperty.RegisterAttached<InfiniteCanvas, InfiniteCanvasBlock, Point>("Position");

		public static readonly AttachedProperty<Size> SizeProperty = AvaloniaProperty.RegisterAttached<InfiniteCanvas, InfiniteCanvasBlock, Size>("Size");

		static InfiniteCanvas()
		{
			AffectsParentArrange<InfiniteCanvas>(PositionProperty, SizeProperty);
		}

		public static Point GetPosition(InfiniteCanvasBlock block)
		{
			return block.GetValue(PositionProperty);
		}

		public static void SetPosition(InfiniteCanvasBlock block, Point value)
		{
			block.SetValue(PositionProperty, new Point(Math.Round(value.X), Math.Round(value.Y)));
		}

		public static Size GetSize(InfiniteCanvasBlock block)
		{
			return block.GetValue(SizeProperty);
		}

		public static void SetSize(InfiniteCanvasBlock block, Size value)
		{
			block.SetValue(SizeProperty, new Size(Math.Round(value.Width), Math.Round(value.Height)));
		}

		#endregion

		private static readonly double[] zoomPoints = { 0.125d, 0.25d, 0.5d, 1.0d, 1.5d, 2.0d };
		private static readonly int zoomPointBase = 3;

		private static readonly double coordinateSize = 10.0d;

		protected Point offset = new Point(0, 0);
		private Point? lastPointerMove = null;

		protected int zoomPoint = zoomPointBase;
		public double ZoomFactor => zoomPoints[zoomPoint];

		public InfiniteCanvas()
		{
			Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));

			PointerPressed += InfiniteCanvas_PointerPressed;
			PointerMoved += InfiniteCanvas_PointerMoved;
			PointerReleased += InfiniteCanvas_PointerReleased;
			PointerWheelChanged += InfiniteCanvas_PointerWheelChanged;
		}

		private void InfiniteCanvas_PointerPressed(object sender, PointerPressedEventArgs e)
		{
			if (e.GetCurrentPoint(this).Properties.IsMiddleButtonPressed)
			{
				lastPointerMove = e.GetPosition(this);
			}
		}

		private void InfiniteCanvas_PointerMoved(object sender, PointerEventArgs e)
		{
			if (e.GetCurrentPoint(this).Properties.IsMiddleButtonPressed && lastPointerMove.HasValue)
			{
				Point nextPointerMove = e.GetPosition(this);
				offset += nextPointerMove - lastPointerMove.Value;
				lastPointerMove = nextPointerMove;

				InvalidateArrange();
			}
		}

		private void InfiniteCanvas_PointerReleased(object sender, PointerReleasedEventArgs e)
		{
			if (e.InitialPressMouseButton == MouseButton.Middle)
			{
				lastPointerMove = null;
			}
		}

		private void InfiniteCanvas_PointerWheelChanged(object sender, PointerWheelEventArgs e)
		{
			double zoomFactorBefore = ZoomFactor;
			Point offsetBefore = offset;
			Point pointer = e.GetPosition(this);

			zoomPoint = Math.Max(0, Math.Min(zoomPoint + (int)(e.Delta.Y / Math.Abs(e.Delta.Y)), zoomPoints.Length - 1));

			offset = pointer - ((pointer - offsetBefore) * (ZoomFactor / zoomFactorBefore));

			InvalidateArrange();
		}

		protected override Size ArrangeOverride(Size arrangeSize)
		{
			foreach (IControl child in Children)
			{
				if (child is InfiniteCanvasBlock childBlock)
				{
					Size originalSize = childBlock.Bounds.Size;
					Size zoomedSize = originalSize * ZoomFactor;
					Vector halfDimensions = new Vector(zoomedSize.Width - originalSize.Width, zoomedSize.Height - originalSize.Height) / 2.0d;
					Point position = offset + GetPosition(childBlock) * coordinateSize * ZoomFactor + halfDimensions;

					Size size = GetSize(childBlock) * coordinateSize;

					childBlock.Arrange(new Rect(position, size));
					childBlock.Scale = ZoomFactor;
				}
			}

			return arrangeSize;
		}
	}
}