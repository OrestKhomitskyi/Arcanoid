using Arcanoid.Views;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Media;
using Arcanoid.Music;

namespace Arcanoid
{
    public class Game
    {
        private static Game instance;
        private static object syncRoot = new Object();
        private GameMode GameMode;
        private SaveGameStateOriginator Originator;
        //private MusicPlayer MusicPlayer = new MusicPlayer();


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
            Originator = new SaveGameStateOriginator();
            BuildWindowActions();
            //EnableMusic();
        }

        public async void EnableMusic()
        {
            //await MusicPlayer.Play();
        }

        public void BuildWindowActions()
        {
            window.Close += () => Closed();
            window.OpenMenu += OpenMenu;
            window.SaveGame += Window_SaveGame;

            MenuWindow.ContinueGame += MenuWindow_ContinueGame;
            MenuWindow.Close += () =>
            {
                Closed();
            };
            MenuWindow.OpenGame += OpenGame;
            MenuWindow.ToggleMusic += MenuWindow_ToggleMusic;
        }

        private async void MenuWindow_ToggleMusic(bool? obj)
        {
            //if (obj == false)
            //    await MusicPlayer.Play();
            //else MusicPlayer.Dispose();
        }

        private void MenuWindow_ContinueGame()
        {
            MenuWindow.Hide();
            //window=new GameWindow((GameSystem)Originator.GetMemento());
        }

        private void Window_SaveGame(GameSystemDataState obj)
        {
            if(obj!=null)
                Originator.SetMemento(obj);
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

        public void StartUp()
        {
            //MenuWindow.Show();
            window.Show();
        }
        
    }
}
