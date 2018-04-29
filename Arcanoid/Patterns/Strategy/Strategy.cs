using System;

namespace Arcanoid.Patterns.Strategy
{
    public abstract class Strategy
    {
        public abstract Object Result { get; set; }
        public abstract void Algorithm();
    }
}