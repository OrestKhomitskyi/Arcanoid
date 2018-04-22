using Arcanoid.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Serialization;

namespace Arcanoid
{
    public class GameSystem
    {
        public event Action StopAction;
        public event Action OnGameOver;
        public event Action<int> IncreaseScore;

        private Canvas canvas;
        private DispatcherTimer movingTimer;
        private Brick lastCollapsed = default(Brick);
        public GameSystemDataState State { get; set; }

        public GameSystem(ref Canvas canvas)
        {
            this.canvas = canvas;
            StopAction += () => { movingTimer.Stop(); };
            OnGameOver += () => { movingTimer.Stop(); };
            State = new GameSystemDataState();
        }


        //public void ContinuePrevious(GameSystemDataState lastState)
        //{
        //    State = lastState;
        //    SetInitialState();
        //    movingTimer?.Stop();
        //    movingTimer = new DispatcherTimer();
        //    movingTimer.Interval = TimeSpan.FromMilliseconds(1);
        //    movingTimer.Tick += MovingTimer_Tick;
        //    movingTimer.Start();
        //}

        public void Start()
        {
            SetInitialState();
            movingTimer?.Stop();
            movingTimer = new DispatcherTimer();
            movingTimer.Interval = TimeSpan.FromMilliseconds(1);
            movingTimer.Tick += MovingTimer_Tick;
            movingTimer.Start();
        }
        public void Pause()
        {
            movingTimer.Stop();
        }
        public void Resume()
        {
            movingTimer.Start();
        }

        private void MovingTimer_Tick(object sender, EventArgs e)
        {


            //State.RedBallCurrentDirection = getDirection(
            //    (Ellipse)canvas.FindName("GameBallRed"),
            //    State.RedBallCurrentDirection,
            //    (Rectangle)canvas.FindName("rectangleRed")
            //);

            Debug.WriteLine(Guid.NewGuid().ToString());

            moveGameBall(State.RedBallCurrentDirection, ref State.RedGameBallTop, ref State.RedGameBallLeft,
                (Ellipse)canvas.FindName("GameBallRed"));


            //checkBreakCollapse(ref State.RedBallCurrentDirection, (Ellipse)canvas.FindName("GameBallRed"));
        }
        private void SetInitialState()
        {
            clearCanvas();
            //State.SetInitialState();

            BrickDrawer.DrawGrid(State.bricks, ref canvas);

            Rectangle rectangleRed = (Rectangle) canvas.FindName("rectangleRed");

            double left = Canvas.GetLeft(rectangleRed)+(rectangleRed.ActualWidth/2);
            double top = canvas.ActualHeight - 60;
            State.RedGameBallLeft = left;
            State.RedGameBallTop = top;
            

            Canvas.SetLeft((Ellipse)canvas.FindName("GameBallRed"), State.RedGameBallLeft);
            Canvas.SetTop((Ellipse)canvas.FindName("GameBallRed"), State.RedGameBallTop);
        }


        private int getDirection(Ellipse _gameBall, int _currentDirection, Rectangle _bottomBrick)
        {
            double _ballLeft = Canvas.GetLeft(_gameBall);
            double _ballTop = Canvas.GetTop(_gameBall);

            if (!checkBottomBreakCollapse(ref _currentDirection, _gameBall, _bottomBrick))
            {
                if (_ballLeft >= canvas.ActualWidth - _gameBall.Width)
                {
                    if (_currentDirection == 0)
                    {
                        State._isClockWise = true;
                        return 1;
                    }
                    else
                    {
                        State._isClockWise = false;
                        return 2;
                    }
                }
                else if (_ballLeft <= 0)
                {
                    if (_currentDirection == 2)
                    {
                        State._isClockWise = true;
                        return 3;
                    }
                    else
                    {
                        State._isClockWise = false;
                        return 0;
                    }
                }
                else if (_ballTop <= 0)
                {
                    if (_currentDirection == 3)
                    {
                        State._isClockWise = true;
                        return 0;
                    }
                    else
                    {
                        State._isClockWise = false;
                        return 1;
                    }
                }
                else if (_ballTop >= canvas.Height - _gameBall.Width)
                {
                    if (_currentDirection == 1)
                    {
                        State._isClockWise = true;
                        return 2;
                    }
                    else
                    {
                        State._isClockWise = false;
                        return 3;
                    }
                }
            }

            return _currentDirection;
        }
        public void key_down(KeyEventArgs e)
        {
            Rectangle rectangle = (Rectangle)canvas.FindName("rectangleRed");

            double left = Canvas.GetLeft(rectangle);


            switch (e.Key)
            {
                case Key.Right:
                    left += 15;
                    if (left + rectangle.ActualWidth <= canvas.ActualWidth)
                        Canvas.SetLeft(rectangle, left);
                    break;
                case Key.Left:
                    left -= 15;
                    if (left >= 0)
                        Canvas.SetLeft(rectangle, left);
                    break;
            }

        }
        #region Logic



