using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Arcanoid.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private GameOverPage GameOverPage;
        private InstructionPage InstructionPage;
        private GameSystem GameSystem;
        private double originX;
        private double prevX;
        private int gameScore = 0;
        public event Action Close;
        public event Action OpenMenu;
        public event Action<GameSystemDataState> SaveGame;


        public int GameScore
        {
            get { return gameScore; }
            set
            {
                gameScore = value;
            }
        }


        public GameWindow(GameSystem loaded=null)
        {
            InitializeComponent();
            this.DataContext = this;
            if (loaded == null)
            {
                GameSystem = new GameSystem(ref game_canvas);
                GameSystem.IncreaseScore += (score) =>
                {
                    GameScore += score;
                    Score.Text = GameScore.ToString();
                };
                GameSystem.OnGameOver += () =>
                {
                    MessageBox.Show("Game Over");
                };
            }
            else
                GameSystem = loaded;
        }
        private void SetPages()
        {
            GameOverPage = new GameOverPage();
            InstructionPage = new InstructionPage();
        }
        private void start_game_Click(object sender, RoutedEventArgs e)
        {
            GameSystem.Start();
        }
        private bool IsPaused = false;

        private void move_carriage_key_down(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                if (IsPaused)
                    GameSystem.Resume();
                else
                    GameSystem.Pause();
                IsPaused = !IsPaused;
            }
            else
                GameSystem.key_down(e);
        }

        private void pause_game_Click(object sender, RoutedEventArgs e)
        {


        }

        public void go_to_menu_Click(object sender, RoutedEventArgs e)
        {
            OpenMenu();
        }






        private void RectangleRed_OnMouseMove(object Sender, MouseEventArgs E)
        {

        }

        private void RectangleRed_OnMouseLeave(object Sender, MouseEventArgs E)
        {

        }

        private void RectangleRed_OnMouseUp(object Sender, MouseButtonEventArgs E)
        {
            //isPressed = false;
        }




        private void GameWindow_OnMouseDown(object Sender, MouseButtonEventArgs E)
        {
            Point position = Mouse.GetPosition(MoveArea);
            originX = position.X;
            prevX = Canvas.GetLeft(rectangleRed);
        }

        private void GameWindow_OnMouseMove(object Sender, MouseEventArgs E)
        {
            if (E.LeftButton == MouseButtonState.Pressed)
            {
                double x = Mouse.GetPosition(MoveArea).X;
                double result = x - originX + prevX;

                if (result >= 0 && result <= game_canvas.ActualWidth - rectangleRed.ActualWidth)
                {
                    Canvas.SetLeft(rectangleRed, x - originX + prevX);
                }
            }
        }

        private void GameWindow_OnClosed(object Sender, EventArgs E)
        {
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveGame(GameSystem.State);
            OpenMenu();
        }
    }
}
