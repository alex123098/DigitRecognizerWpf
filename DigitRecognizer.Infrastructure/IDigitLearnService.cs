namespace DigitRecognizer.Infrastructure
{
    public interface IDigitLearnService
    {
        void Learn(IObservableArray<byte> image, byte digit);
    }
}