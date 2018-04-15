using Arcanoid.Views;
using System;

namespace Arcanoid
{
    public class Game
    {
        private static Game instance;
        private static object syncRoot = new Object();
        private GameMode GameMode;
        public event Action Closed;
        public GameWindow window { get; set; }
        public MenuWindow MenuWindow { get; set; }


        //Singletone thread-safety
        public static Game GetInstance()
        {
            lock (syncRoot)
            {
                if (instance == null)
                    instance = new Game();
            }

            return instance;
        }



        private Game()
        {
            window = new GameWindow();
            MenuWindow = new MenuWindow();
            BuildWindowActions();
        }

        public void BuildWindowActions()
        {
            window.Close += () => Closed();
            window.OpenMenu += OpenMenu;

            MenuWindow.Close += () =>
            {
                Closed();
            };
            MenuWindow.OpenGame += OpenGame;
        }
        public void OpenMenu()
        {
            window.Hide();
            MenuWindow.Show();
        }

        public void OpenGame()
        {
            MenuWindow.Hide();
            window.Show();
        }


        public void Show()
        {
            //MenuWindow.Show();
            window.Show();
        }
    }
}
