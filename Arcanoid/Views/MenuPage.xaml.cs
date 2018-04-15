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

        public MenuPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenMultiplayerGame();
        }
    }
}
