using System.Threading.Tasks;

namespace DigitRecognizer.Infrastructure
{
    public interface IDigitRecognizer
    {
        Task<byte> Recognize(IObservableArray<byte> image);
    }
}