        #region BREAKCHECKERS
        //Check for carriage and walls
        private bool checkBottomBreakCollapse(ref int currentDirection, Ellipse _gameBall, Rectangle _bottomBreak)
        {
            double _RTBrick = canvas.ActualHeight - Canvas.GetBottom(_bottomBreak) - _bottomBreak.Height;
            double _RLBrick = Canvas.GetLeft(_bottomBreak);
            double _RRBrick = Canvas.GetLeft(_bottomBreak) + _bottomBreak.Width;
            double _ballBottom = Canvas.GetTop(_gameBall) + _gameBall.Width;
            double _ballLeft = Canvas.GetLeft(_gameBall) + (_gameBall.Width / 2);

            if ((_ballBottom >= _RTBrick && _ballLeft > _RLBrick && _ballLeft < _RRBrick))
            {
                if (currentDirection == 0)
                {
                    currentDirection = 3;
                }
                else if (currentDirection == 1)
                {
                    currentDirection = 2;
                }

                return true;
            }
            else if ((_ballBottom >= _RTBrick && (_ballLeft < _RLBrick || _ballLeft > _RRBrick)))
            {
                OnGameOver();
                return true;
            }

            return false;
        }
        //Check for bricks
        private bool checkBreakCollapse(ref int currentDirection, Ellipse _gameBall)
        {
            double _ballBottom = Canvas.GetTop(_gameBall) + _gameBall.Width;
            double _ballLeft = Canvas.GetLeft(_gameBall) - (_gameBall.Width / 2);
            double _ballRight = Canvas.GetLeft(_gameBall) + _gameBall.Width;
            double _ballTop = Canvas.GetTop(_gameBall);
            double _ballCenterX = Canvas.GetTop(_gameBall) + (_gameBall.Width / 2);
            double _ballCenterY = Canvas.GetLeft(_gameBall) + (_gameBall.Width / 2);

            var ballCoordinate = getCircularPoints(_gameBall);
            var conflictedBrick = State.bricks.Where(s => ballCoordinate.Any(p =>
                    s.Type != "skip" &&
                    p.X >= s.Left &&
                    p.X <= s.Left + s.Width &&
                    p.Y <= s.Top + s.Height &&
                    p.Y >= s.Top));


            if (conflictedBrick.Count() > 0)
            {
                Brick cBrick = conflictedBrick.FirstOrDefault();
                Rectangle cRectangle = canvas.FindRectangleByName(cBrick.Id) as Rectangle;

                if (lastCollapsed == cBrick && State.skipTick > 0)
                {
                    State.skipTick--;
                    return false;
                }
                else
                {
                    State.skipTick = 5;
                    lastCollapsed = cBrick;
                }
                var nearCoordinate1 = ballCoordinate.Where(p =>
                                        p.X >= cBrick.Left  &&
                                        p.X <= cBrick.Left + cBrick.Width &&
                                        p.Y <= cBrick.Top + cBrick.Height &&
                                        p.Y >= cBrick.Top);

                var xmin = nearCoordinate1.Min(s => s.X);
                var ymin = nearCoordinate1.Min(s => s.Y);

                var nearCoordinate = xmin < ymin ? nearCoordinate1.OrderByDescending(s => s.X).First() : nearCoordinate1.OrderByDescending(s => s.Y).First();

                if (cBrick == default(Brick))
                    MessageBox.Show("Somethign issue");

                if (cBrick.Top <= _ballBottom &&        // top
                    cBrick.Top + cBrick.Height > _ballBottom &&
                    cBrick.Left <= _ballCenterY &&
                    cBrick.Left + cBrick.Width >= _ballCenterY)
                {
                    State._isClockWise = currentDirection == 0 ? false : true;
                }
                else if (cBrick.Top + cBrick.Height >= _ballCenterX &&             // left
                         cBrick.Top < _ballCenterX &&
                         cBrick.Left <= _ballRight &&
                         cBrick.Left + cBrick.Width > _ballRight)
                {
                    State._isClockWise = currentDirection == 3 ? false : true;
                }
                else if (cBrick.Top + cBrick.Height >= _ballTop &&                 // bottom
                                                        cBrick.Top < _ballTop &&
                                                        cBrick.Left <= _ballCenterY &&
                                                        cBrick.Left + cBrick.Width >= _ballCenterY)
                {
                    State._isClockWise = currentDirection == 3 ? true : false;
                }
                else if (cBrick.Top + cBrick.Height >= _ballCenterX &&             // right
                                                        cBrick.Top < _ballCenterX &&
                                                        cBrick.Left < _ballLeft &&
                                                        cBrick.Left + cBrick.Width >= _ballLeft)
                {
                    State._isClockWise = currentDirection == 2 ? true : false;
                }

                changeBallDirection(ref currentDirection, _gameBall, cRectangle, nearCoordinate);
                //Strange code
                //int index = State.bricks.Select(x => x.Rectangle).ToList().IndexOf(cBrick);
                int index = State.bricks.IndexOf(cBrick);

                if (index < 0)
                {
                    MessageBox.Show("Incorrect brick");
                    throw new Exception("Incorrect Brick");
                }

                //Lifes
                if (cBrick.Life > 1)
                {
                    cBrick.HexColor = BrickLoader.getColor("#ffffff");
                    cBrick.Life--;
                    cRectangle.Fill = BrickLoader.GetColorBrush(cBrick.HexColor);

                    Brick b=State.bricks.Single(x => x == cBrick);
                }
                else
                {
                    IncreaseScore(10);
                    cRectangle.Visibility = System.Windows.Visibility.Collapsed;
                    //Brick toDelete = State.bricks.Where(x => x.Rectangle == cBrick).First();
                    State.bricks.Remove(cBrick);
                    //brickInfo[index] = "0";
                }
                //canvas.UpdateLayout();

                //brickInfo = brickInfo.Where(s => s.ToString() != "0").ToArray();

                if (State.bricks.Where(s => s.Type != "skip").Count() <= 0)
                {
                    MessageBox.Show($"You have completed Stage : {State._currentStage++}!!! ");
                    State._currentStage++;
                    State.bricks = BrickLoader.load(State._currentStage);
                    return true;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
        private void clearCanvas()
        {
            foreach (Brick brick in State.bricks)
            {
                if (brick.Type != "skip" && brick.Id!=null)
                {
                    Rectangle r=canvas.FindRectangleByName(brick.Id) as Rectangle;
                    r.Visibility = System.Windows.Visibility.Collapsed;
                    canvas.Children.Remove(r);
                }
            }
            canvas.UpdateLayout();
            State.bricks.Clear();
        }
        private void moveGameBall(int direction, ref double gameBallTop, ref double gameBallLeft, Ellipse gameBall)
        {

            switch (direction)
            {
                case 0:
                    gameBallTop += State.motionRatio;
                    gameBallLeft += State.motionRatio;
                    break;
                case 1:
                    gameBallTop += State.motionRatio;
                    gameBallLeft -= State.motionRatio;
                    break;
                case 2:
                    gameBallTop -= State.motionRatio;
                    gameBallLeft -= State.motionRatio;
                    break;
                case 3:
                    gameBallTop -= State.motionRatio;
                    gameBallLeft += State.motionRatio;
                    break;
                default:
                    MessageBox.Show("Ehhh Error occur!!!");
                    break;
            }
            Canvas.SetTop(gameBall, gameBallTop);
            Canvas.SetLeft(gameBall, gameBallLeft);
        }
        private List<Coordinate> getCircularPoints(Ellipse gameBall)
        {

            int distance = (int)gameBall.Width / 2;
            double originX = Canvas.GetLeft(gameBall) + distance;
            double originY = Canvas.GetTop(gameBall) - distance;

            List<Coordinate> pointLists = new List<Coordinate>();
            Coordinate point;
            for (int i = 0; i < 360; i = i + 24)
            {
                point = new Coordinate();

                point.X = (int)Math.Round(originX + distance * Math.Sin(i));
                point.Y = (int)(gameBall.Width + Math.Round(originY - distance * Math.Cos(i)));
                pointLists.Add(point);
            }

            return pointLists;
        }
        private void changeBallDirection(ref int _currentDirection, Ellipse _gameBall, Rectangle _crashBrick, Coordinate nearCoordinate)
        {

            int hitAt;
            int left = (int)(nearCoordinate.X - Canvas.GetLeft(_crashBrick));
            int right = (int)(nearCoordinate.X - (Canvas.GetLeft(_crashBrick) + _crashBrick.Width));
            int top = (int)(nearCoordinate.Y - Canvas.GetTop(_crashBrick));
            int bottom = (int)(nearCoordinate.Y - (Canvas.GetTop(_crashBrick) + _crashBrick.Height));

            int[] values = { Math.Abs(left), Math.Abs(right), Math.Abs(top), Math.Abs(bottom) };
            Array.Sort(values);

            if (values[0] == left)
                hitAt = 3;
            else if (values[0] == right)
                hitAt = 1;
            else if (values[0] == top)
                hitAt = 0;
            else
                hitAt = 2;

            switch (_currentDirection)
            {
                case 0:

                    if (hitAt == 3)
                        _currentDirection = 1;
                    else if (hitAt == 0)
                        _currentDirection = 3;
                    else if (top < left)
                        _currentDirection = 3;
                    else
                        _currentDirection = 1;

                    break;

                case 1:

                    if (hitAt == 1)
                        _currentDirection = 0;
                    else if (hitAt == 0)
                        _currentDirection = 2;
                    else if (top < right)
                        _currentDirection = 2;
                    else
                        _currentDirection = 0;
                    break;

                case 2:

                    if (hitAt == 2)
                        _currentDirection = 1;
                    else if (hitAt == 1)
                        _currentDirection = 3;
                    else if (bottom < right)
                        _currentDirection = 1;
                    else
                        _currentDirection = 3;
                    break;

                case 3:

                    if (hitAt == 2)
                        _currentDirection = 0;
                    else if (hitAt == 3)
                        _currentDirection = 2;
                    else if (bottom < right)
                        _currentDirection = 0;
                    else
                        _currentDirection = 2;
                    break;
            }
        }
        #endregion
    }
    
}
