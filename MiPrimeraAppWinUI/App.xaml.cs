using Microsoft.UI.Xaml;

namespace MiPrimeraAppWinUI
{

    public partial class App : Application
    {

        public Window MainWindow { get; private set; }

        public App()
        {
            this.InitializeComponent();
        }

        /*protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            m_window.Activate();
        }*/

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            MainWindow = new MainWindow();
            MainWindow.Activate();
        }


        private Window? m_window;
    }
}
