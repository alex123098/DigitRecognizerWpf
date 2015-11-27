using System.Windows.Input;

namespace DigitRecognizer.Infrastructure
{
    public interface IDigit
    {
        byte? Digit { get; set; }
        IObservableArray<byte> Image { get; }

        ICommand Clear { get; }
        ICommand Recognize { get; }
        ICommand Learn { get; }
    }
}
