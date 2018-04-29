using System;

namespace Arcanoid.Patterns.Mediator
{
    class Colleague1 : Colleague
    {
        public Colleague1(Mediator Mediator) : base(Mediator)
        {
        }

        public override void Notify(string msg)
        {
            Console.WriteLine($"I am Colleague1 got message: {msg}");
        }
    }
}