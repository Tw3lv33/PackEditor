using System.Windows;
using System.Windows.Media.Imaging;

namespace PackEditor.Models
{
    public class ImageHistory
    {
        private readonly Stack<WriteableBitmap> undoStack = new();
        private readonly Stack<WriteableBitmap> redoStack = new();

        //Push a new image state onto the undo stack and clear the redo stack
        public void Push(WriteableBitmap image)
        {
            undoStack.Push(Clone(image));
            redoStack.Clear();
        }

        //Undo the last action and return the previous image state
        public WriteableBitmap Undo(WriteableBitmap current)
        {
            if (undoStack.Count == 0) return current;
            redoStack.Push(Clone(current));
            return undoStack.Pop();
        }

        //Redo the last undone action and return the next image state
        public WriteableBitmap Redo(WriteableBitmap current)
        {
            if (redoStack.Count == 0) return current;
            undoStack.Push(Clone(current));
            return redoStack.Pop();
        }

        // Properties to check if undo/redo is possible
        public bool CanUndo => undoStack.Count > 0;
        public bool CanRedo => redoStack.Count > 0;

        // Helper method to clone a WriteableBitmap
        private WriteableBitmap Clone(WriteableBitmap source)
        {
            var clone = new WriteableBitmap(source.PixelWidth, source.PixelHeight, source.DpiX, source.DpiY, source.Format, null);
            int stride = source.BackBufferStride;
            byte[] pixels = new byte[source.PixelHeight * stride];
            source.CopyPixels(pixels, stride, 0);
            clone.WritePixels(new Int32Rect(0, 0, source.PixelWidth, source.PixelHeight), pixels, stride, 0);
            return clone;
        }
    }
}
