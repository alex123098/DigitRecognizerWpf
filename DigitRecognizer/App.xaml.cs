using System.Windows;
using DigitRecognizer.Infrastructure;

namespace DigitRecognizer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow = new MainWindow { 
                DataContext = new DigitViewModel(
                    new RecognizeLibrary.Recognizer(),
                    new RecognizeLibrary.LearnService())
            };

            MainWindow.Show();
        }
    }
}
