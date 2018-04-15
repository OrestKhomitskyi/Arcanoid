using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace Arcanoid.Particles
{
    public class Particle
    {
        public Point3D Position { get; set; }
        public Point3D Velocity { get; set; }
        public double Size { get; set; }

        public Ellipse Ellipse { get; set; }

        public BlurEffect Blur { get; set; }
        public Brush Brush { get; set; }
    }
}
