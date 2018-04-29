namespace Arcanoid.Patterns.Mediator
{
    abstract class Colleague
    {
        protected Mediator Mediator;

        protected Colleague(Mediator Mediator)
        {
            this.Mediator = Mediator;
        }

        public abstract void Notify(string msg);
    }
}