using System;

namespace Arcanoid.Patterns.Mediator
{
    class Colleague2 : Colleague
    {
        public Colleague2(Mediator Mediator) : base(Mediator)
        {

        }


        public override void Notify(string msg)
        {
            Console.WriteLine($"I am {GetType().ToString()} got message: {msg}");
        }

        public override void Send(string msg)
        {
            this.Mediator.Send(msg, this);
        }
    }
}