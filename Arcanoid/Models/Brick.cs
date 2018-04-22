using System;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Arcanoid.Models
{
    [Serializable]
    public class Brick
    {
        public string Id { get; set; }
        public string HexColor { get; set; }
        public int Life { get; set; }
        public string Type { get; set; }
        public double Top { get; set; }
        public double Left { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }
}
