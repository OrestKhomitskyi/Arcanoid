using Arcanoid.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Linq;
using System.Windows.Documents;

namespace Arcanoid
{
    static class BrickDrawer
    {
        private const int BRICKHEIGHT = 25;
        private const int BRICKWIDTH = 60;


        private static void Draw(Brick brick, ref Canvas canvas)
        {
            if (brick.Type == "skip")
                return;

            Rectangle rect = new Rectangle();
            rect.Fill = BrickLoader.GetColorBrush(brick.HexColor);
            rect.Height = brick.Height;
            rect.Width = brick.Width;
            rect.RadiusX = 4;
            rect.RadiusY = 4;
            rect.StrokeThickness = 1;

            rect.Name = brick.Id;

            Canvas.SetTop(rect, brick.Top);
            Canvas.SetLeft(rect, brick.Left);
            canvas.Children.Add(rect);
        }

        public static void DrawGrid(List<Brick> bricks, ref Canvas canvas)
        {
            //Set amount of items for a line&row in canvas
            int itemsForAline = (int)Math.Floor(canvas.ActualWidth / 60);
            int amountOfLevels = (int)Math.Ceiling(bricks.Count * 1.0 / itemsForAline);
            var index = bricks.GetEnumerator();
            for (int i = 0; i < amountOfLevels; i++)
                for (int j = 0; j < itemsForAline; j++)
                    if (index.MoveNext())
                    {
                        index.Current.Height = BRICKHEIGHT;
                        index.Current.Width = BRICKWIDTH;
                        index.Current.Top = i * (BRICKHEIGHT + 5) + 5;
                        index.Current.Left = j * (BRICKWIDTH + 5) + 5;

                        index.Current.Id = UiIdentyfier.CreateUIElementId(Guid.NewGuid());
                        Draw(index.Current, ref canvas);
                    }

            int a = 2;

        }
    }

    static class CanvasExtension
    {
        public static Rectangle FindRectangleByName(this Canvas canvas, string name)
        {
            foreach (var canvasChild in canvas.Children)
            {
                if (canvasChild is Rectangle)
                {
                    Rectangle rectangle = canvasChild as Rectangle;

                    if (rectangle.Name == name)
                    {
                        return rectangle;
                    }
                }
            }

            return null;
        }
    }



}
