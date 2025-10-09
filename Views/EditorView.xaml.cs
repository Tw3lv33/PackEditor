using PackEditor.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PackEditor.Views
{
    public partial class EditorView : UserControl
    {
        private Point _panStart;
        private bool _isPanning = false;
        private double _zoom = 1.0;

        public EditorView(EditorViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            ImageCanvas.MouseWheel += ImageCanvas_MouseWheel;
        }

        private EditorViewModel ViewModel => (EditorViewModel)DataContext;

        private void ImageCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                _isPanning = true;
                _panStart = e.GetPosition(this);
                ImageCanvas.CaptureMouse();
            }
            else if (e.LeftButton == MouseButtonState.Pressed)
            {
                var pos = GetImagePixelPosition(e);
                ViewModel.StartDrawing(pos);
            }
        }

        private void ImageCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isPanning && e.RightButton == MouseButtonState.Pressed)
            {
                var pos = e.GetPosition(this);
                var delta = pos - _panStart;
                _panStart = pos;

                PanTransform.X += delta.X;
                PanTransform.Y += delta.Y;
            }
            else if (e.LeftButton == MouseButtonState.Pressed)
            {
                var pos = GetImagePixelPosition(e);
                ViewModel.ContinueDrawing(pos);
            }
        }

        private void ImageCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_isPanning)
            {
                _isPanning = false;
                ImageCanvas.ReleaseMouseCapture();
            }

            ViewModel.StopDrawing();
        }

        private void ImageCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            const double zoomFactor = 1.1;

            if (e.Delta > 0)
                _zoom *= zoomFactor;
            else
                _zoom /= zoomFactor;

            _zoom = Math.Max(0.1, Math.Min(_zoom, 10));

            ZoomTransform.ScaleX = _zoom;
            ZoomTransform.ScaleY = _zoom;
        }

        private Point GetImagePixelPosition(MouseEventArgs e)
        {
            Point canvasPos = e.GetPosition(ImageCanvas);

            var transformGroup = new TransformGroup();
            transformGroup.Children.Add(ZoomTransform);
            transformGroup.Children.Add(PanTransform);

            var inverse = transformGroup.Inverse;
            return inverse.Transform(canvasPos);
        }
    }
}
