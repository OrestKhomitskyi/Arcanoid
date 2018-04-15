using System.Windows.Media;
using System.Windows.Shapes;

namespace Arcanoid.Models
{
    public class Brick
    {
        public SolidColorBrush Color { get; set; }
        public int Life { get; set; }
        public string Type { get; set; }
        public Rectangle Rectangle { get; set; }

    }
}
