namespace Arcanoid.Patterns.Mediator
{
    class MyMediator : Mediator
    {
        public MyMediator(string msg, Colleague Colleague)
        {

        }

        public override void Send(string msg, Colleague Colleague)
        {
            throw new System.NotImplementedException();
        }
    }
}