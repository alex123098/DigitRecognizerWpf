using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using DigitRecognizer.Common.EnumerableExtensions;
using DigitRecognizer.Infrastructure;

namespace DigitRecognizer
{
    /// <summary>
    /// Interaction logic for DigitDrawCanvas.xaml
    /// </summary>
    public partial class DigitDrawCanvas
    {
        #region ImageContainer

        public static readonly DependencyProperty ImageContainerProperty = DependencyProperty.Register(
            "ImageContainer",
            typeof (IObservableArray<byte>),
            typeof (DigitDrawCanvas),
            new PropertyMetadata(default(IObservableArray<byte>), PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var element = (DigitDrawCanvas) d;
            var oldValue = args.OldValue as IObservableArray<byte>;
            if (oldValue != null)
            {
                element.UnBindCollection(oldValue);
            }
            var newValue = args.NewValue as IObservableArray<byte>;
            if (newValue == null)
            {
                return;
            }

            var dimension = (int) Math.Sqrt(newValue.Length);
            element.PixelsCanvas.Columns = dimension;
            element.PixelsCanvas.Rows = dimension;
            element.InitPixels(element.PixelsCanvas);
            element.BindCollection(newValue);
        }

        private void UnBindCollection(INotifyCollectionChanged collection)
        {
            collection.CollectionChanged -= ImagePixelChanged;
        }

        private void BindCollection(INotifyCollectionChanged collection)
        {
            collection.CollectionChanged += ImagePixelChanged;
        }

        private void ImagePixelChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.Action != NotifyCollectionChangedAction.Replace)
            {
                return;
            }
            var newItem = args.NewItems.OfType<byte>().First();
            var index = args.NewStartingIndex;
            SetPixelColor(PixelsCanvas.Children[index] as Rectangle, newItem);
        }

        public IObservableArray<byte> ImageContainer
        {
            get { return (IObservableArray<byte>) GetValue(ImageContainerProperty); }
            set { SetValue(ImageContainerProperty, value); }
        }

        #endregion ImageContainer

        public DigitDrawCanvas()
        {
            InitializeComponent();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            SetCurrentPixel(e.GetPosition(PixelsCanvas));
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Released)
            {
                return;
            }
            SetCurrentPixel(e.GetPosition(PixelsCanvas));
        }

        private void SetCurrentPixel(Point position)
        {
            var hitTest = VisualTreeHelper.HitTest(PixelsCanvas, position);
            var rectangle = hitTest?.VisualHit as Rectangle;
            if (rectangle == null)
            {
                return;
            }
            var index = (int)rectangle.Tag;
            SetBlackPixel(rectangle);
            SetBlackPixelData(index);
        }

        private void SetBlackPixelData(int index)
        {
            ImageContainer[index] = 0xFF;
        }

        private static void SetBlackPixel(Shape rectangle)
        {
            var brushColor = rectangle.Fill as SolidColorBrush;
            if (brushColor == null || brushColor.Equals(Brushes.Black))
            {
                return;
            }
            rectangle.Fill = Brushes.Black;
        }

        private static void SetPixelColor(Shape rectangle, byte value)
        {
            if (rectangle == null)
            {
                return;
            }
            var brush = new SolidColorBrush(
                Color.FromRgb(
                    InvertByteValue(value),
                    InvertByteValue(value),
                    InvertByteValue(value)));
            brush.Freeze();
            rectangle.Fill = brush;
        }

        private void InitPixels(Panel pixelsContainer)
        {
            pixelsContainer.Children.Clear();
            ImageContainer.ForEach(
                (index, item) => {
                    pixelsContainer.Children.Add(
                        new Rectangle { Tag = index, Fill = ConvertColor(item) });
                });
        }

        private static Brush ConvertColor(byte value)
        {
            var brush = new SolidColorBrush(
                Color.FromRgb(InvertByteValue(value), InvertByteValue(value), InvertByteValue(value)));
            brush.Freeze();
            return brush;
        }

        private static byte InvertByteValue(byte value) => (byte)~value;
    }
}
