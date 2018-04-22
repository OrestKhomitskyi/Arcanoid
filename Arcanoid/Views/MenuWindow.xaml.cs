using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
        private SettingsPage SettingsPage = new SettingsPage();

        public event Action Close;
        public event Action ContinueGame;
        public event Action OpenGame;
        public event Action<bool?> ToggleMusic;



        public MenuWindow()
        {
            InitializeComponent();
            Frame.Navigate(MenuPage);
            MenuPage.OpenMultiplayerGame += () => Frame.Navigate(MultiplayerPage);
            MenuPage.OpenSinglePlayerGame += () => OpenGame();
            MenuPage.ContinueGame += () => ContinueGame();
            MenuPage.Exit += () => MenuWindow_OnClosed(null, null);
            MenuPage.OpenSettings += () => Frame.Navigate(SettingsPage);
            SettingsPage.ToggleMusic += (ch) => ToggleMusic(ch);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
        
        public void MenuWindow_OnClosed(object Sender, EventArgs E)
        {
            Close();
        }
    }
}
