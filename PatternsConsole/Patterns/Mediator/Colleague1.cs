using System;

namespace Arcanoid.Patterns.Mediator
{
    class Colleague1 : Colleague
    {
        public Colleague1(Mediator Mediator) : base(Mediator)
        {
        }


        public override void Send(string msg)
        {
            Mediator.Send(msg, this);
        }

        public override void Notify(string msg)
        {
            Console.WriteLine($"I am {GetType().ToString()} got message: {msg}");
        }
    }
}