using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.Input;
using PackEditor.Models;

namespace PackEditor.ViewModels
{
    public partial class EditorViewModel : ObservableObject
    {
        // Editable image and brush properties
        [ObservableProperty]
        private WriteableBitmap editableImage;
        [ObservableProperty]
        private int brushSize = 1;

        public Action<WriteableBitmap>? OnImageChanged { get; set; }
        // Drawing state
        private bool isDrawing;
        private readonly ImageHistory imageHistory = new();
        private Point lastPoint;

        // Start a new drawing stroke
        public void StartDrawing(Point point)
        {
            if (EditableImage != null)
            {
                imageHistory.Push(EditableImage);
                OnPropertyChanged(nameof(CanUndo));
            }

            lastPoint = point;
            isDrawing = true;
        }
        public EditorViewModel()
        {
            // Optionally push the initial image to history
            if (EditableImage != null)
                imageHistory.Push(EditableImage);
        }

        // Continue the drawing stroke
        public void ContinueDrawing(Point point)
        {
            if (!isDrawing || EditableImage == null) return;
            var drawColor = HslToRgb(BrushHue, BrushSaturation, BrushLightness);
            DrawLine(lastPoint, point, drawColor, BrushSize);
            lastPoint = point;
        }

        // Stop the drawing stroke
        public void StopDrawing() => isDrawing = false;

        // Function to draw a line using Bresenham's Algorithm
        private void DrawLine(Point p1, Point p2, Color color, int thickness)
        {
            int x0 = (int)p1.X;
            int y0 = (int)p1.Y;
            int x1 = (int)p2.X;
            int y1 = (int)p2.Y;

            EditableImage.Lock();

            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = -Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = dx + dy, e2;

            while (true)
            {
                SetPixelSafe(EditableImage, x0, y0, color, thickness);
                if (x0 == x1 && y0 == y1) break;
                e2 = 2 * err;
                if (e2 >= dy) { err += dy; x0 += sx; }
                if (e2 <= dx) { err += dx; y0 += sy; }
            }

            EditableImage.AddDirtyRect(new Int32Rect(0, 0, EditableImage.PixelWidth, EditableImage.PixelHeight));
            EditableImage.Unlock();
        }

        // Safely set a pixel color with boundary checks and thickness
        private void SetPixelSafe(WriteableBitmap bmp, int x, int y, Color color, int thickness = 1)
        {
            int half = thickness / 2;

            for (int dx = -half; dx <= half; dx++)
            {
                for (int dy = -half; dy <= half; dy++)
                {
                    int px = x + dx;
                    int py = y + dy;

                    if (px < 0 || px >= bmp.PixelWidth || py < 0 || py >= bmp.PixelHeight) continue;

                    var colorData = new byte[] { color.B, color.G, color.R, color.A };
                    Int32Rect rect = new Int32Rect(px, py, 1, 1);
                    bmp.WritePixels(rect, colorData, 4, 0);
                }
            }
        }

        // Use the Undo functionality from ImageHistory
        [RelayCommand]
        private void Undo()
        {
            if (EditableImage == null || !imageHistory.CanUndo) return;

            var previousImage = imageHistory.Undo(EditableImage);
            EditableImage = previousImage;
            OnImageChanged?.Invoke(EditableImage);
        }

        // Properties to indicate if undo/redo is possible
        public bool CanUndo => imageHistory.CanUndo;
        
        public void ApplyEdit(WriteableBitmap newImage)
        {
            imageHistory.Push(EditableImage);
            EditableImage = newImage;
            OnImageChanged?.Invoke(EditableImage); 
        }
        public void InitializeHistory()
        {
            if (EditableImage != null)
                imageHistory.Push(EditableImage);
        }

        // HSL Color properties for the brush
        [ObservableProperty]
        private double brushHue = 0.0; // 0-360

        [ObservableProperty]
        private double brushSaturation = 1.0; // 0–1

        [ObservableProperty]
        private double brushLightness = 0.5; // 0–1

        // Property to get the current brush color in RGB
        public Color PreviewColor => HslToRgb(BrushHue, BrushSaturation, BrushLightness);

        // Update the PreviewBrush when HSL values change
        partial void OnBrushHueChanged(double value)
        {
            OnPropertyChanged(nameof(PreviewBrush));
        }
        partial void OnBrushSaturationChanged(double value)
        {
            OnPropertyChanged(nameof(PreviewBrush));
        }
        partial void OnBrushLightnessChanged(double value)
        {
            OnPropertyChanged(nameof(PreviewBrush));
        }

        // Convert HSL to RGB color
        private Color HslToRgb(double h, double s, double l)
        {
            h /= 360.0;
            double r = l, g = l, b = l;

            if (s != 0)
            {
                double q = l < 0.5 ? l * (1 + s) : l + s - l * s;
                double p = 2 * l - q;
                r = HueToRgb(p, q, h + 1.0 / 3);
                g = HueToRgb(p, q, h);
                b = HueToRgb(p, q, h - 1.0 / 3);
            }

            return Color.FromRgb((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
        }

        // Helper function for HSL to RGB conversion
        private double HueToRgb(double p, double q, double t)
        {
            if (t < 0) t += 1;
            if (t > 1) t -= 1;
            if (t < 1.0 / 6) return p + (q - p) * 6 * t;
            if (t < 1.0 / 2) return q;
            if (t < 2.0 / 3) return p + (q - p) * (2.0 / 3 - t) * 6;
            return p;
        }

        // Property to get a SolidColorBrush for previewing the brush color
        public SolidColorBrush PreviewBrush => new SolidColorBrush(HslToRgb(BrushHue, BrushSaturation, BrushLightness));

        // BrushSize stays within 1 to 8
        partial void OnBrushSizeChanged(int value)
        {
            if (value < 1) BrushSize = 1;
            else if (value > 8) BrushSize = 8;
        }

    }
}