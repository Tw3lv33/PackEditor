# PackEditor
Cel programu:
	- Wczytywanie obrazów do edycji
	- Modyfikacja (rysowanie)
	- Zapis obrazu oddzielnie (.png) lub w wersji testowej/głównej (.zip)
	- Podgląd kolorów z ustalonych obszarów foliagecolor.png i grasscolor.png w menu

1. Pierwszy Ribbon - Edytor obrazu:
	a) Wczytywanie plików (.png) do dalszej edycji 
		- Klasa BitmapImage
	b) Przechowywanie obrazu na czas edycji
		- Klasa WriteableBitmap 
	c) Warstwa graficzna do edycji 
		- Komponent Canvas edytujący WriteableBitmap
	d) Historia edycji obrazu do ewentualnego cofnięcia  
		-  Stack<WriteableBitmap> przechowujący każdą edycje
	e) Zapis 
		- Komponent SaveFileDialog do zapisu zwykłego pliku .png
		- Zestaw klas System.IO do tworzenia folderów i biblioteka System.IO.Compression do kompresji gotowej paczki testowej do pliku .zip

2. Drugi Ribbon - Podgląd obszarów dostępny tylko dla plików foliagecolor.png i grasscolor.png
	a) Lista rozwijana z listą biomów do ustalenia obszaru wyświetlenia
		- ComboBox 
		- Dictionary<string, Int32Rect> do przechowywania współrzędnych obszarów (X, Y, Width i Height)
		- Klasa CroppedBitmap wycinająca wybrany obszar
		- Kontrolka Image wyświetlająca obraz wycięty przez CroppedImage
		- Grid umieszczający na szczycie wyciętego koloru półprzeźroczystą nakładkę textury bloku który będzie pod wpływem zmiany koloru
