using System;
using System.Windows;

namespace Arcanoid.Views
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        private MultiplayerPage MultiplayerPage = new MultiplayerPage();
        private MenuPage MenuPage = new MenuPage();

        public event Action Close;
        public MenuWindow()
        {
            InitializeComponent();
            Frame.Navigate(MenuPage);
            MenuPage.OpenMultiplayerGame += () => { Frame.Navigate(MultiplayerPage); };
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Frame.Source = new Uri("MenuPage.xaml");
        }



        public void MenuWindow_OnClosed(object Sender, EventArgs E)
        {
            Close();
        }

        public event Action OpenGame;

        private void ButtonBase_OnClick(object Sender, RoutedEventArgs E)
        {
            OpenGame();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
