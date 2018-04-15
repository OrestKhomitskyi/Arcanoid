using Arcanoid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;

namespace Arcanoid
{
    public class BrickLoader
    {
        public static List<Brick> load(int stage)
        {
            List<Brick> bricks = new List<Brick>();
            try
            {
                XDocument document = XDocument.Load("Bricks.xml");

                document.Root.Descendants("stage")
                    .Where(x => x.Attribute("id").Value == stage.ToString())
                    .Elements("brick")
                    .ToList().ForEach(x => bricks.Add(new Brick()
                    {
                        Type = x.Element("type").Value,
                        Color = getColor(x.Element("color").Value),
                        Life = Convert.ToInt16(x.Element("life").Value)
                    }));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return bricks;
        }
        public static SolidColorBrush getColor(string color)
        {
            BrushConverter conv = new BrushConverter();
            SolidColorBrush brush = conv.ConvertFrom(color) as SolidColorBrush;
            return brush;
        }
    }
}
