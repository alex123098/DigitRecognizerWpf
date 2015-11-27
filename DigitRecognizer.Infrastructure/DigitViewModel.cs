using System.Windows.Input;
using DigitRecognizer.Common;
using DigitRecognizer.Common.ObservableExtensions;
using JetBrains.Annotations;

namespace DigitRecognizer.Infrastructure
{
    public class DigitViewModel : ViewModelBase, IDigit
    {
        private readonly IDigitRecognizer _recognizer;
        private readonly IDigitLearnService _learnService;
        private byte? _digit;

        public byte? Digit
        {
            get { return _digit; }
            set
            {
                _digit = value;
                OnPropertyChanged();
            }
        }

        public IObservableArray<byte> Image { get; }

        public ICommand Clear { get; }
        public ICommand Recognize { get; }
        public ICommand Learn { get; }

        public DigitViewModel(
            [NotNull] IDigitRecognizer recognizer, 
            [NotNull] IDigitLearnService learnService)
        {
            Guard.NotNull(recognizer, nameof(recognizer));
            Guard.NotNull(learnService, nameof(learnService));

            _recognizer = recognizer;
            _learnService = learnService;

            Image = new byte[28 * 28].ToObservableArray();

            Clear = new RelayCommand(Reset);
            Recognize = new RelayCommand(RecognizeImage);
            Learn = new RelayCommand(LearnDigit);
        }

        private void LearnDigit(object param)
        {
            if (!Digit.HasValue)
            {
                return;
            }
            _learnService.Learn(Image, Digit.Value);
        }

        private async void RecognizeImage(object param)
        {
            Digit = await _recognizer.Recognize(Image);
        }

        private void Reset(object obj)
        {
            for (var i = 0; i < Image.Length; i++)
            {
                Image[i] = 0;
            }
            Digit = null;
        }
    }
}