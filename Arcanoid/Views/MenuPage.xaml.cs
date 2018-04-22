using System;
using System.Windows;
using System.Windows.Controls;

namespace Arcanoid.Views
{
    /// <summary>
    /// Interaction logic for MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        public event Action OpenMultiplayerGame;
        public event Action OpenSinglePlayerGame;
        public event Action ContinueGame;
        public event Action OpenSettings;

        public event Action Exit;

        public MenuPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenMultiplayerGame();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenSinglePlayerGame();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ContinueGame();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Exit();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            OpenSettings();
        }
    }
}
