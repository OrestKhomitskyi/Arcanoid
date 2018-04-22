using System.Windows;

namespace Arcanoid
{

    public enum GameType { SinglePlayer, MultiPlayer };
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private Game game;

        public App()
        {
            game = Game.GetInstance();
            game.Closed += () => Application.Current.Shutdown();
            game.StartUp();
        }

        
    }
}