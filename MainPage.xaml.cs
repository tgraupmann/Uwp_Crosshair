using System;
using System.Runtime.InteropServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Uwp_Crosshair
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private DispatcherTimer _mDispatcherTimer = null;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            _mDispatcherTimer = new DispatcherTimer();
            _mDispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            _mDispatcherTimer.Tick += DispatcherTimer_Tick;
            _mDispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, object e)
        {
            App.EnableClickThrough();
        }
    }
}
