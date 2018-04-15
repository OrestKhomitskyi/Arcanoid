using Arcanoid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Arcanoid
{
    public class GameSystem
    {
        public event Action StopAction;

        private Canvas canvas;
        private int CurrentStage;
        private List<Brick> bricks = new List<Brick>();
        int skipTick = 5;
        private double RedGameBallLeft = 0;
        private double RedGameBallTop = 0;
        private int RedBallCurrentDirection = 1;
        private float motionRatio = 0;
        private bool _isClockWise = true; // true = clockwise , false = anti-clockwise
        private static DispatcherTimer movingTimer;

        private Rectangle lastCollapsed = default(Rectangle);

        public event Action<int> IncreaseScore;



        public GameSystem(ref Canvas canvas)
        {
            this.canvas = canvas;
            StopAction += () => { movingTimer.Stop(); };
        }



        public void Start()
        {
            SetInitialState();
            if (movingTimer != null)
            {
                movingTimer.Stop();
            }

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
            RedBallCurrentDirection = getDirection(
                (Ellipse)canvas.FindName("GameBallRed"),
                RedBallCurrentDirection,
                (Rectangle)canvas.FindName("rectangleRed")
            );

            moveGameBall(RedBallCurrentDirection, ref RedGameBallTop, ref RedGameBallLeft,
                (Ellipse)canvas.FindName("GameBallRed"));
            checkBreakCollapse(ref RedBallCurrentDirection, (Ellipse)canvas.FindName("GameBallRed"));


        }
        private void SetInitialState()
        {
            clearCanvas();
            bricks = new List<Brick>();
            motionRatio = 4;
            CurrentStage = 1;
            bricks = BrickLoader.load(CurrentStage);
            BrickDrawer.DrawGrid(bricks, ref canvas);
            RedBallCurrentDirection = 3;
            RedGameBallLeft = 40;
            RedGameBallTop = 200;
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
                        _isClockWise = true;
                        return 1;
                    }
                    else
                    {
                        _isClockWise = false;
                        return 2;
                    }
                }
                else if (_ballLeft <= 0)
                {
                    if (_currentDirection == 2)
                    {
                        _isClockWise = true;
                        return 3;
                    }
                    else
                    {
                        _isClockWise = false;
                        return 0;
                    }
                }
                else if (_ballTop <= 0)
                {
                    if (_currentDirection == 3)
                    {
                        _isClockWise = true;
                        return 0;
                    }
                    else
                    {
                        _isClockWise = false;
                        return 1;
                    }
                }
                else if (_ballTop >= canvas.Height - _gameBall.Width)
                {
                    if (_currentDirection == 1)
                    {
                        _isClockWise = true;
                        return 2;
                    }
                    else
                    {
                        _isClockWise = false;
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
                MessageBox.Show("Game Over!!!");
                movingTimer.Stop();
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
            var conflictedBrick = bricks.Where(s => ballCoordinate.Any(p =>
                    s.Type != "skip" &&
                    p.X >= Canvas.GetLeft(s.Rectangle) &&
                    p.X <= Canvas.GetLeft(s.Rectangle) + s.Rectangle.Width &&
                    p.Y <= Canvas.GetTop(s.Rectangle) + s.Rectangle.Height &&
                    p.Y >= Canvas.GetTop(s.Rectangle)));


            if (conflictedBrick.Count() > 0)
            {
                Rectangle cBrick = conflictedBrick.FirstOrDefault().Rectangle;

                if (lastCollapsed == cBrick && skipTick > 0)
                {
                    skipTick--;
                    return false;
                }
                else
                {
                    skipTick = 5;
                    lastCollapsed = cBrick;
                }
                var nearCoordinate1 = ballCoordinate.Where(p =>
                                        p.X >= Canvas.GetLeft(cBrick) &&
                                        p.X <= Canvas.GetLeft(cBrick) + cBrick.Width &&
                                        p.Y <= Canvas.GetTop(cBrick) + cBrick.Height &&
                                        p.Y >= Canvas.GetTop(cBrick));

                var xmin = nearCoordinate1.Min(s => s.X);
                var ymin = nearCoordinate1.Min(s => s.Y);

                var nearCoordinate = xmin < ymin ? nearCoordinate1.OrderByDescending(s => s.X).First() : nearCoordinate1.OrderByDescending(s => s.Y).First();

                if (cBrick == default(Rectangle))
                    MessageBox.Show("Somethign issue");

                if (Canvas.GetTop(cBrick) <= _ballBottom &&        // top
                     Canvas.GetTop(cBrick) + cBrick.Height > _ballBottom &&
                     Canvas.GetLeft(cBrick) <= _ballCenterY &&
                     Canvas.GetLeft(cBrick) + cBrick.Width >= _ballCenterY)
                {
                    _isClockWise = currentDirection == 0 ? false : true;
                }
                else if (Canvas.GetTop(cBrick) + cBrick.Height >= _ballCenterX &&             // left
                     Canvas.GetTop(cBrick) < _ballCenterX &&
                     Canvas.GetLeft(cBrick) <= _ballRight &&
                     Canvas.GetLeft(cBrick) + cBrick.Width > _ballRight)
                {
                    _isClockWise = currentDirection == 3 ? false : true;
                }
                else if (Canvas.GetTop(cBrick) + cBrick.Height >= _ballTop &&                 // bottom
                                                        Canvas.GetTop(cBrick) < _ballTop &&
                                                        Canvas.GetLeft(cBrick) <= _ballCenterY &&
                                                        Canvas.GetLeft(cBrick) + cBrick.Width >= _ballCenterY)
                {
                    _isClockWise = currentDirection == 3 ? true : false;
                }
                else if (Canvas.GetTop(cBrick) + cBrick.Height >= _ballCenterX &&             // right
                                                        Canvas.GetTop(cBrick) < _ballCenterX &&
                                                        Canvas.GetLeft(cBrick) < _ballLeft &&
                                                        Canvas.GetLeft(cBrick) + cBrick.Width >= _ballLeft)
                {
                    _isClockWise = currentDirection == 2 ? true : false;
                }

                changeBallDirection(ref currentDirection, _gameBall, cBrick, nearCoordinate);
                int index = bricks.Select(x => x.Rectangle).ToList().IndexOf(cBrick);

                if (index < 0)
                    MessageBox.Show("Incorrect brick");

                //Lifes

                if (bricks[index].Life > 1)
                {
                    bricks[index].Color = BrickLoader.getColor("#ffffff");
                    bricks[index].Life--;
                    bricks[index].Rectangle.Fill = bricks[index].Color;
                }
                else
                {
                    IncreaseScore(10);
                    conflictedBrick.FirstOrDefault().Rectangle.Visibility = System.Windows.Visibility.Collapsed;
                    Brick toDelete = bricks.Where(x => x.Rectangle == cBrick).First();
                    bricks.Remove(toDelete);
                    //brickInfo[index] = "0";
                }

                //brickInfo = brickInfo.Where(s => s.ToString() != "0").ToArray();

                if (bricks.Where(s => s.Type != "skip" && s.Rectangle.Visibility == System.Windows.Visibility.Visible).Count() == 0)
                {
                    MessageBox.Show($"You have completed Stage : {CurrentStage++}!!! ");

                    SetInitialState();
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
            foreach (Brick brick in bricks)
            {
                if (brick.Type != "skip")
                {
                    brick.Rectangle.Visibility = System.Windows.Visibility.Collapsed;
                    canvas.Children.Remove(brick.Rectangle);
                }
            }
            canvas.UpdateLayout();
            bricks.Clear();
        }
        private void moveGameBall(int direction, ref double gameBallTop, ref double gameBallLeft, Ellipse gameBall)
        {
            switch (direction)
            {
                case 0:
                    gameBallTop += motionRatio;
                    gameBallLeft += motionRatio; break;
                case 1:
                    gameBallTop += motionRatio;
                    gameBallLeft -= motionRatio;
                    break;
                case 2:
                    gameBallTop -= motionRatio;
                    gameBallLeft -= motionRatio;
                    break;
                case 3:
                    gameBallTop -= motionRatio;
                    gameBallLeft += motionRatio;
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
