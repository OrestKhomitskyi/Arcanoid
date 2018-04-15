using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Arcanoid.Particles
{
    public class ParticleSystem
    {
        private Random random;
        private List<Particle> particles;
        private List<Particle> deadList;
        private DispatcherTimer timer;
        private Canvas Canvas;

        public ParticleSystem(ref Canvas Canvas)
        {
            this.Canvas = Canvas;
        }
        public void Activate()
        {
            random = new Random();
            particles = new List<Particle>();
            deadList = new List<Particle>();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            UpdateParticules();
        }

        double elapsed = 0.1;
        private void UpdateParticules()
        {
            // clear dead particles list
            deadList.Clear();
            // update existing particles
            foreach (Particle p in this.particles)
            {
                // kill a particle when its too high or on the sides
                if (p.Position.Y < -p.Size || p.Position.X < -p.Size || p.Position.X > Canvas.ActualWidth + p.Size)
                {
                    deadList.Add(p);
                }
                else
                {
                    // update position

                    Point3D point3D = new Point3D();
                    point3D = p.Position;

                    point3D.X += p.Velocity.X * elapsed;
                    point3D.Y += p.Velocity.Y * elapsed;
                    point3D.Z += p.Velocity.Z * elapsed;
                    p.Position = point3D;
                    TranslateTransform t = (p.Ellipse.RenderTransform as TranslateTransform);
                    t.X = point3D.X;
                    t.Y = point3D.Y;

                    // update brush/blur
                    p.Ellipse.Fill = p.Brush;
                    p.Ellipse.Effect = p.Blur;
                }
            }

            // create new particles (up to 10 or 25)
            for (int i = 0; i < 10 && this.particles.Count < 25; i++)
            {
                // attempt to recycle ellipses if they are in the deadlist
                if (deadList.Count - 1 >= i)
                {
                    SpawnParticle(deadList[i].Ellipse);
                    deadList[i].Ellipse = null;
                }
                else
                {
                    SpawnParticle(null);
                }
            }

            // Remove dead particles
            foreach (Particle p in deadList)
            {
                if (p.Ellipse != null) Canvas.Children.Remove(p.Ellipse);
                this.particles.Remove(p);
            }
        }

        private void SpawnParticle(Ellipse e)
        {
            // Randomization
            double x = RandomWithVariance(Canvas.ActualWidth / 2, Canvas.ActualWidth / 2);
            double y = Canvas.ActualHeight;
            double z = 10 * (random.NextDouble() * 100);
            double speed = RandomWithVariance(20, 15);
            double size = RandomWithVariance(75, 50);

            Particle p = new Particle();
            p.Position = new Point3D(x, y, z);
            p.Size = size;

            // Blur
            var blur = new BlurEffect();
            blur.RenderingBias = RenderingBias.Performance;
            blur.Radius = RandomWithVariance(10, 15);
            p.Blur = blur;

            // Brush (for opacity)
            var brush = (Brush)Brushes.White.Clone();
            brush.Opacity = RandomWithVariance(0.5, 0.5);
            p.Brush = brush;

            TranslateTransform t;

            if (e != null) // re-use
            {
                e.Fill = null;
                e.Width = e.Height = size;
                p.Ellipse = e;

                t = e.RenderTransform as TranslateTransform;
            }
            else
            {
                p.Ellipse = new Ellipse();
                p.Ellipse.Width = p.Ellipse.Height = size;
                Canvas.Children.Add(p.Ellipse);

                t = new TranslateTransform();
                p.Ellipse.RenderTransform = t;
                p.Ellipse.RenderTransformOrigin = new Point(0.5, 0.5);
            }

            t.X = p.Position.X;
            t.Y = p.Position.Y;

            // Speed
            double velocityMultiplier = (random.NextDouble() + 0.25) * speed;
            double vX = (1.0 - (random.NextDouble() * 2.0)) * velocityMultiplier;
            // Only going from the bottom of the screen to the top (for now)
            double vY = -Math.Abs((1.0 - (random.NextDouble() * 2.0)) * velocityMultiplier);

            p.Velocity = new Point3D(vX, vY, 0);
            this.particles.Add(p);
        }


        private double RandomWithVariance(double midvalue, double variance)
        {
            double min = Math.Max(midvalue - (variance / 2), 0);
            double max = midvalue + (variance / 2);
            double value = min + ((max - min) * random.NextDouble());
            return value;
        }
    }

}
