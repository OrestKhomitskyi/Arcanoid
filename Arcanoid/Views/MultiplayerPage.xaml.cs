using Arcanoid.Multiplayer;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace Arcanoid.Views
{
    /// <summary>
    /// Interaction logic for MultiplayerPage.xaml
    /// </summary>
    public partial class MultiplayerPage : Page
    {


        public MultiplayerPage()
        {
            InitializeComponent();
        }




        private void Connect_OnClick(object Sender, RoutedEventArgs E)
        {
            TcpPlayerClient client = new TcpPlayerClient(new IPEndPoint(IPAddress.Any, 3001));
        }

        private void Host_OnClick(object Sender, RoutedEventArgs E)
        {
            TcpPlayerFinderServer finderServer = new TcpPlayerFinderServer(new IPEndPoint(IPAddress.Any, 3000));
            finderServer.ServeAsync();
        }

        private void MultiplayerPage_OnLoaded(object Sender, RoutedEventArgs E)
        {
           
        }
    }
}
