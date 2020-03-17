using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;

namespace GUI.Views.Infinite
{
	public class InfiniteCanvasBlock : UserControl
	{
		private Point pointerMoveStart;
		private Point coordinateMoveStart;

		public ScaleTransform Scale { get; } = new ScaleTransform(1.0d, 1.0d);

		public InfiniteCanvasBlock()
		{
			RenderTransformOrigin = new RelativePoint(0.5d, 0.5d, RelativeUnit.Relative);
			TransformGroup transforms = new TransformGroup();
			if (RenderTransform != null)
			{
				transforms.Children.Add(RenderTransform);
			}
			transforms.Children.Add(Scale);
			RenderTransform = transforms;

			Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));

			PointerPressed += InfiniteCanvasBlock_PointerPressed;
			PointerMoved += InfiniteCanvasBlock_PointerMoved;
		}

		private void InfiniteCanvasBlock_PointerPressed(object sender, PointerPressedEventArgs e)
		{
			if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
			{
				pointerMoveStart = Bounds.Center;
				coordinateMoveStart = InfiniteCanvas.GetPosition(this);
			}
		}

		private void InfiniteCanvasBlock_PointerMoved(object sender, PointerEventArgs e)
		{
			if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
			{
				Point pointer = e.GetPosition(Parent);
				Point delta = pointer - pointerMoveStart;

				Size actualSize = Bounds.Size;
				Size infiniteCanvasSize = InfiniteCanvas.GetSize(this);
				Point deltaPosition = new Point(
					delta.X / (actualSize.Width / infiniteCanvasSize.Width),
					delta.Y / (actualSize.Height / infiniteCanvasSize.Height)
				);

				InfiniteCanvas.SetPosition(this, coordinateMoveStart + deltaPosition);
			}
		}
	}
}