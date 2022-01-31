using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Windows.Graphics.Imaging;
using Windows.Media.FaceAnalysis;
using Windows.Media.Ocr;
using Windows.Media.SpeechSynthesis;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace OCR_TTS_FaceDetection
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private BitmapDecoder decoder;
        private SoftwareBitmap bitmap;
        private WriteableBitmap imgSource;
        private readonly uint sourceImageHeightLimit = 1280;
        private readonly SolidColorBrush fillBrush = new SolidColorBrush(Windows.UI.Colors.Transparent);
        private readonly double lineThickness = 2.0;
        private readonly SolidColorBrush lineBrush = new SolidColorBrush(Windows.UI.Colors.Yellow);
        private List<WordOverlay> wordBoxes = new List<WordOverlay>();
        private string speakmessage;
        private string imageFound;
        private string message;
        private SpeechSynthesizer synthesizer;

        public MainPage()
        {
            this.InitializeComponent();
            synthesizer = new SpeechSynthesizer();
        }

        private async void buttonLoadImage_Click(object sender, RoutedEventArgs e)
        {
            media.Stop();           // When inputing a new image, stop speech
            ClearVisualization();   // When inputing a new image, removes previous image. Or changes background back to white
            //speakmessage = "";
            try
            {
                FileOpenPicker photoPicker = new FileOpenPicker();
                photoPicker.ViewMode = PickerViewMode.Thumbnail;
                photoPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                photoPicker.FileTypeFilter.Add(".jpg");
                photoPicker.FileTypeFilter.Add(".jpeg");
                photoPicker.FileTypeFilter.Add(".png");
                photoPicker.FileTypeFilter.Add(".bmp");

                StorageFile photoFile = await photoPicker.PickSingleFileAsync();
                if (photoFile == null)
                {
                    return;
                }

                this.textBlockResults.Text = "Opening...";
                await LoadFileImage(photoFile);
                this.textBlockResults.Text = "Image is loaded";
            }
            catch (Exception ex)
            {
                this.textBlockResults.Text = ex.ToString();
            }

        }

        private async void buttonDetectTextAndFace_Click(object sender, RoutedEventArgs e)
        {
            await DetectWords(bitmap);
            //Speak(speakmessage);
            //Speak(imageFound);

            // detect faces
            IList<DetectedFace> faces = null;
            faces = await DetectFaces(bitmap);
            SetupVisualization(imgSource, faces);
        }

        private async Task LoadFileImage(StorageFile file)
        {
            using (var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
            {
                decoder = await BitmapDecoder.CreateAsync(stream);
                BitmapTransform transform = ComputeScalingTransformForSourceImage(decoder);
                bitmap = await decoder.GetSoftwareBitmapAsync(decoder.BitmapPixelFormat, BitmapAlphaMode.Ignore, transform, ExifOrientationMode.IgnoreExifOrientation, ColorManagementMode.DoNotColorManage);
                imgSource = new WriteableBitmap(bitmap.PixelWidth, bitmap.PixelHeight);
                bitmap.CopyToBuffer(imgSource.PixelBuffer);
                ImageBrush brush = new ImageBrush();
                brush.ImageSource = imgSource;
                brush.Stretch = Stretch.Fill;
                PhotoCanvas.Width = Window.Current.Bounds.Width;
                //update photo canvas height following the scale of bitmap width and height
                PhotoCanvas.Height = bitmap.PixelHeight * PhotoCanvas.Width / bitmap.PixelWidth;
                this.PhotoCanvas.Background = brush;
                buttonDetectTextAndFace.IsEnabled = true;
            }
        }

        private BitmapTransform ComputeScalingTransformForSourceImage(BitmapDecoder sourceDecoder)
        {
            BitmapTransform transform = new BitmapTransform();

            if (sourceDecoder.PixelHeight > this.sourceImageHeightLimit)
            {
                float scalingFactor = (float)this.sourceImageHeightLimit / (float)sourceDecoder.PixelHeight;

                transform.ScaledWidth = (uint)Math.Floor(sourceDecoder.PixelWidth * scalingFactor);
                transform.ScaledHeight = (uint)Math.Floor(sourceDecoder.PixelHeight * scalingFactor);
            }
            return transform;
        }

        private async Task DetectWords(SoftwareBitmap bitmap)
        {
            // List<WordOverlay> words = new List<WordOverlay>();
            // Check if OcrEngine supports image resoulution.
            if (bitmap.PixelWidth > OcrEngine.MaxImageDimension || bitmap.PixelHeight > OcrEngine.MaxImageDimension)
            {

                String message = String.Format("Bitmap dimensions ({0}x{1}) are too big for OCR.", bitmap.PixelWidth, bitmap.PixelHeight) +
                    "Max image dimension is " + OcrEngine.MaxImageDimension + ".";
                textBlockResults.Text += Environment.NewLine + message;
                return;
            }

            OcrEngine ocrEngine = null;
            ocrEngine = OcrEngine.TryCreateFromLanguage(new Windows.Globalization.Language("en"));
            if (ocrEngine != null)
            {
                // Recognize text from image.
                var ocrResult = await ocrEngine.RecognizeAsync(bitmap);

                if (ocrResult.TextAngle != null)
                {
                    // If text is detected under some angle in this sample 
                    // scenario we want to overlay word boxes
                    // over original image, so we rotate overlay boxes.
                    TextOverlay.RenderTransform = new RotateTransform
                    {
                        Angle = (double)ocrResult.TextAngle,
                        CenterX = PhotoCanvas.ActualWidth / 2,
                        CenterY = PhotoCanvas.ActualHeight / 2
                    };
                }
                if (ocrResult.Text.Count() > 0)
                {
                    speakmessage = "The recognized words are: " + ocrResult.Text;
                }
                else
                {
                    speakmessage = "No words found.";
                }

                // Display recognized text.
                textBlockResults.Text = speakmessage;

                // Create overlay boxes over recognized words.
                foreach (var line in ocrResult.Lines)
                {
                    Rect lineRect = Rect.Empty;
                    foreach (var word in line.Words)
                    {
                        lineRect.Union(word.BoundingRect);
                    }

                    // Determine if line is horizontal or vertical.
                    // Vertical lines are supported only in Chinese Traditional and Japanese languages.
                    bool isVerticalLine = lineRect.Height > lineRect.Width;

                    foreach (var word in line.Words)
                    {
                        WordOverlay wordBoxOverlay = new WordOverlay(word);

                        // Keep references to word boxes.
                        wordBoxes.Add(wordBoxOverlay);

                        // Define overlay style.
                        var overlay = new Border()
                        {
                            Style = isVerticalLine ?
                                        (Style)this.Resources["HighlightedWordBoxVerticalLine"] :
                                        (Style)this.Resources["HighlightedWordBoxHorizontalLine"]
                        };

                        // Bind word boxes to UI.
                        overlay.SetBinding(Border.MarginProperty, wordBoxOverlay.CreateWordPositionBinding());
                        overlay.SetBinding(Border.WidthProperty, wordBoxOverlay.CreateWordWidthBinding());
                        overlay.SetBinding(Border.HeightProperty, wordBoxOverlay.CreateWordHeightBinding());

                        // Put the filled textblock in the results grid.
                        TextOverlay.Children.Add(overlay);
                    }
                }

                // Rescale word boxes to match current UI size.
                UpdateWordBoxTransform();
            }
        }

        private async void Speak(string text)
        {
            if (media.CurrentState.Equals(MediaElementState.Playing))
            {
                media.Stop();
            }
            else
            {
                if (!String.IsNullOrEmpty(text))
                {
                    try
                    {
                        SpeechSynthesisStream synthesisStream = await synthesizer.SynthesizeTextToStreamAsync(text);

                        // Set the source and start playing the synthesized 
                        // audio stream.
                        media.AutoPlay = true;
                        media.SetSource(synthesisStream, synthesisStream.ContentType);
                        media.Play();
                    }
                    catch (System.IO.FileNotFoundException)
                    {
                        // If media player components are unavailable, 
                        // (eg, using a N SKU of windows), we won't be able 
                        // to start media playback. Handle this gracefully 
                        var messageDialog = new Windows.UI.Popups.MessageDialog("Media player components unavailable");
                        await messageDialog.ShowAsync();
                    }
                    catch (Exception)
                    {
                        // If the text is unable to be synthesized, throw 
                        // an error message to the user.
                        media.AutoPlay = false;
                        var messageDialog = new Windows.UI.Popups.MessageDialog("Unable to synthesize text");
                        await messageDialog.ShowAsync();
                    }
                }
            }
        }

        private void UpdateWordBoxTransform()
        {
            // Used for text overlay.
            // Prepare scale transform for words since image is not displayed in original size.
            ScaleTransform scaleTrasform = new ScaleTransform
            {
                CenterX = 0,
                CenterY = 0,
                ScaleX = PhotoCanvas.ActualWidth / bitmap.PixelWidth,
                ScaleY = PhotoCanvas.ActualHeight / bitmap.PixelHeight
            };

            foreach (var item in wordBoxes)
            {
                item.Transform(scaleTrasform);
            }
        }

        private void ClearVisualization()
        {
            this.PhotoCanvas.Background = null;
            this.PhotoCanvas.Children.Clear();
            TextOverlay.RenderTransform = null;
            TextOverlay.Children.Clear();
            wordBoxes.Clear();
            textBlockResults.Text = "";
        }

        private async Task<IList<DetectedFace>> DetectFaces(SoftwareBitmap originalBitmap)
        {
            IList<DetectedFace> faces = null;
            SoftwareBitmap detectorInput = null;

            // We need to convert the image into a format that's compatible 
            // with FaceDetector. Gray8 should be a good type.
            const BitmapPixelFormat InputPixelFormat = BitmapPixelFormat.Gray8;

            if (FaceDetector.IsBitmapPixelFormatSupported(InputPixelFormat))
            {
                using (detectorInput = SoftwareBitmap.Convert(originalBitmap, InputPixelFormat))
                {
                    // Initialize our FaceDetector and execute it against our 
                    // input image.
                    FaceDetector detector = await FaceDetector.CreateAsync();
                    faces = await detector.DetectFacesAsync(detectorInput);
                }
            }
            else
            {
                textBlockResults.Text = "PixelFormat '" + InputPixelFormat.ToString() + "' is not supported by FaceDetector";
            }
            return faces;
        }

        private void SetupVisualization(WriteableBitmap displaySource, IList<DetectedFace> foundFaces)
        {
            ImageBrush brush = new ImageBrush();
            brush.ImageSource = displaySource;
            brush.Stretch = Stretch.Fill;
            PhotoCanvas.Background = brush;

            if (foundFaces != null)
            {
                double widthScale = displaySource.PixelWidth / this.PhotoCanvas.ActualWidth;
                double heightScale = displaySource.PixelHeight / this.PhotoCanvas.ActualHeight;

                foreach (DetectedFace face in foundFaces)
                {
                    // Create a rectangle element for displaying the face box 
                    // but since we're using a Canvas we must scale the 
                    // rectangles according to the image’s actual size. The 
                    // original FaceBox values are saved in the Rectangle's Tag 
                    // field so we can update the boxes when Canvas is resized. 
                    Rectangle box = new Rectangle();
                    box.Tag = face.FaceBox;
                    box.Width = (uint)(face.FaceBox.Width / widthScale);
                    box.Height = (uint)(face.FaceBox.Height / heightScale);
                    box.Fill = this.fillBrush;
                    box.Stroke = this.lineBrush;
                    box.StrokeThickness = this.lineThickness;
                    box.Margin = new Thickness((uint)(face.FaceBox.X / widthScale), (uint)(face.FaceBox.Y / heightScale), 0, 0);

                    this.PhotoCanvas.Children.Add(box);
                }
            }
            if (foundFaces.Count == 1)
            {
                imageFound = "Found a Human Face in the Image";
            }
            else if (foundFaces.Count > 1)
            {
                imageFound = "Found " + foundFaces.Count + " Human Face in the Image";
            }
            else
            {
                imageFound = "Didn't find any human faces in the image";
            }
            // Display recognized text.
            textBlockResults.Text += Environment.NewLine + imageFound;
            message = speakmessage + imageFound;
            Speak(message);
        }
        
    }
}
