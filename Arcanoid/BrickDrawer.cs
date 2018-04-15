using Arcanoid.Models;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Arcanoid
{
    static class BrickDrawer
    {
        private static void Draw(Brick brick, ref Canvas canvas, int posI, int posJ)
        {
            if (brick.Type == "skip")
                return;

            int distance = 5;
            Rectangle rect = new Rectangle();
            rect.Fill = brick.Color;
            rect.Height = 25;
            rect.Width = 60;
            //rect.Stroke = Brushes.Black;
            rect.RadiusX = 4;
            rect.RadiusY = 4;
            rect.StrokeThickness = 1;
            rect.Name = "Brick";


            double posX = posJ * (rect.Width + distance) + 5;
            double posY = posI * (rect.Height + distance) + 5;

            Canvas.SetTop(rect, posY);
            Canvas.SetLeft(rect, posX);

            brick.Rectangle = rect;
            canvas.Children.Add(rect);
        }


        public static void DrawGrid(List<Brick> bricks, ref Canvas canvas)
        {
            //ClearBricks(bricks,ref canvas);
            //Set amount of items for a line&row in canvas
            int itemsForAline = (int)Math.Floor(canvas.ActualWidth / 60);
            int amountOfLevels = (int)Math.Ceiling(bricks.Count * 1.0 / itemsForAline);
            var index = bricks.GetEnumerator();
            for (int i = 0; i < amountOfLevels; i++)
                for (int j = 0; j < itemsForAline; j++)
                    if (index.MoveNext())
                        Draw(index.Current, ref canvas, i, j);

        }
    }
}
