## PackEditor

### Program Goals
- Loading images for editing
- Modification (drawing)
- Saving images separately (.png) or as a test/main version (.zip)
- Previewing colors from specific areas of `foliagecolor.png` and `grasscolor.png` in the menu

---

### 1. First Part – Image Editor
- Loading files (.png) for further editing – using the `BitmapImage` class
- Storing the image during editing – using the `WriteableBitmap` class
- Graphic layer for editing – using the `Canvas` component that edits the `WriteableBitmap`
- History of image edits for possible undo – using `Stack<WriteableBitmap>` storing each edit

### 2. Saving
- `SaveFileDialog` component for saving a regular `.png` file
- `System.IO` for creating folders and `System.IO.Compression` for compressing the final test package into a `.zip` file

---

### 3. Second Part – Biome Color Preview (available only for `foliagecolor.png` and `grasscolor.png`)
- A tab to display the biome previews – `MenuItem`
- `Dictionary<string, Int32Rect>` storing coordinates of areas (X, Y, Width, Height)
- `CroppedBitmap` class cropping the selected area
- `Image` displaying the cropped image
- `Grid` placing a semi-transparent overlay of the block texture affected by color changes above the cropped color 
